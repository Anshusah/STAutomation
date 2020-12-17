using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class RegisterUserViewModel
    {

        [Key]
        public string Id { get; set; }

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
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckEmailIdDuplication", "User", "Admin", ErrorMessage = "Email Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id, TenantId")]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "Your password must be a minimum of 6 characters in length, with at least one upper case letter, one number and one special character.")]
        [Required(ErrorMessage = "Please Enter Password.")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords do not match. Please re-enter.")]
        public string ConfirmPassword { get; set; }

        public short Status { get; set; }

        public string Lockout { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public int TenantId { get; set; }

        public string RoleId { get; set; }
        [RegularExpression(@"^(?!070|076|4470|4476)(07[\d]{8,9}|01[\d]{8,9}|02[\d]{8,9}|447[\d]{7,10})$", ErrorMessage = "Invalid phone number")]
        [Required]
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Agreement Check")]

        public bool IsAgreed { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Address { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string PostCode { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string City { get; set; }
    }
}
