using System;
using System.Collections.Generic;
using System.Web.Http;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.Products;

namespace PremiumDiesel.Web.Api.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IClientUserService _clientUserService;

        public ProductsController(IProductService productService, IClientUserService clientUserService)
        {
            _productService = productService;
            _clientUserService = clientUserService;
        }

        // GET: api/Products
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Products/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                var productDTO = _productService.Get(id);
                if (productDTO == null)
                    return NotFound();

                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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
            try
            {
                var deleted = _productService.Delete(id);
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