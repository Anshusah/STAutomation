using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        //[Display(Name = "Email Address")]
        //[Required]
        //[StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        //[Remote("CheckEmailIdDuplication", "User", ErrorMessage = "Email Address is Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        //[EmailAddress]
        //public string Email { get; set; }

        [RegularExpression(@"(?=^.{5,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "Password should have A-Z, a-z, 0-9 and some special characters #,!,@ etc..")]
        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [RegularExpression(@"(?=^.{5,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "Password should have A-Z, a-z, 0-9 and some special characters #,!,@ etc..")]
        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

    }
}
