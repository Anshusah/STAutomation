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
using MaritalStatus = Cicero.Data.Entities.SimpleTransfer.MaritalStatus;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IMaritalStatusSetupService
    {
        DTResponseModel GetMaritalStatusSetupListByFilter(DTPostModel model);
        Task<MaritalStatusSetupViewModel> GetMaritalStatusSetupByIdAsync(int id);
        Task<MaritalStatusSetupViewModel> CreateOrUpdate(MaritalStatusSetupViewModel model);
        List<SelectListItem> GetMaritalStatusAsync(string tenantId);

        Task<bool> ActiveMaritalStatusSetupById(int id);
        Task<bool> InActiveMaritalStatusSetupById(int id);
        Task<bool> DeleteMaritalStatusSetupById(int id);
        bool CheckDuplicate(MaritalStatusSetupViewModel model);

    }
    public class MaritalStatusSetupService : IMaritalStatusSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IMaritalStatusSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public MaritalStatusSetupService(SimpleTransferApplicationDbContext _db, ILogger<IMaritalStatusSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetMaritalStatusSetupListByFilter(DTPostModel model)
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

            var MaritalStatusSetupList = from c in db.MaritalStatus
                                    select new
                                    {
                                        id = c.Id,
                                        name = c.MaritalStatusName,
                                        tenantid=c.TenantId,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/MaritalStatusSetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit MaritalStatus' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit MaritalStatus</span></a>"
                                    };
            totalResultsCount = MaritalStatusSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                MaritalStatusSetupList = MaritalStatusSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            MaritalStatusSetupList = MaritalStatusSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = MaritalStatusSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<MaritalStatusSetupViewModel> CreateOrUpdate(MaritalStatusSetupViewModel model)
        {
            model.CreatedBy = model.UpdatedBy = commonService.getLoggedInUserId();
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            MaritalStatus maritalStatusSetup = new MaritalStatus
            {
                Id = model.Id,
                MaritalStatusName = model.MaritalStatusName,
                TenantId=model.TenantId,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status
            };

            if (model.Id == 0)
            {
                maritalStatusSetup.CreatedBy = maritalStatusSetup.UpdatedBy;
                maritalStatusSetup.CreatedDate = DateTime.Now;
                db.MaritalStatus.Add(maritalStatusSetup);
                await db.SaveChangesAsync();
                return Mapper.Map<MaritalStatusSetupViewModel>(maritalStatusSetup);
            }
            else
            {
                var maritalStatusSetupData = Mapper.Map<MaritalStatus>(maritalStatusSetup);
                db.MaritalStatus.Attach(maritalStatusSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<MaritalStatusSetupViewModel> GetMaritalStatusSetupByIdAsync(int id)
        {
            var MaritalStatusSetupList = await (from c in db.MaritalStatus
                                         where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<MaritalStatusSetupViewModel>(MaritalStatusSetupList);
        }

        public async Task<bool> ActiveMaritalStatusSetupById(int id)
        {
            var MaritalStatusSetup = await db.MaritalStatus.FindAsync(id);
            if (MaritalStatusSetup != null)
            {
                MaritalStatusSetup.Status = true;
                var result = db.MaritalStatus.Update(MaritalStatusSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "MaritalStatus changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/MaritalStatusSetup/" + utils.EncryptId(MaritalStatusSetup.Id) + "/edit.html'>" + MaritalStatusSetup.MaritalStatusName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("MaritalStatusService - ActiveMaritalStatusById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveMaritalStatusSetupById(int id)
        {
            var MaritalStatusSetup = await db.MaritalStatus.FindAsync(id);
            if (MaritalStatusSetup != null)
            {
                MaritalStatusSetup.Status = false;
                var result = db.MaritalStatus.Update(MaritalStatusSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "MaritalStatus changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/MaritalStatusSetup/" + utils.EncryptId(MaritalStatusSetup.Id) + "/edit.html'>" + MaritalStatusSetup.MaritalStatusName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("MaritalStatusService - ActiveMaritalStatusById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteMaritalStatusSetupById(int id)
        {
            var MaritalStatusSetup = await db.MaritalStatus.FindAsync(id);
            if (MaritalStatusSetup != null)
            {
                db.MaritalStatus.Remove(MaritalStatusSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary MaritalStatus deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/MaritalStatusSetup/" + utils.EncryptId(MaritalStatusSetup.Id) + "/edit.html'>" + MaritalStatusSetup.MaritalStatusName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("MaritalStatusService - ActiveMaritalStatusById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetMaritalStatusAsync(string tenantId)
        {
            return db.MaritalStatus.Where(x => x.TenantId == Convert.ToInt32(tenantId) && x.Status)
                .Select(x=>new SelectListItem() { Text=x.MaritalStatusName, Value=x.Id.ToString()}).ToList();
        }
        public bool CheckDuplicate(MaritalStatusSetupViewModel model)
        {
            if (db.MaritalStatus.Where(x => x.MaritalStatusName.ToUpper() == model.MaritalStatusName.ToUpper() && x.Id != model.Id).Any())
                return false;
            else
                return true;
        }
    }
}
