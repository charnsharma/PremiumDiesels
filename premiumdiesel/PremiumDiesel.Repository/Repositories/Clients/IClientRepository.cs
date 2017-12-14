using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Clients
{
    public interface IClientRepository : IRepository<Client>
    {
        IEnumerable<Client> GetMyClients();
    }
}