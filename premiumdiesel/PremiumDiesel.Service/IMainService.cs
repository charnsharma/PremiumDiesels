using System.Collections.Generic;

namespace PremiumDiesel.Service
{
    public interface IMainService
    {
        Dictionary<int, string> GetCustomerDropdownItems();

        Dictionary<string, string> GetCountryDropdownItems();

        Dictionary<string, string> GetProvinceDropdownItems();

        Dictionary<string, string> GetLocationTypeDropdownItems();

        Dictionary<int, string> GetLocationDropdownItems(int customerId);

        Dictionary<int, string> GetProductDropdownItems();

        Dictionary<string, string> GetUserDropdownItems();
    }
}