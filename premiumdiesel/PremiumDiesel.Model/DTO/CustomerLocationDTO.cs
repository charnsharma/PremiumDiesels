using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.DTOs
{
    public class CustomerLocationDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual CustomerDTO Customer { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2), StringLength(2)]
        public string Type { get; set; }

        [Display(Name = "Address")]
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