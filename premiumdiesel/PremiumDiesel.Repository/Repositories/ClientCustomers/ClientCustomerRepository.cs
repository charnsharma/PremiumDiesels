using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.ClientCustomers
{
    public class ClientCustomerRepository : Repository<ClientCustomer>, IClientCustomerRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientCustomerRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<ClientCustomer> GetClientCustomersByClientId(int clientId)
        {
            var query = from p in ApplicationContext.ClientCustomers
                        where
                            p.ClientId == clientId &&
                            p.Status == Status.Active
                        orderby p.Customer.Name
                        select p;

            return query.ToList();
        }
    }
}