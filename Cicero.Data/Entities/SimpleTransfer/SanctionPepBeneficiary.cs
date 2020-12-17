using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SanctionPepBeneficiary
    {
        public int Id { get; set; }
        public int LexisNexisId { get; set; }
        public string TransactionId { get; set; }
        public int TransactionType { get; set; }
        public int PepsType { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsVerified { get; set; }
        public bool IsMatch { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
