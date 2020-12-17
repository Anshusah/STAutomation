using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class WorkflowPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }
        
        [ForeignKey("FirstWorkflowObject")]
        public string FWObjectId { get; set; }

        [ForeignKey("LastWorkflowObject")]
        public string LWObjectId { get; set; }

        public int TenantId { get; set; }

        public string Type { get; set; }

        public virtual CaseForm CaseForm { get; set; }

        public virtual WorkflowObject FirstWorkflowObject { get; set; }
        public virtual WorkflowObject LastWorkflowObject { get; set; }
    }
}
