using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PremiumDiesel.Model.Context;

namespace PremiumDiesel.Model.Models
{
    public class BaseModelHistory
    {
        [Key]
        public int Id { get; set; }

        #region Date-User-Status

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(128)]
        public string ModifiedByUserId { get; set; }

        [ForeignKey("ModifiedByUserId")]
        public virtual ApplicationUser ModifedByUser { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }

        #endregion Date-User-Status
    }
}