using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Cicero.Data.Entities
{
    public class Actions
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
        public string Type { get; set; }

        public bool Status { get; set; }

        public string UrlIdentifier { get; set; }

        public string ActionType { get; set; }

        public int TemplateId { get; set; }

        public int TenantId { get; set; } 

        public int CaseFormId { get; set; } 

        public virtual ICollection<ActionsReceiver> ActionsReceiverLst { get; set; }

        public virtual ICollection<ActionsSender> ActionsSenderLst { get; set; }

        public virtual ICollection<StateToState> StateToStatelst { get; set; }
    }
}
