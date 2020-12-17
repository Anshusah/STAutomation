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
using Gender = Cicero.Data.Entities.SimpleTransfer.Gender;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IGenderSetupService
    {
        DTResponseModel GetGenderSetupListByFilter(DTPostModel model);
        Task<GenderSetupViewModel> GetGenderSetupByIdAsync(int id);
        Task<GenderSetupViewModel> CreateOrUpdate(GenderSetupViewModel model);
        List<SelectListItem> GetGenderAsync(string tenantId);
        List<SelectListItem> GetGenderByCode(string genderCode);

        Task<bool> ActiveGenderSetupById(int id);
        Task<bool> InActiveGenderSetupById(int id);
        Task<bool> DeleteGenderSetupById(int id);
        bool CheckDuplicate(GenderSetupViewModel model);
    }
    public class GenderSetupService : IGenderSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IGenderSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public GenderSetupService(SimpleTransferApplicationDbContext _db, ILogger<IGenderSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetGenderSetupListByFilter(DTPostModel model)
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

            var GenderSetupList = from c in db.Gender
                                    select new
                                    {
                                        id = c.Id,
                                        name = c.Name,
                                        code=c.Code,
                                        tenantid=c.TenantId,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/genderSetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Gender Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Gender</span></a>"
                                    };
            totalResultsCount = GenderSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                GenderSetupList = GenderSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            GenderSetupList = GenderSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = GenderSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<GenderSetupViewModel> CreateOrUpdate(GenderSetupViewModel model)
        {
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            Gender genderSetup = new Gender
            {
                Id = model.Id,
                Name=model.Name,
                Code=model.Code.ToUpper(),
                TenantId=model.TenantId,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status
            };

            if (model.Id == 0)
            {
                genderSetup.CreatedBy = genderSetup.UpdatedBy;
                genderSetup.CreatedDate = DateTime.Now;
                db.Gender.Add(genderSetup);
                await db.SaveChangesAsync();

                return Mapper.Map<GenderSetupViewModel>(genderSetup);
            }
            else
            {
                var genderSetupData = Mapper.Map<Gender>(genderSetup);

                db.Gender.Attach(genderSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<GenderSetupViewModel> GetGenderSetupByIdAsync(int id)
        {
            var genderSetupList = await (from c in db.Gender
                                         where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<GenderSetupViewModel>(genderSetupList);
        }

        public async Task<bool> ActiveGenderSetupById(int id)
        {
            var genderSetup = await db.Gender.FindAsync(id);
            if (genderSetup != null)
            {
                genderSetup.Status = true;
                var result = db.Gender.Update(genderSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Gender changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/GenderSetup/" + utils.EncryptId(genderSetup.Id) + "/edit.html'>" + genderSetup.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveGenderSetupById(int id)
        {
            var genderSetup = await db.Gender.FindAsync(id);
            if (genderSetup != null)
            {
                genderSetup.Status = false;
                var result = db.Gender.Update(genderSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Gender changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/GenderSetup/" + utils.EncryptId(genderSetup.Id) + "/edit.html'>" + genderSetup.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteGenderSetupById(int id)
        {
            var genderSetup = await db.Gender.FindAsync(id);
            if (genderSetup != null)
            {
                db.Gender.Remove(genderSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary Gender deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/GenderSetup/" + utils.EncryptId(genderSetup.Id) + "/edit.html'>" + genderSetup.Name + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetGenderAsync(string tenantId)
        {
            return db.Gender.Where(x => x.Status)
                .Select(x=>new SelectListItem() { Text=x.Name,Value=x.Code.ToString()}).ToList();
        }
        public List<SelectListItem> GetGenderByCode(string genderCode)
        {
            var gender = db.Gender.Where(x => x.Status && x.Code.ToUpper() != genderCode.ToUpper()).Select(x=>new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code
            }).ToList();

            var genderSelect = db.Gender.Where(x => x.Status && x.Code.ToUpper() == genderCode.ToUpper()).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code,
                Selected = true
            }).ToList();
          
            gender.AddRange(genderSelect);
            gender = gender.OrderBy(x => x.Text).ToList();

            gender.Insert(0, new SelectListItem()
            {
                Text = "Select Gender",
                Value = ""
            });
            return gender;
        }
        public bool CheckDuplicate(GenderSetupViewModel model)
        {
            if (db.Gender.Where(x => x.Code.ToUpper() == model.Code.ToUpper()&&x.Id!=model.Id).Any())
                return false;
            else
                return true;
        }
    }
}
