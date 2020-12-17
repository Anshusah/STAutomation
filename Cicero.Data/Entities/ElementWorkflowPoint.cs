using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class ElementWorkflowPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        [ForeignKey("FirstElementWorkflowObject")]
        public string FWObjectId { get; set; }

        [ForeignKey("LastElementWorkflowObject")]
        public string LWObjectId { get; set; }

        public int TenantId { get; set; }

        public string Type { get; set; }

        public string ElementId { get; set; }

        public int EventType { get; set; }

        public virtual ElementWorkflowObject FirstElementWorkflowObject { get; set; }
        public virtual ElementWorkflowObject LastElementWorkflowObject { get; set; }
    }
}
