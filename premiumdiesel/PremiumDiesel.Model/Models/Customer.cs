using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumDiesel.Model.Models
{
    public class Customer : BaseModel
    {
        #region Public Properties

        [Required]
        [Display(Name = "Name or Company")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Member #")]
        [MaxLength(50)]
        [Index("MemberNumberIndex", IsUnique = true)]
        public string MemberNumber { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        public ICollection<CustomerLocation> Locations { get; set; }

        #endregion Public Properties
    }
}