using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.API.LexisNexis
{
    public class SanctionPepCustomerViewModel
    {
        public int Id { get; set; }
        public int LexisNexisId { get; set; }
        public string TransactionId { get; set; }
        public int TransactionType { get; set; }
        //public int PepsType { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsVerified { get; set; }
        public bool IsMatch { get; set; }
        public bool NewMatchVerified { get; set; }
        public bool PeriodicVerified { get; set; }
        public string Remarks { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
