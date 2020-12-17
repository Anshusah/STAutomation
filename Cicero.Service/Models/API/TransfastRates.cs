using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class TransfastRateModel
    {
        public string PayerName {get;set;}
        public string ModeOfPaymentName {get;set;}
        public decimal StartRate {get;set;}
        public decimal EndRate {get;set;}
        public string PayerId {get;set;}
        public string ModeOfPaymentId {get;set;}
        public string ReceiveCountryIsoCode {get;set;}
        public string ReceiveCountryName {get;set;}
        public string RateStartDate {get;set;}
        public string RateExpiryDate {get;set;}
        public string ReceiveCurrencyIsoCode {get;set;}
        public TransfastErrorModel TransfastErrorModel { get; set; }
    }
    public class ReceivedData
    {
        public bool ApplyBackendFee { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class BusinessError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }
    }

    public class DataValidationError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }
    }

    public class TransfastErrorModel
    {
        public ReceivedData ReceivedData { get; set; }
        public List<BusinessError> BusinessErrors { get; set; }
        public List<DataValidationError> DataValidationErrors { get; set; }
    }
    public class TransfastBankModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class TransfastBankBranchModel
    {
        public string BankBranchID { get; set; }
        public string BankBranchName { get; set; }
        public string BankID { get; set; }
        public string CityCode { get; set; }
    }
    public class TransfastCityModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StateId { get; set; }

        public string StateName { get; set; }

        public string CountryIsoCode { get; set; }

    }

    public class TransactionModel
    {
        public string TfPin { get; set; }   
        public SenderModel Sender { get; set; }
        public ReceiverModel Receiver { get; set; }
        public TransactionInfoModel TransactionInfo { get; set; }
        public ComplianceModel Compliance { get; set; }

    }

    public class SenderModel
    {
        public int? SenderId { get; set; }
        public string LoyalityCardNumber { get; set; }
        public string Name { get; set; }
        public string NameOtherLanguage { get; set; }
        public string Address { get; set; }
        public string AddressOtherLanguage { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
        public string StateId { get; set; }
        public string CountryIsoCode { get; set; }
        public string TypeOfId { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdExpiryDate { get; set; }
        public string NationalityIsoCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool IsIndividual { get; set; }
        public int? SenderOccupation { get; set; }
    }

    public class ReceiverModel
    {
        public string FirstName { get; set; }
        public string FirstNameOtherLanguage { get; set; }
        public string SecondName { get; set; }
        public string SecondNameOtherLanguage { get; set; }
        public string LastName { get; set; }
        public string LastNameOtherLanguage { get; set; }
        public string SecondLastName { get; set; }
        public string SecondLastNameOtherLanguage { get; set; }
        public string FullNameOtherLanguage { get; set; }
        public string CompleteAddress { get; set; }
        public string CompleteAddressOtherLanguage { get; set; }
        public int? CityId { get; set; }
        public string TownId { get; set; }
        public string StateId { get; set; }
        public string CountryIsoCode { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string ZipCode { get; set; }
        public string NationalityIsoCode { get; set; }
        public string Email { get; set; }
        public bool IsIndividual { get; set; }
        public string Cpf { get; set; }
        public int? ReceiverTypeOfId { get; set; }
        public string ReceiverIdNumber { get; set; }
        public string Notes { get; set; }
        public string NotesOtherLanguage { get; set; }
    }

    public class TransactionInfoModel
    {
        public string PaymentModeId { get; set; }
        public string ReceiveCurrencyIsoCode { get; set; }
        public string BankId { get; set; }
        public string Account { get; set; }
        public string AccountTypeId { get; set; }
        public string BankBranchId { get; set; }
        public string PayingBranchId { get; set; }
        public string PayerId { get; set; }
        public string PurposeOfRemittanceId { get; set; }
        public string SourceCurrencyIsoCode { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalSentAmount { get; set; }
        public decimal SentAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal USDServiceFee { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal CashAmount { get; set; }
        public decimal Payout { get; set; }
        public string FormOfPaymentId { get; set; }
        public string ReferenceNumber { get; set; }
        public int? SourceOfFundsID { get; set; }
        public string FeeProduct { get; set; }
    }

    public class ComplianceModel
    {
        public string CountryIssueIsoCode { get; set; }
        public string StateIssueId { get; set; }
        public DateTime? SenderDateOfBirth { get; set; }
        public string ReceiverRelationship { get; set; }
        public string SourceOfFunds { get; set; }
        public string SourceOfFundsID { get; set; }
        public string TypeOfId { get; set; }
        public string IdNumber { get; set; }
        public string Ssn { get; set; }
        public string RemittanceReasonId { get; set; }
        public string SenderOccupation { get; set; }
        public string SenderEmployerName { get; set; }
        public string SenderEmployerAddress { get; set; }
        public string SenderEmployerPhone { get; set; }
        public DateTime? ReceiverDateOfBirth { get; set; }
        public string ReceiverFullName { get; set; }
        public string SenderFullName { get; set; }
    }

    public class TransactionResponseModel
    {
        public string StatusName { get; set; }
        public string StatusId { get; set; }
        public string TfPin { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool CanBeCancelled { get; set; }
        public bool RequiresPreAml { get; set; }
        public bool RequiresPostAml { get; set; }
        public SenderResponseModel Sender { get; set; }
        public ReceiverResponseModel Receiver { get; set; }
        public TransactionInfoResponseModel TransactionInfo { get; set; }
        public ComplianceResponseModel Compliance { get; set; }
    }

    public class SenderResponseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public bool IsIndividual { get; set; }
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string TypeOfId { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdExpiryDate { get; set; }
        public int? SenderId { get; set; }
        public int? SenderOccupation { get; set; }
    }

    public class ReceiverResponseModel
    {
        public string FullName { get; set; }
        public string FullNameOtherLanguage { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string CompleteAddress { get; set; }
        public string ZipCode { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public bool IsIndividual { get; set; }
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string StateId { get; set; }
        public string CityName { get; set; }
        public int? CityId { get; set; }
        public int? ReceiverTypeId { get; set; }
    }

    public class TransactionInfoResponseModel
    {
        public string PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public string ReceiveCurrencyIsoCode { get; set; }
        public string BankName { get; set; }
        public string BankBranchId { get; set; }
        public string Account { get; set; }
        public string PurposeOfRemittanceId { get; set; }
        public string PayerName { get; set; }
        public string PayerId { get; set; }
        public string PayingBranchId { get; set; }
        public string PayingBranchName { get; set; }
        public string SourceCurrencyName { get; set; }
        public string SourceCurrencyIsoCode { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalSentAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal HandlingAmount { get; set; }
        public decimal FeeRate { get; set; }
        public string FormOfPaymentId { get; set; }
        public string FormOfPaymentName { get; set; }
        public decimal CashAmount { get; set; }
        public decimal Payout { get; set; }
        public decimal CreditAmount { get; set; }
        public string ProductId { get; set; }
        public decimal SentAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal SettlementRate { get; set; }
        public string CashierName { get; set; }
        public string ProductName { get; set; }
        public string PayerBranchAddress { get; set; }
        public string PayerBranchPhone1 { get; set; }
        public string PayerBranchPhone2 { get; set; }
        public List<InvoiceStatus> InvoiceStatusTimeStamps { get; set; }
        public string ReferenceNumber { get; set; }
        public string SourceOfFunds { get; set; }
        public decimal? BackendFee { get; set; }
    }

    public class InvoiceStatus
    {
        public int? ReceiverID { get; set; }
        public string Cashier { get; set; }
        public DateTime? ChangeStatusDate { get; set; }
        public string FlagId { get; set; }
        public string FlagName { get; set; }
    }

    public class ComplianceResponseModel
    {
        public bool IsPreAml { get; set; }
        public decimal? Accumulation { get; set; }
        public bool SourceOfFundsRequired { get; set; }
        public bool IsSsnRequired { get; set; }
        public bool IsSecondIdRequired { get; set; }
    }

    public class ReleaseCancelTransaction
    {
        public string TfPin { get; set; }
        public string ReasonId { get; set; }
        public int CaseId { get; set; }
    }

    public class CreditTransaction
    {
        public string ReferenceNo { get; set; }
        public string Type { get; set; } = "Bank";
    }

    public class GetTransactionModel
    {
        public string Status { get; set; }
        public string StatusId { get; set; }
        public string TfPin { get; set; }
        public string CancellationReason { get; set; }
        public DateTime TransactionDate { get; set; }
        public SenderModel Sender { get; set; }
        public ReceiverModel Receiver { get; set; }
        public TransactionInfoResponseModel TransactionInfo { get; set; }
        public ComplianceResponseModel Compliance { get; set; }
    }

    public class GetSenderModel
    {
        public int? SenderId { get; set; }
        public string LoyaltyCardNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public bool IsIndividual { get; set; }
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string TypeOfId { get; set; }
        public string IdNumber { get; set; }
        public DateTime? IdExpiryDate { get; set; }
        public int? SenderOccupation { get; set; }
    }

    public class GetReceiverModel
    {
        public string FullName { get; set; }
        public string FullNameOtherLanguage { get; set; }
        public string FirstName { get; set; }
        public string FirstNameOtherLanguage { get; set; }
        public string SecondName { get; set; }
        public string SecondNameOtherLanguage { get; set; }
        public string LastName { get; set; }
        public string LastNameOtherLanguage { get; set; }
        public string SecondLastName { get; set; }
        public string SecondLastNameOtherLanguage { get; set; }
        public string CompleteAddress { get; set; }
        public string CompleteAddressOtherLanguage { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public bool IsIndividual { get; set; }
        public string CountryIsoCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string StateId { get; set; }
        public string CityName { get; set; }
        public int? CityId { get; set; }
        public string TownName { get; set; }
        public string TownId { get; set; }
        public int? ReceiverTypeId { get; set; }
    }

    public class GetTransactionInfoModel
    {
        public string PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public string ReceiveCurrencyIsoCode { get; set; }
        public string PurposeOfRemittanceId { get; set; }
        public string PayerName { get; set; }
        public string PayerId { get; set; }
        public string PayingBranchId { get; set; }
        public string PayingBranchName { get; set; }
        public string SourceCurrencyName { get; set; }
        public string SourceCurrencyIsoCode { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalSentAmount { get; set; }
        public decimal ServiceFee { get; set; }
        public string FormOfPaymentId { get; set; }
        public string FormOfPaymentName { get; set; }
        public decimal Payout { get; set; }
        public decimal SentAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string PayerBranchAddress { get; set; }
        public string PayerBranchPhone1 { get; set; }
        public string PayerBranchPhone2 { get; set; }
        public string ReferenceNumber { get; set; }
        public string SourceOfFunds { get; set; }
        public decimal? BackendFee { get; set; }
        public List<InvoiceStatus> InvoiceStatusTimeStamps { get; set; }
    }

    public class GetComplianceResponseModel
    {
        public decimal? Accumulation { get; set; }
        public bool SourceOfFundsRequired { get; set; }
        public bool IsSsnRequired { get; set; }
        public bool IsSecondIdRequired { get; set; }
    }

    public class TransactionList
    {
        public List<Results> Results { get; set; }
    }

    public class Results
    {
        public DateTime ReceiverDate { get; set; }
        public string BranchId { get; set; }
        public string ReceiveCountryIsoCode { get; set; }
        public string ReceiveCountryName { get; set; }
        public decimal ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public decimal ReceiverChangeRate { get; set; }
        public decimal ReceiverNetAmount { get; set; }
        public string InvoiceStatusId { get; set; }
        public decimal AgencyConversionRate { get; set; }
        public string TfPin { get; set; }
        public string ReceiveCurrencyIsoCode { get; set; }
        public string PaymentMode { get; set; }
        public string InvoiceStatus { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string PayoutLocationId { get; set; }
        public string PayerId { get; set; }
        public string ReferenceNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public List<InvoiceStatus> InvoiceStatusTimeStamps { get; set; }
    }

    public class TransactionStatus
    {
        public string JazzCashAccountNumber { get; set; }
        public string RequestId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
