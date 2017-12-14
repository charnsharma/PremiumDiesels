using PremiumDiesel.Model.Context;
using PremiumDiesel.Repository.ClientCustomers;
using PremiumDiesel.Repository.Clients;
using PremiumDiesel.Repository.ClientUsers;
using PremiumDiesel.Repository.CustomerLocations;
using PremiumDiesel.Repository.Customers;
using PremiumDiesel.Repository.Products;
using PremiumDiesel.Repository.Repositories.ClientCustomers;
using PremiumDiesel.Repository.WorkOrders;

namespace PremiumDiesel.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Fields

        private readonly ApplicationDbContext _context;

        #endregion Private Fields

        #region Public Properties

        public IClientRepository Clients { get; private set; }
        public IClientHistoryRepository ClientsHistory { get; private set; }

        public IClientUserRepository ClientUsers { get; private set; }
        public IClientUserHistoryRepository ClientUsersHistory { get; private set; }

        public ICustomerRepository Customers { get; private set; }
        public ICustomerHistoryRepository CustomersHistory { get; private set; }

        public IClientCustomerRepository ClientCustomers { get; private set; }
        public IClientCustomerHistoryRepository ClientCustomersHistory { get; private set; }

        public ICustomerLocationRepository CustomerLocations { get; private set; }
        public ICustomerLocationHistoryRepository CustomerLocationsHistory { get; private set; }

        public IProductRepository Products { get; private set; }
        public IProductHistoryRepository ProductsHistory { get; private set; }

        public IWorkOrderRepository WorkOrders { get; private set; }
        public IWorkOrderHistoryRepository WorkOrdersHistory { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Clients = new ClientRepository(_context);
            ClientsHistory = new ClientHistoryRepository(_context);

            ClientUsers = new ClientUserRepository(_context);
            ClientUsersHistory = new ClientUserHistoryRepository(_context);

            Customers = new CustomerRepository(_context);
            CustomersHistory = new CustomerHistoryRepository(_context);

            ClientCustomers = new ClientCustomerRepository(_context);
            ClientCustomersHistory = new ClientCustomerHistoryRepository(_context);

            CustomerLocations = new CustomerLocationRepository(_context);
            CustomerLocationsHistory = new CustomerLocationHistoryRepository(_context);

            Products = new ProductRepository(_context);
            ProductsHistory = new ProductHistoryRepository(_context);

            WorkOrders = new WorkOrderRepository(_context);
            WorkOrdersHistory = new WorkOrderHistoryRepository(_context);
        }

        #endregion Public Constructors

        #region Public Methods

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion Public Methods
    }
}