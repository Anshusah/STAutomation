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

namespace Cicero.Service.Services
{
    
    public interface IRolePermissionService
    {
        Task<List<PermissionViewModel>> GetPermissionListByRoleIdAsync(string id);
        List<PermissionViewModel> GetPermissionListByRoleId(string id);
        Task<List<PermissionViewModel>> GetPermissionListAsync();
        Task UpdateRolePermission(RoleViewModel model);
        Task<List<PermissionGroupViewModel>> GetPermissionGroupViewModels(List<PermissionViewModel>allPermission);
        int GetPermissionGroupByFormId(int id);
    }
    public class RolePermissionService:IRolePermissionService
    {
        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<RolePermissionService> Log;
        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly IPermissionService _permissionService;

        public RolePermissionService(ApplicationDbContext _db, IPermissionService permissionService, Utils _Utils,ILogger<RolePermissionService> _Log,RoleManager<ApplicationRole> _roleManager)
        {
            db = _db;
            _permissionService = permissionService;
            Utils = _Utils;
            Log = _Log;

            RoleManager = _roleManager;
        }

        public async Task<List<PermissionViewModel>> GetPermissionListByRoleIdAsync(string id){
            
            return  await (from p in db.Permission join r in db.RolePermission on p.Id equals r.PermissionId 
                           where r.RoleId == id
                          select new PermissionViewModel()
                          {
                              Id = p.Id,
                              Name = p.Name

            }).ToListAsync();
        }

        public List<PermissionViewModel> GetPermissionListByRoleId(string id)
        {

            return (from p in db.Permission
                    join r in db.RolePermission on p.Id equals r.PermissionId
                    where r.RoleId == id
                    select new PermissionViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        GroupId= r.PermissionGroupId
                    }).ToList();
        }

        public int GetPermissionGroupByFormId(int id)
        {
            int a = 0;
            try
            {
                a = db.PermissionGroup.Where(x => x.CaseFormId == id).SingleOrDefault().Id;
            }
            catch (Exception e)
            { }
          return a;
        }
        public async Task<List<PermissionViewModel>> GetPermissionListAsync(){

            return await (from p in db.Permission
                          select new PermissionViewModel()
                          {
                              Id = p.Id,
                              Name = p.Name

                          }).ToListAsync();

        }

        public async Task UpdateRolePermission(RoleViewModel model)
        {
            if(db.RolePermission.Where(e=> e.RoleId == model.Id)!= null)
            {
                db.RolePermission.RemoveRange(db.RolePermission.Where(e => e.RoleId == model.Id));
            }

            if (model.PermissionListSelected!=null)
            {
                List<RolePermission> list = new List<RolePermission>();
                foreach (var i in model.PermissionListSelected)
                {
                    list.Add(new RolePermission { RoleId = model.Id, PermissionId = Convert.ToInt32(i.Split(",")[0]), PermissionGroupId =Convert.ToInt32(i.Split(",")[1])});
                }
                db.RolePermission.AddRange(list);

            }
            await db.SaveChangesAsync();
        }

        public async Task<List<PermissionGroupViewModel>> GetPermissionGroupViewModels(List<PermissionViewModel> allPermission)
        {
            return await(from p in db.PermissionGroup
                    select new PermissionGroupViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        PermissionIds = p.PermissionIds,
                        Permission = _permissionService.GetPermissionViewModels(p.PermissionIds, allPermission), 
                        TenantId = p.TenantId,
                        CreatedBy =p.CreatedBy
                    }).ToListAsync();
            
        }
    }
}
