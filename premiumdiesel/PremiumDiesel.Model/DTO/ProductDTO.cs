using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PremiumDiesel.Model.DTOs
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public virtual IEnumerable<WorkOrderDTO> WorkOrders { get; set; }
    }
}