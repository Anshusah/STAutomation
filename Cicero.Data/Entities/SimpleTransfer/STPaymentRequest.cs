using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class STPaymentRequest
    {
        public int Id { get; set; }

        public string PayerId { get; set; }

        public string PayeeId { get; set; }
        public string PayeeCountry { get; set; }

        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerMobileNumber { get; set; }

        public string RequestId { get; set; }

        public decimal RequestAmount { get; set; }
        public string CurrencyCode { get; set; }

        public int Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
