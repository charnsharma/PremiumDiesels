using System.Net;
using System.Web.Mvc;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.Products;

namespace PremiumDiesel.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IClientUserService _clientUserService;

        public ProductsController(IProductService productService, IClientUserService clientUserService)
        {
            _productService = productService;
            _clientUserService = clientUserService;
        }

        // GET: Products
        public ActionResult Index()
        {
            // Get the list of their products
            var productDTOs = _productService.GetProducts();

            return View(productDTOs);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var productDTO = _productService.Get((int)id);

            return View(productDTO);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                _productService.Create(productDTO);

                return RedirectToAction("Index");
            }

            return View(productDTO);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var productDTO = _productService.Get((int)id);

            return View(productDTO);
        }

        // POST: Products/Edit/5 To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                _productService.Update(productDTO.Id, productDTO);

                return RedirectToAction("Index");
            }
            return View(productDTO);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var productDTO = _productService.Get((int)id);

            return View(productDTO);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _productService.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}