using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cicero.service.models.simpletransfer.transaction
{
    public class TransactionDataViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string tradeDate { get; set; }
        public int transactionId { get; set; }
        public int caseId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string transactionRefNo { get; set; }
        public decimal transferFee { get; set; }
        public decimal sendAmount { get; set; }
        public int sourceOfFund { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string stateId { get; set; }
        public decimal payoutAmount { get; set; }
        public int supplierId { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string sendCurrency { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string senderName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string receiverName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string receiverCurrency { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string paymentStatus { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string authorisationStatus { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string providerStatus { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string onfido { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ofac { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string blackList { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string rules { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string lexisNexis { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string sanctionPep { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string tradeTime { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string provider { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string senderAccount { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string action { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string requestReference { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string requestReferenceTransfast { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string paymentMethod { get; set; }
        public int type { get; set; }
        public string transactionType { get; set; }
    }
}
