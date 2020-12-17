using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.SecureTrading
{
    public class SecureTradingPaymentRequest
    {
        public string alias { get; set; }
        public string version { get; set; }
        public PaymentRequest request { get; set; }
    }
    public class SecureTradingRefundRequest
    {
        public string alias { get; set; }
        public string version { get; set; }
        public RefundRequest request { get; set; }
    }
    public class PaymentRequest
    {
        public string currencyiso3a { get; set; }
        public string requesttypedescriptions { get; set; }
        public string sitereference { get; set; }
        public string baseamount { get; set; }
        public string orderreference { get; set; }
        public string accounttypedescription { get; set; }
        public string pan { get; set; }
        public string expirydate { get; set; }
        public string securitycode { get; set; }

    }
    public class RefundRequest
    {
        public string requesttypedescriptions { get; set; }
        public string sitereference { get; set; }
        public string parenttransactionreference { get; set; }

    }
    public class SecureTradingPaymentResonse
    {
        public string requestreference { get; set; }
        public string version { get; set; }
        public object[] response { get; set; }
        public string secrand { get; set; }

    }
    public class PaymentResponse
    {
        public string transactionstartedtimestamp { get; set; }
        public string livestatus { get; set; }
        public string issuer { get; set; }
        public string splitfinalnumber { get; set; }
        public string dccenabled { get; set; }
        public string settleduedate { get; set; }
        public string errorcode { get; set; }
        public string baseamount { get; set; }
        public string tid { get; set; }
        public string merchantnumber { get; set; }
        public string securityresponsepostcode { get; set; }
        public string transactionreference { get; set; }
        public string merchantname { get; set; }
        public string paymenttypedescription { get; set; }
        public string orderreference { get; set; }
        public string accounttypedescription { get; set; }
        public string acquirerresponsecode { get; set; }
        public string requesttypedescription { get; set; }
        public string securityresponsesecuritycode { get; set; }
        public string currencyiso3a { get; set; }
        public string authcode { get; set; }
        public string errormessage { get; set; }
        public string operatorname { get; set; }
        public string merchantcountryiso2a { get; set; }
        public string maskedpan { get; set; }
        public string securityresponseaddress { get; set; }
        public string issuercountryiso2a { get; set; }
        public string settlestatus { get; set; }

    }
    public class Operation
    {
        public string accounttypedescription { get; set; }
        public string authmethod { get; set; }
        public string credentialsonfile { get; set; }
        public string initiationreason { get; set; }
        public string parenttransactionreference { get; set; }
        public string requesttypedescriptions { get; set; }
        public string sitereference { get; set; }
    }
    public class Payment
    {
        public string baseamount { get; set; }
        public string currencyiso3a { get; set; }
        public string expirydate { get; set; }
        public string pan { get; set; }
        public string paymenttypedescription { get; set; }
        public string securitycode { get; set; }
    }
    public class Merchant
    {
        public string chargedescription { get; set; }
        public string merchantemail { get; set; }
        public string operatorname { get; set; }
        public string orderreference { get; set; }
    }
    public class Billing
    {
        public string billingpremise { get; set; }
        public string billingstreet { get; set; }
        public string billingtown { get; set; }
        public string billingcounty { get; set; }
        public string billingcountryiso2a { get; set; }
        public string billingpostcode { get; set; }
        public string billingemail { get; set; }
        public string billingtelephonetype { get; set; }
        public string billingtelephone { get; set; }
        public string billingprefixname { get; set; }
        public string billingfirstname { get; set; }
        public string billingmiddlename { get; set; }
        public string billinglastname { get; set; }
        public string billingsuffixname { get; set; }
    }
    public class CustomerDelivery
    {
        public string customerpremise { get; set; }
        public string customerstreet { get; set; }
        public string customertown { get; set; }
        public string customercounty { get; set; }
        public string customercountryiso2a { get; set; }
        public string customerpostcode { get; set; }
        public string customeremail { get; set; }
        public string customertelephonetype { get; set; }
        public string customertelephone { get; set; }
        public string customerprefixname { get; set; }
        public string customerfirstname { get; set; }
        public string customermiddlename { get; set; }
        public string customerlastname { get; set; }
        public string customersuffixname { get; set; }
        public string customerforwardedip { get; set; }
        public string customerip { get; set; }
    }
    public class Settlement 
    { 
        public string settleduedate { get; set; }
        public string settlestatus { get; set; }
    }

}
