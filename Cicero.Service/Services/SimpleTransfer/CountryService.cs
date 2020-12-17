using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Cicero.Service.Models;
using Cicero.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Data;
using Cicero.Service.Helpers;
using System.Security.Policy;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Service.Models.SimpleTransfer.Country;
using Cicero.Service.Models.Core;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ICountryService
    {
        DTResponseModel GetCountryListByFilter(DTPostModel model);
        //  List<SelectListItem> GetCountryList();
        Task<CountryViewModel> GetCountryByIdAsync(int id);
        List<SelectListItem> GetCountryByCodeAsync(string countryCode);

        Task<CountryViewModel> CreateOrUpdate(CountryViewModel model);
        Task<List<SelectListItemWithIcon>> GetCountryList();
        Task<List<SelectListItemWithIcon>> GetCountryListExceptUk(string countryCode);
        Task<List<SelectListItemWithIcon>> GetSenderCountryList();

        // Task<bool> DeleteCountryById(string id);
        //  bool RemoveCountryById(string id);
        Task<bool> ActiveCountryById(int id);
        Task<bool> InactiveCountryById(int id);
        Task<string> GetCountryCurrencyCode(string countryCode);
        Task<string> GetCountryName(string countryCode);
        //  bool IsDuplicateName(string DisplayName, string Id);
        List<SelectListItemWithIcon> GetCountryFromCurrenciesList();
        List<SelectListItemWithIcon> GetCountryToCurrenciesList(string countryCode);
        string GetPhoneCodeByCountryCode(string countryCode);
        Task<string> GetCountryCodeByUserId(string userId);
    }
    public class CountryService : ICountryService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ICountryService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public CountryService(SimpleTransferApplicationDbContext _db, ILogger<ICountryService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetCountryListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = false;

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

            var countryList = (from c in db.CountryList.OrderBy(x => x.Name)
                               select new
                               {
                                   id = c.Id,
                                   name = c.Name,
                                   code = c.Code,
                                   currencyCode = c.CurrencyCode,
                                   created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                   updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                   status = (c.IsActive) ? "Active" : "Inactive",
                                   action = "<a href = '/admin/country/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Country' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Country</span></a>"
                               });

            //
            //<div class='dropdown' title='More' data-toggle='tooltip' data-placement='top'><a data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='ri-more-fill'></i></a><div class='dropdown-menu dropdown-menu--custom dropdown-menu-right'>" +
            //                    "<a href='/admin/country/" + c.Id + "/edit.html' title='Edit User' class='dropdown-item' data-toggle='tooltip' data-placement='top'><span>Edit</span></a>" +
            //                    "<a href='' class='dropdown-item d-flex justify-content-between align-items-center'>Status<div class='custom-control custom-switch'>" +
            //                    "<input type = 'checkbox' class='custom-control-input' id='customSwitch1'>" +
            //                    "<label class='custom-control-label' for='customSwitch1'></label>" +
            //                    "</div> </a>" +
            //                    "<div class='dropdown-divider'></div>" +
            //                    "<a href= 'javascript:void(0)' class='dropdown-item' data-toggle='modal' data-target='#deleteCountry-01'>Delete</a>" +
            //                    "</div></div>


            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                countryList = countryList.Where(o => o.name.ToLower().Contains(searchByLower) || o.code.ToLower().Contains(searchByLower) || o.currencyCode.ToLower().Contains(searchByLower) || o.created_at.ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }

            totalResultsCount = countryList.Count();
            countryList = countryList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = countryList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<CountryViewModel> CreateOrUpdate(CountryViewModel model)
        {
            CountryList country = new CountryList
            {
                Id = model.Id,
                Code = model.Code,
                CurrencyCode = model.CurrencyCode,
                CurrencyName = model.CurrencyName,
                DisplayOrder = model.DisplayOrder,
                FlagImageUrl = model.FlagImageUrl,
                Name = model.Name,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                IsActive = model.Status
            };

            if (model.Id == 0)
            {
                country.CreatedBy = country.UpdatedBy;
                country.CreatedDate = DateTime.Now;
                db.CountryList.Add(country);
                await db.SaveChangesAsync();

                return Mapper.Map<CountryViewModel>(country);
            }
            else
            {
                var countryData = Mapper.Map<CountryList>(country);

                db.CountryList.Attach(countryData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<CountryViewModel> GetCountryByIdAsync(int id)
        {
            CountryList countryList = await (from c in db.CountryList
                                             where c.Id == id
                                             select c).FirstOrDefaultAsync();

            return Mapper.Map<CountryViewModel>(countryList);
        }

        public async Task<List<SelectListItemWithIcon>> GetCountryList()
        {
            var countryList = await db.CountryList.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).Select(x => new SelectListItemWithIcon
            {
                Text = x.Name,
                Value = x.Code,
                Icon = "flag-icon flag-icon-" + x.Code.ToLower()
            }).ToListAsync();

            return countryList;
        }

        public async Task<List<SelectListItemWithIcon>> GetCountryListExceptUk(string countryCode)
        {
            var countryList = await db.CountryList.Where(x => x.IsActive && NullToString(x.Code).ToLower() != "gb").OrderBy(x => x.DisplayOrder).Select(x => new SelectListItemWithIcon
            {
                Text = x.Name,
                Value = x.Code,
                Icon = "flag-icon flag-icon-" + x.Code.ToLower(),
                Selected = x.Code == countryCode ? true : false
            }).ToListAsync();

            return countryList;
        }

        static string NullToString(object Value)
        {
            return Value == null ? "" : Value.ToString();
        }

        public async Task<List<SelectListItemWithIcon>> GetSenderCountryList()
        {
            var countryList = await db.CountryList.Where(x => x.Code == "GB").Select(x => new SelectListItemWithIcon
            {
                Text = x.Name,
                Value = x.Code,
                Icon = "flag-icon flag-icon-" + x.Code.ToLower()
            }).ToListAsync();

            return countryList;
        }

        public async Task<List<SelectListItem>> GetAllCountryList()
        {
            var countryList = await db.CountryList.OrderBy(x => x.DisplayOrder).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Code,
            }).ToListAsync();
            return countryList;
        }
        public async Task<bool> ActiveCountryById(int id)
        {
            var country = await db.CountryList.FindAsync(id);
            if (country != null)
            {
                country.IsActive = true;
                var result = db.CountryList.Update(country);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Country changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/Country/" + utils.EncryptId(country.Id) + "/edit.html'>" + country.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CountryService - ActiveCountryById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveCountryById(int id)
        {
            var country = await db.CountryList.FindAsync(id);
            if (country != null)
            {
                country.IsActive = false;
                var result = db.CountryList.Update(country);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Country changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/Country/" + utils.EncryptId(country.Id) + "/edit.html'>" + country.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("CountryService - InactiveCountryById - " + id + " - : ");
            return false;
        }

        public async Task<string> GetCountryCurrencyCode(string countryCode)
        {
            var currencyCode = await db.CountryList.Where(x => x.Code == countryCode).Select(x => x.CurrencyCode).FirstOrDefaultAsync();
            return currencyCode;
        }

        public async Task<string> GetCountryName(string countryCode)
        {
            var countryName = await db.CountryList.Where(x => x.Code == countryCode).Select(x => x.Name).FirstOrDefaultAsync();
            return countryName;
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
        public List<SelectListItemWithIcon> GetCountryToCurrenciesList(string countryCode)
        {
            List<SelectListItemWithIcon> countryList = new List<SelectListItemWithIcon>();
            if (countryCode == null || countryCode == "")
            {
                countryList.Add(new SelectListItemWithIcon()
                {
                    Text = "BDT",
                    Value = "BD",
                    Icon = "flag-icon flag-icon-bd"
                });
                return countryList;
            }
            countryList = db.CountryList.Where(x => x.IsActive && x.Code.ToUpper() == countryCode.ToUpper()).Select(x => new SelectListItemWithIcon
            {
                Text = x.CurrencyCode.ToUpper(),
                Value = x.Code.ToUpper(),
                Icon = "flag-icon flag-icon-" + x.Code.ToLower()
            }).ToList();
            return countryList;
        }

        public List<SelectListItem> GetCountryByCodeAsync(string countryCode)
        {
            var countryList = db.CountryList.Where(x => x.IsActive && x.Code.ToUpper() != countryCode.ToUpper())
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Code }).ToList();

            var countrySelect = db.CountryList.Where(x => x.IsActive && x.Code.ToUpper() == countryCode.ToUpper()).FirstOrDefault();
            countryList.Add(new SelectListItem()
            {
                Text = countrySelect.Name,
                Value = countrySelect.Code,
                Selected = true
            });
            return countryList.OrderBy(x => x.Text).ToList();
        }

        public string GetPhoneCodeByCountryCode(string countryCode)
        {
            if (countryCode != null)
            {
                return db.CountryList.Where(x => x.IsActive && x.Code.ToUpper() == countryCode.ToUpper()).FirstOrDefault().CountryPhoneCode;
            }
            return "";
        }

        public async Task<string> GetCountryCodeByUserId(string userId)
        {
            var countryCode = await db.Customer.Where(x => x.UserId == userId).Select(x => x.CountryCode).FirstOrDefaultAsync();
            return countryCode;
        }
    }
}
