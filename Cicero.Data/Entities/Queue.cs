using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class Queue
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public bool Status { get; set; }

        public string UrlIdentifier { get; set; }

        public int TenantId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Icon { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Color { get; set; }

        //public int Order { get; set; }

        //public string RoleId { get; set; }

        public int CaseFormId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<QueueToState> QueueToState { get; set; }

        public virtual ICollection<QueueForForm> QueueForForm { get; set; }

    }
}
