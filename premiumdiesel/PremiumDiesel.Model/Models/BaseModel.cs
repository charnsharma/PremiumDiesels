using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        #region Date-User-Status

        public DateTime? CreatedDate { get; set; }

        [MaxLength(128)]
        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }

        #endregion Date-User-Status
    }
}