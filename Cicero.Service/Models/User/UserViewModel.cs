using Cicero.Data.Entities;
using Cicero.Service.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class UserViewModel
    {
        
        [Key]
        public string Id { get; set; }

        [Display(Name = "User Id")]
        //[Required(ErrorMessage = "Please Enter User Id.")]
        //[RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-Z, 0-9 and _")]
        //[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        //[Remote("CheckUserIdDuplication", "OrganisationUser", ErrorMessage = "User Id Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter First Name.")]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Last Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckEmailIdDuplication","User","Admin", ErrorMessage = "Email Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id, TenantId")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number.")]
        [RegularExpression(@"^[0-9_+ ]*$", ErrorMessage = "Provide valid Phone Number")]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"(?=^.{5,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "Password should have A-Z, a-z, 0-9 and some special characters #,!,@ etc..")]
        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "Address")]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Address { get; set; }

        public IEnumerable<int> Images { get; set; }

        public string CreatedBy { get; set; }

        public bool Status { get; set; }

        public bool Lockout { get; set; }

        [Display(Name = "Onfido Verification Status")]
        public string IsOnfidoVerify { get; set; }

        [Display(Name = "Onfido Checks Result")]
        public string OnfidoChecksResult { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public int TenantId { get; set; }

        public List<string> Ids { get; set; }

        [Required(ErrorMessage = "Please Select an option.")]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        public List<SelectListItem> RoleList { get; set; }
        public ICollection<UserMediaViewModel> UserMedias { get; set; }

        public List<string> OnfidoDocuments { get; set; }

        public List<SelectListItem> BeneficiaryList { get; set; }

        public string DOB { get; set; }

        public string IdType { get; set; }

        public string IdNumber { get; set; }

        public string IdExpirationDate { get; set; }

        [Required(ErrorMessage = "Please Select an option for Skill set.")]
        [Display(Name = "Skill")]
        public int UserSkillSetId { get; set; }

    }
}
