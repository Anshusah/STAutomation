using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class RoleViewModel
    {
        
        [Key]
        public string Id { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
      //  [Required(ErrorMessage = "Please Enter Role Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter Role Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckNameForTenantDuplication", "Role", ErrorMessage = "Role Name must be unique", HttpMethod = "Post", AdditionalFields = "Id, TenantId")]
        [Display(Name = "Role Name")]
        public string DisplayName { get; set; }

        public short Status { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public int TenantId { get; set; }

        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Please Select an option for side.")]
        [Display(Name = "Role Type")]
        public string WebSide { get; set; }

        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        public IEnumerable<string> PermissionListSelected { get; set; }

        public List<PermissionViewModel> PermissionList { get; set; }

        public List<PermissionGroupViewModel> PermissionListGroup { get; set; }

    }
}
