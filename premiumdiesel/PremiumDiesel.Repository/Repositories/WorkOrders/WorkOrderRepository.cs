using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.WorkOrders
{
    public class WorkOrderRepository : Repository<WorkOrder>, IWorkOrderRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public WorkOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Returns all the products that belong to a specific client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>List of WorkOrders</returns>
        public IEnumerable<WorkOrder> GetWorkOrdersByClientId(int clientId)
        {
            var query = from p in ApplicationContext.WorkOrders
                        where
                            p.ClientId == clientId &&
                            p.Status == Status.Active
                        orderby p.DueDate
                        select p;

            return query.ToList();
        }
    }
}