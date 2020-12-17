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
using Cicero.Service.Models.SimpleTransfer.BankSetting;
using Cicero.Service.Models.Core;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IBankSettingService
    {
        DTResponseModel GetBankSettingListByFilter(DTPostModel model);
        //  List<SelectListItem> GetBankSettingList();
        Task<BankSettingViewModel> GetBankSettingByIdAsync(int id);
        Task<BankSettingViewModel> CreateOrUpdate(BankSettingViewModel model);
        // Task<bool> DeleteBankSettingById(string id);
        //  bool RemoveBankSettingById(string id);
        Task<bool> ActiveBankSettingById(int id);
        Task<bool> InactiveBankSettingById(int id);
        //  bool IsDuplicateName(string DisplayName, string Id);

    }
    public class BankSettingService : IBankSettingService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IBankSettingService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public BankSettingService(SimpleTransferApplicationDbContext _db, ILogger<IBankSettingService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetBankSettingListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
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
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            var BankSettingList = (from c in db.SupplierBank
                                   select new
                                   {
                                       id = c.Id,
                                       name = c.BankName,
                                       code = c.BankCode,
                                       city = db.SupplierBankBranch.Where(x=>x.BankCode == c.BankCode).Join(db.SupplierCity, bb => bb.CityCode, sc => sc.CityCode, (bb, sc) => new { bb, sc}).Select(x=>x.sc.CityName).FirstOrDefault(),
                                       country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                       created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                       updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                       status = (c.Status) ? "Active" : "Inactive",
                                       action = "<a href='/admin/BankSetting/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Bank Setting' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Bank Setting</span></a>"
                                   });
            if (!String.IsNullOrEmpty(searchBy))
            {
                BankSettingList = BankSettingList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()) || o.code.ToLower().Contains(searchBy.ToLower()) || o.country.ToLower().Contains(searchBy.ToLower()) || o.created_at.ToLower().Contains(searchBy.ToLower()) || o.updated_at.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = BankSettingList.Count();
            BankSettingList = BankSettingList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = BankSettingList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<BankSettingViewModel> CreateOrUpdate(BankSettingViewModel model)
        {
            SupplierBank BankSetting = new SupplierBank
            {
                //SupplierId = (int)RateSupplierEnum.NecMoney,
                SupplierId = (int)RateSupplierEnum.Transfast,
                CountryCode = model.CountryCode.ToUpper(),
                BankCode = model.BankCode.ToUpper(),
                BankName = model.BankName.ToUpper(),
                CreatedBy = model.CreatedBy,
                CreatedAt = Convert.ToDateTime(model.CreatedAt),
                UpdatedAt = DateTime.Now,
                Status = true
            };

            if (model.Id == 0)
            {
                BankSetting.CreatedAt = DateTime.Now;
                BankSetting.CreatedBy = commonService.getLoggedInUserId();
                db.SupplierBank.Add(BankSetting);
                await db.SaveChangesAsync();

                return Mapper.Map<BankSettingViewModel>(BankSetting);
            }
            else
            {
                var BankSettingData = Mapper.Map<SupplierBank>(BankSetting);

                db.SupplierBank.Attach(BankSettingData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<BankSettingViewModel> GetBankSettingByIdAsync(int id)
        {
            var BankSettingList = await (from c in db.SupplierBank
                                         where c.Id == id
                                         select c).FirstOrDefaultAsync();

            return Mapper.Map<SupplierBank, BankSettingViewModel>(BankSettingList);
        }


        public async Task<bool> ActiveBankSettingById(int id)
        {
            var BankSetting = await db.SupplierBank.FindAsync(id);
            if (BankSetting != null)
            {
                BankSetting.Status = true;
                var result = db.SupplierBank.Update(BankSetting);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Bank changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/BankSetting/" + utils.EncryptId(BankSetting.Id) + "/edit.html'>" + BankSetting.BankName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BankSettingService - ActiveBankSettingById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveBankSettingById(int id)
        {
            var BankSetting = await db.SupplierBank.FindAsync(id);
            if (BankSetting != null)
            {
                BankSetting.Status = false;
                var result = db.SupplierBank.Update(BankSetting);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Bank changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/BankSetting/" + utils.EncryptId(BankSetting.Id) + "/edit.html'>" + BankSetting.BankName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("BankSettingService - InactiveBankSettingById - " + id + " - : ");
            return false;
        }
    }
}
