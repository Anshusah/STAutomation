using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Cicero.Service
{
    public class SimpleTransferEnum
    {
        public enum SecureTradingErrorCode
        {
            Success=0,
            FieldError=30000,
            PaymentDeclinedByBank=70000,
            UnknownError1=60010,
            UnknownError2 = 60034,
            UnknownError3 = 99999
        }
        public enum SecureTradingSettleStatus
        {
            ScheduledToSettle = 0,
            ScheduledToSettle1 = 1,
            ScheduledToSettle2 = 10,
            Settled = 100,
            Suspended = 2,
            Cancelled = 3
        }
        public enum SecureTradingSecurityResponse
        {
            CorrectDetails = 2,
            UnableCustomerDetailCheck = 1,
            CustomerDetailNotSent = 0,
            IncorrectDetails = 4
        }
        public enum SecureTradingRequestType
        {
            [Description("ACCOUNTCHECK")]
            ACCOUNTCHECK,//Perform checks on the cardholder’s first line of address, the cardholder’s postcode and the security code, without reserving funds on the customer’s account.
            [Description("AUTH")]
            AUTH,//Performs a payment, which debits funds from the customer’s account.
            [Description("CURRENCYRATE")]
            CURRENCYRATE,//For calculating the transaction amounts in both the currency associated with your site and the local currency associated with the customer’s card. 
            [Description("ORDER")]
            ORDER,//Initiate a new payment with PayPal.
            [Description("ORDERDETAILS")]
            ORDERDETAILS,//Retrieve the order details from PayPal, after the customer has signed in to their account and agreed to the payment on PayPal’s servers.
            [Description("REFUND")]
            REFUND,//For transferring funds back to the customer.
            [Description("RISKDEC")]
            RISKDEC//To process requests to Protect Plus, our sophisticated counter-fraud service. It makes use of the industry’s largest negative database to perform a comprehensive suite of fraud assessments, including identity checks against the UK electoral roll and BT databases.
        }
        public enum SecureTradingAccountCheckResponse
        {
            NotGiven = 0,
            NotChecked = 1,
            Matched = 2,
            NotMatched = 4
        }
        public enum STPaymentRequestStatus
        {
            Pending=1,
            PayerAccept=2,
            PayerProcessed=3,
            BOApprove=4,
            PayeeReceive=5,
            Complete=6
        }
    }
}
