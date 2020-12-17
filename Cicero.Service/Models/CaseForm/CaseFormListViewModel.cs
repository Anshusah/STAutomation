using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class CaseFormListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UrlIdentifier { get; set; }

        public string Icon { get; set; }

        public bool Status { get; set; }

        public int TenantId { get; set; }

        public string EncryptedId { get; set; }
        public object Url { set; get; }

    }
}
