using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Cicero.Service.Models
{
    public class CaseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a name for the form.")]
        [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Name { get; set; }

        public string Fields { get; set; }

        public string TabEnable { get; set; }
        public string ModelName { get; set; }

        public string UrlIdentifier { get; set; }

        public string Icon { get; set; }

        public string ModelTitle { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }

        public object FormBuilder { set; get; }
        public object url { set; get; }
    }
}
