using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.DTOs
{
    [Table("Customers")]
    public class CustomerDTO
    {
        #region Public Properties

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Display(Name = "Member #")]
        public string MemberNumber { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        #endregion Public Properties
    }
}