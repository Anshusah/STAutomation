using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class WorkflowObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }
        
        public string Type { get; set; }
        
        public int TenantId { get; set; }

        public virtual CaseForm CaseForm { get; set; }

    }
}
