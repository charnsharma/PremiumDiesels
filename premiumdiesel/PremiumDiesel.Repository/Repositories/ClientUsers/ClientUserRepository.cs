using System.Linq;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.ClientUsers
{
    public class ClientUserRepository : Repository<ClientUser>, IClientUserRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ClientUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns a ClientUser by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ClientUser</returns>
        public ClientUser GetClientUserByUserId(string userId)
        {
            var clientUser = (from cu in ApplicationContext.ClientUsers
                              where
                                  cu.UserId == userId &&
                                  cu.Status == Status.Active
                              select cu).SingleOrDefault();

            return clientUser;
        }
    }
}