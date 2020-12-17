using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.PayOutMode
{
    public class PayOutModeViewModel
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string PayoutModeName { get; set; }

        public bool Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
