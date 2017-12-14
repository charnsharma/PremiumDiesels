using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Model.Models;
using PremiumDiesel.Repository.UnitOfWork;
using PremiumDiesel.Service.ClientUsers;

namespace PremiumDiesel.Service.Products
{
    public class ProductService : IProductService
    {
        #region Private Fields

        private readonly IClientUserService _clientUserService;
        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        public ProductService(IClientUserService clientUserService)
        {
            _clientUserService = clientUserService;
            _db = new ApplicationDbContext();
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
        }

        #endregion Public Constructors

        #region Get

        /// <summary>
        /// Returns a ProductDTO by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDTO Get(int id)
        {
            var productDTO = new ProductDTO();
            var product = _unitOfWork.Products.Get(id);
            if (product != null)
                productDTO = AutoMapper.Mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        /// <summary>
        /// Returns all products (SuperAdmin only)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDTO> GetAll()
        {
            var products = _unitOfWork.Products.GetAll();
            var productDTOs = AutoMapper.Mapper.Map<IEnumerable<ProductDTO>>(products);

            return productDTOs;
        }

        /// <summary>
        /// Returns the list of products that belongs to the current logged in user
        /// </summary>
        /// <returns>List of products or empty list</returns>
        public IEnumerable<ProductDTO> GetProducts()
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            if (!clientId.HasValue)
                return new List<ProductDTO>();

            var products = _unitOfWork.Products.GetProductsByClientId((int)clientId);
            var productDTOs = AutoMapper.Mapper.Map<IEnumerable<ProductDTO>>(products);

            return productDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// Updates a Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDTO"></param>
        /// <returns>The saved ProductDTO</returns>
        public ProductDTO Update(int id, ProductDTO productDTO)
        {
            // get the existing product
            var product = _unitOfWork.Products.Get(id);
            // merge the DTO
            AutoMapper.Mapper.Map(productDTO, product);

            _unitOfWork.Products.Update(product);

            // History
            var productHistory = AutoMapper.Mapper.Map<ProductHistory>(product);
            productHistory.ProductId = id;
            productHistory.ModifiedByUserId = HttpContext.Current.User.Identity.GetUserId();
            productHistory.ModifiedDate = DateTime.UtcNow;
            productHistory.Status = product.Status;

            _unitOfWork.ProductsHistory.Add(productHistory);

            _unitOfWork.SaveChanges();

            return productDTO;
        }

        #endregion Update

        #region Create

        /// <summary>
        /// Adds a new Product to the database
        /// </summary>
        /// <param name="productDTO"></param>
        /// <returns>The added Product</returns>
        public ProductDTO Create(ProductDTO productDTO)
        {
            var product = AutoMapper.Mapper.Map<Product>(productDTO);

            product.CreatedByUserId = _currentUserId;
            product.ClientId = (int)_clientUserService.GetUserClientId(product.CreatedByUserId);
            product.CreatedDate = DateTime.UtcNow;
            product.Status = Status.Active;

            _unitOfWork.Products.Add(product);
            // Have to call this here to get the new ProductId
            _unitOfWork.SaveChanges();

            // History
            var productHistory = AutoMapper.Mapper.Map<ProductHistory>(product);
            productHistory.ProductId = product.Id;
            productHistory.ModifiedByUserId = _currentUserId;
            productHistory.ModifiedDate = product.CreatedDate;
            productHistory.Status = Status.Active;

            _unitOfWork.ProductsHistory.Add(productHistory);

            _unitOfWork.SaveChanges();

            return productDTO;
        }

        #endregion Create

        #region Delete

        /// <summary>
        /// Soft deletes a Product
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var product = _unitOfWork.Products.Get(id);
            if (product == null)
                return false;

            product.Status = Status.Deleted;

            _unitOfWork.Products.Delete(product);

            // History
            var productHistory = AutoMapper.Mapper.Map<ProductHistory>(product);
            productHistory.ProductId = id;
            productHistory.ModifiedByUserId = _currentUserId;
            productHistory.ModifiedDate = DateTime.UtcNow;
            productHistory.Status = Status.Deleted;

            _unitOfWork.ProductsHistory.Add(productHistory);

            _unitOfWork.SaveChanges();

            return true;
        }

        #endregion Delete

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}