using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class ElementWorkflowObject
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        public string ElementId { get; set; }

        public string Type { get; set; }

        public int TenantId { get; set; }

        public int EventType { get; set; }



    }
}
