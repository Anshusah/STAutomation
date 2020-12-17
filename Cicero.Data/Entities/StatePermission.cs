using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class StatePermission
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("StateForForm")]
        public int StateForFormId { get; set; }

        public string DisplayName { get; set; }

        [ForeignKey("RoleForState")]
        public string RoleId { get; set; }

        public bool CanEdit { get; set; }

        public bool ViewMode { get; set; }

        public virtual StateForForm StateForForm { get; set; }

        public virtual ApplicationRole RoleForState { get; set; }
    }
}
