using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Cicero.Data.Entities
{
    public class CoreCaseTable
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        public string Fields { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }

        public bool Status { get; set; }

        public int? TenantId { get; set; }

        public int? CaseFormId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("CaseFormId")]
        public virtual CaseForm CaseForm { get; set; }

    }
}
