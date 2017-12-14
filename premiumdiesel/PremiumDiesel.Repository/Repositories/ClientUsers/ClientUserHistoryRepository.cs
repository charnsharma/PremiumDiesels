using System.Data.Entity;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.ClientUsers
{
    public class ClientUserHistoryRepository : Repository<ClientUserHistory>, IClientUserHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientUserHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}