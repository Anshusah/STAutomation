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
using Permission = Cicero.Data.Entities.Permission;

namespace Cicero.Service.Services
{

    public interface IPermissionService
    {
        Task<PermissionViewModel> GetPermissionById(int id);
        List<PermissionViewModel> GetPermissionViewModels(string ids, List<PermissionViewModel> allPermission);
        bool CheckForPermissionGroup(string name,int caseFormId);
        bool CreatePermissionGroup(string name);
        PermissionGroupViewModel GetPermissionGroupViewModelsByTenantId(string name);
        bool DeletePermissionGroup(List<int> ids);
    }
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<PermissionService> Log;
        private readonly IMapper Mapper;
        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly ICommonService commonService;

        public PermissionService(ApplicationDbContext _db, ICommonService _commonService, Utils _Utils, ILogger<PermissionService> _Log, IMapper _mapper, RoleManager<ApplicationRole> _roleManager)
        {
            db = _db;
            Utils = _Utils;
            Log = _Log;
            Mapper = _mapper;
            RoleManager = _roleManager;
            commonService = _commonService;
        }

        public async Task<PermissionViewModel> GetPermissionById(int id)
        {

            return await (from p in db.Permission
                          where p.Id == id
                          select new PermissionViewModel()
                          {
                              Id = p.Id,
                              Name = p.Name

                          }).FirstOrDefaultAsync();
        }

        private PermissionViewModel GetPermission(int id)
        {

            return (from p in db.Permission
                    where p.Id == id
                    select new PermissionViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name

                    }).FirstOrDefault();
        }
        public List<PermissionViewModel> GetPermissionViewModels(string ids, List<PermissionViewModel> allPermission)
        {
            List<PermissionViewModel> permissionViewModels = new List<PermissionViewModel>();
            string[] permissionIds = ids.Split(",");
            foreach (string id in permissionIds)
            {
                PermissionViewModel permission = allPermission.Where(x => x.Id == Convert.ToInt32(id)).SingleOrDefault();
                permissionViewModels.Add(permission);
            }
            return permissionViewModels;
        }

        public bool CheckForPermissionGroup(string name,int caseFormId)
        {

            int count = db.PermissionGroup.Where(x => (x.Name.ToLower() == name.ToLower())&& (x.CaseFormId==caseFormId)).Count();
            if (count > 0) { return true; } else { return false; }

        }

        public PermissionGroupViewModel GetPermissionGroupViewModelsByTenantId(string name)
        {
            string groupName = name.Split(",")[0];
            int caseFormId = Convert.ToInt32(name.Split(",")[2]);
            List<PermissionViewModel> allPermission = (from p in db.Permission
                                                       select new PermissionViewModel()
                                                       {
                                                           Id = p.Id,
                                                           Name = p.Name
                                                       }).ToList();
            int tenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            return  (from p in db.PermissionGroup
                          select new PermissionGroupViewModel()
                          {
                              Id = p.Id,
                              Name = p.Name,
                              PermissionIds = p.PermissionIds,
                              Permission = GetPermissionViewModels(p.PermissionIds, allPermission),
                              TenantId = p.TenantId,
                              CreatedBy = p.CreatedBy,
                              CaseFormId =p.CaseFormId
                          }).Where(x=>x.TenantId ==tenantId && x.Name==groupName && x.CaseFormId==caseFormId).SingleOrDefault();
        }

        public bool CreatePermissionGroup(string name)
        {
            try
            {

                if (!CheckForPermissionGroup(name.Split(",")[0],Convert.ToInt32(name.Split(",")[2])))
                {
                    PermissionGroup permissionGroup = new PermissionGroup
                    {
                        Name = name.Split(",")[0],
                        TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession()),
                        CreatedBy = commonService.getLoggedInUserId(),
                        CreatedAt = DateTime.Now,
                        CaseFormId = Convert.ToInt32(name.Split(",")[2]),
                        PermissionIds = "45,46,47,48",

                    };
                    db.PermissionGroup.Add(permissionGroup);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
       
        }
           

        public bool DeletePermissionGroup(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    List<RolePermission> rolePermissions = db.RolePermission.Where(x => x.PermissionGroupId == id).ToList();
                    foreach (RolePermission item in rolePermissions)
                    {
                        db.RolePermission.Remove(item);
                    }
                    PermissionGroup permissionGroup = db.PermissionGroup.Where(x => x.Id == id).SingleOrDefault();
                    db.PermissionGroup.Remove(permissionGroup);
                }
                    db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }


}
