using System.Data.Entity;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;
using PremiumDiesel.Repository.Repositories.ClientCustomers;

namespace PremiumDiesel.Repository.ClientCustomers
{
    public class ClientCustomerHistoryRepository : Repository<ClientCustomerHistory>, IClientCustomerHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientCustomerHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}