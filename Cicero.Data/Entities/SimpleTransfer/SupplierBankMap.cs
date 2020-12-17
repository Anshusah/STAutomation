using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SupplierBankMap
    {   
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string CountryCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string TransfastBankCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string NecMoneyBankCode { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }

    }  
}
