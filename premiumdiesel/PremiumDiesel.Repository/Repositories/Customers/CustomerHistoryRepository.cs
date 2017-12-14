using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Customers
{
    public class CustomerHistoryRepository : Repository<CustomerHistory>, ICustomerHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public CustomerHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Customer> GetMyCustomers()
        {
            return ApplicationContext.Customers.OrderByDescending(c => c.Name).ToList();
        }
    }
}