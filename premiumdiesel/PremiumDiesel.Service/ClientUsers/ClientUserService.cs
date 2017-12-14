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

namespace PremiumDiesel.Service.ClientUsers
{
    public class ClientUserService : IClientUserService
    {
        #region Private Fields

        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;

        #endregion Private Fields

        public ClientUserService()
        {
            _db = new ApplicationDbContext();
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
        }

        #region Get

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ClientUserDTO GetClientUserByUserId(string userId)
        {
            var clientUser = _unitOfWork.ClientUsers.GetClientUserByUserId(userId);
            var clientUserDTO = AutoMapper.Mapper.Map<ClientUserDTO>(clientUser);

            return clientUserDTO;
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int? GetUserClientId(string userId)
        {
            var clientUserDTO = GetClientUserByUserId(userId);
            if (clientUserDTO == null)
                return null;

            return clientUserDTO.ClientId;
        }

        /// <summary>
        /// Returns all the (active) users that belong to the logged in client
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientUserDTO> GetUsers()
        {
            var clientId = GetUserClientId(_currentUserId);
            var clientUsers = (from cu in _unitOfWork.ClientUsers.GetAll()
                               where
                               cu.ClientId == clientId
                               && cu.Status == Status.Active
                               select cu
                         ).ToList();
            var clientUserDTOs = AutoMapper.Mapper.Map<IEnumerable<ClientUserDTO>>(clientUsers);

            return clientUserDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientUserDTO"></param>
        /// <returns></returns>
        public ClientUserDTO Update(int id, ClientUserDTO clientUserDTO)
        {
            // get the existing clientUser
            var clientUser = _unitOfWork.ClientUsers.Get(id);
            // merge the DTO
            AutoMapper.Mapper.Map(clientUserDTO, clientUser);

            _unitOfWork.ClientUsers.Update(clientUser);

            // History
            var clientUserHistory = AutoMapper.Mapper.Map<ClientUserHistory>(clientUser);
            clientUserHistory.ModifiedByUserId = _currentUserId;
            clientUserHistory.ModifiedDate = DateTime.UtcNow;
            clientUserHistory.Status = clientUser.Status;

            _unitOfWork.ClientUsersHistory.Add(clientUserHistory);

            _unitOfWork.SaveChanges();

            return clientUserDTO;
        }

        #endregion Update

        #region Create

        /// <summary>
        /// </summary>
        /// <param name="clientUserDTO"></param>
        /// <returns></returns>
        public ClientUserDTO Create(ClientUserDTO clientUserDTO)
        {
            // first, find if there is already a row with this clientId and userId
            var clientUser = (from cu in _unitOfWork.ClientUsers.GetAll()
                              where cu.ClientId == clientUserDTO.ClientId && cu.UserId == clientUserDTO.UserId
                              select cu
                         ).SingleOrDefault();

            // if it doesn't exist, create a new row
            if (clientUser != null)
                return Update(clientUser.Id, clientUserDTO);

            //otherwise re-enable the existing one
            clientUser = AutoMapper.Mapper.Map<ClientUser>(clientUserDTO);

            clientUser.CreatedByUserId = _currentUserId;
            if (clientUser.CreatedDate != null)
                clientUser.CreatedDate = DateTime.UtcNow;
            clientUser.Status = Status.Active;

            _unitOfWork.ClientUsers.Add(clientUser);

            // History
            var clientUserHistory = AutoMapper.Mapper.Map<ClientUserHistory>(clientUser);
            clientUserHistory.ModifiedByUserId = _currentUserId;
            clientUserHistory.ModifiedDate = clientUser.CreatedDate;
            clientUserHistory.Status = Status.Active;

            _unitOfWork.ClientUsersHistory.Add(clientUserHistory);

            _unitOfWork.SaveChanges();

            return clientUserDTO;
        }

        #endregion Create

        #region Delete

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var clientUser = _unitOfWork.ClientUsers.Get(id);
            if (clientUser == null)
                return false;

            clientUser.Status = Status.Deleted;

            _unitOfWork.ClientUsers.Delete(clientUser);

            // History
            var clientUserHistory = AutoMapper.Mapper.Map<ClientUserHistory>(clientUser);
            clientUserHistory.ModifiedByUserId = _currentUserId;
            clientUserHistory.ModifiedDate = DateTime.UtcNow;
            clientUserHistory.Status = Status.Deleted;

            _unitOfWork.ClientUsersHistory.Add(clientUserHistory);

            _unitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        /// Deletes a ClientUser row based on ClientId and CustomerId
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Delete(int clientId, string userId)
        {
            var clientUser = (from cu in _unitOfWork.ClientUsers.GetAll()
                              where cu.ClientId == clientId && cu.UserId == userId
                              select cu).SingleOrDefault();
            if (clientUser == null)
                return false;

            return Delete(clientUser.Id);
        }

        /// <summary>
        /// Delete all ClientUser rows when deleting a Client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool DeleteAll(int clientId)
        {
            // find all the ClientUser rows that have the same ClientId
            var clientUsers = (from cc in _unitOfWork.ClientUsers.GetAll()
                               where cc.ClientId == clientId
                               select cc).ToList();

            if (clientUsers == null)
                return false;

            var now = DateTime.UtcNow;
            // soft-delete all the rows
            foreach (var clientUser in clientUsers)
            {
                Delete(clientUser.Id);
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