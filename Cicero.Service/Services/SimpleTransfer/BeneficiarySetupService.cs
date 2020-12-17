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
using Beneficiary = Cicero.Data.Entities.SimpleTransfer.Beneficiary;
using Cicero.Service.Models.SimpleTransfer.Beneficiary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IBeneficiarySetupService
    {
        DTResponseModel GetBeneficiarySetupListByFilter(DTPostModel model);
        JObject GetBeneficiaryDetailByIdAsync(int id);
        Task<BeneficiarySetupViewModel> GetBeneficiarySetupByIdAsync(int id);
        Task<BeneficiarySetupViewModel> CreateOrUpdate(BeneficiarySetupViewModel model);
        List<SelectListItem> GetBeneficiaryListAsync(string userId, string countryCode = null);

        Task<bool> ActiveBeneficiarySetupById(int id);
        Task<bool> InActiveBeneficiarySetupById(int id);
        Task<bool> DeleteBeneficiarySetupById(int id);
        Task<List<SelectListItem>> GetBeneficiaryByUserId(string userId);

    }
    public class BeneficiarySetupService : IBeneficiarySetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ApplicationDbContext applicationDb;

        private readonly ILogger<IBeneficiarySetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public BeneficiarySetupService(SimpleTransferApplicationDbContext _db, ILogger<IBeneficiarySetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils,
            ApplicationDbContext applicationDbContext)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            applicationDb = applicationDbContext;
        }

        public DTResponseModel GetBeneficiarySetupListByFilter(DTPostModel model)
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

            var BeneficiarySetupList = from c in db.Beneficiary
                                    select new
                                    {
                                        id = c.Id,
                                        name = c.FirstName,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/BeneficiarySetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Beneficiary Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Beneficiary</span></a>"
                                    };
            totalResultsCount = BeneficiarySetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                BeneficiarySetupList = BeneficiarySetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            BeneficiarySetupList = BeneficiarySetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = BeneficiarySetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<BeneficiarySetupViewModel> CreateOrUpdate(BeneficiarySetupViewModel model)
        {
            Beneficiary beneficiarySetup = Mapper.Map<BeneficiarySetupViewModel, Beneficiary>(model);
            beneficiarySetup.UpdatedBy = commonService.getLoggedInUserId();
            beneficiarySetup.CreatedDate = Convert.ToDateTime(model.CreatedDate);
            beneficiarySetup.UpdatedDate = DateTime.Now;
            beneficiarySetup.UserId = commonService.getLoggedInUserId();

            if (model.Id == 0)
            {
                beneficiarySetup.CreatedBy = beneficiarySetup.UpdatedBy;
                beneficiarySetup.CreatedDate = DateTime.Now;
                db.Beneficiary.Add(beneficiarySetup);
                await db.SaveChangesAsync();

                return Mapper.Map<BeneficiarySetupViewModel>(beneficiarySetup);
            }
            else
            {
                var BeneficiarySetupData = Mapper.Map<Beneficiary>(beneficiarySetup);

                db.Beneficiary.Attach(BeneficiarySetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public JObject GetBeneficiaryDetailByIdAsync(int id)
        {
            var beneList = applicationDb.CustomEntities.FromSql($"SELECT * FROM [BeneficiaryTemp] WHERE id={id}");
            JObject jsonObj = new JObject();
            foreach (var item in beneList)
            {
                jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
            }
            return jsonObj;
        }
        public async Task<BeneficiarySetupViewModel> GetBeneficiarySetupByIdAsync(int id)
        {
            var BeneficiarySetupList = await (from c in db.Beneficiary
                                              where c.Id == id
                                              select c).FirstOrDefaultAsync();

            return Mapper.Map<BeneficiarySetupViewModel>(BeneficiarySetupList);
        }
        public async Task<bool> ActiveBeneficiarySetupById(int id)
        {
            var BeneficiarySetup = await db.Beneficiary.FindAsync(id);
            if (BeneficiarySetup != null)
            {
                BeneficiarySetup.Status = true;
                var result = db.Beneficiary.Update(BeneficiarySetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/BeneficiarySetup/" + utils.EncryptId(BeneficiarySetup.Id) + "/edit.html'>" + BeneficiarySetup.FirstName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BeneficiaryService - ActiveBeneficiaryById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveBeneficiarySetupById(int id)
        {
            var BeneficiarySetup = await db.Beneficiary.FindAsync(id);
            if (BeneficiarySetup != null)
            {
                BeneficiarySetup.Status = false;
                var result = db.Beneficiary.Update(BeneficiarySetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/BeneficiarySetup/" + utils.EncryptId(BeneficiarySetup.Id) + "/edit.html'>" + BeneficiarySetup.FirstName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BeneficiaryService - ActiveBeneficiaryById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteBeneficiarySetupById(int id)
        {
            var BeneficiarySetup = await db.Beneficiary.FindAsync(id);
            if (BeneficiarySetup != null)
            {
                db.Beneficiary.Remove(BeneficiarySetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary Beneficiary deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/BeneficiarySetup/" + utils.EncryptId(BeneficiarySetup.Id) + "/edit.html'>" + BeneficiarySetup.FirstName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BeneficiaryService - ActiveBeneficiaryById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetBeneficiaryListAsync(string userId, string countryCode = null)
        {
            int tenantId = commonService.GetTenantIdByUserId(userId);          
            var beneList = applicationDb.CustomEntities.FromSql($"SELECT * FROM [BeneficiaryTemp] WHERE Tenantid={tenantId} and userid={userId}");
            List<SelectListItem> beneSelectItem = new List<SelectListItem>();
            foreach (var item in beneList)
            {
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                var countryId = jsonObj["CountryId"].ToString();
                if (countryCode != null && countryId != countryCode)
                {
                    continue;
                }
                beneSelectItem.Add(new SelectListItem()
                {
                    Text = jsonObj["FirstName"] + " " + jsonObj["LastName"],
                    Value = item.Id.ToString()
                });
            }
            return beneSelectItem;
        }

        public async Task<List<SelectListItem>> GetBeneficiaryByUserId(string userId)
        {
            var benelist = await db.Beneficiary.OrderBy(x => x.UserId==userId).Select(x => new SelectListItem
            {
                Text = x.FirstName + " "+x.MiddleName+" "+x.LastName,
                Value = x.Id.ToString(),
            }).ToListAsync();
            return benelist;
        }
    }
}
