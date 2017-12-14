using System.ComponentModel.DataAnnotations;

namespace PremiumDiesel.Model.DTOs
{
    public class ClientDTO
    {
        #region Public Properties

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Province { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        #endregion Public Properties
    }
}