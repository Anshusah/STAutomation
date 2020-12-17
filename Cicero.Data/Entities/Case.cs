using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class Case
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CaseGeneratedId { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime? AssignedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime? DueDate { get; set; }

        public string AssignedTo { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }

        public int CaseFormId { get; set; }

        public int StateId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("CaseFormId")]
        public virtual CaseForm CaseForm { get;}

    }
}
