using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using System;
using System.Linq;
using Cicero.Service.Extensions;
using static Cicero.Service.Extensions.Extensions;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IExchangeRateSettingService
    {
        DTResponseModel GetExchangeRateSettingListByFilter(DTPostModel model);
    }

    public class ExchangeRateSettingService : IExchangeRateSettingService
    {
        private readonly SimpleTransferApplicationDbContext db;

        public ExchangeRateSettingService(SimpleTransferApplicationDbContext db)
        {
            this.db = db;
        }

        public DTResponseModel GetExchangeRateSettingListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "updated_at";
            bool sortDir = true;

            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            int draw = 0;

            if (model != null)
            {
                searchBy = (model.search != null) ? model.search.value : null;
                take = model.length;
                skip = model.start;
                draw = model.draw;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "desc";
                }
            }

            var ExchangeRateSetting = from c in db.ExchangeRates.Where(x => x.BankCode != "")
                                      select new
                                      {
                                          id = c.Id,
                                          fromcountrycode = c.FromCountryCode,
                                          tocountrycode = c.ToCountryCode,
                                          fromcurrencycode = c.FromCurrencyCode,
                                          tocurrencycode = c.ToCurrencyCode,
                                          bankcode = c.BankCode,
                                          bankName = db.SupplierBank.Where(x=>x.BankCode == c.BankCode).Select(x=>x.BankName).FirstOrDefault(),
                                          paymentmode = EnumModel<TransfastPayoutMode>.GetDescription(c.ModeOfPayment),
                                          exchangerate = c.ExchangeRate,
                                          source = c.Source,
                                          updated_at = c.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                      };
            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                ExchangeRateSetting = ExchangeRateSetting.Where(o => o.fromcountrycode.ToLower().Contains(searchByLower) || o.tocountrycode.ToLower().Contains(searchByLower) || o.fromcurrencycode.ToLower().Contains(searchByLower) || o.tocurrencycode.ToLower().Contains(searchByLower) || o.bankcode.ToLower().Contains(searchByLower) || o.paymentmode.ToLower().Contains(searchByLower) || o.exchangerate.ToString().ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }

            totalResultsCount = ExchangeRateSetting.Count();
            ExchangeRateSetting = ExchangeRateSetting.OrderBy(sortBy, sortDir).Skip(skip).Take(take);

            var list = ExchangeRateSetting.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }


    }
}
