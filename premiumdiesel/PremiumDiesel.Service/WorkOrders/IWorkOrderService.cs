using System;
using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Service.WorkOrders
{
    public interface IWorkOrderService : IDisposable
    {
        IEnumerable<WorkOrderDTO> GetAll();

        WorkOrderDTO Get(int id);

        IEnumerable<WorkOrderDTO> GetWorkOrders();

        WorkOrderDTO Update(int id, WorkOrderDTO productDTO);

        WorkOrderDTO Create(WorkOrderDTO productDTO);

        bool Delete(int id);
    }
}