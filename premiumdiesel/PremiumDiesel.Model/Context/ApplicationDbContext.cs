using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Model.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientHistory> ClientsHistory { get; set; }

        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<ClientUserHistory> ClientUsersHistory { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerHistory> CustomersHistory { get; set; }

        public DbSet<ClientCustomer> ClientCustomers { get; set; }
        public DbSet<ClientCustomerHistory> ClientCustomersHistory { get; set; }

        public DbSet<CustomerLocation> CustomerLocations { get; set; }
        public DbSet<CustomerLocationHistory> CustomerLocationsHistory { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductHistory> ProductsHistory { get; set; }

        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderHistory> WorkOrdersHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<CustomerLocation>()
                .HasRequired(c => c.Customer)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasRequired(c => c.Client)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WorkOrder>()
                .HasRequired(c => c.Location)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}