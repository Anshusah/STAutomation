using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class AuditLog
    {
        [Key]
        public long Id { get; set; }

        public string Object { get; set; }

        public string ObjectId { get; set; }

        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

        public string User { get; set; }

        public int? TenantId { get; set; }

        public int OperationType { get; set; }

        public string Description { get; set; }

        public string ParentId { get; set; }

        public string ParentObject { get; set; }

        public bool? IsManual { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime DeletedTimeStamp { get; set; }
    }
}
