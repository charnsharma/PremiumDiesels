using System.Collections.Generic;
using PremiumDiesel.Model.DTOs;

namespace PremiumDiesel.Web.Models
{
    public class CustomerAndLocationsViewModel
    {
        public CustomerDTO CustomerDTO { get; set; }

        public IEnumerable<CustomerLocationDTO> LocationDTOs { get; set; }
    }
}