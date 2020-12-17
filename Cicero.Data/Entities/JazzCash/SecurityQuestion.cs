using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.JazzCash
{
    public class SecurityQuestion
    {
        [Key]
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Question { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; }
    }
}
