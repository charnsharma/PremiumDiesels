using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Model.Models;
using PremiumDiesel.Repository.UnitOfWork;
using PremiumDiesel.Service.ClientCustomers;
using PremiumDiesel.Service.ClientUsers;

namespace PremiumDiesel.Service.CustomerLocations
{
    public class CustomerLocationService : ICustomerLocationService
    {
        #region Private Fields

        private readonly IClientUserService _clientUserService;
        private readonly IClientCustomerService _clientCustomerService;
        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;

        #endregion Private Fields

        public CustomerLocationService(IClientUserService clientUserService, IClientCustomerService clientCustomerService)
        {
            _clientUserService = clientUserService;
            _clientCustomerService = clientCustomerService;
            _db = new ApplicationDbContext();
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
        }

        #region Get

        /// <summary>
        /// Returns a CustomerLocationDTO by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerLocationDTO Get(int id)
        {
            var customerLocationDTO = new CustomerLocationDTO();
            var customerLocation = _unitOfWork.CustomerLocations.Get(id);
            if (customerLocation != null)
                customerLocationDTO = AutoMapper.Mapper.Map<CustomerLocationDTO>(customerLocation);

            return customerLocationDTO;
        }

        /// <summary>
        /// Returns all customer locations (SuperAdmin only)
        /// </summary>
        /// <returns>List of customer locations or empty list</returns>
        public IEnumerable<CustomerLocationDTO> GetAll()
        {
            var customerLocations = _unitOfWork.CustomerLocations.GetAll();
            if (customerLocations == null)
                return new List<CustomerLocationDTO>();

            var customerLocationDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerLocationDTO>>(customerLocations);

            return customerLocationDTOs;
        }

        /// <summary>
        /// Get all the (active) locations that belong to my customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerLocationDTO> GetAllMyCustomerLocations()
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);
            var clientCustomers = _clientCustomerService.GetClientCustomers();

            var customerLocations = (from cl in _unitOfWork.CustomerLocations.GetAll()
                                     join cc in clientCustomers on cl.CustomerId equals cc.CustomerId
                                     where cc.ClientId == clientId && cl.Status == Status.Active
                                     select cl).ToList();
            if (customerLocations == null)
                return new List<CustomerLocationDTO>();

            var customerLocationDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerLocationDTO>>(customerLocations);

            return customerLocationDTOs;
        }

        /// <summary>
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerLocationDTO GetHeadOffice(int customerId)
        {
            var customerLocation = new CustomerLocation();
            customerLocation = _unitOfWork.CustomerLocations.GetHeadOffice(customerId);
            var customerLocationDTO = AutoMapper.Mapper.Map<CustomerLocationDTO>(customerLocation);

            return customerLocationDTO;
        }

        /// <summary>
        /// Returns the list of (active) customer locations that belongs to a customerId
        /// </summary>
        /// <returns>List of customer locations or empty list</returns>
        public IEnumerable<CustomerLocationDTO> GetCustomerLocationsByCustomerId(int customerId)
        {
            // find all rows by customerId
            var customerLocations = _unitOfWork.CustomerLocations.GetByCustomer(customerId).Where(x => x.Status == Status.Active);
            if (customerLocations == null)
                return new List<CustomerLocationDTO>();

            var customerLocationDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerLocationDTO>>(customerLocations);

            return customerLocationDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// Updates a Customer Location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerLocationDTO"></param>
        /// <returns>The saved customerLocationDTO</returns>
        public CustomerLocationDTO Update(int id, CustomerLocationDTO customerLocationDTO)
        {
            var customerLocation = _unitOfWork.CustomerLocations.Get(id);
            //AutoMapper.Mapper.Map(customerLocationDTO, customerLocation);
            customerLocation.CustomerId = customerLocationDTO.CustomerId;
            customerLocation.Name = customerLocationDTO.Name;
            customerLocation.Type = customerLocationDTO.Type;
            customerLocation.Address1 = customerLocationDTO.Address1;
            customerLocation.Address2 = customerLocationDTO.Address2;
            customerLocation.City = customerLocationDTO.City;
            customerLocation.Province = customerLocationDTO.Province;
            customerLocation.PostalCode = customerLocationDTO.PostalCode;
            customerLocation.Country = customerLocationDTO.Country;
            customerLocation.CreatedByUserId = _currentUserId;
            _unitOfWork.CustomerLocations.Update(customerLocation);

            // History
            var customerLocationHistory = AutoMapper.Mapper.Map<CustomerLocationHistory>(customerLocation);
            customerLocationHistory.CustomerLocationId = customerLocation.Id;
            customerLocationHistory.ModifiedByUserId = _currentUserId;
            customerLocationHistory.ModifiedDate = DateTime.UtcNow;
            customerLocationHistory.Status = customerLocation.Status;

            _unitOfWork.CustomerLocationsHistory.Add(customerLocationHistory);

            _unitOfWork.SaveChanges();

            return customerLocationDTO;
        }

        #endregion Update

        #region Create

        /// <summary>
        /// Adds a new customer location to the database (location + history)
        /// </summary>
        /// <param name="customerLocationDTO"></param>
        /// <returns>The added Customer</returns>
        public CustomerLocationDTO Create(CustomerLocationDTO customerLocationDTO)
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            // CustomerLocation
            var customerLocation = AutoMapper.Mapper.Map<CustomerLocation>(customerLocationDTO);
            customerLocation.CreatedByUserId = _currentUserId;
            customerLocation.CreatedDate = DateTime.UtcNow;
            customerLocation.Status = Status.Active;

            _unitOfWork.CustomerLocations.Add(customerLocation);
            // Have to call this here to get the new CustomerId
            _unitOfWork.SaveChanges();

            // History
            var customerLocationHistory = AutoMapper.Mapper.Map<CustomerLocationHistory>(customerLocation);
            customerLocationHistory.CustomerLocationId = customerLocation.Id;
            customerLocationHistory.ModifiedByUserId = _currentUserId;
            customerLocationHistory.ModifiedDate = customerLocation.CreatedDate;
            customerLocationHistory.Status = Status.Active;

            _unitOfWork.CustomerLocationsHistory.Add(customerLocationHistory);

            _unitOfWork.SaveChanges();

            return customerLocationDTO;
        }

        #endregion Create

        #region Delete

        /// <summary>
        /// Soft deletes a CustomerLocation
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var customerLocation = _unitOfWork.CustomerLocations.Get(id);
            if (customerLocation == null)
                return false;

            customerLocation.Status = Status.Deleted;

            // delete it from the Customers table
            _unitOfWork.CustomerLocations.Delete(customerLocation);

            // History
            var customerLocationHistory = AutoMapper.Mapper.Map<CustomerLocationHistory>(customerLocation);
            customerLocationHistory.CustomerLocationId = id;
            customerLocationHistory.ModifiedByUserId = _currentUserId;
            customerLocationHistory.ModifiedDate = DateTime.UtcNow;
            customerLocationHistory.Status = Status.Deleted;

            _unitOfWork.CustomerLocationsHistory.Add(customerLocationHistory);

            _unitOfWork.SaveChanges();

            return true;
        }

        #endregion Delete

        /// <summary>
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool HasHeadOffice(int customerId)
        {
            var headOffice = GetHeadOffice(customerId);
            if (headOffice == null)
                return false;

            return true;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}