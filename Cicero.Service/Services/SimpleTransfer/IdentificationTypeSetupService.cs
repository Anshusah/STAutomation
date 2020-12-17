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
using IdentificationType = Cicero.Data.Entities.SimpleTransfer.IdentificationType;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IIdentificationTypeSetupService
    {
        DTResponseModel GetIdentificationTypeSetupListByFilter(DTPostModel model);
        Task<IdentificationTypeSetupViewModel> GetIdentificationTypeSetupByIdAsync(int id);
        Task<IdentificationTypeSetupViewModel> CreateOrUpdate(IdentificationTypeSetupViewModel model);
        List<SelectListItem> GetIdentificationTypeAsync(string tenantId);
        Task<List<SelectListItem>> GetIdentificationType();

        Task<bool> ActiveIdentificationTypeSetupById(int id);
        Task<bool> InActiveIdentificationTypeSetupById(int id);
        Task<bool> DeleteIdentificationTypeSetupById(int id);
        bool CheckDuplicate(IdentificationTypeSetupViewModel model);
    }
    public class IdentificationTypeSetupService : IIdentificationTypeSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IIdentificationTypeSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public IdentificationTypeSetupService(SimpleTransferApplicationDbContext _db, ILogger<IIdentificationTypeSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetIdentificationTypeSetupListByFilter(DTPostModel model)
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

            var IdentificationTypeSetupList = from c in db.IdentificationType
                                              select new
                                              {
                                                  id = c.Id,
                                                  name = c.IdentificationTypeName,
                                                  code = c.Code,
                                                  tenantid = c.TenantId,
                                                  created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                                  updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                                  status = (c.Status) ? "Active" : "Inactive",
                                                  action = "<a href='/admin/IdentificationTypeSetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit IdentificationType Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit IdentificationType</span></a>"
                                              };
            totalResultsCount = IdentificationTypeSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                IdentificationTypeSetupList = IdentificationTypeSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            IdentificationTypeSetupList = IdentificationTypeSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = IdentificationTypeSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<IdentificationTypeSetupViewModel> CreateOrUpdate(IdentificationTypeSetupViewModel model)
        {
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            IdentificationType IdentificationTypeSetup = new IdentificationType
            {
                Id = model.Id,
                IdentificationTypeName = model.IdentificationTypeName,
                TenantId = model.TenantId,
                Code = model.Code.ToUpper(),
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status
            };

            if (model.Id == 0)
            {
                IdentificationTypeSetup.CreatedDate = DateTime.Now;
                IdentificationTypeSetup.CreatedBy = IdentificationTypeSetup.UpdatedBy;
                db.IdentificationType.Add(IdentificationTypeSetup);
                await db.SaveChangesAsync();

                return Mapper.Map<IdentificationTypeSetupViewModel>(IdentificationTypeSetup);
            }
            else
            {
                var IdentificationTypeSetupData = Mapper.Map<IdentificationType>(IdentificationTypeSetup);

                db.IdentificationType.Attach(IdentificationTypeSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<IdentificationTypeSetupViewModel> GetIdentificationTypeSetupByIdAsync(int id)
        {
            var IdentificationTypeSetupList = await (from c in db.IdentificationType
                                                     where c.Id == id
                                                     select c).FirstOrDefaultAsync();

            return Mapper.Map<IdentificationTypeSetupViewModel>(IdentificationTypeSetupList);
        }

        public async Task<bool> ActiveIdentificationTypeSetupById(int id)
        {
            var IdentificationTypeSetup = await db.IdentificationType.FindAsync(id);
            if (IdentificationTypeSetup != null)
            {
                IdentificationTypeSetup.Status = true;
                var result = db.IdentificationType.Update(IdentificationTypeSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "IdentificationType changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/IdentificationTypeSetup/" + utils.EncryptId(IdentificationTypeSetup.Id) + "/edit.html'>" + IdentificationTypeSetup.IdentificationTypeName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("IdentificationTypeService - ActiveIdentificationTypeById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveIdentificationTypeSetupById(int id)
        {
            var IdentificationTypeSetup = await db.IdentificationType.FindAsync(id);
            if (IdentificationTypeSetup != null)
            {
                IdentificationTypeSetup.Status = false;
                var result = db.IdentificationType.Update(IdentificationTypeSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "IdentificationType changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/IdentificationTypeSetup/" + utils.EncryptId(IdentificationTypeSetup.Id) + "/edit.html'>" + IdentificationTypeSetup.IdentificationTypeName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("IdentificationTypeService - ActiveIdentificationTypeById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteIdentificationTypeSetupById(int id)
        {
            var IdentificationTypeSetup = await db.IdentificationType.FindAsync(id);
            if (IdentificationTypeSetup != null)
            {
                db.IdentificationType.Remove(IdentificationTypeSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary IdentificationType deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/IdentificationTypeSetup/" + utils.EncryptId(IdentificationTypeSetup.Id) + "/edit.html'>" + IdentificationTypeSetup.IdentificationTypeName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("IdentificationTypeService - ActiveIdentificationTypeById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetIdentificationTypeAsync(string tenantId)
        {
            return db.IdentificationType.Where(x => x.TenantId == Convert.ToInt32(tenantId) && x.Status)
                .Select(x => new SelectListItem() { Text = x.IdentificationTypeName, Value = x.Id.ToString() }).ToList();
        }

        public async Task<List<SelectListItem>> GetIdentificationType()
        {
            return await db.IdentificationType.Where(x => x.Status)
                .Select(x => new SelectListItem() { Text = x.IdentificationTypeName, Value = x.Id.ToString() }).ToListAsync();
        }

        public bool CheckDuplicate(IdentificationTypeSetupViewModel model)
        {
            if (db.IdentificationType.Where(x => x.Code.ToUpper() == model.Code.ToUpper() && x.Id != model.Id).Any())
                return false;
            else
                return true;
        }
    }
}

