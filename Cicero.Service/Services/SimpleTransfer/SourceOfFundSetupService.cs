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
using SourceOfFund = Cicero.Data.Entities.SimpleTransfer.SourceOfFund;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ISourceOfFundSetupService
    {
        DTResponseModel GetSourceOfFundSetupListByFilter(DTPostModel model);
        Task<SourceOfFundSetupViewModel> GetSourceOfFundSetupByIdAsync(int id);
        Task<SourceOfFundSetupViewModel> CreateOrUpdate(SourceOfFundSetupViewModel model);
        bool CreateOrUpdateTransfastSOF(List<TransfastBankModel> model);

        List<SelectListItem> GetSourceOfFundAsync(string tenantId);
        List<SelectListItem> GetTransfastSourceOfFundAsync(int transfastId);

        Task<bool> ActiveSourceOfFundSetupById(int id);
        Task<bool> InActiveSourceOfFundSetupById(int id);
        Task<bool> DeleteSourceOfFundSetupById(int id);
        bool CheckDuplicate(SourceOfFundSetupViewModel model);
    }
    public class SourceOfFundSetupService : ISourceOfFundSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ISourceOfFundSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public SourceOfFundSetupService(SimpleTransferApplicationDbContext _db, ILogger<ISourceOfFundSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetSourceOfFundSetupListByFilter(DTPostModel model)
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

            var SourceOfFundSetupList = from c in db.SourceOfFund
                                    select new
                                    {
                                        id = c.Id,
                                        name = c.SourceOfFundName,
                                        tenantid=c.TenantId,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/SourceOfFundSetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit SourceOfFund Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit SourceOfFund</span></a>"
                                    };
            totalResultsCount = SourceOfFundSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                SourceOfFundSetupList = SourceOfFundSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            SourceOfFundSetupList = SourceOfFundSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = SourceOfFundSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<SourceOfFundSetupViewModel> CreateOrUpdate(SourceOfFundSetupViewModel model)
        {
            model.CreatedBy = model.UpdatedBy = commonService.getLoggedInUserId();
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            SourceOfFund SourceOfFundSetup = new SourceOfFund
            {
                Id = model.Id,
                SourceOfFundName=model.SourceOfFundName,
                TenantId=model.TenantId,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status,
                TransfastId=model.TransfastId
            };

            if (model.Id == 0)
            {
                SourceOfFundSetup.CreatedBy = SourceOfFundSetup.UpdatedBy;
                SourceOfFundSetup.CreatedDate = DateTime.Now;
                db.SourceOfFund.Add(SourceOfFundSetup);
                await db.SaveChangesAsync();

                return Mapper.Map<SourceOfFundSetupViewModel>(SourceOfFundSetup);
            }
            else
            {
                var SourceOfFundSetupData = Mapper.Map<SourceOfFund>(SourceOfFundSetup);

                db.SourceOfFund.Attach(SourceOfFundSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<SourceOfFundSetupViewModel> GetSourceOfFundSetupByIdAsync(int id)
        {
            var SourceOfFundSetupList = await (from c in db.SourceOfFund
                                         where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<SourceOfFundSetupViewModel>(SourceOfFundSetupList);
        }

        public async Task<bool> ActiveSourceOfFundSetupById(int id)
        {
            var SourceOfFundSetup = await db.SourceOfFund.FindAsync(id);
            if (SourceOfFundSetup != null)
            {
                SourceOfFundSetup.Status = true;
                var result = db.SourceOfFund.Update(SourceOfFundSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "SourceOfFund changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/SourceOfFundSetup/" + utils.EncryptId(SourceOfFundSetup.Id) + "/edit.html'>" + SourceOfFundSetup.SourceOfFundName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SourceOfFundService - ActiveSourceOfFundById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveSourceOfFundSetupById(int id)
        {
            var SourceOfFundSetup = await db.SourceOfFund.FindAsync(id);
            if (SourceOfFundSetup != null)
            {
                SourceOfFundSetup.Status = false;
                var result = db.SourceOfFund.Update(SourceOfFundSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "SourceOfFund changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/SourceOfFundSetup/" + utils.EncryptId(SourceOfFundSetup.Id) + "/edit.html'>" + SourceOfFundSetup.SourceOfFundName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SourceOfFundService - ActiveSourceOfFundById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteSourceOfFundSetupById(int id)
        {
            var SourceOfFundSetup = await db.SourceOfFund.FindAsync(id);
            if (SourceOfFundSetup != null)
            {
                db.SourceOfFund.Remove(SourceOfFundSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, " SourceOfFund deleted <a href ='/admin" + utils.GetTenantForUrl(false) + "/SourceOfFundSetup/" + utils.EncryptId(SourceOfFundSetup.Id) + "/edit.html'>" + SourceOfFundSetup.SourceOfFundName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SourceOfFundService - ActiveSourceOfFundById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetSourceOfFundAsync(string tenantId)
        {
            return db.SourceOfFund.Where(x => x.TenantId == Convert.ToInt32(tenantId) && x.Status)
                .Select(x=>new SelectListItem() { Text=x.SourceOfFundName, Value=x.Id.ToString()}).ToList();
        }
        public List<SelectListItem> GetTransfastSourceOfFundAsync(int transfastId)
        {
            var sourceOfFund=db.SourceOfFund.Where(x=>x.Status &&x.TenantId==commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession())).Select(x => new SelectListItem() { Text = x.SourceOfFundName, Value = x.TransfastId.ToString() }).ToList();
            var transfastSourceOfFund= db.TransfastSourceOfFund.Where(x => x.Status&&x.TenantId == commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession()))
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            var newList = transfastSourceOfFund.Where(x => !sourceOfFund.Any(y => y.Value == x.Value && y.Value!=transfastId.ToString())).ToList();
            return newList;
        }
        public bool CheckDuplicate(SourceOfFundSetupViewModel model)
        {
            if (db.SourceOfFund.Where(x => x.SourceOfFundName.ToUpper() == model.SourceOfFundName.ToUpper() && x.Id != model.Id).Any())
                return false;
            else
                return true;
        }

        public bool CreateOrUpdateTransfastSOF(List<TransfastBankModel> model)
        {
            var data = Mapper.Map<List<TransfastSourceOfFund>>(model);
            data.ForEach(x => x.CreatedDate = DateTime.Now);
            data.ForEach(x => x.Status = true);
            data.ForEach(x => x.TenantId = Convert.ToInt32(commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession())));
            db.TransfastSourceOfFund.AddRange(data);
            db.SaveChanges();
            return true;
        }
        
    }
}
