using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns all the products that belong to a specific client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>List of Products</returns>
        public IEnumerable<Product> GetProductsByClientId(int clientId)
        {
            var query = from p in ApplicationContext.Products
                        where
                            p.ClientId == clientId &&
                            p.Status == Status.Active
                        orderby p.Name
                        select p;

            return query.ToList();
        }
    }
}