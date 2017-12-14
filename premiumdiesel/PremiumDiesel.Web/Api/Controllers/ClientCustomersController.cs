using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service.ClientCustomers;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class ClientCustomersController : ApiController
    {
        private IClientCustomerService _clientCustomerService;

        public ClientCustomersController(IClientCustomerService clientCustomerService)
        {
            _clientCustomerService = clientCustomerService;
        }

        // GET: api/ClientCustomers
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ClientCustomers/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ClientCustomers
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ClientCustomers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ClientCustomers/5
        /// <summary>
        /// </summary>
        /// <param name="id">The CustomerId, not the primary key</param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            var deleted = _clientCustomerService.Delete(null, id);
            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}