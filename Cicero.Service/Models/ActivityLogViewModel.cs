using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class ActivityLogViewModel
    {

        public string UserId { get; set; }

        public string Details { get; set; }

        public string DisplayTo { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ClaimId { get; set; }

        public string StateName { get; set; }

        public string FullName { get; set; }
    }
}
