using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class ProductConfig
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string ProductName { get; set; }
        public string KeyId { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
    }
}
