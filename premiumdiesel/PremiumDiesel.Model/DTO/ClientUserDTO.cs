using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.DTOs
{
    public class ClientUserDTO
    {
        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual ClientDTO Client { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}