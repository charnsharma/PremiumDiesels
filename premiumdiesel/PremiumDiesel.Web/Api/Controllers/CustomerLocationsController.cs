using System;
using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service.CustomerLocations;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class CustomerLocationsController : ApiController
    {
        private ICustomerLocationService _customerLocationService;

        public CustomerLocationsController(ICustomerLocationService customerLocationService)
        {
            _customerLocationService = customerLocationService;
        }

        #region Locations

        // GET: api/CustomerLocations
        [Route("api/locations/")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CustomerLocations/5
        [Route("api/locations/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var customerLocationDTO = _customerLocationService.Get(id);
                if (customerLocationDTO == null)
                    return NotFound();

                return Ok(customerLocationDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/CustomerLocations
        [Route("api/locations/")]
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CustomerLocations/5
        [Route("api/locations/{id}")]
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomerLocations/5
        [Route("api/locations/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var deleted = _customerLocationService.Delete(id);
                if (!deleted)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion Locations

        #region CustomerLocations

        /// <summary>
        /// Returns the list of locations by customer id
        /// </summary>
        /// <param name="id">The customerId</param>
        /// <returns></returns>
        //[Route("api/customerlocations/{id}")]
        [Route("api/customerlocations/{id}")]
        [HttpGet]
        public IHttpActionResult GetByCustomer(int id)
        {
            try
            {
                var customerLocationDTOs = _customerLocationService.GetCustomerLocationsByCustomerId(id);
                if (customerLocationDTOs == null)
                    return NotFound();

                return Ok(customerLocationDTOs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion CustomerLocations
    }
}