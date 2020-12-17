using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class CustomEntity
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }
        public string Extras { get; set; }

        public string UserId { get; set; }

        //public int StateId { get; set; }

        public int TenantId { get; set; }

        public int CoreCaseTableId { get; set; }

        public int Order { get; set; }
        public string CaseId { get; set; }

        public string JsonExtras { get; set; }
         
         

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
 

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("CoreCaseTableId")]
        public virtual CaseForm CoreCaseTable { get;}

          
    }
}
