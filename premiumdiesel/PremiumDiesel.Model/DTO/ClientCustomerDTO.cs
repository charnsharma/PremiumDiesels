using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Models;

namespace PremiumDiesel.Model.DTOs
{
    public class ClientCustomerDTO
    {
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}