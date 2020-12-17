using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Helpers
{
    public class SetSafkhanRateRequest
    {
        public decimal? ExchangeRateValue { get; set; }
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }

        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }
        public string BankCode { get; set; }
        public string PaymentMode { get; set; }
    }
    public class SetSafkhanRateResponse : ValidationFailureError
    {
        [DataMember]
        public bool isSuccess;
        [DataMember]
        public string FromCountryCode { get; set; }
        [DataMember]
        public string ToCountryCode { get; set; }
        [DataMember]
        public decimal? ExchangeRateValue { get; set; }
        [DataMember]
        public string UpdatedOn { get; set; }


        public bool GetIsSuccess()
        {
            return isSuccess;
        }

        public void SetIsSuccess(bool value)
        {
            isSuccess = value;
        }
    }
}
