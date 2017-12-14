using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.CustomerLocations
{
    public class CustomerLocationHistoryRepository : Repository<CustomerLocationHistory>, ICustomerLocationHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public CustomerLocationHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<CustomerLocation> GetMyCustomerLocations()
        {
            return ApplicationContext.CustomerLocations.OrderByDescending(c => c.Name).ToList();
        }
    }
}