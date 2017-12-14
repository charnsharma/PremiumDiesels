using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByClientId(int clientId);
    }
}