using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Cicero.Service.SimpleTransferEnums;
using static Cicero.Data.Enumerations;
using Cicero.Service.Helpers;
using Cicero.Service.Models;

namespace Cicero.Service.Services.API
{
    public interface IExchangeRateServices
    {
        Task<bool> CreateOrUpdateExchangeRate(SetSafkhanRateRequest request);
        Task<bool> CreateOrUpdateExchangeRateHistory(SetSafkhanRateRequest request);
        Task<bool> CreateOrUpdate(List<ExchangeRates> data);
        Task<bool> CreateOrUpdate(List<ExchangeRatesHistory> data);
        Task<List<ExchangeRates>> GetExchangerateList(string countryCode);
        bool CheckCurrencyCode(string fromCurrencyCode, string toCurrencyCode);
        bool CheckCountryCode(string fromCountryCode, string toCountryCode);
        bool CheckPaymentMode(int paymentMode);
        List<ExchangeRates> ExchangeRates(List<RateViewModel> data, string source);
        List<ExchangeRatesHistory> ExchangeRatesHistory(List<RateViewModel> data, string source);


    }

    public class ExchangeRateServices : IExchangeRateServices
    {
        private readonly SimpleTransferApplicationDbContext db;

        public ExchangeRateServices(SimpleTransferApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateOrUpdateExchangeRate(SetSafkhanRateRequest request)
        {
            try
            {
                var setNecRateRequest = new ExchangeRates();
                setNecRateRequest.DateTime = DateTime.Now;
                setNecRateRequest.ExchangeRate = request.ExchangeRateValue;
                setNecRateRequest.FromCurrencyCode = request.FromCurrencyCode;
             //   setNecRateRequest.Source = RateSupplierEnum.Safkhan.ToString();
                setNecRateRequest.ToCurrencyCode = request.ToCurrencyCode;
                setNecRateRequest.UpdatedBy = "System";
                setNecRateRequest.UpdatedOn = DateTime.Now;
                setNecRateRequest.FromCountryCode = request.FromCountryCode;
                setNecRateRequest.ToCountryCode = request.ToCountryCode;
                setNecRateRequest.ModeOfPayment = Convert.ToInt32(request.PaymentMode);
                setNecRateRequest.BankCode = request.BankCode;

                var exchangeRate = db.ExchangeRates.Where(x =>  x.FromCurrencyCode == request.FromCurrencyCode && x.ToCurrencyCode == request.ToCurrencyCode
                && x.FromCountryCode==request.FromCountryCode&& x.ToCountryCode==request.ToCountryCode &&x.BankCode==request.BankCode && x.ModeOfPayment==Convert.ToInt32(request.PaymentMode)).FirstOrDefault();
                if(exchangeRate != null)
                {
                    exchangeRate.ExchangeRate = setNecRateRequest.ExchangeRate;
                    exchangeRate.UpdatedOn = DateTime.Now;
                    db.Update(exchangeRate);
                    await db.SaveChangesAsync();
                    return true;
                }
                db.ExchangeRates.Add(setNecRateRequest);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdate(List<ExchangeRates> data)
        {
            try
            {
                var prevData = db.ExchangeRates.Where(x => x.Source == data.Select(y => y.Source).FirstOrDefault()
                && x.FromCountryCode== data.Select(y => y.FromCountryCode).FirstOrDefault()
                && x.ToCountryCode == data.Select(y => y.ToCountryCode).FirstOrDefault()
                ).ToList();
                db.ExchangeRates.RemoveRange(prevData);
                await db.SaveChangesAsync();

                db.ExchangeRates.AddRange(data);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdateExchangeRateHistory(SetSafkhanRateRequest request)
        {
            try
            {
                var exchangeRateHistory = new ExchangeRatesHistory();
                exchangeRateHistory.DateTime = DateTime.Now;
                exchangeRateHistory.ExchangeRate = request.ExchangeRateValue;
                exchangeRateHistory.FromCurrencyCode = request.FromCurrencyCode;
               // exchangeRateHistory.Source = RateSupplierEnum.Safkhan.ToString();
                exchangeRateHistory.ToCurrencyCode = request.ToCurrencyCode;
                exchangeRateHistory.UpdatedBy = "System";
                exchangeRateHistory.UpdatedOn = DateTime.Now;
                exchangeRateHistory.FromCountryCode = request.FromCountryCode;
                exchangeRateHistory.ToCountryCode = request.ToCountryCode;
                exchangeRateHistory.ModeOfPayment = Convert.ToInt32(request.PaymentMode);


                db.ExchangeRatesHistory.Add(exchangeRateHistory);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateOrUpdate (List<ExchangeRatesHistory> data)
        {
            try
            {
                db.ExchangeRatesHistory.AddRange(data);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ExchangeRates>> GetExchangerateList(string countryCode)
        {
            var exchangeRatesList = await db.ExchangeRates.Where(x=>x.ToCountryCode == countryCode && x.BankCode != "" && x.Source == RateSupplierEnum.Transfast.ToString()).ToListAsync();

            return exchangeRatesList;
        }

        public bool CheckCurrencyCode(string fromCurrencyCode, string toCurrencyCode)
        {
            if (db.CountryList.Where(x => x.CurrencyCode.ToUpper() == fromCurrencyCode.ToUpper()) != null)
            {
                if (db.CountryList.Where(x => x.CurrencyCode.ToUpper() == toCurrencyCode.ToUpper()) != null)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public bool CheckCountryCode(string fromCountryCode, string toCountryCode)
        {
            if (db.CountryList.Where(x => x.Code.ToUpper() == fromCountryCode.ToUpper()) != null)
            {
                if (db.CountryList.Where(x => x.Code.ToUpper() == toCountryCode.ToUpper()) != null)
                    return true;
                else
                    return false;
            }
            return false;
        }
        public bool CheckPaymentMode(int paymentMode)
        {
            return Enum.IsDefined(typeof(PayoutMode), paymentMode);
        }

        public List<ExchangeRates> ExchangeRates(List<RateViewModel> data, string source)
        {
            var exchangeRate = new List<ExchangeRates>();
            foreach (var item in data)
            {
                int supplierId = (int)Enum.Parse(typeof(SchedulerList), source);
                var bankExists = db.SupplierBank.Where(x => x.BankName.ToUpper().Replace(" ", "") == item.Bank.ToUpper().Replace(" ", "") && x.SupplierId == supplierId && x.CountryCode == item.DestinationCountry).FirstOrDefault();
                var bankCode = bankExists == null ? "" : bankExists.BankCode;
                exchangeRate.Add(new ExchangeRates
                {
                    DateTime = DateTime.Now,
                    Bank = item.Bank,
                    BankCode= /*(supplierId==(int)SchedulerList.NecMoney)?bankCode:*/ item.PayerId,
                    ModeOfPayment = item.ModeOfPayment,
                    ExchangeRate = item.Rate,
                    FromCountryCode = item.SourceCountry,
                    ToCountryCode = item.DestinationCountry,
                    FromCurrencyCode = "GBP",
                    ToCurrencyCode = item.Currency,
                    Source = source,
                    UpdatedBy = "System",
                    UpdatedOn = DateTime.Now,
                    IsActive=true
                });
            }
            return exchangeRate;
        }

        public List<ExchangeRatesHistory> ExchangeRatesHistory(List<RateViewModel> data, string source)
        {
            var exchangeRateHistory = new List<ExchangeRatesHistory>();
            foreach (var item in data)
            {
                exchangeRateHistory.Add(new ExchangeRatesHistory
                {
                    DateTime = DateTime.Now,
                    Bank = item.Bank,
                    ModeOfPayment = item.ModeOfPayment,
                    ExchangeRate = item.Rate,
                    FromCountryCode = item.SourceCountry,
                    ToCountryCode = item.DestinationCountry,
                    FromCurrencyCode = "GBP",
                    ToCurrencyCode = item.Currency,
                    Source = source,
                    UpdatedBy = "System",
                    UpdatedOn = DateTime.Now
                });
            }
            return exchangeRateHistory;
        }
    }
}
