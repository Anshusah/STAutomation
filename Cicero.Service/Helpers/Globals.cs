using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Helpers
{
    public class Globals
    {
        private readonly IUserService userService;
        private List<SelectListItem> allowedTenants;
        private readonly ApplicationDbContext db;
        private readonly  ITenantService tenantService;

        public Globals(IUserService _userService, ApplicationDbContext _db, IRolePermissionService _RolePermissionService, ITenantService _tenantService)
        {
            userService = _userService;
            db = _db;
            tenantService = _tenantService;
        }

        public async Task<List<SelectListItem>> CanAsync()
        {
            if (allowedTenants == null || allowedTenants.Count() == 0)
            {
                allowedTenants = await tenantService.GetTenantListByUserIdAsync();
            }
            if (allowedTenants != null)
            {
                return allowedTenants;
            }
            return null;
        }

        public string GetTenantIdentifier()
        {
            string tenant_identifier = null;
            if (tenant_identifier != null)
            {
                tenant_identifier = tenant_identifier.ToLower().Replace(".html", "");
                return tenant_identifier;
            }

            return tenant_identifier;
        }
    }
}
