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
using Cicero.Data.Entities;
using Cicero.Data;
using Cicero.Service.Helpers;
using System.Security.Policy;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cicero.Service.Services
{

    public interface IRoleService
    {
        DTResponseModel GetRoleListByFilter(DTPostModel model);
        List<SelectListItem> GetRoleList();
        Task<RoleViewModel> GetRoleByIdAsync(string id);
        Task<RoleViewModel> CreateOrUpdate(RoleViewModel model);
        List<RoleViewModel> GetRoleAddedList(DateTime startDateTime);
        Task<bool> DeleteRoleById(string id);
        bool RemoveRoleById(string id);
        Task<bool> ActiveRoleById(string id);
        Task<bool> InactiveRoleById(string id);
        bool IsDuplicateName(string DisplayName, string Id, int TenantId);
    }
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<RoleService> Log;
        private readonly IMapper Mapper;
        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly IRolePermissionService RolePermissionService;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public RoleService(ApplicationDbContext _db, Utils _Utils, ILogger<RoleService> _Log, IMapper _mapper, RoleManager<ApplicationRole> _roleManager, IRolePermissionService _RolePermissionService, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _Utils;
            Log = _Log;
            Mapper = _mapper;
            RoleManager = _roleManager;
            RolePermissionService = _RolePermissionService;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public DTResponseModel GetRoleListByFilter(DTPostModel model)
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

            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            var rolelist = (from r in db.Roles
                            where r.TenantId == tenantid || tenantid == 0
                            join t in db.Tenant on r.TenantId equals t.Id
                            select new
                            {
                                id = r.Id,
                                name = r.Name,
                                display_name = r.DisplayName,
                                tenant_name = t.Name,
                                created_at = Utils.GetDefaultDateFormatToDetail(r.CreatedAt),
                                updated_at = Utils.GetDefaultDateFormatToDetail(r.UpdatedAt),
                                status = (r.Status == 1) ? "Active" : "Inactive",
                                action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/role/" + r.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Role' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Role</span></a>"
                            });
            totalResultsCount = rolelist.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                rolelist = rolelist.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = rolelist.Count();
            rolelist = rolelist.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = rolelist.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public List<SelectListItem> GetRoleList()
        {
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            var roleList = db.Roles
                                .Where(b => b.TenantId == tenantid || tenantid == 0)
                                .Select(x => new SelectListItem
                                {
                                    Text = x.DisplayName,
                                    Value = x.Id
                                });
            return roleList.ToList();
        }

        public async Task<RoleViewModel> GetRoleByIdAsync(string id)
        {
            //var claimFind = await RoleManager.GetClaimsAsync(role);
            return await (from r in db.Roles
                          join c in db.RoleClaims on r.Id equals c.RoleId
                          where r.Id == id && c.ClaimType == "Side"
                          select new RoleViewModel()
                          {
                              Id = r.Id,
                              Name = r.Name,
                              DisplayName = r.DisplayName,
                              Status = r.Status,
                              CreatedAt = Utils.GetDefaultDateFormatToDetail(r.CreatedAt),
                              UpdatedAt = Utils.GetDefaultDateFormatToDetail(r.UpdatedAt),
                              // PermissionListSelected = r.RolePermissions.Join(db.Permission, c => c.PermissionId, a => a.Id, (c, a) => new { c, a }).Select(i => i.c.PermissionId).AsEnumerable(),
                              PermissionListSelected = r.RolePermissions.Join(db.Permission, c => c.PermissionId, a => a.Id, (c, a) => new { c, a }).Select(i => i.c.PermissionId.ToString() + "," + i.c.PermissionGroupId.ToString()).AsEnumerable(),
                              WebSide = c.ClaimValue,
                              OrganizationName = r.OrganizationName
                          }).FirstOrDefaultAsync();

        }

        private async Task<string> Create(RoleViewModel rvm)
        {
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            ApplicationRole role = new ApplicationRole
            {
                Name = rvm.Name,
                DisplayName = rvm.DisplayName,
                Status = rvm.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Type = 1,
                CreatedBy = rvm.CreatedBy,
                OrganizationName = rvm.OrganizationName,
                TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession())
            };

            var result = await RoleManager.CreateAsync(role);

            if (result.Succeeded)
            {

                await activityLogService.CreateLog(loggedUser, "New Role created <a href ='/admin" + Utils.GetTenantForUrl(false) + "/role/" + rvm.Id + "/edit.html'>" + rvm.Name + "</a>. Created By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                if (db.RoleClaims.Where(x => x.RoleId == rvm.Id).FirstOrDefault() != null)
                {
                    var claimFind = await RoleManager.GetClaimsAsync(role);
                    await RoleManager.RemoveClaimAsync(role, claimFind.FirstOrDefault());

                }
                await RoleManager.AddClaimAsync(role, new Claim("Side", rvm.WebSide));

                return role.Id;
            }
            else
            {

                return null;
            }
        }

        private async Task Update(RoleViewModel rvm)
        {
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            var role = await RoleManager.FindByIdAsync(rvm.Id);
            role.DisplayName = rvm.DisplayName;
            role.Name = rvm.Name;
            role.UpdatedAt = DateTime.Now;
            role.Status = rvm.Status;
            role.OrganizationName = rvm.OrganizationName;
            //role.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            var result = await RoleManager.UpdateAsync(role);
            if (result.Succeeded)
            {

                await activityLogService.CreateLog(loggedUser, "Role updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/role/" + rvm.Id + "/edit.html'>" + rvm.Name + "</a>. Created By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                if (db.RoleClaims.Where(x => x.RoleId == rvm.Id).FirstOrDefault() != null)
                {
                    var claimFind = await RoleManager.GetClaimsAsync(role);
                    await RoleManager.RemoveClaimAsync(role, claimFind.FirstOrDefault());

                }
                await RoleManager.AddClaimAsync(role, new Claim("Side", rvm.WebSide));
            }

        }

        public async Task<RoleViewModel> CreateOrUpdate(RoleViewModel model)
        {

            string tenant = commonService.GetTenantById(commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession())).Name;

            model.Name = model.DisplayName + " " + tenant;
            model.CreatedBy = commonService.getLoggedInUserId();
            if (model.Id == "0")
            {
                model.Id = await Create(model);
            }
            else
            {

                await Update(model);

            }


            return model;
        }

        public async Task<bool> DeleteRoleById(string id)
        {
            var role = await db.Roles.Where(d => d.Id == id).FirstOrDefaultAsync();

            string Name = role.Name;
            if (role != null)
            {
                if (CheckRoleForDeletion(id))
                {
                    if (StepsBeforeRoleDelete(id))
                    {
                        db.Roles.Remove(role);
                        db.SaveChanges();
                        var loggedUser = commonService.getLoggedInUserId();
                        var fullName = commonService.GetUserFullName().Result;
                        await activityLogService.CreateLog(loggedUser, "Role Deleted " + Name + ". Deleted By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                        return true;
                    }
                }
            }

            Log.LogError("RoleServices - DeleteRoleById - " + id);
            return false;
        }
        private bool StepsBeforeRoleDelete(string id)
        {
            try
            {
                List<RolePermission> permission = db.RolePermission.Where(x => x.RoleId == id).ToList();
                permission.ForEach(x => db.RolePermission.Remove(x));
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        private bool CheckRoleForDeletion(string id)
        {
            //if we need to find out which role this state permission is for we need to use StateForForm

            //if ((db.State.Where(x => x.RoleId == id).ToList().Count() > 0) && (db.Queue.Where(x => x.RoleId == id).ToList().Count() > 0) && (db.UserRoles.Where(x => x.RoleId == id).ToList().Count() > 0))
            if ((db.StatePermission.Where(x => x.RoleId == id).ToList().Count() > 0) && (db.QueuePermission.Where(x => x.RoleId == id).ToList().Count() > 0) && (db.UserRoles.Where(x => x.RoleId == id).ToList().Count() > 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> ActiveRoleById(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            role.Status = 1;
            var result = RoleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Article changed to Active <a href ='/admin" + Utils.GetTenantForUrl(false) + "/role/" + role.Id + "/edit.html'>" + role.Name + "</a>. Changed By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }
            Log.LogError("RoleServices - ActiveRoleById - " + id + " - : " + result.Errors.ToString());
            return false;
        }

        public async Task<bool> InactiveRoleById(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            role.Status = 0;
            var result = RoleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Article changed to InActive <a href ='/admin" + Utils.GetTenantForUrl(false) + "/role/" + role.Id + "/edit.html'>" + role.Name + "</a>. Changed By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }
            Log.LogError("RoleServices - InactiveRoleById - " + id + " - : " + result.Errors.ToString());
            return false;
        }

        public bool IsDuplicateName(string DisplayName, string Id, int TenantId)
        {

            if (Id == "0")
            {
                return (!db.Roles.Any(d => d.DisplayName == DisplayName && d.TenantId == TenantId));
            }
            else
            {
                return (!db.Roles.Any(d => d.DisplayName == DisplayName && d.Id != Id && d.TenantId == TenantId));
            }

        }
        public List<RoleViewModel> GetRoleAddedList(DateTime startDateTime)
        {
            string loggedUser = commonService.getLoggedInUserId();
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<ApplicationRole> listRole = db.Roles.Where(x => x.TenantId == tenantid && (x.CreatedBy == loggedUser) && x.CreatedAt >= startDateTime).ToList();
            List<RoleViewModel> listRoleVM = new List<RoleViewModel>();
            foreach (ApplicationRole item in listRole)
            {
                RoleViewModel rvm = new RoleViewModel();
                rvm.Id = item.Id;
                rvm.Name = item.Name;
                rvm.TenantId = item.TenantId;
                rvm.CreatedAt = Convert.ToString(item.CreatedAt);
                rvm.CreatedBy = item.CreatedBy;
                rvm.DisplayName = item.DisplayName;
                rvm.Status = item.Status;
                listRoleVM.Add(rvm);
            }

            return listRoleVM;
        }

        public bool RemoveRoleById(string id)
        {
            var role = db.Roles.Where(d => d.Id == id).FirstOrDefault();

            string Name = role.Name;
            if (role != null)
            {
                db.Roles.Remove(role);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                activityLogService.CreateLog(loggedUser, "Role Deleted " + Name + ". Deleted By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("RoleServices - DeleteRoleById - " + id);
            return false;
        }
    }
}
