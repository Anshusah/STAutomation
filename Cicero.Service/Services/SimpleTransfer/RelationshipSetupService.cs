using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.RelationshipSetup;
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

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IRelationshipSetupService
    {
        DTResponseModel GetRelationshipSetupListByFilter(DTPostModel model);
        Task<RelationshipSetupViewModel> GetRelationshipSetupByIdAsync(int id);
        Task<RelationshipSetupViewModel> CreateOrUpdate(RelationshipSetupViewModel model);
        List<SelectListItem> GetRelationshipAsync(string tenantId);
        List<SelectListItem> GetRelationshipById(string id);
        Task<bool> ActiveRelationshipSetupById(int id);
        Task<bool> InActiveRelationshipSetupById(int id);
        Task<bool> DeleteRelationshipSetupById(int id);
        bool CheckDuplicate(RelationshipSetupViewModel model);

    }
    public class RelationshipSetupService : IRelationshipSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IRelationshipSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public RelationshipSetupService(SimpleTransferApplicationDbContext _db, ILogger<IRelationshipSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetRelationshipSetupListByFilter(DTPostModel model)
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

            var RelationshipSetupList = from c in db.BeneficiaryRelationship
                                        select new
                                        {
                                            id = c.Id,
                                            name = c.RelationshipName,
                                            tenantid = c.TenantId,
                                            created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                            updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                            status = (c.Status) ? "Active" : "Inactive",
                                            action = "<a href='/admin/relationshipsetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Relationship Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Relationship</span></a>"
                                        };
            totalResultsCount = RelationshipSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                RelationshipSetupList = RelationshipSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            RelationshipSetupList = RelationshipSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = RelationshipSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<RelationshipSetupViewModel> CreateOrUpdate(RelationshipSetupViewModel model)
        {
            BeneficiaryRelationship RelationshipSetup = new BeneficiaryRelationship
            {
                Id = model.Id,
                RelationshipName = model.RelationshipName,
                TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession()),
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status
            };

            if (model.Id == 0)
            {
                RelationshipSetup.CreatedBy = RelationshipSetup.UpdatedBy;
                RelationshipSetup.CreatedDate = DateTime.Now;
                db.BeneficiaryRelationship.Add(RelationshipSetup);
                await db.SaveChangesAsync();
                return Mapper.Map<RelationshipSetupViewModel>(RelationshipSetup);
            }
            else
            {
                var RelationshipSetupData = Mapper.Map<BeneficiaryRelationship>(RelationshipSetup);
                db.BeneficiaryRelationship.Attach(RelationshipSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<RelationshipSetupViewModel> GetRelationshipSetupByIdAsync(int id)
        {
            var RelationshipSetupList = await (from c in db.BeneficiaryRelationship
                                               where c.Id == id
                                               select c).FirstOrDefaultAsync();

            return Mapper.Map<RelationshipSetupViewModel>(RelationshipSetupList);
        }
        public List<SelectListItem> GetRelationshipById(string id)
        {
            var beneficiaryRelationship = db.BeneficiaryRelationship.Where(x => x.Status && x.Id != Convert.ToInt32(id)).Select(x => new SelectListItem()
            {
                Text = x.RelationshipName,
                Value = x.Id.ToString()
            }).ToList();

            var beneficiaryRelationshipSelect = db.BeneficiaryRelationship.Where(x => x.Status && x.Id == Convert.ToInt32(id)).Select(x => new SelectListItem()
            {
                Text = x.RelationshipName,
                Value = x.Id.ToString(),
                Selected = true
            }).ToList();

            beneficiaryRelationship.AddRange(beneficiaryRelationshipSelect);

            beneficiaryRelationship = beneficiaryRelationship.OrderBy(x => x.Text).ToList();

            beneficiaryRelationship.Insert(0, new SelectListItem()
            {
                Text = "Select Relationship",
                Value = ""
            });
            return beneficiaryRelationship;
        }

        public async Task<bool> ActiveRelationshipSetupById(int id)
        {
            var RelationshipSetup = await db.BeneficiaryRelationship.FindAsync(id);
            if (RelationshipSetup != null)
            {
                RelationshipSetup.Status = true;
                var result = db.BeneficiaryRelationship.Update(RelationshipSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Relationship changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/RelationshipSetup/" + utils.EncryptId(RelationshipSetup.Id) + "/edit.html'>" + RelationshipSetup.RelationshipName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RelationshipService - ActiveRelationshipById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveRelationshipSetupById(int id)
        {
            var RelationshipSetup = await db.BeneficiaryRelationship.FindAsync(id);
            if (RelationshipSetup != null)
            {
                RelationshipSetup.Status = false;
                var result = db.BeneficiaryRelationship.Update(RelationshipSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary relationship changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/RelationshipSetup/" + utils.EncryptId(RelationshipSetup.Id) + "/edit.html'>" + RelationshipSetup.RelationshipName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RelationshipService - ActiveRelationshipById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteRelationshipSetupById(int id)
        {
            var RelationshipSetup = await db.BeneficiaryRelationship.FindAsync(id);
            if (RelationshipSetup != null)
            {
                db.BeneficiaryRelationship.Remove(RelationshipSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary relationship deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/RelationshipSetup/" + utils.EncryptId(RelationshipSetup.Id) + "/edit.html'>" + RelationshipSetup.RelationshipName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RelationshipService - ActiveRelationshipById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetRelationshipAsync(string tenantId)
        {
            return db.BeneficiaryRelationship.Where(x => x.TenantId == Convert.ToInt32(tenantId) && x.Status)
                .Select(x => new SelectListItem() { Text = x.RelationshipName, Value = x.Id.ToString() }).ToList();
        }
        public bool CheckDuplicate(RelationshipSetupViewModel model)
        {
            if (db.BeneficiaryRelationship.Where(x => x.RelationshipName.ToUpper() == model.RelationshipName.ToUpper() && x.Id != model.Id).Any())
                return false;
            else
                return true;
        }
    }
}
