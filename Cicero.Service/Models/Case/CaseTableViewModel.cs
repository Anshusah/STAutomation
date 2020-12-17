using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{
    public class CaseTableViewModel
    {
        public int id { get; set; }

        public string claimId { get; set; }

        public string createdBy { get; set; }

        public string email { get; set; }

        public string createdAt { get; set; }

        public DateTime createdOrder { get; set; }

        public string updatedAt { get; set; }

        public string status { get; set; }

        public string state { get; set; }

        public string extras { get; set; }

        public string fullName { get; set; }

        public int stateId { get; set; }

        public dynamic states { get; set; }

        public string action { get; set; }
    }
}
