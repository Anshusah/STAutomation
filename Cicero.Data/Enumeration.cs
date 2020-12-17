using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Data
{
    public static class Enumerations
    {
        public enum EnumValidationFailureError
        {
            [Description("Login failed. Email or Password is incorrect.")]
            LOGIN_FAILED = 1000,
            [Description("Your account is not Active. Please contact Simple Transfer.")]
            USER_INACTIVE = 1001,
            [Description("Token has been expired.")]
            TOKEN_EXPIRED = 1002,
            [Description("Invalid User Token has been supplied.")]
            INVALID_USERTOKEN = 1003,
            [Description("AuthToken was not supplied in the header.")]
            AUTHTOKEN_REQUIRED = 1004,
            [Description("SQL internal server error has been encountered. Please contact the API service team.")]
            SQL_INTERNAL_SERVER_ERROR = 5000,
            [Description("An internal server error has been encountered. Please contact the API service team.")]
            INTERNAL_SERVER_ERROR = 5001,
            [Description("Cannot Refresh Auth_Token at the Moment. Please Log out the application.")]
            PLEASE_LOGOUT = 1005,
            [Description("Invalid credentials.")]
            INVALID_CREDENTIAL = 1006,
            [Description("Country Code is invalid.")]
            INVALID_COUNTRYCODE = 8000,
            [Description("Currency Code is invalid.")]
            INVALID_CURRENCYCODE = 8001,
            [Description("Payment Method is invalid.")]
            INVALID_PAYMENTMETHOD = 8002

        }

        public enum SanctionPep
        {
            [Description("Sanction")]   
            Sanction = 1,
            [Description("Pep")]
            Pep = 2
        }

        public enum PurposeOfRequest
        {
            [Description("Bill Sharing")]
            BillSharing = 1,
            [Description("Lend/Borrow")]
            LendBorrow = 2,
            [Description("Other")]
            Other = 3,
        }

        public enum TransfastTransactionStatus
        {
            [Description("R")]
            PendingRelease = 1,
            [Description("W")]
            Web = 2,
            [Description("X")]
            Prestore = 3,
            [Description("I")]
            InProcess = 4,
            [Description("T")]
            Transmit = 5,
            [Description("H")]
            Hold = 6,
            [Description("C")]
            Cancel = 7,
            [Description("P")]
            Paid = 8,
            [Description("S")]
            Escrow = 9,
        }

        //public enum SimpleTransferTransactionStatus
        //{
        //    [Description("N")]
        //    New = 1,
        //    [Description("NC")]
        //    NotCredited = 2,
        //    [Description("Cr")]
        //    Credited = 3,
        //    [Description("I")]
        //    Inprogress = 4,
        //    [Description("S")]
        //    Success = 5,
        //    [Description("F")]
        //    Failed = 6,
        //    [Description("RR")]
        //    RefundRequest = 7,
        //    [Description("R")]
        //    Refunded = 8,
        //    [Description("RPS")]
        //    Reprocessing = 9,
        //    [Description("RPD")]
        //    Reprocessed = 10,
        //    [Description("FPX")]
        //    FPXFailed = 11,
        //    [Description("RE")]
        //    RefundEnabled = 12,
        //    [Description("TE")]
        //    TransfastError = 13,
        //    [Description("C")]
        //    Cancel = 14,
        //    [Description("E")]
        //    Expired = 15,
        //    [Description("FCC")]
        //    FraudCheck = 16,
        //    [Description("KYC")]
        //    KYC = 17,
        //    [Description("SC")]
        //    Sanction = 18,
        //    [Description("CH")]
        //    ComplianceHold = 19,
        //    [Description("PF")]
        //    PaymentFailure = 20,
        //    [Description("FCF")]
        //    FraudCheckFailure = 21,
        //    [Description("KCF")]
        //    KYCCheckFailure = 22,
        //    [Description("RCC")]
        //    RulesCheck = 23,
        //    [Description("RCF")]
        //    RulesCheckFailure = 24,
        //    [Description("SCF")]
        //    SanctionCheckFailure = 25,
        //}

        public enum SimpleTransferTransactionStatus
        {
            [Description("N")]
            New = 1,
            [Description("BTN")]
            BTTrxNew = 2,
            [Description("TL")]
            TRXLive = 3,
            [Description("auth")]
            Authorised = 4,
            [Description("PF")]
            PaymentFailure = 5,
            [Description("TKY")]
            TRXKY = 6,
            [Description("FCC")]
            FraudCheck = 7,
            [Description("FCF")]
            FraudCheckFailed = 8,
            [Description("KYC")]
            KYC = 9,
            [Description("KCF")]
            KYCCheckFailed = 10,
            [Description("KCFD")]
            KYCCheckFailedDeclined = 11,
            [Description("RCC")]
            RulesCheck = 12,
            [Description("RCF")]
            RulesCheckFailed = 13,
            [Description("SC")]
            Sanction = 14,
            [Description("SCF")]
            SanctionCheckFailed = 15,
            [Description("CH")]
            ComplianceHold = 16,
            [Description("C")]
            Cancel = 17,
            [Description("E")]
            Expired = 18
        }

        public enum SimpleTransferTransactionManagementStatus
        {
            [Description("Transaction Placed")]
            New = 1,
            [Description("Bank Transfer Not Received")]
            BTTrxNew = 2,
            [Description("Transaction Live")]
            TRXLive = 3,
            [Description("Transaction Authorized")]
            Authorised = 4,
            [Description("Payment Failed")]
            PaymentFailure = 5,
            [Description("Bank Transfer Received")]
            TRXKY = 6,
            [Description("Fraud Check Completed")]
            FraudCheckComplete = 7,
            [Description("Fraud Check Failed")]
            FraudCheckFailed = 8,
            [Description("KYC Completed")]
            KYCComplete = 9,
            [Description("KYC Check Failed")]
            KYCCheckFailed = 10,
            [Description("KYC Check Failed Declined")]
            KYCCheckFailedDeclined = 11,
            [Description("Rules Check Completed")]
            RulesCheckComplete = 12,
            [Description("Rules Check Failed")]
            RulesCheckFailed = 13,
            [Description("Sanction Completed")]
            SanctionComplete = 14,
            [Description("Sanction Check Failed")]
            SanctionCheckFailed = 15,
            [Description("Compliance Held")]
            ComplianceHold = 16,
            [Description("Transaction Declined")]
            Cancel = 17,
            [Description("Transaction Expired")]
            Expired = 18
        }

        public enum PaymentRequestStatus
        {
            [Description("Payment Request Pending")]
            PaymentRequestPending = 1,
            [Description("Payment In Progress")]
            PaymentInProgress = 2,
            [Description("Payment Received")]
            PaymentReceived = 3
        }

        public enum TransactionType
        {
            [Description("Remittance")]
            Remittance = 1,
            [Description("Payment Request")]
            PaymentRequest = 2
        }

        public enum TransfastPayoutMode
        {
            [Description("Bank Transfer")]
            BankTransfer = 1,
            [Description("Cash Pickup")]
            CashPickup = 2,
            [Description("Mobile Cash")]
            MobileMoney = 3,
            [Description("Cash Card")]
            AirtimeTopup = 4
        }

        public enum TransfastApiPayoutMode
        {
            [Description("C")]
            BankTransfer = 1,
            [Description("2")]
            CashPickup = 2,
            [Description("Mobile Cash")]
            MobileMoney = 3,
            [Description("Cash Card")]
            AirtimeTopup = 4
        }

        public enum PayoutMode
        {
            [Description("Bank Transfer")]
            BankTransfer = 1,
            [Description("Cash Pickup")]
            CashPickup = 2,
            [Description("Mobile Money")]
            MobileMoney = 3,
            [Description("Airtime Topup")]
            AirtimeTopup = 4,
        }
        public enum PaymentMethod
        {
            [Description("Credit or Debit Card")]
            CardPayment = 1,
            [Description("Bank Transfer")]
            BankTransfer = 2
        }
        public enum RateSupplierEnum
        {
            [Description("Transfast")]
            Transfast = 1,
            //[Description("NecMoney")]
            //NecMoney = 2,
            //[Description("Safkhan")]
            //Safkhan = 3
        }
        public enum GenderEnum
        {
            [Description("Male")]
            Male = 1,
            [Description("Female")]
            Female = 2
        }

    }
}
