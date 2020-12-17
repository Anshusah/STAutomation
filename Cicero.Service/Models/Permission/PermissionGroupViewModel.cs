using System;
using System.Collections.Generic;
using System.Text;
using Cicero.Service.Models;
using Cicero.Service.Services;

namespace Cicero.Service.Models
{
   public class PermissionGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PermissionIds { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public int TenantId { get; set; }
        public List<PermissionViewModel> Permission { get; set; }
        public int CaseFormId { get; set; }

    }
}
