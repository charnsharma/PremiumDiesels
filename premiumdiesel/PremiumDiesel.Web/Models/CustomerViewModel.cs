using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Web.Models
{
    public class CustomerViewModel
    {
        public CustomerDTO CustomerDTO { get; set; }

        public CustomerLocationDTO HeadOfficeLocationDTO { get; set; }

        public CustomerViewModel()
        {
            CustomerDTO = new CustomerDTO();
            HeadOfficeLocationDTO = new CustomerLocationDTO();
        }
    }
}