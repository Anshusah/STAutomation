using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class StateForForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public bool AllUser { get; set; }

        public bool FirstFrontState { get; set; }

        public bool FirstBackState { get; set; }

        public virtual ICollection<StatePermission> StatePermissions { get; set; }

        public virtual CaseForm CaseForm { get; set; }

        public virtual State State { get; set; }
    }
}
