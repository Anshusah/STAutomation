using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class PayoutModeConfig
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string PayoutModeName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string CreatedBy { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }       

    }
}
