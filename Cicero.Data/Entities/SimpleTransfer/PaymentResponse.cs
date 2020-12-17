using Cicero.Data.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SecureTradingPaymentDetail : Entity<Guid>
    {
        public SecureTradingPaymentDetail()
        {
            Id = Guid.NewGuid();
        }
        public string requestreference { get; set; }
        public string version { get; set; }
        public string secrand { get; set; }
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
        public int CardDetailId { get; set; }
        public string securityresponseaddress { get; set; }
        public string issuercountryiso2a { get; set; }
        public string settlestatus { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool isRefund { get; set; }

    }
}
