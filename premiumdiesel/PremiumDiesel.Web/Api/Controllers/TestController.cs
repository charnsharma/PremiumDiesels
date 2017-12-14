using System;
using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.Products;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class TestController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IClientUserService _clientUserService;

        public TestController(IProductService productService, IClientUserService clientUserService)
        {
            _productService = productService;
            _clientUserService = clientUserService;
        }

        // GET: api/Test
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Test/5
        public IHttpActionResult Get(int id)
        {
            //return NotFound();
            return Ok("value");
        }

        // POST: api/Test
        public IHttpActionResult Post([FromBody]string value)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // create a new entity
            int createdEntityId = 5; //example

            return Created(new Uri(Request.RequestUri + "/" + createdEntityId), string.Empty);
        }

        // PUT: api/Test/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok(value);
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}