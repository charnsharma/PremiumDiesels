using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.WorkOrders
{
    public interface IWorkOrderHistoryRepository : IRepository<WorkOrderHistory>
    {
        IEnumerable<WorkOrderHistory> GetWorkOrdersByClientId(int clientId);
    }
}