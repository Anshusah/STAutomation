using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class QueueForForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Queue")]
        public int QueueId { get; set; }

        [ForeignKey("CaseForm")]
        public int CaseFormId { get; set; }

        public int Order { get; set; }

        public bool AllUser { get; set; }

        public virtual ICollection<QueuePermission> QueuePermissions { get; set; }

        public virtual CaseForm CaseForm { get; set; }

        public virtual Queue Queue { get; set; }
    }
}
