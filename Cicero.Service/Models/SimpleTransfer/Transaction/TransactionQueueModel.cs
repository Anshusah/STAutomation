using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Transaction
{
    public class TransactionQueueModel
    {
        public int TodaysPaymentRequest { get; set; }
        public int ExpiredPaymentRequest { get; set; }
        public int TodaysTRX { get; set; }
        public int UnsettledTRX { get; set; }
        public int PaymentHeldTRX { get; set; }
        public int ComplianceHeldTRX { get; set; }
        public int UnauthorisedTRX { get; set; }
        public int RejectedTRXForLimitBreach { get; set; }
        public int CancelledTRXPendingRefundCardPayment { get; set; }
        public int CancelledTRXPendingRefundBankTransfer { get; set; }
    }
}
