using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class PolicyManagement
    {
        [Key]
        public int Id { get; set; }

        public string Fields { get; set; }

        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }

        public int CaseFormId { get; set; }

        public int TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("CaseFormId")]
        public virtual CaseForm CaseForm { get; }
    }
}
