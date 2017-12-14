using System.Web.Http;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using PremiumDiesel.Service;
using PremiumDiesel.Service.ClientCustomers;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Service.Products;
using PremiumDiesel.Service.WorkOrders;

namespace PremiumDiesel.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var settins = config.Formatters.JsonFormatter.SerializerSettings;
            settins.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settins.Formatting = Newtonsoft.Json.Formatting.Indented;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionBased",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();

            container.RegisterType<IMainService, MainService>(new HierarchicalLifetimeManager());
            container.RegisterType<IClientCustomerService, ClientCustomerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IClientUserService, ClientUserService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerLocationService, CustomerLocationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerService, CustomerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new HierarchicalLifetimeManager());
            container.RegisterType<IWorkOrderService, WorkOrderService>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new ApiDependencyResolver(container);
        }
    }
}