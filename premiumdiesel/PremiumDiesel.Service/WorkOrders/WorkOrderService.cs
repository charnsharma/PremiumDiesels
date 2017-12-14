using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Model.Models;
using PremiumDiesel.Repository.UnitOfWork;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Service.Products;

namespace PremiumDiesel.Service.WorkOrders
{
    public class WorkOrderService : IWorkOrderService
    {
        #region Private Fields

        private readonly IClientUserService _clientUserService;
        private ApplicationDbContext _db;
        private string _currentUserId;
        private UnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private IProductService _productService;
        private ICustomerLocationService _customerLocationService;
        private ICustomerService _customerService;

        #endregion Private Fields

        #region Public Constructors

        public WorkOrderService(
            IClientUserService clientUserService,
            IProductService productService,
            ICustomerLocationService customerLocationService,
            ICustomerService customerService
            )
        {
            _db = new ApplicationDbContext();
            _clientUserService = clientUserService;
            _productService = productService;
            _customerLocationService = customerLocationService;
            _customerService = customerService;
            _currentUserId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = new UnitOfWork(_db);
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }

        #endregion Public Constructors

        #region Get

        /// <summary>
        /// Returns a WorkOrderDTO by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorkOrderDTO Get(int id)
        {
            var workOrder = _unitOfWork.WorkOrders.Get(id);
            if (workOrder == null)
                return null;

            var workOrderDTO = AutoMapper.Mapper.Map<WorkOrderDTO>(workOrder);

            return workOrderDTO;
        }

        /// <summary>
        /// Returns all workOrders (SuperAdmin only)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkOrderDTO> GetAll()
        {
            var workOrders = _unitOfWork.WorkOrders.GetAll();
            var workOrderDTOs = AutoMapper.Mapper.Map<IEnumerable<WorkOrderDTO>>(workOrders);

            return workOrderDTOs;
        }

        /// <summary>
        /// Returns the list of workOrders that belongs to the current logged in user
        /// </summary>
        /// <returns>List of workOrders or empty list</returns>
        public IEnumerable<WorkOrderDTO> GetWorkOrders()
        {
            var clientId = _clientUserService.GetUserClientId(_currentUserId);

            if (!clientId.HasValue)
                return new List<WorkOrderDTO>();

            var workOrders = _unitOfWork.WorkOrders.GetWorkOrdersByClientId((int)clientId);
            var workOrderDTOs = AutoMapper.Mapper.Map<IEnumerable<WorkOrderDTO>>(workOrders);

            return workOrderDTOs;
        }

        #endregion Get

        #region Update

        /// <summary>
        /// Updates a WorkOrder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workOrderDTO"></param>
        /// <returns>The saved WorkOrderDTO</returns>
        public WorkOrderDTO Update(int id, WorkOrderDTO workOrderDTO)
        {
            try
            {
                // get the existing workOrder
                var workOrder = _unitOfWork.WorkOrders.Get(id);

                if (workOrder == null)
                    return null;

                // merge the DTO

                //    AutoMapper.Mapper.Map(workOrderDTO, workOrder);

                workOrder.CustomerId = workOrderDTO.CustomerId;
                workOrder.LocationId = workOrderDTO.LocationId;
                workOrder.ProductId = workOrderDTO.ProductId;
                workOrder.Quantity = workOrderDTO.Quantity;
                workOrder.Notes = workOrderDTO.Notes;
                workOrder.DueDate = workOrderDTO.DueDate;
                workOrder.AssignedToUserId = workOrderDTO.AssignedToUserId;
                workOrder.CreatedByUserId = _currentUserId;
                _unitOfWork.WorkOrders.Update(workOrder);
                _unitOfWork.SaveChanges();

                // History
                var workOrderHistory = AutoMapper.Mapper.Map<WorkOrderHistory>(workOrder);
                workOrderHistory.WorkOrderId = id;
                workOrderHistory.ModifiedByUserId = HttpContext.Current.User.Identity.GetUserId();
                workOrderHistory.ModifiedDate = DateTime.UtcNow;
                workOrderHistory.Status = workOrder.Status;

                _unitOfWork.WorkOrdersHistory.Add(workOrderHistory);
                _unitOfWork.SaveChanges();

                return workOrderDTO;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return null;
            }
        }

        #endregion Update

        #region Create

        /// <summary>
        /// Adds a new WorkOrder to the database
        /// </summary>
        /// <param name="workOrderDTO"></param>
        /// <returns>The added WorkOrder</returns>
        public WorkOrderDTO Create(WorkOrderDTO workOrderDTO)
        {
            var workOrder = AutoMapper.Mapper.Map<WorkOrder>(workOrderDTO);

            workOrder.CreatedByUserId = _currentUserId;
            workOrder.ClientId = (int)_clientUserService.GetUserClientId(workOrder.CreatedByUserId);
            workOrder.CreatedDate = DateTime.UtcNow;
            workOrder.Status = Status.Active;

            _unitOfWork.WorkOrders.Add(workOrder);
            // Have to call this here to get the new WorkOrderId
            _unitOfWork.SaveChanges();

            // History
            var workOrderHistory = AutoMapper.Mapper.Map<WorkOrderHistory>(workOrder);
            workOrderHistory.WorkOrderId = workOrder.Id;
            workOrderHistory.ModifiedByUserId = _currentUserId;
            workOrderHistory.ModifiedDate = workOrder.CreatedDate;
            workOrderHistory.Status = Status.Active;

            _unitOfWork.WorkOrdersHistory.Add(workOrderHistory);

            _unitOfWork.SaveChanges();

            return workOrderDTO;
        }

        #endregion Create

        #region Delete

        /// <summary>
        /// Soft deletes a WorkOrder
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var workOrder = _unitOfWork.WorkOrders.Get(id);
            if (workOrder == null)
                return false;

            workOrder.Status = Status.Deleted;

            _unitOfWork.WorkOrders.Delete(workOrder);

            // History
            var workOrderHistory = AutoMapper.Mapper.Map<WorkOrderHistory>(workOrder);
            workOrderHistory.WorkOrderId = id;
            workOrderHistory.ModifiedByUserId = _currentUserId;
            workOrderHistory.ModifiedDate = DateTime.UtcNow;
            workOrderHistory.Status = Status.Deleted;

            _unitOfWork.WorkOrdersHistory.Add(workOrderHistory);

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