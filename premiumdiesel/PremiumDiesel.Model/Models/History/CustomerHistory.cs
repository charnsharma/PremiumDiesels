using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.Models
{
    [Table("CustomersHistory")]
    public class CustomerHistory : BaseModelHistory
    {
        #region Public Properties

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string MemberNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        #endregion Public Properties
    }
}