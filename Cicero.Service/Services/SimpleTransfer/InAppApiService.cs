using Cicero.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cicero.Service.Models.Core;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IInAppApiServiceServices
    {
        List<SelectListItemWithIcon> GetCountryFromCurrenciesList();
        List<SelectListItemWithIcon> GetCountryToCurrenciesList();
    }

    public class InAppApiServiceServices : IInAppApiServiceServices
    {
        private readonly SimpleTransferApplicationDbContext db;

        public InAppApiServiceServices(SimpleTransferApplicationDbContext db)
        {
            this.db = db;
        }
        public List<SelectListItemWithIcon> GetCountryFromCurrenciesList()
        {
            //var countryList = await db.CountryList.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).Select(x => new SelectListItemWithIcon
            //{
            //    Text = x.CurrencyCode,
            //    Value = x.Code,
            //    Icon = "flag-icon flag-icon-" + x.CurrencyCode.ToLower()
            //}).ToListAsync();
            var countryList = new List<SelectListItemWithIcon>();
            countryList.Add(new SelectListItemWithIcon()
            {
                Text = "GBP",
                Value = "GB",
                Icon = "flag-icon flag-icon-gb"
            });

            return countryList;
        }
        public List<SelectListItemWithIcon> GetCountryToCurrenciesList()
        {
        //    var countryList = await db.CountryList.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).Select(x => new SelectListItemWithIcon
        //    {
        //        Text = x.CurrencyCode,
        //        Value = x.Code,
        //        Icon = "flag-icon flag-icon-" + x.CurrencyCode.ToLower()
        //    }).ToListAsync();
            var countryList = new List<SelectListItemWithIcon>();
            countryList.Add(new SelectListItemWithIcon()
            {
                Text = "BDT",
                Value = "BD",
                Icon = "flag-icon flag-icon-bd"
            });
            return countryList;
        }



    }
}
