using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class STPaymentRequestDetails
    {
        public int Id { get; set; }

        public string STPaymentRequestId { get; set; }

        public string Description { get; set; }

        public int PurposeOfRequest { get; set; }

        public string Bank { get; set; }

        public string Branch { get; set; }

        public string AccountNumber { get; set; }

        public string Invoice { get; set; }

        public string Reminder { get; set; }

        public DateTime DueDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
