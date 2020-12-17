using Cicero.Service.Models.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Cicero.Service.Extensions.Extensions;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class JazzCashTransactionMgmtViewModel
    {
        public JazzCashTransactionMgmtViewModel() { }
        [Key]
        public int JazzCashTransactionId { get; set; }
        public int PaymentRequestId { get; set; }
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

        public int Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        public TransactionSearchCriteria TransactionSearchCriterias { get; set; }
        public string Provider { get; set; }
        public string SenderName { get; set; }
        public string SendCurrency { get; set; }
        public string ReceiverCurrency { get; set; }
        public string PaymentStatus { get; set; }
        public string Onfido { get; set; }
        public string Ofac { get; set; }
        public string BlackList { get; set; }
        public string Rules { get; set; }
        public string LexisNexis { get; set; }
        public string SanctionPep { get; set; }
        public string AuthorisationStatus { get; set; }
        public string ProviderStatus { get; set; }
        public string TradeDate { get; set; }
        public string TradeTime { get; set; }
        public string SenderAccount { get; set; }

    }

    public class JazzCashCreateTransaction
    {
        [Required]
        public string PayerId { get; set; }

        [Required]
        public string PaymentReferenceNumber { get; set; }

        [Required]
        public string PayeeFirstName { get; set; }

        [Required]
        public string PayeeLastName { get; set; }

        [Required]
        public string PayeeMobileNumber { get; set; }

        [Required]
        public string PayeeCountryCode { get; set; }

        [Required]
        public int PaymentModeId { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The field {0} must be greater than Zero.")]
        public decimal SentAmount { get; set; }
    }

    public class JazzCashCancelTransaction
    {
        [Required]
        public string ReferenceNo { get; set; }
    }
}
