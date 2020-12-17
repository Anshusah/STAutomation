using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class ElementComponent
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FieldKey { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string FieldValue { get; set; }

        public int? FormId { get; set; }

        public string ElementId { get; set; }

        public int? EventType { get; set; }
       
        public int? TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
    }
}