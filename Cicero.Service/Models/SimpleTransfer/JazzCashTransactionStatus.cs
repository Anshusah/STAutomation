using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class JazzCashTransactionStatus
    {
        public string AccounNumber { get; set; }
        public string RequestId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
