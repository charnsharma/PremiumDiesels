namespace PremiumDiesel.Web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PremiumDiesel.Model.Constants;
    using PremiumDiesel.Model.Context;
    using PremiumDiesel.Model.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private string admin1Username;
        private string admin2Username;
        private string user1Username;
        private string user2Username;

        private ApplicationUser admin1User;
        private ApplicationUser admin2User;
        private ApplicationUser user1User;
        private ApplicationUser user2User;

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        /// <summary>
        /// CustomerId, LocationId
        /// </summary>
        private Dictionary<int, int> _customerHeadOffice;

        /// <summary>
        /// ClientId, CustomerId
        /// </summary>
        private List<List<int>> _clientCustomers;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
#if DEBUG
            AutomaticMigrationDataLossAllowed = true;
#endif
            admin1Username = "admin1@coop.abc";
            admin2Username = "admin2@coop.abc";
            user1Username = "user1@coop.abc";
            user2Username = "user2@coop.abc";

            _customerHeadOffice = new Dictionary<int, int>();
            _clientCustomers = new List<List<int>>();
        }

        protected override void Seed(ApplicationDbContext context)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // This method will be called after migrating to the latest version.

            // You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating
            // duplicate seed data. E.g.
            //
            // context.People.AddOrUpdate( p => p.FullName, new Person { FullName = "Andrew Peters"
            // }, new Person { FullName = "Brice Lambson" }, new Person { FullName = "Rowan Miller" } );

            SeedRoles(context);
            SeedUsers(context);

            admin1User = userManager.FindByName(admin1Username);
            admin2User = userManager.FindByName(admin2Username);
            user1User = userManager.FindByName(user1Username);
            user2User = userManager.FindByName(user2Username);

            SeedClients(context, "Coop Vauxhall", "Vauxhall");
            SeedClients(context, "Coop Medicine Hat", "Medicine Hat");

            SeedClientUsers(context, 1, admin1User.Id, admin1User.Id);
            SeedClientUsers(context, 1, user1User.Id, admin1User.Id);
            SeedClientUsers(context, 2, admin2User.Id, admin2User.Id);
            SeedClientUsers(context, 2, user2User.Id, admin2User.Id);

            SeedCustomers(context, admin1User.Id);

            SeedProducts(context, 1, admin1User.Id);
            SeedProducts(context, 2, admin2User.Id);

            SeedWorkOrders(context, 1, user1User.Id, admin1User.Id);
            SeedWorkOrders(context, 2, user2User.Id, admin2User.Id);
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            if (!roleManager.RoleExists(UserRoles.SuperAdmin))
                roleManager.Create(new IdentityRole(UserRoles.SuperAdmin));
            if (!roleManager.RoleExists(UserRoles.Admin))
                roleManager.Create(new IdentityRole(UserRoles.Admin));
            if (!roleManager.RoleExists(UserRoles.User))
                roleManager.Create(new IdentityRole(UserRoles.User));
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var userNamesAndRoles = new Dictionary<string, string>
            {
                { "superadmin@coop.abc", UserRoles.SuperAdmin },
                { "admin1@coop.abc", UserRoles.Admin },
                { "user1@coop.abc", UserRoles.User },
                { "admin2@coop.abc", UserRoles.Admin },
                { "user2@coop.abc", UserRoles.User }
            };
            string password = "Abc123!!!";

            foreach (var userNameAndRole in userNamesAndRoles)
            {
                var user = userManager.FindByName(userNameAndRole.Key);

                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = userNameAndRole.Key,
                        Email = userNameAndRole.Key,
                        EmailConfirmed = true,
                    };

                    IdentityResult userResult = userManager.Create(user, password);

                    if (userResult.Succeeded)
                    {
                        var result = userManager.AddToRole(user.Id, userNameAndRole.Value);
                    }
                }
            }
        }

        private void SeedClients(ApplicationDbContext context, string name, string city)
        {
            var client = new Client()
            {
                Name = name,
                City = city,
                Country = "Canada",
                Province = "AB",
                CreatedByUserId = admin1User.Id,
                CreatedDate = DateTime.UtcNow,
                Status = Status.Active,
            };

            var clientHistory = new ClientHistory()
            {
                Name = name,
                City = city,
                Country = "Canada",
                Province = "AB",
                ModifiedByUserId = admin1User.Id,
                ModifiedDate = client.CreatedDate,
                Status = Status.Active,
            };

            //if (!context.Clients.Any(c => c.Id == 1))
            context.Clients.AddOrUpdate(client);
            context.ClientsHistory.AddOrUpdate(clientHistory);
            context.SaveChanges();
        }

        private void SeedClientUsers(ApplicationDbContext context, int clientId, string userId, string createdByUserId)
        {
            var clientUser = new ClientUser()
            {
                ClientId = clientId,
                UserId = userId,
                CreatedByUserId = createdByUserId,
                CreatedDate = DateTime.UtcNow,
                Status = Status.Active,
            };
            context.ClientUsers.AddOrUpdate(clientUser);

            var clientUserHistory = new ClientUserHistory()
            {
                ClientId = clientId,
                UserId = userId,
                ModifiedByUserId = createdByUserId,
                ModifiedDate = DateTime.UtcNow,
                Status = Status.Active,
            };
            context.ClientUsersHistory.AddOrUpdate(clientUserHistory);

            context.SaveChanges();
        }

        private void SeedCustomers(ApplicationDbContext context, string createdByUserId)
        {
            _clientCustomers.Add(new List<int>()); //empty, just to have 1 based indexes
            _clientCustomers.Add(new List<int>()); //clientId 1
            _clientCustomers.Add(new List<int>()); //clientId 2

            for (int i = 0; i < 10; i++)
            {
                var customer = new Customer()
                {
                    Name = $"Customer {i}",
                    MemberNumber = $"ABC-123-{i}",
                    CreatedByUserId = createdByUserId,
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                };
                context.Customers.AddOrUpdate(customer);
                context.SaveChanges();

                var customerHistory = new CustomerHistory()
                {
                    CustomerId = customer.Id,
                    Name = $"Customer {i}",
                    MemberNumber = $"ABC-123-{i}",
                    ModifiedByUserId = createdByUserId,
                    ModifiedDate = customer.CreatedDate,
                    Status = Status.Active,
                };
                context.CustomersHistory.AddOrUpdate(customerHistory);

                var customerLocation = new CustomerLocation()
                {
                    CustomerId = customer.Id,
                    Name = "Head Office Location " + customer.Id,
                    Type = LocationType.HeadOffice,
                    CreatedByUserId = createdByUserId,
                    CreatedDate = customer.CreatedDate,
                    Province = "AB",
                    Country = "Canada",
                    Status = Status.Active,
                };
                context.CustomerLocations.AddOrUpdate(customerLocation);
                context.SaveChanges();
                _customerHeadOffice.Add(customer.Id, customerLocation.Id);

                var customerLocationHistory = new CustomerLocationHistory()
                {
                    CustomerLocationId = customerLocation.Id,
                    CustomerId = customer.Id,
                    Name = customerLocation.Name,
                    Type = LocationType.HeadOffice,
                    ModifiedByUserId = createdByUserId,
                    ModifiedDate = customer.CreatedDate,
                    Province = "AB",
                    Country = "Canada",
                    Status = Status.Active,
                };
                context.CustomerLocationsHistory.AddOrUpdate(customerLocationHistory);

                Random rnd = new Random();
                var clientCustomer = new ClientCustomer()
                {
                    ClientId = rnd.Next(1, 3),
                    CustomerId = customer.Id,
                    CreatedByUserId = createdByUserId,
                    CreatedDate = customer.CreatedDate,
                    Status = Status.Active,
                };
                context.ClientCustomers.AddOrUpdate(clientCustomer);

                var clientCustomerHistory = new ClientCustomerHistory()
                {
                    ClientId = rnd.Next(1, 3),
                    CustomerId = customer.Id,
                    ModifiedByUserId = createdByUserId,
                    ModifiedDate = customer.CreatedDate,
                    Status = Status.Active,
                };
                context.ClientCustomersHistory.AddOrUpdate(clientCustomerHistory);

                context.SaveChanges();

                _clientCustomers[clientCustomer.ClientId].Add(customer.Id);
            }
        }

        private void SeedProducts(ApplicationDbContext context, int clientId, string createdByUserId)
        {
            var productNames = new List<string>
            {
                "Dyed Diesel",
                "Dyed Gas",
                "Clear Diesel",
                "Clear Gas"
            };

            foreach (var name in productNames)
            {
                var product = new Product()
                {
                    Name = name,
                    ClientId = clientId,
                    CreatedByUserId = createdByUserId,
                    CreatedDate = DateTime.UtcNow,
                    Status = Status.Active,
                    Notes = string.Empty
                };
                context.Products.AddOrUpdate(product);
                // have to call it here to get the new productId
                context.SaveChanges();

                var productHistory = new ProductHistory()
                {
                    ProductId = product.Id,
                    Name = name,
                    ClientId = clientId,
                    ModifiedByUserId = createdByUserId,
                    ModifiedDate = product.CreatedDate,
                    Status = Status.Active,
                    Notes = string.Empty
                };

                context.ProductsHistory.AddOrUpdate(productHistory);
                context.SaveChanges();
            }
        }

        private void SeedWorkOrders(ApplicationDbContext context, int clientId, string userId, string createdByUserId)
        {
            for (int i = 0; i < 20; i++)
            {
                int headOfficeLocationId;
                Random rnd = new Random();
                var randomCustomerId = _clientCustomers[clientId].OrderBy(x => rnd.Next()).Take(1).FirstOrDefault();
                _customerHeadOffice.TryGetValue(randomCustomerId, out headOfficeLocationId);

                var workOrder = new WorkOrder()
                {
                    ClientId = clientId,
                    CustomerId = randomCustomerId,
                    LocationId = headOfficeLocationId,
                    ProductId = rnd.Next(1, 5),
                    Quantity = $"{rnd.Next(1, 500)} L.",
                    CreatedByUserId = createdByUserId,
                    CreatedDate = DateTime.UtcNow,
                    AssignedToUserId = userId,
                    DueDate = DateTime.UtcNow.AddDays(rnd.Next(1, 31)),
                    Notes = $"Work order # {i}",
                    Status = Status.Active,
                };

                context.WorkOrders.AddOrUpdate(workOrder);
                context.SaveChanges();

                var workOrderHistory = new WorkOrderHistory()
                {
                    WorkOrderId = workOrder.Id,
                    ClientId = clientId,
                    CustomerId = randomCustomerId,
                    LocationId = headOfficeLocationId,
                    ProductId = rnd.Next(1, 5),
                    Quantity = $"{rnd.Next(1, 500)} L.",
                    ModifiedByUserId = createdByUserId,
                    ModifiedDate = DateTime.UtcNow,
                    AssignedToUserId = userId,
                    DueDate = DateTime.UtcNow.AddDays(rnd.Next(1, 31)),
                    Status = Status.Active,
                };

                context.WorkOrdersHistory.AddOrUpdate(workOrderHistory);
                context.SaveChanges();
            }
        }
    }
}