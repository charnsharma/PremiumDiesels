using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Service;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Web.Models;

namespace PremiumDiesel.Web.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IMainService _mainService;

        private ICustomerService _customerService;

        private ICustomerLocationService _customerLocationService;

        public CustomersController(IMainService mainService, ICustomerService customerService, ICustomerLocationService customerLocationService)
        {
            _mainService = mainService;
            _customerService = customerService;
            _customerLocationService = customerLocationService;
        }

        #region Show

        // GET: Customers
        public ActionResult Index()
        {
            var customers = _customerService.GetCustomers();
            var customersDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerDTO>>(customers);
            var customersViewModel = new List<CustomerViewModel>();

            foreach (var customerDTO in customersDTOs)
            {
                var customerLocation = _customerLocationService.GetHeadOffice(customerDTO.Id);
                var customerLocationDTO = AutoMapper.Mapper.Map<CustomerLocationDTO>(customerLocation);

                customersViewModel.Add(new CustomerViewModel()
                {
                    // Pass the customer and the head office location
                    CustomerDTO = customerDTO,
                    HeadOfficeLocationDTO = customerLocationDTO
                });
            }

            return View(customersViewModel);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Get the cusomer
            var customerDTO = _customerService.Get((int)id);

            if (customerDTO == null)
                return HttpNotFound();

            // Get the customer locations
            var customerLocationDTOs = _customerLocationService.GetCustomerLocationsByCustomerId((int)id);

            // Build the view model
            var customerAndLocationsViewModel = new CustomerAndLocationsViewModel()
            {
                // Pass the customer + the list of their locations
                CustomerDTO = AutoMapper.Mapper.Map<CustomerDTO>(customerDTO),
                LocationDTOs = AutoMapper.Mapper.Map<IEnumerable<CustomerLocationDTO>>(customerLocationDTOs),
            };

            return View(customerAndLocationsViewModel);
        }

        #endregion Show

        #region Create

        // GET: Customers/Create
        public ActionResult Create()
        {
            var customerViewModel = new CustomerViewModel();
            customerViewModel.HeadOfficeLocationDTO.Type = LocationType.HeadOffice;
            customerViewModel.HeadOfficeLocationDTO.Country = Country.Canada;

            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();

            return View(customerViewModel);
        }

        // POST: Customers/Create To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Separate customer and head office location
                var newCustomerDTO = _customerService.Create(customerViewModel.CustomerDTO);
                customerViewModel.HeadOfficeLocationDTO.CustomerId = newCustomerDTO.Id;
                _customerLocationService.Create(customerViewModel.HeadOfficeLocationDTO);

                return RedirectToAction("Index");
            }

            ViewBag.CountryList = _mainService.GetCountryDropdownItems();
            ViewBag.ProvinceList = _mainService.GetProvinceDropdownItems();
            ViewBag.LocationTypeList = _mainService.GetLocationTypeDropdownItems();

            return View(customerViewModel);
        }

        #endregion Create

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customerDTO = _customerService.Get((int)id);
            if (customerDTO == null)
                return HttpNotFound();

            return View(customerDTO);
        }

        // POST: Customers/Edit/5 To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                _customerService.Update(customerDTO.Id, customerDTO);

                return RedirectToAction("Index");
            }

            return View(customerDTO);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customerDTO = _customerService.Get((int)id);
            if (customerDTO == null)
                return HttpNotFound();

            return View(customerDTO);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _customerService.Delete(id);

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