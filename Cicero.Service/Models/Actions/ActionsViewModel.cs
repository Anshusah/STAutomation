using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace Cicero.Service.Models
{
    public class ActionsViewModel
    {

        public int Id { get; set; }

        [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Actions Name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Actions url")]
        [Required]
        [RegularExpression(@"^[a-z\-]*$", ErrorMessage = "Provide valid url (a-z, -)")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckUrlIdentifierDuplicate", "Actions", ErrorMessage = "Actions url Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        public string UrlIdentifier { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }
         
        public bool Status { get; set; } = true;
        public string ActionType { get; set; }

        public int TenantId { get; set; }
        public int CaseFormId { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> TypeList { get; set; }
        public string TemplateId { get; set; }
        public List<SelectListItem> TemplateList { get; set; }
        public string ActionsReceiver { get; set; }
        public List<SelectListItem> ActionsReceiverLst { get; set; }
        public string ActionsSender { get; set; }
        public List<SelectListItem> ActionsSenderlst { get; set; }
       

    }
     

    
}
