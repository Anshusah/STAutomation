using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Transaction
{
    public class TransactionDetailsViewModel
    {
        public int TransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionPin { get; set; }
        public string TransactionStatus { get; set; }
        public List<SelectListItem> CustomerLexisNexisReports { get; set; }
        public List<SelectListItem> BeneficiaryLexisNexisReports { get; set; }
        public TransactionDetailsSender Sender { get; set; }
        public TransactionDetailsReceiver Receiver { get; set; }
        public TransactionDetailsCompliance Compliance { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public InformationForPayer informationForPayer { get; set; }
    }

    public class TransactionDetailsSender
    {
        public bool BlackList { get; set; }
        public string MembershipCard { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }
        public DateTime? DocumentExpirationDate { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string LegalIdentificationNumber { get; set; }
        public string LegalIdentificationType { get; set; }
        public string PlaceOfIssueOfTheIdentification { get; set; }
        public string Passport { get; set; }
        public string SendingReason { get; set; }
        public string FundsOrigin { get; set; }
    }

    public class TransactionDetailsReceiver
    {
        public bool BlackList { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string LegalIdentificationType { get; set; }
        public string RelationshipToBeneficiary { get; set; }
    }

    public class TransactionDetailsCompliance
    {
        public DateTime FlagHoldDate { get; set; }
        public string FlagHoldUser { get; set; }
        public string AdditionalNotes { get; set; }
        public string LimitHoldUser { get; set; }
        public string Reason { get; set; }
    }

    public class PaymentInformation
    {
        public string OriginCurrency { get; set; }
        public decimal AmountSent { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CorrespondentExchangeRate { get; set; }
        public decimal Fees { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal TotalAdditionalCharges { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal CommissionCorrespondent { get; set; }
        public string DestinationCurrency { get; set; }
        public decimal AmountToReceive { get; set; }
        public decimal AdditionalChargesPayment { get; set; }
        public decimal TotalAdditionalChargesPayment { get; set; }
        public decimal TotalAmountToReceive { get; set; }
    }

    public class InformationForPayer
    {
        public string PaymentType { get; set; }
        public string OfficeAddress { get; set; }
        public string Bank { get; set; }
        public string BankAddress { get; set; }
        public string Correspondent { get; set; }
        public string Telephone { get; set; }
        public string Account { get; set; }
        public string Office { get; set; }
        public string Contact { get; set; }
        public string Delegation { get; set; }
        public string PaymentOffice { get; set; }
        public string Schedule { get; set; }
        public string BankBranch { get; set; }
    }

    public class TransactionEvents
    {
        public DateTime Date { get; set; }  
        public DateTime RealDate { get; set; }  
        public string Username { get; set; }
        public string Events { get; set; }
        public string Comments { get; set; }
        public bool Transmitted { get; set; }
        public string Status { get; set; }
    }

    public class BeneficiaryChangeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LegalIdentification { get; set; }
        public string Account { get; set; }
        public DateTime Date { get; set; }
    }

    public class TransactionDetailsSMS
    {
        public DateTime Date { get; set; }
        public string Telephone { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
    }
}
