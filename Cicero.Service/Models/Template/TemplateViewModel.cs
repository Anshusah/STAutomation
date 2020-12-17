using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class TemplateViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Template Name is Required")]
        [Display(Name = "Template Name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Html Editor is Required")]
        [Display(Name = "Html Editor")]
        public string Content { get; set; }

        [Display(Name = "Short Description")]
        public string Excerpt { get; set; }

        [Display(Name = "Created At")]
        public string CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public string UpdatedAt { get; set; }

        [Display(Name = "Version")]
        public int Version { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Recipient Type")]
        public int RecipientType { get; set; }

        [Display(Name = "Recipient Field")]
        public string RecipientField { get; set; }

        [Display(Name = "Recipient Database Table")]
        public string RecipientDatabaseTable { get; set; }

        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Display(Name = "Email Group")]
        public string EmailGroupId { get; set; }

        public int TemplateType { get; set; }

        public int TenantId { get; set; }

        [Required(ErrorMessage="Template For is required.")]
        public int FormId { get; set; }

        public List<CaseFormViewModel> CaseForms { get; set; }
    }
}
