using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class CaseForm
    {
        [Key]
        public int Id { get; set; }
        public string TabEnable { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ModelName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string UrlIdentifier { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Icon { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ModelTitle { get; set; }

        public string Fields { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }

        public bool Status { get; set; }

        public int? TenantId { get; set; }

        public int? Default { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<Case> Cases { get; set; }

        public virtual ICollection<PolicyManagement> PolicyManagement { get; set; }

    }
}
