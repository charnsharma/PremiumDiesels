using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.DTOs
{
    public class WorkOrderDTO
    {
        #region Public Properties

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual CustomerDTO Customer { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual CustomerLocationDTO Location { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductDTO Product { get; set; }

        [Required]
        public string Quantity { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "A work order must be assigned to someone. You can assign it to yourself for now")]
        [Display(Name = "Assigned To")]
        public string AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual ApplicationUser AssignedToUser { get; set; }

        #endregion Public Properties
    }
}