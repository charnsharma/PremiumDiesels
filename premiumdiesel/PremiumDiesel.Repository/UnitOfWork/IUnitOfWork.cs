using System;
using PremiumDiesel.Repository.Clients;
using PremiumDiesel.Repository.ClientUsers;
using PremiumDiesel.Repository.CustomerLocations;
using PremiumDiesel.Repository.Customers;
using PremiumDiesel.Repository.Products;
using PremiumDiesel.Repository.WorkOrders;

namespace PremiumDiesel.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region Public Properties

        IClientRepository Clients { get; }
        IClientHistoryRepository ClientsHistory { get; }

        IClientUserRepository ClientUsers { get; }
        IClientUserHistoryRepository ClientUsersHistory { get; }

        ICustomerRepository Customers { get; }
        ICustomerHistoryRepository CustomersHistory { get; }

        ICustomerLocationRepository CustomerLocations { get; }
        ICustomerLocationHistoryRepository CustomerLocationsHistory { get; }

        IProductRepository Products { get; }
        IProductHistoryRepository ProductsHistory { get; }

        IWorkOrderRepository WorkOrders { get; }
        IWorkOrderHistoryRepository WorkOrdersHistory { get; }

        #endregion Public Properties

        int SaveChanges();
    }
}