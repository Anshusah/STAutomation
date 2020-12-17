using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Service.Models.SimpleTransfer.User
{
    public class RegisterUserViewModel
    {

        [Key]
        public string Id { get; set; }

        public string DisplayName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format.")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckUserEmailIdDuplication", "User", "Admin", ErrorMessage = "Email Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id, TenantId")]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "Invalid Password")]
        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords do not match. Please re-enter.")]
        public string ConfirmPassword { get; set; }

        public short Status { get; set; }

        public string Lockout { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public int TenantId { get; set; }

        public string RoleId { get; set; }
        //[RegularExpression(@"^(07[\d]{9}|7[\d]{9})$", ErrorMessage = "Invalid phone number")]
        [Required]
        [Display(Name = "Phone Number")]

        [Remote("CheckIfMobileNumberExists", "User", "Admin", ErrorMessage = "There is an existing account with this number. Please contact customer support if this is your number.", HttpMethod = "Post", AdditionalFields = "PhoneNumber")]
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

        [Required]
        public string CountryCode { get; set; }

        public int IdType { get; set; }

        public string IdNumber { get; set; }

        public DateTime IdExpiryDate { get; set; }

        public int SecurityQuestionId { get; set; }

        public string Answer { get; set; }

        public string IssuingCountry { get; set; }

        public string CompanyRegistrationNumber { get; set; }
        public DateTime DOB { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName_Company { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string LastName_Company { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string CompanyName { get; set; }

        public string TypeOfBusinessEntity { get; set; }

        public string CompanyWebsite { get; set; }

        public PayeeType PayeeType { get; set; }
    }
}
