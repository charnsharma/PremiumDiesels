using System;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.Models
{
    [Table("WorkOrdersHistory")]
    public class WorkOrderHistory : BaseModelHistory
    {
        #region Public Properties

        public int? WorkOrderId { get; set; }
        [ForeignKey("WorkOrderId")]
        public WorkOrder WorkOrder { get; set; }

        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual CustomerLocation Location { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public string Quantity { get; set; }

        // I am not going to log changes to the Notes otherwise the DB size can grow esponentially
        //[MaxLength(255)]
        //public string Notes { get; set; }

        public DateTime? DueDate { get; set; }

        public string AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual ApplicationUser AssignedToUser { get; set; }

        #endregion Public Properties
    }
}