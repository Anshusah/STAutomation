using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class TransactionHistory
    {
        [Key]
        public int TransactionHistoryId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string TransactionRefNo { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Column(TypeName = "varchar(50)")]
        public string SupplierTxnRefNo { get; set; }

        public int Status { get; set; }

        public string SupplierTxnStatus { get; set; }

        public string Remark { get; set; }

        public string RemarkBy { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

    }
}
