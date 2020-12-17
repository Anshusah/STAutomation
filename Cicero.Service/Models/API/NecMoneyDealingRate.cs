using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class NecMoneyDealingRate
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Bank { get; set; }
        public string Rate { get; set; }
    }
    public class NecMoneyGetTodayRateViewModel
    {
        public string Rate { get; set; }
        public int StatusCode { get; set; }
        public string StatusDetail { get; set; }
    }
    public class RateViewModel
    {
        public string SourceCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public string Bank { get; set; }
        public int ModeOfPayment { get; set; }
        public string PayerId { get; set; }
    }
    public class NecBankViewModel
    {
        public string CountryIsoCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class NecBankBranchViewModel
    {
        public string CountryIsoCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string BankCode { get; set; }
        public string City { get; set; }

    }
}
