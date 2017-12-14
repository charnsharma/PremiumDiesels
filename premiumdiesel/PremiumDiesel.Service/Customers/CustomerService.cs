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

namespace PremiumDiesel.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        #region Private Fields

        private readonly IClientUserService _clientUserService;
        private readonly IClientCustomerService _clientCustomerService;
        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;

        #endregion Private Fields

        public CustomerService(IClientUserService clientUserService, IClientCustomerService clientCustomerService)
        {
            _clientUserService = clientUserService;
            _clientCustomerService = clientCustomerService;
            _db = new ApplicationDbContext();
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
        }

        #region Get

        /// <summary>
        /// Returns a CustomerDTO by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CustomerDTO Get(int id)
        {
            var customerDTO = new CustomerDTO();
            var customer = _unitOfWork.Customers.Get(id);
            if (customer != null)
                customerDTO = AutoMapper.Mapper.Map<CustomerDTO>(customer);

            return customerDTO;
        }

        /// <summary>
        /// Returns all customers (SuperAdmin only)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerDTO> GetAll()
        {
            var customers = _unitOfWork.Customers.GetAll();
            var customerDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerDTO>>(customers);

            return customerDTOs;
        }

        /// <summary>
        /// Returns the list of customers that belongs to the current logged in user
        /// </summary>
        /// <returns>List of customers or empty list</returns>
        public IEnumerable<CustomerDTO> GetCustomers()
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            if (!clientId.HasValue)
                return new List<CustomerDTO>();

            var customers = _unitOfWork.Customers.GetCustomersByClientId((int)clientId).Where(x => x.Status == Status.Active);
            var customerDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerDTO>>(customers);

            return customerDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// Updates a Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerDTO"></param>
        /// <returns>The saved CustomerDTO</returns>
        public CustomerDTO Update(int id, CustomerDTO customerDTO)
        {
            var customer = _unitOfWork.Customers.Get(id);
            AutoMapper.Mapper.Map(customerDTO, customer);

            _unitOfWork.Customers.Update(customer);

            // History
            var customerHistory = AutoMapper.Mapper.Map<CustomerHistory>(customer);
            customerHistory.CustomerId = id;
            customerHistory.ModifiedByUserId = _currentUserId;
            customerHistory.ModifiedDate = DateTime.UtcNow;
            customerHistory.Status = customer.Status;

            _unitOfWork.CustomersHistory.Add(customerHistory);

            _unitOfWork.SaveChanges();

            return customerDTO;
        }

        #endregion Update

        #region Create

        /// <summary>
        /// Adds a new Customer to the database (Customers + CustomersHistory)
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns>The added Customer</returns>
        public CustomerDTO Create(CustomerDTO customerDTO)
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            // Customer
            var customer = AutoMapper.Mapper.Map<Customer>(customerDTO);
            customer.CreatedByUserId = _currentUserId;
            customer.CreatedDate = DateTime.UtcNow;
            customer.Status = Status.Active;

            _unitOfWork.Customers.Add(customer);
            // Have to call this here to get the new CustomerId
            _unitOfWork.SaveChanges();

            customerDTO.Id = customer.Id;

            // ClientCustomer + ClientCustomerHistory
            var clientCustomerDTO = new ClientCustomerDTO()
            {
                ClientId = (int)clientId,
                CustomerId = customer.Id,
            };
            _clientCustomerService.Create(clientCustomerDTO);

            // History
            var customerHistory = AutoMapper.Mapper.Map<CustomerHistory>(customer);
            customerHistory.CustomerId = customer.Id;
            customerHistory.ModifiedByUserId = _currentUserId;
            customerHistory.ModifiedDate = customer.CreatedDate;
            customerHistory.Status = Status.Active;

            _unitOfWork.CustomersHistory.Add(customerHistory);

            _unitOfWork.SaveChanges();

            return customerDTO;
        }

        #endregion Create

        #region Delete

        /// <summary>
        /// Soft deletes a Customer (superadmin only)
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var customer = _unitOfWork.Customers.Get(id);
            if (customer == null)
                return false;

            customer.Status = Status.Deleted;

            // delete it from the Customers table
            _unitOfWork.Customers.Delete(customer);

            // delete all the references in the ClientCustomers table as well
            _clientCustomerService.DeleteAll(id);

            // History
            var customerHistory = AutoMapper.Mapper.Map<CustomerHistory>(customer);
            customerHistory.CustomerId = id;
            customerHistory.ModifiedByUserId = _currentUserId;
            customerHistory.ModifiedDate = DateTime.UtcNow;
            customerHistory.Status = Status.Deleted;

            _unitOfWork.CustomersHistory.Add(customerHistory);

            _unitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        /// Removes a customer from a client, but leaves the customer active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            return _clientCustomerService.Delete(clientId, id);
        }

        #endregion Delete

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}