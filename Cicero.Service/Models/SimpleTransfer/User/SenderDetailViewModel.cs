using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models.SimpleTransfer.User
{
    public class SenderDetailViewModel
    {

        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string ApplicantId { get; set; }

        [Required]
        public string Title { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter First Name.")]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Middle Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Last Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string AddressLine { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        [Required]
        public string Country { get; set; }

        public string City { get; set; }

        [Required]
        public string IdNumber { get; set; }

        public DateTime DOB { get; set; }

        [Required]
        public string IdType { get; set; }

        [Required]
        public DateTime IdExpiryDate { get; set; }

        public string PostCode { get; set; }        

        [Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        public bool Terms { get; set; }

        public short Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

    }
    public class SenderIdentityViewModel
    {

        [Key]
        public string Id { get; set; }
        public int IdType { get; set; }
        
        public string FrontUrl { get; set; }
        public string BackUrl { get; set; }
        public string UserId { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

    }
}
