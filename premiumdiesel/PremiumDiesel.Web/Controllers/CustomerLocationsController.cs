using System.Net;
using System.Web.Mvc;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Service;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;

namespace PremiumDiesel.Web.Controllers
{
    public class CustomerLocationsController : Controller
    {
        private IMainService _mainService;
        private ICustomerLocationService _customerLocationsService;
        private ICustomerService _customerService;

        public CustomerLocationsController(IMainService mainService, ICustomerLocationService customerLocationSService, ICustomerService customerService)
        {
            _mainService = mainService;
            _customerLocationsService = customerLocationSService;
            _customerService = customerService;
        }

        // GET: CustomerLocations
        public ActionResult Index()
        {
            // Get the list of their products
            var customerLocationsDTOs = _customerLocationsService.GetAllMyCustomerLocations();

            return View(customerLocationsDTOs);
        }

        // GET: CustomerLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customerLocationDTO = _customerLocationsService.Get((int)id);
            if (customerLocationDTO == null)
                return HttpNotFound();

            return View(customerLocationDTO);
        }

        // GET: CustomerLocations/Create
        public ActionResult Create(int? customerId)
        {
            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();
            ViewBag.CustomersList = _mainService.GetCustomerDropdownItems();

            if (customerId.HasValue)
            {
                var customerLocationDTO = new CustomerLocationDTO()
                {
                    CustomerId = (int)customerId,
                };
                return View(customerLocationDTO);
            }

            return View();
        }

        // POST: CustomerLocations/Create To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerLocationDTO customerLocationDTO)
        {
            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();
            ViewBag.CustomersList = _mainService.GetCustomerDropdownItems();

            if (ModelState.IsValid)
            {
                if (customerLocationDTO.Type == LocationType.HeadOffice)
                {
                    if (_customerLocationsService.HasHeadOffice((int)customerLocationDTO.CustomerId))
                    {
                        ModelState.AddModelError("Type", "This customer already has a head office location");
                        return View(customerLocationDTO);
                    }
                }
                _customerLocationsService.Create(customerLocationDTO);

                return RedirectToAction("Index");
            }

            return View(customerLocationDTO);
        }

        // GET: CustomerLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customerLocationDTO = _customerLocationsService.Get((int)id);

            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();

            return View(customerLocationDTO);
        }

        // POST: CustomerLocations/Edit/5 To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerLocationDTO customerLocationDTO)
        {
            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();

            if (ModelState.IsValid)
            {
                if (customerLocationDTO.Type == LocationType.HeadOffice)
                {
                    var headOffice = _customerLocationsService.GetHeadOffice(customerLocationDTO.CustomerId);
                    if (headOffice != null && headOffice.Id != customerLocationDTO.Id)
                    {
                        ModelState.AddModelError("Type", "This customer already has a head office location");
                        return View(customerLocationDTO);
                    }
                }
                //customerLocationDTO.Customer = _customerService.Get((int)customerLocationDTO.CustomerId);
                _customerLocationsService.Update(customerLocationDTO.Id, customerLocationDTO);

                return RedirectToAction("Index");
            }
            return View(customerLocationDTO);
        }

        // GET: CustomerLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customerLocationDTO = _customerLocationsService.Get((int)id);

            return View(customerLocationDTO);
        }

        // POST: CustomerLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _customerLocationsService.Delete(id);

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