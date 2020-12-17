using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class CaseFrontViewModel
    {

        public int Id { get; set; }

        //[Required]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Case Id")]
        public string CaseGeneratedId { get; set; }

        public string FullName { get; set; }

        public int Version { get; set; }

        [Required(ErrorMessage = "Please Select an option.")]
        [Display(Name = "Status")]
        public short Status { get; set; }

        //[Display(Name = "Updated Date")]
        //public string UpdatedAt { get; set; }

        //[Display(Name = "Created Date")]
        //public string CreatedAt { get; set; }

        //[Required(ErrorMessage = "Claim Title is required")]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string UserId { get; set; }

        [Display(Name ="Created By")]
        public string CreatedBy { get; set; }

        public int OrganisationId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter your First Name.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter your Last Name.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string SurName { get; set; }

        [Display(Name = "Insurance Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Preferred Contact Day")]
        [Required(ErrorMessage = "Please Select an option.")]
        public string ContactDay { get; set; }

        [Required(ErrorMessage = "Please Enter Preferred Contact Number.")]
        [RegularExpression(@"^[0-9_+ ]*$", ErrorMessage = "Provide valid Phone Number. Length should be more than 6 digits and less than 15")]
        [Display(Name = "Preferred Contact Number")]
        [StringLength(15, MinimumLength = 6)]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }

        //[Display(Name = "Maritial Status")]
        //[Required(ErrorMessage = "Please Enter your Maritial Status.")]
        //public string MaritialStatus { get; set; }


        //[Display(Name = "Date of Birth")]
        //[Required(ErrorMessage = "Please Enter Date of Birth")]
        //public DateTime DOB { get; set; }

        //[Display(Name = "Address")]
        //[Required(ErrorMessage = "Please Enter your Full Address.")]
        //public string Address1 { get; set; }

        //public string Address2 { get; set; }

        //public string Address3 { get; set; }

        //[Display(Name = "PostCode")]
        //[Required(ErrorMessage = "Please Enter your Postal Code.")]
        //public string PostCode { get; set; }

        //[Display(Name = "City/Town")]
        //[Required(ErrorMessage = "Please Enter your City.")]
        //public string City { get; set; }

        //[Display(Name = "Country")]
        //[Required(ErrorMessage = "Please Enter your Country.")]
        //public string Country { get; set; }

        //[Required(ErrorMessage = "Please Enter Phone Number.")]
        //[RegularExpression(@"^[0-9_+ ]*$", ErrorMessage = "Provide valid Phone Number. Length should be more than 11 digits")]
        //[Display(Name = "Phone Number")]
        //[StringLength(50, MinimumLength = 11)]
        //[DataType(DataType.PhoneNumber)]
        //public string TelephoneNumber { get; set; }

        //[Display(Name = "Email Address")]
        //[Required]
        //[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        ////[Remote("CheckEmailIdDuplication", "Administrator", ErrorMessage = "Email Address is Already Exists.", HttpMethod = "Post", AdditionalFields = "Id")]
        //[EmailAddress]
        //public string Email { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        //[Display(Name = "Agent/Broker (Optional)")]
        //public string Agent { get; set; }

        //[Display(Name = "Business Occupation (Optional)")]
        //public string Occupation { get; set; }

        //[Display(Name = "Vat Registered")]
        //[Required(ErrorMessage = "Please Enter Vat Registered for your claim.")]
        //public string VatRegistered { get; set; }

        ////[Display(Name = "Vat Rate (%)")]
        ////[Range(0, 100, ErrorMessage = "Vat rate cannot be more that 100%")]
        //////[Required(ErrorMessage = "Please Enter Vat Rate for your claim.")]
        ////public int VatRate { get; set; }

        ////[Display(Name = "Insurance Type")]
        ////[Required(ErrorMessage = "Please Enter Insurance Type for your claim.")]
        ////public string InsuranceType { get; set; }

        //[Display(Name = "Additional Policy Holder")]
        //[Required(ErrorMessage = "Please Select an option.")]
        //public string AdditionalPolicyHolder { get; set; }

        //[Display(Name = "Additional Policy Holder")]
        //public string AdditionalFirstName { get; set; }

        //[Display(Name = "Additional Policy Holder")]
        //public string AdditionalLastName { get; set; }

        //[Display(Name = "Additional Policy Holder")]
        //public string RelationshipToPolicyHolder { get; set; }

        //[Display(Name = "Do you have any children in home?")]
        //public string HasChildren { get; set; }

        //[Display(Name = "How many children do you have in your home?")]
        //public int NumberOfChildren { get; set; }

        //[Display(Name = "Inception Date")]
        //[Required(ErrorMessage = "Please Enter Inception date")]
        //public DateTime InceptionDate { get; set; }

        //[Display(Name = "Expiry Date")]
        //public DateTime ExpiryDate { get; set; }

        //[Display(Name = "Excess")]
        //public decimal Excess { get; set; }

        //[Display(Name = "Pay To")]
        //[Required(ErrorMessage = "Please Enter details of the person/orgnaization paid to.")]
        //public string PayTo { get; set; }

        //[Display(Name = "Payment Details")]
        //[Required(ErrorMessage = "Please Enter any payment details.")]
        //public string PaymentDetails { get; set; }

        //[Display(Name = "Bank Address")]
        //[Required(ErrorMessage = "Please Enter Bank Address.")]
        //public string BankAddress { get; set; }

        //[Display(Name = "Geo Location")]
        //public string GeoLocation { get; set; }

        [Display(Name = "Account Holder Name")]
        [Required(ErrorMessage = "Please Enter Account Holder Name.")]
        public string BankAccountName { get; set; }

        [Display(Name = "Bank Branch Sort Code")]
        [Required(ErrorMessage = "Please Enter Bank Branch Sort Code.")]
        public string BankSortCode { get; set; }

        [Display(Name = "Bank Account Number")]
        [Required(ErrorMessage = "Please Enter Bank Account Number.")]
        public string BankAccountNumber { get; set; }

        [StringLength(250, MinimumLength = 0)]
        [Display(Name = "Additional Infromation")]
        public string OtherInformation { get; set; }

        public string Extras { get; set; }

        public List<SelectListItem> CountryList { get; set; }

        public int StateId { get; set; }

        public int MessageParentId { get; set; }

        [Display(Name = "State Name")]
        public string StateName { get; set; }

        public int DraftState { get; set; } 

        public string StateColor { get; set; }

        public string QueueName { get; set; }

        public int TenantId { get; set; }

        [Display(Name = "Insurance Policy Type")]
        [Required(ErrorMessage = "Please Select a policy type.")]
        public int CaseFormId { get; set; }

        [Display(Name = "Case Type")]
        public string CaseFormName { get; set; }

        public List<SelectListItem> StateList { get; set; }

        public List<QueueListViewModel> QueueList { get; set; }

        public IEnumerable<int> Images { get; set; }

        public int SignatureId { get; set; }

        public List<SelectListItem> CaseList { get; set; }

        public List<StateViewModel> CaseTasks { get; set; }

        public List<MessageViewModel> MessageList { get; set; }

        public List<ActivityLogViewModel> ClaimActivityList { get; set; }

        public DynamicFormViewModel FormbuilderModel { get; set; }

    }
}