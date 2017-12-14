using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.ClientUsers
{
    public interface IClientUserRepository : IRepository<ClientUser>
    {
        ClientUser GetClientUserByUserId(string userId);
    }
}