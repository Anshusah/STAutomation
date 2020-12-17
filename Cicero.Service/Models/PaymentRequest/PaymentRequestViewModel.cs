using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using static Cicero.Service.SimpleTransferEnum;

namespace Cicero.Service.Models.PaymentRequest
{
    public class PaymentRequestViewModel
    {
        public int Id { get; set; }

        public string PayerId { get; set; }

        public string PaymentRequestId { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format.")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string PayerEmail { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string PayerFirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Display(Name = "Last Name")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string PayerLastName { get; set; }

        public string PayerName
        {
            get { return PayerFirstName + " " + PayerLastName; }
        }

        public string PayeeName { get; set; }

        [Required(ErrorMessage = "Please Enter Sender Phone.")]
        [RegularExpression(@"^[0-9_+ ]*$", ErrorMessage = "Provide valid Mobile Number")]
        [Display(Name = "Sender Phone")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string PayerMobileNumber { get; set; }

        public string PayeeId { get; set; }
        public string PayeeCountry { get; set; }

        public string RequestId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Range(20, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Remote("CheckAmountLimit", "PaymentRequest", "Admin", ErrorMessage = "You have exceeded the maximum transaction amount.", HttpMethod = "Get", AdditionalFields = "RequestAmount, PayeeCountry")]
        public decimal RequestAmount { get; set; }

        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; }

        public int Status { get; set; }

        public string PaymentRequestStatus { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; } 

        public string UpdatedDate { get; set; }

        public STPaymentRequestDetailsViewModel STPaymentRequestDetails { get; set; }
        public List<SelectListItem> PayerList { get; set; }
    }

    public class STPaymentRequestDetailsViewModel
    {
        public int Id { get; set; }

        public string STPaymentRequestId { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Purpose of Request")]
        public int PurposeOfRequest { get; set; }

        [Required]
        [Display(Name = "Bank")]
        public string Bank { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Provide valid Account Number")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string AccountNumber { get; set; }

        [Display(Name = "Invoice File")]
        public IFormFile InvoiceFile { get; set; }

        [Required]
        public string Invoice { get; set; }

        public string OldInvoice { get; set; }

        public string Reminder { get; set; }

        [Required]
        [CustomValidation(typeof(CustomerWeekendValidation), nameof(CustomerWeekendValidation.WeekendValidate))]
        public DateTime DueDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }
    }

    public class PaymentRequestDetails
    {
        public PaymentRequestViewModel PaymentRequestViewModel { get; set; }
        public List<TransactionEvents> TransactionEvents { get; set; }
    }

    public class CustomerWeekendValidation
    {
        public static ValidationResult WeekendValidate(DateTime date)
        {
            return date.Date < DateTime.Now.Date 
                ? new ValidationResult("The Due Date must be equal to or greater than today's date.")
                : ValidationResult.Success;
        }
    }
}
