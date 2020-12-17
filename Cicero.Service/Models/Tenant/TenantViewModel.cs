using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class TenantViewModel
    {

        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z and _")]
        [Required(ErrorMessage = "Please Enter Role Name.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Identifier")]
        [Required]
        [RegularExpression(@"^[a-z\-]*$", ErrorMessage = "Provide valid url (a-z, -)")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckIdentifierIdDuplication", "Tenant", ErrorMessage = "The Tenant Identifier Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        public string Identifier { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckEmailIdDuplication", "Tenant", ErrorMessage = "Email Address Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Phone Number.")]
        [RegularExpression(@"^[0-9_+ ]*$", ErrorMessage = "Provide valid Phone Number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "Address 1")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string AddressPrimary { get; set; }

        [Display(Name = "Address 2")]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string AddressSecondary { get; set; }

        [Required(ErrorMessage = "Please Enter PostCode")]
        [Display(Name = "Post Code")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Provide valid Post Code")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string City { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public bool Status { get; set; }
    }
}
