using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using PremiumDiesel.Service;
using PremiumDiesel.Service.ClientCustomers;
using PremiumDiesel.Service.ClientUsers;
using PremiumDiesel.Service.CustomerLocations;
using PremiumDiesel.Service.Customers;
using PremiumDiesel.Service.Products;
using PremiumDiesel.Service.WorkOrders;

[assembly: OwinStartupAttribute(typeof(PremiumDiesel.Web.Startup))]

namespace PremiumDiesel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            IUnityContainer unityContainer = new UnityContainer();
            ConfigureServices(unityContainer);
            DependencyResolver.SetResolver(new WebDependencyResolver(unityContainer));
        }

        private void ConfigureServices(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IMainService, MainService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IClientUserService, ClientUserService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<ICustomerService, CustomerService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<ICustomerLocationService, CustomerLocationService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IClientCustomerService, ClientCustomerService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IProductService, ProductService>(new HierarchicalLifetimeManager());
            unityContainer.RegisterType<IWorkOrderService, WorkOrderService>(new HierarchicalLifetimeManager());
        }

        //private void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddTransient<IProductService, ProductService>();
        //    services.AddTransient<IClientUserService, ClientUserService>();
        //}
    }
}