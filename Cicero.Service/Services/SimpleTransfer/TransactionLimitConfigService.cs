using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Cicero.Data.Entities.SimpleTransfer;
using Microsoft.EntityFrameworkCore;
using Cicero.Service.Models.SimpleTransfer.TransactionLimitConfig;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ITransactionLimitConfigService
    {
        DTResponseModel GetTransactionLimitConfigByFilter(DTPostModel model);
        Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByIdAsync(int id);
        Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByCountryCodeAsync(string countryCode);
        Task<TransactionLimitConfigViewModel> CreateOrUpdate(TransactionLimitConfigViewModel model);
        Task<List<SelectListItem>> GetAllCountryList();

        Task<bool> CheckDuplicate(int id, string countryCode);

        Task<bool> ActiveTransactionLimitConfigById(int id);
        Task<bool> InActiveTransactionLimitConfigById(int id);
        Task<bool> DeleteTransactionLimitConfigById(int id);
    }

    public class TransactionLimitConfigService : ITransactionLimitConfigService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ITransactionLimitConfigService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public TransactionLimitConfigService(SimpleTransferApplicationDbContext _db, ILogger<ITransactionLimitConfigService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetTransactionLimitConfigByFilter(DTPostModel model)
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

            var transactionLimitConfigList = from c in db.TransactionLimitConfig
                                             select new
                                             {
                                                 id = c.Id,
                                                 country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                                 limitamountpertxn = c.LimitAmountPerTxn,
                                                 limitamountperday = c.LimitAmountPerDay,
                                                 limitamountpermonth = c.LimitAmountPerMonth,
                                                 limitnoperday = c.LimitNoPerDay,
                                                 limitnopermonth = c.LimitNoPerMonth,
                                                 created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                                 updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                                 status = (c.Status) ? "Active" : "Inactive",
                                                 action = "<a href='/admin/transactionlimitconfig/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Transaction Limit Config' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Transaction Limit Config</span></a>"
                                             };
            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                transactionLimitConfigList = transactionLimitConfigList.Where(o => o.country.ToLower().Contains(searchByLower) || o.limitamountpertxn.ToString().ToLower().Contains(searchByLower) || o.limitamountperday.ToString().ToLower().Contains(searchByLower) || o.limitamountpermonth.ToString().ToLower().Contains(searchByLower) || o.limitnoperday.ToString().ToLower().Contains(searchByLower) || o.limitnopermonth.ToString().ToLower().Contains(searchByLower) || o.created_at.ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }

            totalResultsCount = transactionLimitConfigList.Count();
            transactionLimitConfigList = transactionLimitConfigList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = transactionLimitConfigList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<TransactionLimitConfigViewModel> CreateOrUpdate(TransactionLimitConfigViewModel model)
        {
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            TransactionLimitConfig transactionLimitConfig = new TransactionLimitConfig
            {
                Id = model.Id,
                TenantId = model.TenantId,
                CountryCode = model.CountryCode,
                LimitAmountPerDay = model.LimitAmountPerDay,
                LimitAmountPerMonth = model.LimitAmountPerMonth,
                LimitAmountPerTxn = model.LimitAmountPerTxn,
                LimitNoPerDay = model.LimitNoPerDay,
                LimitNoPerMonth = model.LimitNoPerMonth,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status
            };

            if (model.Id == 0)
            {
                transactionLimitConfig.CreatedBy = transactionLimitConfig.UpdatedBy;
                transactionLimitConfig.CreatedDate = transactionLimitConfig.UpdatedDate;
                db.TransactionLimitConfig.Add(transactionLimitConfig);
                await db.SaveChangesAsync();
                return Mapper.Map<TransactionLimitConfigViewModel>(transactionLimitConfig);
            }
            else
            {
                db.TransactionLimitConfig.Attach(transactionLimitConfig).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByIdAsync(int id)
        {
            var transactionLimitConfigList = await (from c in db.TransactionLimitConfig
                                                    where c.Id == id
                                                    select c).FirstOrDefaultAsync();

            return Mapper.Map<TransactionLimitConfigViewModel>(transactionLimitConfigList);
        }

        public async Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByCountryCodeAsync(string countryCode)
        {
            var transactionLimitConfigList = await (from c in db.TransactionLimitConfig
                                                    where c.CountryCode == countryCode
                                                    select c).FirstOrDefaultAsync();

            return Mapper.Map<TransactionLimitConfigViewModel>(transactionLimitConfigList);
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

        public async Task<bool> ActiveTransactionLimitConfigById(int id)
        {
            var transactionLimitConfig = await db.TransactionLimitConfig.FindAsync(id);
            if (transactionLimitConfig != null)
            {
                transactionLimitConfig.Status = true;
                var result = db.TransactionLimitConfig.Update(transactionLimitConfig);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Transaction Limit Config changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionLimitConfig/" + utils.EncryptId(transactionLimitConfig.Id) + "/edit.html'>" + transactionLimitConfig.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("TransactionLimitConfigService - ActiveTransactionLimitConfig - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveTransactionLimitConfigById(int id)
        {
            var transactionLimitConfig = await db.TransactionLimitConfig.FindAsync(id);
            if (transactionLimitConfig != null)
            {
                transactionLimitConfig.Status = false;
                var result = db.TransactionLimitConfig.Update(transactionLimitConfig);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Transaction Limit Config changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionLimitConfig/" + utils.EncryptId(transactionLimitConfig.Id) + "/edit.html'>" + transactionLimitConfig.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("TransactionLimitConfigService - InActiveTransactionLimitConfig - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteTransactionLimitConfigById(int id)
        {
            var transactionLimitConfig = await db.TransactionLimitConfig.FindAsync(id);
            if (transactionLimitConfig != null)
            {
                db.TransactionLimitConfig.Remove(transactionLimitConfig);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Transaction Limit Config Deleted <a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionLimitConfig/" + utils.EncryptId(transactionLimitConfig.Id) + "/edit.html'>" + transactionLimitConfig.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("TransactionLimitConfigService - DeleteTransactionLimitConfig - " + id + " - : ");
            return false;
        }

        public async Task<bool> CheckDuplicate(int id, string countryCode)
        {
            var data = await db.TransactionLimitConfig.Where(x => x.Id != id && x.CountryCode == countryCode).FirstOrDefaultAsync();
            if (data == null)
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
