using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Products
{
    public class ProductHistoryRepository : Repository<ProductHistory>, IProductHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ProductHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<ProductHistory> GetProductsByClientId(int clientId)
        {
            return ApplicationContext.ProductsHistory.OrderBy(c => c.Name).ToList();
        }
    }
}