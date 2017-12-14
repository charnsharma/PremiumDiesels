using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.Models
{
    [Table("ClientsHistory")]
    public class ClientHistory : BaseModelHistory
    {
        #region Public Properties

        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required]
        [Display(Name = "Name or Company")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Phone #")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [Display(Name = "Province")]
        [MaxLength(50)]
        public string Province { get; set; }

        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        [MaxLength(6)]
        [StringLength(6, ErrorMessage = "Postal Code must be of 6 characters")]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        #endregion Public Properties
    }
}