using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class WorkFlowState
    {
        [Key]
        public string Id { get; set; }

        public int FromStateId { get; set; }
        
        public int ToStateId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }
        
        public string BeforeChangeActionsId { get; set; }

        public int TenantId { get; set; }
        
        public string AfterChangeActionsId { get; set; }
        
        public virtual CaseForm CaseForm { get; set; }
       
    }

}
