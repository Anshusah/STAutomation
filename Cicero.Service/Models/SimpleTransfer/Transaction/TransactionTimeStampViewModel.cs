using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Transaction
{
    public class TransactionTimeStampViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
    }
}
