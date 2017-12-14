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
using PremiumDiesel.Service.ClientUsers;

namespace PremiumDiesel.Service.ClientCustomers
{
    public class ClientCustomerService : IClientCustomerService
    {
        #region Private Fields

        private readonly IClientUserService _clientUserService;
        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;

        #endregion Private Fields

        public ClientCustomerService(IClientUserService clientUserService)
        {
            _clientUserService = clientUserService;
            _db = new ApplicationDbContext();
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
        }

        #region Get

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientCustomerDTO Get(int id)
        {
            var clientCustomerDTO = new ClientCustomerDTO();
            var customer = _unitOfWork.Customers.Get(id);
            if (customer != null)
                clientCustomerDTO = AutoMapper.Mapper.Map<ClientCustomerDTO>(customer);

            return clientCustomerDTO;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientCustomerDTO> GetAll()
        {
            var clientCustomers = _unitOfWork.ClientCustomers.GetAll();
            var clientCustomerDTOs = AutoMapper.Mapper.Map<IEnumerable<ClientCustomerDTO>>(clientCustomers);

            return clientCustomerDTOs;
        }

        /// <summary>
        /// Get all the (active) customers of the logged in client
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientCustomerDTO> GetClientCustomers()
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            if (!clientId.HasValue)
                return new List<ClientCustomerDTO>();

            var clientCustomers = _unitOfWork.ClientCustomers.GetClientCustomersByClientId((int)clientId).Where(x => x.Status == Status.Active);
            var clientCustomerDTOs = AutoMapper.Mapper.Map<IEnumerable<ClientCustomerDTO>>(clientCustomers);

            return clientCustomerDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientCustomerDTO"></param>
        /// <returns></returns>
        public ClientCustomerDTO Update(int id, ClientCustomerDTO clientCustomerDTO)
        {
            // get the existing clientCustomer
            var clientCustomer = _unitOfWork.ClientCustomers.Get(id);
            // merge the DTO
            AutoMapper.Mapper.Map(clientCustomerDTO, clientCustomer);

            _unitOfWork.ClientCustomers.Update(clientCustomer);

            // History
            var clientCustomerHistory = AutoMapper.Mapper.Map<ClientCustomerHistory>(clientCustomer);
            clientCustomerHistory.ModifiedByUserId = _currentUserId;
            clientCustomerHistory.ModifiedDate = DateTime.UtcNow;
            clientCustomerHistory.Status = clientCustomer.Status;

            _unitOfWork.ClientCustomersHistory.Add(clientCustomerHistory);

            _unitOfWork.SaveChanges();

            return clientCustomerDTO;
        }

        #endregion Update

        #region Create

        /// <summary>
        /// </summary>
        /// <param name="clientCustomerDTO"></param>
        /// <returns></returns>
        public ClientCustomerDTO Create(ClientCustomerDTO clientCustomerDTO)
        {
            // first, find if there is already a row with this clientId and customerId
            var clientCustomer = (from cc in _unitOfWork.ClientCustomers.GetAll()
                                  where cc.ClientId == clientCustomerDTO.ClientId && cc.CustomerId == clientCustomerDTO.CustomerId
                                  select cc
                         ).SingleOrDefault();

            // if it doesn't exist, create a new row
            if (clientCustomer != null)
                return this.Update(clientCustomer.Id, clientCustomerDTO);

            //otherwise re-enable the existing one
            clientCustomer = AutoMapper.Mapper.Map<ClientCustomer>(clientCustomerDTO);

            clientCustomer.CreatedByUserId = _currentUserId;
            if (clientCustomer.CreatedDate != null)
                clientCustomer.CreatedDate = DateTime.UtcNow;
            clientCustomer.Status = Status.Active;

            _unitOfWork.ClientCustomers.Add(clientCustomer);

            // History
            var clientCustomerHistory = AutoMapper.Mapper.Map<ClientCustomerHistory>(clientCustomer);
            clientCustomerHistory.ModifiedByUserId = _currentUserId;
            clientCustomerHistory.ModifiedDate = clientCustomer.CreatedDate;
            clientCustomerHistory.Status = Status.Active;

            _unitOfWork.ClientCustomersHistory.Add(clientCustomerHistory);

            _unitOfWork.SaveChanges();

            return clientCustomerDTO;
        }

        #endregion Create

        #region Delete

        public bool Delete(int id)
        {
            var clientCustomer = _unitOfWork.ClientCustomers.Get(id);
            if (clientCustomer == null)
                return false;

            clientCustomer.Status = Status.Deleted;

            _unitOfWork.ClientCustomers.Delete(clientCustomer);

            // History
            var clientCustomerHistory = AutoMapper.Mapper.Map<ClientCustomerHistory>(clientCustomer);
            clientCustomerHistory.ModifiedByUserId = _currentUserId;
            clientCustomerHistory.ModifiedDate = DateTime.UtcNow;
            clientCustomerHistory.Status = Status.Deleted;

            _unitOfWork.ClientCustomersHistory.Add(clientCustomerHistory);

            _unitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes a ClientCustomer row based on ClientId and CustomerId
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool Delete(int? clientId, int customerId)
        {
            if (!clientId.HasValue)
                clientId = _clientUserService.GetUserClientId(_currentUserId);

            var clientCustomer = (from cc in _unitOfWork.ClientCustomers.GetAll()
                                  where cc.ClientId == clientId && cc.CustomerId == customerId
                                  select cc).SingleOrDefault();
            if (clientCustomer == null)
                return false;

            return Delete(clientCustomer.Id);
        }

        /// <summary>
        /// Delete all ClientCustomer rows when deleting a Customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteAll(int customerId)
        {
            // find all the ClientCustomer rows that have the same CustomerId
            var clientCustomers = (from cc in _unitOfWork.ClientCustomers.GetAll()
                                   where cc.CustomerId == customerId
                                   select cc).ToList();

            if (clientCustomers == null)
                return false;

            // soft-delete all the rows
            foreach (var clientCustomer in clientCustomers)
            {
                Delete(clientCustomer.Id);
            }

            return true;
        }

        #endregion Delete

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}