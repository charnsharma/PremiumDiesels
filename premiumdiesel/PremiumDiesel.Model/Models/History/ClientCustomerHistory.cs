using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.Models
{
    [Table("ClientCustomersHistory")]
    public class ClientCustomerHistory : BaseModelHistory
    {
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}