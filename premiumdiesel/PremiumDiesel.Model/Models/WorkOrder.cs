using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.Models
{
    public class WorkOrder : BaseModel
    {
        #region Public Properties

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Required]
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual CustomerLocation Location { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        public string Quantity { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public string AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual ApplicationUser AssignedToUser { get; set; }

        #endregion Public Properties
    }
}