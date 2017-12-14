using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.ClientCustomers
{
    public interface IClientCustomerRepository : IRepository<ClientCustomer>
    {
        IEnumerable<ClientCustomer> GetClientCustomersByClientId(int clientId);
    }
}