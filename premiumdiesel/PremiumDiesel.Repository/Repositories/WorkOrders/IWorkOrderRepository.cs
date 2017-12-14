using System.Collections.Generic;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Repository.WorkOrders
{
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
        IEnumerable<WorkOrder> GetWorkOrdersByClientId(int clientId);
    }
}