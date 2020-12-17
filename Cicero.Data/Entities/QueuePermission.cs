using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class QueuePermission
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("QueueForForm")]
        public int QueueForFormId { get; set; }

        public string DisplayName { get; set; }

        [ForeignKey("RoleForState")]
        public string RoleId { get; set; }

        public virtual QueueForForm QueueForForm { get; set; }

        public virtual ApplicationRole RoleForQueue { get; set; }
    }
}
