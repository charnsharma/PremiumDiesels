using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.Models
{
    [Table("CustomerLocationsHistory")]
    public class CustomerLocationHistory : BaseModelHistory
    {
        public int? CustomerLocationId { get; set; }
        [ForeignKey("CustomerLocationId")]
        public CustomerLocation CustomerLocation { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

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
    }
}