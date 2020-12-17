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
using AutoMapper;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cicero.Service.Services
{
    public interface ITenantService
    {
        DTResponseModel GetTenantList(DTPostModel model);
        TenantViewModel GetTenantById(int id);
        Task<TenantViewModel> CreateOrUpdate(TenantViewModel tvm);
        Task<List<SelectListItem>> GetTenantListByUserIdAsync();
        int GetTenantIdByIdentifier(string identifier);
        bool IsDuplicateEmail(string email, int id);
        bool IsDuplicateIdentifier(string identifier, int id);
        Task<bool> DeleteTenantById(int id);
        Task<bool> ActiveTenantById(int id);
        Task<bool> InactiveTenantById(int id);
    }

    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TenantService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly IActivityLogService _activityLogService;
        private readonly Utils _utils;
        private readonly IArticleService _articleService;

        public TenantService(ApplicationDbContext db, ILogger<TenantService> Log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService, IActivityLogService activityLogService, Utils utils, IArticleService articleService)
        {
            _db = db;
            _Log = Log;
            _IHttpContextAccessor = httpContextAccessor;
            _HostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _commonService = commonService;
            _activityLogService = activityLogService;
            _utils = utils;
            _articleService = articleService;
        }

        public DTResponseModel GetTenantList(DTPostModel model)
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

            bool isAdmin = _commonService.IsSuperAdmin().Result;
            string loggedUser = (string)_commonService.getLoggedInUserId();
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var tenant = _db.Tenant
                            .Where(y => y.TenantUsers.Any(d => d.UserId == loggedUser && (d.TenantId == tenantid || tenantid == 0)) == true || (isAdmin == true && (tenantid == 0 || tenantid == y.Id)))
                            .Select(x => new
                            {
                                id = x.Id,
                                name = x.Name,
                                email = x.Email,
                                created_at = Utils.GetDefaultDateFormat(x.CreatedAt),
                                updated_at = Utils.GetDefaultDateFormat(x.UpdatedAt),
                                status = (x.Status == true) ? "Active" : "Inactive",
                                city = x.City,
                                action = "<a href='/admin" + _utils.GetTenantForUrl(false) + "/tenant/" + _utils.EncryptId(x.Id) + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Tenant' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Tenant</span></a>"
                            });

            totalResultsCount = tenant.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                tenant = tenant.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = tenant.Count();
            tenant = tenant.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = tenant.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public TenantViewModel GetTenantById(int id)
        {
            try
            {
                var vm = _mapper.Map<TenantViewModel>(_db.Tenant
                                                        .Where(x => x.Id == id).FirstOrDefault());

                return vm;
            }
            catch (Exception ex)
            {
                _Log.LogError("TenantService - GetTenantById - " + ex);
                return new TenantViewModel { };
            }
        }

        private async Task<int> Create(Tenant model)
        {

            string loggedUser = (string)_commonService.getLoggedInUserId();
            string fullName = _commonService.GetUserFullName().Result;

            model.Id = 0;
            model.CreatedBy = loggedUser;
            await _db.Tenant.AddAsync(model);

            await _db.SaveChangesAsync();

            await _articleService.CreateArticleForTenant(model.Id);

            await CreateSettingsForTenant(model.Id);

            await _activityLogService.CreateLog(loggedUser, "New Tenant created <a href ='/admin" + _utils.GetTenantForUrl(false) + "/tenant/" + _utils.EncryptId(model.Id) + "/edit.html'>" + model.Name + "</a>. Created By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

            return model.Id;
        }

        private async Task Update(Tenant model)
        {

            string loggedUser = (string)_commonService.getLoggedInUserId();
            string fullName = _commonService.GetUserFullName().Result;

            _db.Tenant.Attach(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "Tenant updated <a href ='/admin" + _utils.GetTenantForUrl(false) + "/tenant/" + _utils.EncryptId(model.Id) + "/edit.html'>" + model.Name + "</a>. Updated By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        public async Task<TenantViewModel> CreateOrUpdate(TenantViewModel tvm)
        {
            var model = _mapper.Map<Tenant>(tvm);

            if (tvm.Id == 0)
            {

                tvm.Id = await Create(model);
            }
            else
            {
                await Update(model);

            }

            return tvm;
        }

        public async Task<List<SelectListItem>> GetTenantListByUserIdAsync()
        {
            try
            {
                string id = (string)_commonService.getLoggedInUserId();
                if (await _commonService.IsSuperAdmin()==true)
                {
                    var tenants = await _db.Tenant.Select(b => new SelectListItem
                                            {
                                                Value = b.Identifier,
                                                Text = b.Name
                                            }).ToListAsync();
                    return tenants;
                }
                else
                {
                    var tenantUser = await _db.TenantUser.Where(x => x.UserId == id).Include(y => y.TenantForUser).Select(b => new SelectListItem
                    {
                        Value = b.TenantForUser.Identifier,
                        Text = b.TenantForUser.Name
                    }).ToListAsync();

                    if (tenantUser != null)
                    {
                        return tenantUser;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _Log.LogError("TenantService - GetTenantById - " + ex);
                return null;
            }

        }

        public int GetTenantIdByIdentifier(string identifier)
        {
            try
            {
                var result = _db.Tenant
                                .Where(x => x.Identifier == identifier)
                                .Select(b => new { b.Id})
                                .FirstOrDefault();

                if (result != null)
                {
                    return result.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _Log.LogError("TenantService - GetTenantById - " + ex);
                return 0;
            }
        }

        public bool IsDuplicateEmail(string email, int id)
        {

            if (id == 0)
            {
                return (!_db.Tenant.Any(d => d.Email == email));
            }
            else
            {
                return (!_db.Tenant.Any(d => d.Email == email && d.Id != id));
            }

        }

        public bool IsDuplicateIdentifier(string identifier, int id)
        {

            if (id == 0)
            {
                return (!_db.Tenant.Any(d => d.Identifier == identifier));
            }
            else
            {
                return (!_db.Tenant.Any(d => d.Identifier == identifier && d.Id != id));
            }
        }

        public async Task<bool> DeleteTenantById(int id)
        {
            var tvm = await _db.Tenant.FindAsync(id);
            string name = tvm.Name;
            if (tvm != null)
            {
                _db.Tenant.Remove(tvm);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Tenant Deleted " + name + ". Deleted By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            _Log.LogError("TenantService - DeleteTenantById - " + id + " - : ");
            return false;
        }

        public async Task<bool> ActiveTenantById(int id)
        {
            var tvm = await _db.Tenant.FindAsync(id);
            if (tvm != null)
            {
                tvm.Status = true;
                var result = _db.Tenant.Update(tvm);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Tenant changed to Active <a href ='/admin" + _utils.GetTenantForUrl(false) + "/tenant/" + _utils.EncryptId(tvm.Id) + "/edit.html'>" + tvm.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            _Log.LogError("TenantService - ActiveTenantById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveTenantById(int id)
        {
            var tvm = await _db.Tenant.FindAsync(id);
            if (tvm != null)
            {
                tvm.Status = false;
                var result = _db.Tenant.Update(tvm);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Tenant changed to Active <a href ='/admin" + _utils.GetTenantForUrl(false) + "/tenant/" + _utils.EncryptId(tvm.Id) + "/edit.html'>" + tvm.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            _Log.LogError("TenantService - InActiveTenantById - " + id + " - : ");
            return false;
        }

        private async Task CreateSettingsForTenant(int id)
        {
            var baseList = _db.Setting
                                .AsNoTracking()
                                .Where(x => x.TenantId == null && !x.FieldKey.Equals("app_themes", StringComparison.OrdinalIgnoreCase))
                                .OrderBy(b => b.Id)
                                .ToList().AsReadOnly();

            var saveList = baseList.Select(c => {
                                                c.Id = 0;
                                                c.TenantId = id;
                                                return c;
                                            }).ToList();

            foreach (var item in saveList)
            {
                _db.Setting.Add(item);
            }

            await _db.SaveChangesAsync();

        }
    }
}
