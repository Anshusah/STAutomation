using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class ElementWorkflowState
    {
        [Key]
        public string Id { get; set; }

        public long FromStateId { get; set; }

        public long ToStateId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        public string ElementId { get; set; }

        public int EventType { get; set; }

        public string BeforeChangeActionsId { get; set; }

        public int TenantId { get; set; }

        public string AfterChangeActionsId { get; set; }

        public virtual CaseForm CaseForm { get; set; }
    }
}
