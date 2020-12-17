using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Data.Entities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.RateSupplier;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IRateSupplierService
    {
        DTResponseModel GetRateSupplierListByFilter(DTPostModel model);
        RateSupplierViewModel GetRateSupplierById(int id);
        RateSupplierViewModel GetRateSupplierByIdentifier(string id,int tenantid=0);
        Task<RateSupplierViewModel> CreateOrUpdate(RateSupplierViewModel avm);
        Task<List<RateSupplierViewModel>> GetRateSupplierListByParentId(int id);
        Task<bool> DeleteRateSupplierById(int id);
        Task<bool> ActiveRateSupplierById(int id);
        Task<bool> InactiveRateSupplierById(int id);
        List<SelectListItem> GetRateSupplierList();
        Task UpdateRatePriorityAsync(List<string> id, List<string> priority);
        Task<string> GetTopPriorityRateSupplier();
    }

    public class RateSupplierService: IRateSupplierService
    {

        private readonly SimpleTransferApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<RateSupplierService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        public RateSupplierService(SimpleTransferApplicationDbContext _db, Utils _utils, ILogger<RateSupplierService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            IMapper = _IMapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public DTResponseModel GetRateSupplierListByFilter(DTPostModel model)
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

            var RateSupplier =(from c in db.RateSupplier
                                                select new
                                                {
                                                    id = c.Id,
                                                    name = c.Name,
                                                    username = c.Username,
                                                    password = c.Password,
                                                    system_id=c.SystemId,
                                                    created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                                    updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                                    status = (c.IsActive) ? "Active" : "Inactive",
                                                    ratepriority=c.RatePriority,
                                                    //action = "<a href='/admin/ratesupplier/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Rate Supplier' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Rate Supplier</span></a>"
                                                });
            totalResultsCount = RateSupplier.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                RateSupplier = RateSupplier.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = RateSupplier.Count();
            RateSupplier = RateSupplier.Skip(skip).Take(take).OrderBy(sortBy, sortDir);
            var list = RateSupplier.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public RateSupplierViewModel GetRateSupplierById(int id)
        {
           try
            {
                var RateSupplierData = db.RateSupplier
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                    if(RateSupplierData==null){
                        return new RateSupplierViewModel { };
                    }
                var RateSupplier = IMapper.Map<RateSupplierViewModel>(RateSupplierData);
                
                return RateSupplier;
            }
            catch (Exception ex)
            {
                Log.LogError("RateSupplierService - GetRateSupplierById - " + ex);
            }

            return new RateSupplierViewModel { };
        }

        public async Task<List<RateSupplierViewModel>> GetRateSupplierListByParentId(int id)
        {
            try
            {

                var RateSupplier = await db.RateSupplier.ToListAsync();
                return IMapper.Map<List<RateSupplierViewModel>>(RateSupplier);
            }
            catch (Exception ex)
            {
                Log.LogError("RateSupplierService - GetRateSupplierListByParentId - " + ex);
            }

            return new List<RateSupplierViewModel> { };
        }

        public RateSupplierViewModel GetRateSupplierByIdentifier(string id,int tenantid=0)
        {
            if (tenantid == 0)
            {
                if(commonService==null) commonService = IHttpContextAccessor.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            }
            var RateSupplierData = db.RateSupplier.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
            if (RateSupplierData != null)
            {
                var RateSupplier = IMapper.Map<RateSupplierViewModel>(RateSupplierData);

                return RateSupplier;
            }
            else
            {
                return new RateSupplierViewModel { };
            }
        }

        private string GetSlug(string type, string slug, string title)
        {
            Regex re = new Regex("(?:[^a-z0-9]|(?<=['\"])s)");

            if (type != "template")
            {
                if (string.IsNullOrEmpty(slug))
                {
                    slug = re.Replace(title.ToLower(), "-");
                }
                else
                {
                    slug = re.Replace(slug, "-");
                }
            }
            return slug;
        }

        private bool CheckDuplicateSlug(string slug, int id, int tenantid)
        {
            var check = db.RateSupplier.Where(x => x.Id != id ).FirstOrDefault();

            if (check != null)
            {
                
                return true;
            }
            return false;
        }

        private async Task<bool> Update(RateSupplier model)
        {
            try
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;  
                db.RateSupplier.Attach(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "RateSupplier updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/RateSupplier/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Name + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ", (model.Id >0) ? (int)model.Id : 0);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<RateSupplierViewModel> CreateOrUpdate(RateSupplierViewModel avm)
        {

            avm.UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);
            var model = IMapper.Map<RateSupplier>(avm);

            model.UpdatedBy = commonService.getLoggedInUserId();
                await Update(model);
           

            return avm;
        }

        public async Task<bool> DeleteRateSupplierById(int id)
        {
            var RateSupplier = await db.RateSupplier.FindAsync(id);
            string title = RateSupplier.Name;
            if (RateSupplier != null)
            {
                db.RateSupplier.Remove(RateSupplier);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "RateSupplier Deleted " + title + ". Deleted By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RateSupplierService - DeleteRateSupplierById - " + id + " - : ");
            return false;
        }

        public async Task<bool> ActiveRateSupplierById(int id)
        {
            var RateSupplier = await db.RateSupplier.FindAsync(id);
            if (RateSupplier != null)
            {
                RateSupplier.IsActive =true;
                var result = db.RateSupplier.Update(RateSupplier);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "RateSupplier changed to Active <a href ='/admin" + Utils.GetTenantForUrl(false) + "/RateSupplier/" + Utils.EncryptId(RateSupplier.Id) + "/edit.html'>" + RateSupplier.Name + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("RateSupplierService - ActiveRateSupplierById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveRateSupplierById(int id)
        {
            var RateSupplier = await db.RateSupplier.FindAsync(id);
            if (RateSupplier != null)
            {
                RateSupplier.IsActive = false;
                var result = db.RateSupplier.Update(RateSupplier);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "RateSupplier changed to InActive <a href ='/admin" + Utils.GetTenantForUrl(false) + "/RateSupplier/" + Utils.EncryptId(RateSupplier.Id) + "/edit.html'>" + RateSupplier.Name + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("RateSupplierService - InactiveRateSupplierById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetRateSupplierList()
        {
            var tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            return db.RateSupplier.Select(x => new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Name
                                    }).ToList();
        }

        public async Task UpdateRatePriorityAsync(List<string> id, List<string> priority)
        {
            try
            {
                foreach (var (value, index) in id.Select((v, i) => (v, i)))
                {
                    var RateSupplier = await db.RateSupplier.FindAsync(Convert.ToInt32(value));
                    if (RateSupplier != null)
                    {
                        RateSupplier.RatePriority = Convert.ToInt32(priority[index]);
                        RateSupplier.UpdatedBy= commonService.getLoggedInUserId();
                        RateSupplier.UpdatedAt = DateTime.Now;
                        db.RateSupplier.Update(RateSupplier);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async Task<string> GetTopPriorityRateSupplier()
        {
            var rateSupplier = await db.RateSupplier.Where(x => x.RatePriority == 1).Select(x => x.Name).FirstOrDefaultAsync();

            return rateSupplier;
        }
    }
}