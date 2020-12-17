using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
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
using RateSupplierFeeConfig = Cicero.Data.Entities.SimpleTransfer.RateSupplierFeeConfig;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IRateSupplierFeeConfigService
    {
        DTResponseModel GetRateSupplierFeeConfigListByFilter(DTPostModel model);
        Task<RateSupplierFeeConfigViewModel> GetRateSupplierFeeConfigByIdAsync(int id);
        Task<RateSupplierFeeConfigViewModel> CreateOrUpdate(RateSupplierFeeConfigViewModel model);
        Task<bool> ActiveRateSupplierFeeConfigById(int id);
        Task<bool> InActiveRateSupplierFeeConfigById(int id);
        Task<bool> DeleteRateSupplierFeeConfigById(int id);
        Task<RateSupplierFeeConfigViewModel> GetRateSupplierFeeBySendAmountAsync(string sendAmount, string countryCode,int payoutModeId);
        bool CheckDuplicate(RateSupplierFeeConfigViewModel model);
    }
    public class RateSupplierFeeConfigService : IRateSupplierFeeConfigService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IRateSupplierFeeConfigService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public RateSupplierFeeConfigService(SimpleTransferApplicationDbContext _db, ILogger<IRateSupplierFeeConfigService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetRateSupplierFeeConfigListByFilter(DTPostModel model)
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

            var rateSupplierFeeConfigList = from c in db.RateSupplierFeeConfig.Where(x=>x.SupplierId == 1)
                                    select new
                                    {
                                        id = c.Id,
                                        upperlimit = c.UpperLimitAmount,
                                        lowerlimit=c.LowerLimitAmount,
                                        countryname = db.CountryList.Where(x=>x.Code == c.CountryCode).Select(x=>x.Name).FirstOrDefault(),
                                        suppliername= db.RateSupplier.Where(x=>x.Id== c.SupplierId).FirstOrDefault().Name,
                                        payoutmodename=db.PayoutModeConfig.Where(x=>x.Id== c.PayoutModeId).FirstOrDefault().PayoutModeName,
                                        tenantid=c.TenantId,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/RateSupplierFeeConfig/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit RateSupplierFeeConfig Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit RateSupplierFeeConfig</span></a>"
                                    };
            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                rateSupplierFeeConfigList = rateSupplierFeeConfigList.Where(o => o.countryname.ToLower().Contains(searchByLower) || o.payoutmodename.ToLower().Contains(searchByLower) || o.upperlimit.ToString().ToLower().Contains(searchByLower) || o.lowerlimit.ToString().ToLower().Contains(searchByLower) || o.created_at.ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }
            totalResultsCount = rateSupplierFeeConfigList.Count();
            rateSupplierFeeConfigList = rateSupplierFeeConfigList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);
            var list = rateSupplierFeeConfigList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<RateSupplierFeeConfigViewModel> CreateOrUpdate(RateSupplierFeeConfigViewModel model)
        {
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            RateSupplierFeeConfig rateSupplierFeeConfig = Mapper.Map<RateSupplierFeeConfigViewModel, RateSupplierFeeConfig>(model);
            rateSupplierFeeConfig.UpdatedBy = commonService.getLoggedInUserId();
            rateSupplierFeeConfig.CreatedDate = Convert.ToDateTime(model.CreatedDate);
            rateSupplierFeeConfig.UpdatedDate = DateTime.Now;

            if (model.Id == 0)
            {
                rateSupplierFeeConfig.CreatedBy = rateSupplierFeeConfig.UpdatedBy;
                rateSupplierFeeConfig.CreatedDate = DateTime.Now;
                db.RateSupplierFeeConfig.Add(rateSupplierFeeConfig);
                await db.SaveChangesAsync();

                return Mapper.Map<RateSupplierFeeConfigViewModel>(rateSupplierFeeConfig);
            }
            else
            {
                var RateSupplierFeeConfigData = Mapper.Map<RateSupplierFeeConfig>(rateSupplierFeeConfig);

                db.RateSupplierFeeConfig.Attach(RateSupplierFeeConfigData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<RateSupplierFeeConfigViewModel> GetRateSupplierFeeConfigByIdAsync(int id)
        {
            var RateSupplierFeeConfigList = await (from c in db.RateSupplierFeeConfig
                                         where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<RateSupplierFeeConfigViewModel>(RateSupplierFeeConfigList);
        }

        public async Task<bool> ActiveRateSupplierFeeConfigById(int id)
        {
            var RateSupplierFeeConfig = await db.RateSupplierFeeConfig.FindAsync(id);
            if (RateSupplierFeeConfig != null)
            {
                RateSupplierFeeConfig.Status = true;
                var result = db.RateSupplierFeeConfig.Update(RateSupplierFeeConfig);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "RateSupplierFeeConfig changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/RateSupplierFeeConfig/" + utils.EncryptId(RateSupplierFeeConfig.Id) + "/edit.html'>" + RateSupplierFeeConfig.CountryCode.ToString() + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RateSupplierFeeConfigService - ActiveRateSupplierFeeConfigById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveRateSupplierFeeConfigById(int id)
        {
            var RateSupplierFeeConfig = await db.RateSupplierFeeConfig.FindAsync(id);
            if (RateSupplierFeeConfig != null)
            {
                RateSupplierFeeConfig.Status = false;
                var result = db.RateSupplierFeeConfig.Update(RateSupplierFeeConfig);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "RateSupplierFeeConfig changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/RateSupplierFeeConfig/" + utils.EncryptId(RateSupplierFeeConfig.Id) + "/edit.html'>" + RateSupplierFeeConfig.CountryCode.ToString() + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RateSupplierFeeConfigService - ActiveRateSupplierFeeConfigById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteRateSupplierFeeConfigById(int id)
        {
            var RateSupplierFeeConfig = await db.RateSupplierFeeConfig.FindAsync(id);
            if (RateSupplierFeeConfig != null)
            {
                db.RateSupplierFeeConfig.Remove(RateSupplierFeeConfig);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary RateSupplierFeeConfig deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/RateSupplierFeeConfig/" + utils.EncryptId(RateSupplierFeeConfig.Id) + "/edit.html'>" + RateSupplierFeeConfig.CountryCode.ToString() + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RateSupplierFeeConfigService - ActiveRateSupplierFeeConfigById - " + id + " - : ");
            return false;
        }
        public async Task<RateSupplierFeeConfigViewModel> GetRateSupplierFeeBySendAmountAsync(string sendAmount, string countryCode,int payoutModeId)
        {
            try
            {
                var feeAmount = await (from c in db.RateSupplierFeeConfig
                                       where Convert.ToDecimal(sendAmount) >= Convert.ToDecimal(c.LowerLimitAmount) &&
                                       Convert.ToDecimal(sendAmount) <= Convert.ToDecimal(c.UpperLimitAmount) && c.CountryCode == countryCode
                                       && c.PayoutModeId == payoutModeId
                                       select c).FirstOrDefaultAsync();

                return Mapper.Map<RateSupplierFeeConfigViewModel>(feeAmount);
            }
            catch (Exception ex)
            {
                return new RateSupplierFeeConfigViewModel();
            }
           
        }
        public bool CheckDuplicate(RateSupplierFeeConfigViewModel model)
        {
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            if ((db.RateSupplierFeeConfig.Where(x => x.CountryCode == model.CountryCode
                                       && x.SupplierId == model.SupplierId
                                       && x.PayoutModeId == model.PayoutModeId
                                       && x.TenantId == model.TenantId
                                       && x.UpperLimitAmount == model.UpperLimitAmount
                                       && x.LowerLimitAmount == model.LowerLimitAmount && x.Id != model.Id).Any()) ||
                                       (db.RateSupplierFeeConfig.Where(x => x.CountryCode == model.CountryCode
                                       && x.SupplierId == model.SupplierId
                                       && x.PayoutModeId == model.PayoutModeId
                                       && x.TenantId == model.TenantId
                                       && x.UpperLimitAmount == model.UpperLimitAmount && x.Id != model.Id).Any()) ||
                                       (db.RateSupplierFeeConfig.Where(x => x.CountryCode == model.CountryCode
                                       && x.SupplierId == model.SupplierId
                                       && x.PayoutModeId == model.PayoutModeId
                                       && x.TenantId == model.TenantId
                                       && x.LowerLimitAmount == model.LowerLimitAmount && x.Id != model.Id).Any())
                                       )
                return false;
            else
                return true;
        }

    }
}
