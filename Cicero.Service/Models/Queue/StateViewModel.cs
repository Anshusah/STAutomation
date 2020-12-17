using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class StateViewModel
    {

        public int Id { get; set; }

        [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Action Name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ActionName { get; set; }

        //[RegularExpression(@"^[^-\s][a-zA-Z0-9_\s-]+$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [RegularExpression(@"^[^-\s][a-zA-Z_\s-]+$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter System Name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string SystemName { get; set; }

        public string Extras { get; set; }

        public List<StateForFormView> StateForForm { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public bool Status { get; set; } = true;

        public string Color { get; set; }

        public bool NotifyUser { get; set; } = false;

        public bool CanEdit { get; set; } = false;

        public bool CanDelete { get; set; } = false;

        public bool NeedReason { get; set; } = false;

        public int TenantId { get; set; }

        public bool UserAccess { get; set; }

        public int CaseFormId { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public List<SelectListItem> CaseFormList { get; set; }

        public IEnumerable<int> StateSelectedList { get; set; } = null;

        public List<StateViewModel> StateList { get; set; }

        public bool RemoveRelation { get; set; }

        public List<FromState> FromStates { get; set; }
    }

    public class StateForFormView
    {
        public int Id { get; set; }

        public int CaseFormId { get; set; }

        public string CaseFormName { get; set; }

        public bool AllUser { get; set; }

        public bool FirstFrontState { get; set; }

        public bool FirstBackState { get; set; }

        public string Icon { get; set; }

        [Display(Name = "Order Position")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Whole Numbers only")]
        [Remote("CheckStateOrderDuplication", "Queue", ErrorMessage = "Order Position Already Exists. Please Change the previous one if you want this position.", HttpMethod = "Post", AdditionalFields = "Id, CaseFormId")]
        public int Order { get; set; }

        public List<StatePermissionView> StatePermissions { get; set; }
    }

    public class StatePermissionView
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string RoleId { get; set; }

        public bool CanEdit { get; set; }

        public bool ViewMode { get; set; }

    }

    public class FromState
    {
        public int FromStateId { get; set; }

        public int ToStateId { get; set; }

        public int CaseFormId { get; set; }

        public string StatePosX { get; set; }

        public string StatePosY { get; set; }

        public string LinePosX { get; set; }

        public string LinePosY { get; set; }

        public bool Aero { get; set; }

        public string JsonState { get; set; }

    }
}
