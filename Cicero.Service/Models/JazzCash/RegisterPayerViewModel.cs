using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using static Cicero.Service.Enums;

namespace Cicero.Service.Models.JazzCash
{
    public class RegisterPayerViewModel
    {
        public int Id { get; set; }

        [Required]
        public PayeeType PayeeType { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TenantId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName_Company { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string LastName_Company { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string CompanyName { get; set; }

        public string TypeOfBusinessEntity { get; set; }

        public string CompanyWebsite { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format.")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Email { get; set; }

        [Compare(nameof(Email), ErrorMessage = "The email do not match. Please re-enter.")]
        public string ReEmail { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Address { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string PostCode { get; set; }

        [Required]
        [RegularExpression(@"^(07[\d]{9}|7[\d]{9})$", ErrorMessage = "Invalid phone number")]
        [Remote("CheckIfMobileNumberExistsJazzCash", "User", "Admin", ErrorMessage = "Mobile Number Already Exists.", HttpMethod = "Post", AdditionalFields = "MobileNumber")]
        public string MobileNumber { get; set; }

        [Required]
        public int IdType { get; set; }

        [Required]
        public string IdNumber { get; set; }

        public DateTime IdExpiryDate { get; set; }

        public string IssuingCountry { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        [Required]
        public string JazzCashAccount { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format.")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckUserEmailIdDuplicationJazzCash", "User", "Admin", ErrorMessage = "Email Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id, TenantId")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "Invalid Password")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The passwords do not match. Please re-enter.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
