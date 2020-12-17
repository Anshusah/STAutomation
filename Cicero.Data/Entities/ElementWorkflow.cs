using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class ElementWorkflow
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        public string ElementId { get; set; }
        
        public int EventType { get; set; } 

        public int TenantId { get; set; }

        public string JsonData { get; set; }

    }
}
