using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Clients
{
    public class ClientHistoryRepository : Repository<ClientHistory>, IClientHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}