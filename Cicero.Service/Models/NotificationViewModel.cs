using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class NotificationViewModel
    {

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string Detail { get; set; }

        public string Title { get; set; }
        public string Link { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
