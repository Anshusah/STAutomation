using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ActionName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string SystemName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Color { get; set; }

        public bool NotifyUser { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool NeedReason { get; set; }

        public bool UserAccess { get; set; }

        public int TenantId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Icon { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [InverseProperty("FromState")]
        public virtual ICollection<StateToState> FromStates { get; set; }

        [InverseProperty("ToState")]
        public virtual ICollection<StateToState> ToStates { get; set; }


        public virtual ICollection<QueueToState> QueueToState { get; set; }

        public virtual ICollection<StateForForm> StateForForm { get; set; }

    }
}