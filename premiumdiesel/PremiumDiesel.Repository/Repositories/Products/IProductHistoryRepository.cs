using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Products
{
    public interface IProductHistoryRepository : IRepository<ProductHistory>
    {
        IEnumerable<ProductHistory> GetProductsByClientId(int clientId);
    }
}