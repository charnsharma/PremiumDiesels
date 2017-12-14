using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.CustomerLocations
{
    public interface ICustomerLocationRepository : IRepository<CustomerLocation>
    {
        IEnumerable<CustomerLocation> GetByCustomer(int customerId);

        CustomerLocation GetHeadOffice(int customerId);
    }
}