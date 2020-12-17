using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class Transaction
    {   
        public int CaseId { get; set; }

        [Key]
        public int TransactionId { get; set; }
        public int SupplierId { get; set; }
        public int BankId { get; set; }
        public int BankBranchId { get; set; }

        public Guid UserId { get; set; }
        public int BeneficiaryId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SendAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayoutAmount { get; set; }
        public int PaymentMethodId { get; set; }        
        public int PayoutModeId { get; set; }        
        public int StateId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TransferFee { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GST { get; set; }
        public int SendCountryId { get; set; }
        public int PayoutCountryId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExchangeRate { get; set; }
        public bool IsFreePayment { get; set; }

        public int PaymentPurpose { get; set; }
        public int SourceOfFund { get; set; }
        [Column(TypeName = "varchar(500)")]

        public string Remark { get; set; }
        [Column(TypeName = "varchar(50)")]

        public string TransactionRefNo { get; set; }
        [Column(TypeName = "varchar(50)")]

        public string TransactionBookingNo { get; set; }
        [Column(TypeName = "varchar(50)")]

        public string SupplierTxnRefNo { get; set; }
        public string SupplierTxnStatus { get; set; }

        public string SecureTradingReferenceNo { get; set; }

        public int Status { get; set; }
        public int TrasactionType { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }

    }  
}
