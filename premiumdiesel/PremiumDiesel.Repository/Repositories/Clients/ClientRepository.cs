using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Clients
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Client> GetMyClients()
        {
            return ApplicationContext.Clients.OrderByDescending(c => c.Name).ToList();
        }
    }
}