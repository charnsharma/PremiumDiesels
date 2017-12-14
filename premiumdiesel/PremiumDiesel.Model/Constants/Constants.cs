namespace PremiumDiesel.Model.Constants
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
        public const string User = "User";
    }

    public static class Status
    {
        public const string Active = "A";
        public const string Complete = "C";
        public const string Pending = "P";
        public const string Hidden = "H";
        public const string Deleted = "D";
    }

    public static class LocationType
    {
        public const string HeadOffice = "HO";
        public const string Branch = "BR";
    }

    public static class Country
    {
        public const string Canada = "Canada";
    }

    public static class Api
    {
        public struct ClientCustomers
        {
            public const string Delete = "/api/clientcustomers/";
        }

        public struct Customers
        {
            public const string Delete = "/api/customers/";
        }

        public struct CustomerLocations
        {
            public const string Get = "/api/locations/";
            public const string GetByCustomer = "/api/customerlocations/";
            public const string Delete = "/api/locations/";
        }

        public struct Products
        {
            public const string Delete = "/api/products/";
        }

        public struct WorkOrders
        {
            public const string Delete = "/api/workorders/";
        }
    }
}