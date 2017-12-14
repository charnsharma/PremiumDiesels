using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service.WorkOrders;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class WorkOrdersController : ApiController
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        // GET: api/Products
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Products/5
        public IHttpActionResult Get(int id)
        {
            var workOrderDTO = _workOrderService.Get(id);
            if (workOrderDTO == null)
                return NotFound();

            return Ok(workOrderDTO);
        }

        // POST: api/Products
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        public IHttpActionResult Delete(int id)
        {
            var deleted = _workOrderService.Delete(id);
            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}