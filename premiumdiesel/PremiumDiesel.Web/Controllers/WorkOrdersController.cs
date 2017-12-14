using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Service;
using PremiumDiesel.Service.ClientCustomers;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Service.Products;
using PremiumDiesel.Service.WorkOrders;

namespace PremiumDiesel.Web.Controllers
{
    public class WorkOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IWorkOrderService _workOrderService;
        private IClientUserService _clientUserService;
        private IMainService _mainService;
        private IClientCustomerService _clientCustomerService;
        private IProductService _productService;
        private ICustomerLocationService _customerLocationService;
        private ICustomerService _customerService;

        public WorkOrdersController(
            IWorkOrderService workOrderService,
            IClientUserService clientUserService,
            IMainService mainService,
            IClientCustomerService clientCustomerService,
            IProductService productService,
            ICustomerLocationService customerLocationService,
            ICustomerService customerService
)
        {
            _workOrderService = workOrderService;
            _clientUserService = clientUserService;
            _mainService = mainService;
            _clientCustomerService = clientCustomerService;
            _productService = productService;
            _customerLocationService = customerLocationService;
            _customerService = customerService;
        }

        // GET: WorkOrders
        public ActionResult Index()
        {
            // Get the list of their workOrders
            var workOrderDTOs = _workOrderService.GetWorkOrders();

            return View(workOrderDTOs);
        }

        // GET: WorkOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var workOrderDTO = _workOrderService.Get((int)id);
            if (workOrderDTO == null)
                return HttpNotFound();

            return View(workOrderDTO);
        }

        // GET: WorkOrders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerList = _mainService.GetCustomerDropdownItems();
            ViewBag.LocationList = new Dictionary<int, string>();
            ViewBag.ProductList = _mainService.GetProductDropdownItems();
            ViewBag.UserList = _mainService.GetUserDropdownItems();

            return View();
        }

        // POST: WorkOrders/Create To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkOrderDTO workOrderDTO)
        {
            if (ModelState.IsValid)
            {
                _workOrderService.Create(workOrderDTO);

                return RedirectToAction("Index");
            }

            ViewBag.UserList = _mainService.GetUserDropdownItems();
            ViewBag.CustomerList = _mainService.GetCustomerDropdownItems();
            ViewBag.LocationList = new Dictionary<int, string>();
            ViewBag.ProductList = _mainService.GetProductDropdownItems();

            return View(workOrderDTO);
        }

        // GET: WorkOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var workOrderDTO = _workOrderService.Get((int)id);
            if (workOrderDTO == null)
                return HttpNotFound();

            ViewBag.UserList = _mainService.GetUserDropdownItems();
            ViewBag.CustomerList = _mainService.GetCustomerDropdownItems();
            ViewBag.LocationList = _mainService.GetLocationDropdownItems(workOrderDTO.CustomerId);
            ViewBag.ProductList = _mainService.GetProductDropdownItems();

            return View(workOrderDTO);
        }

        // POST: WorkOrders/Edit/5 To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkOrderDTO workOrderDTO)
        {
            if (ModelState.IsValid)
            {
                //workOrderDTO.Customer = _customerService.Get(workOrderDTO.CustomerId);
                //workOrderDTO.AssignedToUser = MyHelper.UserManager.FindById(workOrderDTO.AssignedToUserId);
                //workOrderDTO.Product = _productService.Get(workOrderDTO.ProductId);
                //workOrderDTO.Location = _customerLocationService.Get(workOrderDTO.LocationId);

                var savedWorkOrderDTO = _workOrderService.Update(workOrderDTO.Id, workOrderDTO);
                if (savedWorkOrderDTO == null)
                    return HttpNotFound();

                return RedirectToAction("Index");
            }

            ViewBag.UserList = _mainService.GetUserDropdownItems();
            ViewBag.CustomerList = _mainService.GetCustomerDropdownItems();
            ViewBag.LocationList = _mainService.GetLocationDropdownItems(workOrderDTO.CustomerId);
            ViewBag.ProductList = _mainService.GetProductDropdownItems();

            return View(workOrderDTO);
        }

        // GET: WorkOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var workOrderDTO = _workOrderService.Get((int)id);
            if (workOrderDTO == null)
                return HttpNotFound();

            return View(workOrderDTO);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _workOrderService.Delete(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}