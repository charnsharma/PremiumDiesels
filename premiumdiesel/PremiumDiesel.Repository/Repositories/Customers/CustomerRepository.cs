using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Customers
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Customer> GetCustomersByClientId(int clientId)
        {
            var query = from p in ApplicationContext.ClientCustomers
                        where
                            p.ClientId == clientId &&
                            p.Status == Status.Active
                        orderby p.Customer.Name
                        select p.Customer;

            return query.ToList();
        }
    }
}