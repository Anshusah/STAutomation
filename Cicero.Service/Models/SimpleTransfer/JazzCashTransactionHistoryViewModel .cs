using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class JazzCashTransactionHistoryViewModel
    {
        [Key]
        public int JazzCashTransactionHistoryId { get; set; }

        public string TransactionRefNo { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string SupplierTxnRefNo { get; set; }

        public int Status { get; set; }

        public string SupplierTxnStatus { get; set; }

        public string Remark { get; set; }

        public string RemarkBy { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }
    }
}
