using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Details { get; set; }

        public int? ClaimId { get; set; }

        public int? StateId { get; set; }

        public string DisplayTo { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("ClaimId")]
        public virtual Case Case { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }
}
