using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.Products
{
    public interface IProductService : IDisposable
    {
        IEnumerable<ProductDTO> GetAll();

        ProductDTO Get(int id);

        IEnumerable<ProductDTO> GetProducts();

        ProductDTO Update(int id, ProductDTO productDTO);

        ProductDTO Create(ProductDTO productDTO);

        bool Delete(int id);
    }
}