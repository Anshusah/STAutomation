using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class QueueViewModel
    {

        public int Id { get; set; }

        [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Queue Name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Queue url")]
        [Required]
        [RegularExpression(@"^[a-z\-]*$", ErrorMessage = "Provide valid url (a-z, -)")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckUrlIdentifierDuplicate", "Queue", ErrorMessage = "Queue url Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        public string UrlIdentifier { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        //public string Side { get; set; }

        public bool Status { get; set; } = true;

        public int TenantId { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }
        //[Required(ErrorMessage = "Please Select a role")]
        //public string RoleId { get; set; }

        //public string RoleName { get; set; }

        //[Display(Name = "Order Position")]
        //[Required]
        //[StringLength(3, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        //[Remote("CheckQueueOrderDuplication", "Queue", ErrorMessage = "Order Position Already Exists. Please Change the previous one if you want this position.", HttpMethod = "Post", AdditionalFields = "Id, RoleId")]
        //public string Order { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public IEnumerable<int> StateSelectedList { get; set; } = null;

        public List<StateViewModel> StateList { get; set; }

        public int CaseFormId { get; set; }

        public List<SelectListItem> FormList { get; set; }

        public List<QueueForFormView> QueueForForm { get; set; }

        public bool RemoveRelation { get; set; }

        public List<QueueState> QueueToState { get; set; }

    }

    public class QueueForFormView
    {
        public int Id { get; set; }

        public string CaseFormName { get; set; }

        public int CaseFormId { get; set; }

        public bool AllUser { get; set; }

        [Display(Name = "Order Position")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Whole Numbers only")]
        [Remote("CheckQueueOrderDuplication", "Queue", ErrorMessage = "Order Position Already Exists. Please Change the previous one if you want this position.", HttpMethod = "Post", AdditionalFields = "Id, CaseFormId")]
        public int Order { get; set; }

        public List<QueuePermissionView> QueuePermissions { get; set; }
    }

    public class QueuePermissionView
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string RoleId { get; set; }

    }

    public class QueueState
    {
        public int QueueId { get; set; }

        public int StateId { get; set; }

        public int CaseFormId { get; set; }

        public string PosX { get; set; }

        public string PosY { get; set; }

        public string LinePosX { get; set; }

        public string LinePosY { get; set; }

        public bool IsQueue { get; set; }

        public string JsonState { get; set; }

    }
}
