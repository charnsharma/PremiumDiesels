using System;
using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service.Customers;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class CustomersController : ApiController
    {
        private ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Customers/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                var customerDTO = _customerService.Get(id);
                if (customerDTO == null)
                    return NotFound();

                return Ok(customerDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Customers
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Customers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customers/5
        /// <summary>
        /// Removes a customer from a client but leaves the customer active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var deleted = _customerService.Remove(id);
                if (!deleted)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}