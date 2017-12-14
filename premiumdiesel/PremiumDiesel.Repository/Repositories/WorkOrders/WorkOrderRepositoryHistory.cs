using System.Collections.Generic;
using System.Linq;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.WorkOrders
{
    public class WorkOrderHistoryRepository : Repository<WorkOrderHistory>, IWorkOrderHistoryRepository
    {
        public ApplicationDbContext ApplicationContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public WorkOrderHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<WorkOrderHistory> GetWorkOrdersByClientId(int clientId)
        {
            return ApplicationContext.WorkOrdersHistory.OrderBy(c => c.DueDate).ToList();
        }
    }
}