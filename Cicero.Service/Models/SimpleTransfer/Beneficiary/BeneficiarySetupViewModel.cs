using Cicero.Service.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Cicero.Service.Extensions.Extensions;

namespace Cicero.Service.Models.SimpleTransfer.Beneficiary
{
    public class BeneficiarySetupViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please Enter First Name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Middle Name.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Short Name.")]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 1.")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Please Enter Address Line 2.")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please Enter Suburb.")]
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "Please Enter Postal Code.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Please Enter Account Number.")]
        [Display(Name = "Account Number")] 
        public string AccountNumber { get; set; }

        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int BankId { get; set; }
        public int BankBranchId { get; set; }
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please Enter Phone No.")]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
        public int RelationshipToBeneId { get; set; }
        [Required(ErrorMessage = "Please Enter Gender.")]
        [Display(Name = "Gender")] 
        public int Gender { get; set; }
        public string Remark { get; set; }
        public bool Status { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
