using System.Collections.Generic;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Service.Products;

namespace PremiumDiesel.Service
{
    public class MainService : IMainService
    {
        private ICustomerService _customerService;
        private ICustomerLocationService _customerLocationService;
        private IProductService _productService;
        private IClientUserService _clientUserService;

        public MainService(
            ICustomerService customerService,
            ICustomerLocationService customerLocationService,
            IProductService productService,
            IClientUserService clientUserService)
        {
            _customerService = customerService;
            _customerLocationService = customerLocationService;
            _productService = productService;
            _clientUserService = clientUserService;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetCustomerDropdownItems()
        {
            var dictionary = new Dictionary<int, string>();
            var customers = _customerService.GetCustomers();

            foreach (var customer in customers)
            {
                dictionary.Add(customer.Id, customer.Name);
            }

            return dictionary;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetCountryDropdownItems()
        {
            return new Dictionary<string, string> {
                {"Canada", "Canada"}
            };
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLocationTypeDropdownItems()
        {
            return new Dictionary<string, string> {
                {"HO", "Head Office"},
                {"BR", "Branch"}
            };
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetProvinceDropdownItems()
        {
            return new Dictionary<string, string> {
                {"AB", "Alberta"},
                {"BC", "British Columbia"},
                {"MB", "Manitoba"},
                {"NB", "New Brunswick"},
                {"NL", "Newfoundland and Labrador"},
                {"NS", "Nova Scotia"},
                {"ON", "Ontario"},
                {"PE", "Prince Edward Island"},
                {"QC", "Quebec"},
                {"SK", "Saskatchewan"},
                {"NT", "Northwest Territories"},
                {"NU", "Nunavut"},
                {"YT", "Yukon"},
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetLocationDropdownItems(int customerId)
        {
            var dictionary = new Dictionary<int, string>();
            // get customer's locations
            var customerLocations = _customerLocationService.GetCustomerLocationsByCustomerId(customerId);

            foreach (var customerLocation in customerLocations)
            {
                dictionary.Add(customerLocation.Id, customerLocation.Name);
            }

            return dictionary;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetProductDropdownItems()
        {
            var dictionary = new Dictionary<int, string>();
            var products = _productService.GetProducts();

            foreach (var product in products)
            {
                dictionary.Add(product.Id, product.Name);
            }

            return dictionary;
        }

        public Dictionary<string, string> GetUserDropdownItems()
        {
            var dictionary = new Dictionary<string, string>();
            var users = _clientUserService.GetUsers();

            foreach (var user in users)
            {
                dictionary.Add(user.UserId, user.User.UserName);
            }

            return dictionary;
        }
    }
}