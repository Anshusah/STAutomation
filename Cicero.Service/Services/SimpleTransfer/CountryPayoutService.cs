using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.CountryPayout;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Cicero.Service.Models.General;
using static Cicero.Service.Extensions.Extensions;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ICountryPayoutService
    {
        DTResponseModel GetCountryPayoutListByFilter(DTPostModel model);
        Task<CountryPayoutViewModel> GetCountryPayoutByIdAsync(int id);
        Task<List<string>> GetCountryPayoutMethodByCountryAsync(string csb);
        Task<CountryPayoutViewModel> CreateOrUpdate(CountryPayoutViewModel model);
        Task<List<SelectListItem>> GetCountryList();
        Task<List<SelectListItem>> GetAllCountryList();

        Task<bool> CheckDuplicate(int id, string countryCode, int paymentMethodId);

        Task<bool> ActiveCountryPayoutById(int id);
        Task<bool> InActiveCountryPayoutById(int id);
        Task<bool> DeleteCountryPayoutById(int id);

        List<EnumViewModel> DefaulPaymentMethodFor();
    }
    public class CountryPayoutService : ICountryPayoutService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ICountryPayoutService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public CountryPayoutService(SimpleTransferApplicationDbContext _db, ILogger<ICountryPayoutService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetCountryPayoutListByFilter(DTPostModel model)
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

            var countryPayoutList = from c in db.CountryPayoutConfig
                                    select new
                                    {
                                        id = c.Id,
                                        country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                        paymentMethod = EnumModel<PayoutMode>.GetDescription(c.PaymentMethodId),
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.IsActive) ? "Active" : "Inactive",
                                        action = "<a href='/admin/countryPayout/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Country Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Country Payout</span></a>"
                                    };
            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                countryPayoutList = countryPayoutList.Where(o => o.country.ToLower().Contains(searchByLower) || o.paymentMethod.ToLower().Contains(searchByLower) || o.created_at.ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }

            totalResultsCount = countryPayoutList.Count();
            countryPayoutList = countryPayoutList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = countryPayoutList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<CountryPayoutViewModel> CreateOrUpdate(CountryPayoutViewModel model)
        {
            CountryPayoutConfig countryPayout = new CountryPayoutConfig
            {
                Id = model.Id,
                CountryCode = model.CountryCode,
                PaymentMethodId = model.PaymentMethodId,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                IsActive = model.IsActive
            };

            if (model.Id == 0)
            {
                countryPayout.CreatedBy = countryPayout.UpdatedBy;
                countryPayout.CreatedDate = countryPayout.UpdatedDate;
                db.CountryPayoutConfig.Add(countryPayout);
                await db.SaveChangesAsync();
                return Mapper.Map<CountryPayoutViewModel>(countryPayout);
            }
            else
            {
                var countryPayoutData = Mapper.Map<CountryPayoutConfig>(countryPayout);
                db.CountryPayoutConfig.Attach(countryPayoutData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<CountryPayoutViewModel> GetCountryPayoutByIdAsync(int id)
        {
            var countryPayoutList = await (from c in db.CountryPayoutConfig
                                           where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<CountryPayoutViewModel>(countryPayoutList);
        }

        public async Task<List<SelectListItem>> GetCountryList()
        {
            var countryList = await db.CountryList.Where(x => x.IsActive).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Code,
            }).ToListAsync();

            return countryList;
        }

        public async Task<List<SelectListItem>> GetAllCountryList()
        {
            var countryList = await db.CountryList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Code,
            }).ToListAsync();

            return countryList;
        }

        public async Task<bool> ActiveCountryPayoutById(int id)
        {
            var countryPayout = await db.CountryPayoutConfig.FindAsync(id);
            if (countryPayout != null)
            {
                countryPayout.IsActive = true;
                var result = db.CountryPayoutConfig.Update(countryPayout);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Country changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/CountryPayout/" + utils.EncryptId(countryPayout.Id) + "/edit.html'>" + countryPayout.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CountryService - ActiveCountryById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveCountryPayoutById(int id)
        {
            var countryPayout = await db.CountryPayoutConfig.FindAsync(id);
            if (countryPayout != null)
            {
                countryPayout.IsActive = false;
                var result = db.CountryPayoutConfig.Update(countryPayout);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Country changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/CountryPayout/" + utils.EncryptId(countryPayout.Id) + "/edit.html'>" + countryPayout.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CountryService - ActiveCountryById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteCountryPayoutById(int id)
        {
            var countryPayout = await db.CountryPayoutConfig.FindAsync(id);
            if (countryPayout != null)
            {
                db.CountryPayoutConfig.Remove(countryPayout);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Country deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/CountryPayout/" + utils.EncryptId(countryPayout.Id) + "/edit.html'>" + countryPayout.CountryCode + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CountryService - DeleteCountryById - " + id + " - : ");
            return false;
        }

        public List<EnumViewModel> DefaulPaymentMethodFor()
        {
            List<EnumViewModel> forpaymentmethod = (List<EnumViewModel>)EnumModel<PayoutMode>.List();
            return forpaymentmethod;

        }

        public async Task<List<string>> GetCountryPayoutMethodByCountryAsync(string csb)
        {
            var paymentMethod = await db.CountryPayoutConfig.Where(x => x.CountryCode == csb && x.IsActive).Select(x => EnumModel<PayoutMode>.GetDescription(x.PaymentMethodId)).ToListAsync();
            return paymentMethod;
        }

        public async Task<bool> CheckDuplicate(int id, string countryCode, int paymentMethodId)
        {
            var data = await db.CountryPayoutConfig.Where(x => x.Id != id && x.CountryCode == countryCode && x.PaymentMethodId == paymentMethodId).FirstOrDefaultAsync();
            if(data == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
