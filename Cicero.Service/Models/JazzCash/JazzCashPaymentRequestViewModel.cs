using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.JazzCash
{
    public class JazzCashPaymentRequestViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PayeeName { get; set; }

        public string PayerName { get; set; }

        public string Currency { get; set; }

        public int Reason { get; set; }

        public string RequestId { get; set; }

        public decimal RequestAmount { get; set; }

        public string JazzCashAccountNumber { get; set; }

        public int Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; } 

        public string UpdatedDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
