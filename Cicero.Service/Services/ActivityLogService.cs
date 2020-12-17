using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Services
{
    public interface IActivityLogService
    {
        Task CreateLog(string loggedUser, string message, int? tenantid = 0);
        Task CreateClaimLog(string loggedUser, string message, int claimid, int stateid, int? tenantid);
        List<ActivityLogViewModel> GetClaimLogAsync(int claimid);
        Task<List<ActivityLogViewModel>> GetActivityLogAsync();
        Task<List<ActivityLogViewModel>> GetUnreadActivityLog(string numberOfRecord = "SOME");
        Task<bool> ActivityNotificationRead();
    }

    public class ActivityLogService : IActivityLogService
    {
        private readonly ApplicationDbContext _db;
        private readonly Utils utils;
        private readonly ILogger<ActivityLogService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper mapper;

        public ActivityLogService(ApplicationDbContext db, ILogger<ActivityLogService> Log, IHttpContextAccessor IHttpContextAccessor, IHostingEnvironment HostingEnvironment, IMapper _mapper, Utils _utils)
        {
            _db = db;
            _Log = Log;
            _IHttpContextAccessor = IHttpContextAccessor;
            _HostingEnvironment = HostingEnvironment;
            mapper = _mapper;
            utils = _utils;

        }
        
        public async Task CreateLog(string loggedUser, string message, int? tenantid = 0)
        {
            if (tenantid == 0 || tenantid == null)
            {
                tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

            }

            ActivityLog activityLog = new ActivityLog()
            {
                UserId = loggedUser,
                CreatedOn = DateTime.Now,
                Details = message,
                IsRead = false,
                DisplayTo = null
            };

            if(tenantid != 0)
            {
                activityLog.TenantId = tenantid;

                _db.ActivityLog.Add(activityLog);
            }

            await _db.SaveChangesAsync();

        }

        public async Task CreateClaimLog(string loggedUser, string message, int claimid, int stateid,  int? tenantid = 0)
        {
            if (tenantid == 0 || tenantid == null)
            {
                tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

            }

            ActivityLog activityLog = new ActivityLog()
            {
                UserId = loggedUser,
                CreatedOn = DateTime.Now,
                Details = message,
                IsRead = false,
                DisplayTo = null,
                ClaimId = claimid,
                StateId = stateid
            };

            if (tenantid != 0)
            {
                activityLog.TenantId = tenantid;

                _db.ActivityLog.Add(activityLog);
            }

            await _db.SaveChangesAsync();

        }

        public List<ActivityLogViewModel> GetClaimLogAsync(int claimid)
        {
            try
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();
                var activityLog = mapper.Map<List<ActivityLogViewModel>>(_db.ActivityLog.Include(c => c.User).Where(y => y.ClaimId == claimid && (y.TenantId == tenantid || tenantid == 0)).OrderByDescending(x => x.CreatedOn).ToList());
                return activityLog;
            }
            catch (Exception ex)
            {
                _Log.LogError("ActivityLogService - GetActivityLogAsync - " + ex);
                return null;
            }
        }

        public async Task<List<ActivityLogViewModel>> GetActivityLogAsync()
        {
            try
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();
                var activityLog = mapper.Map<List<ActivityLogViewModel>>(await _db.ActivityLog.Where(y => (y.TenantId == tenantid || tenantid == 0) && (y.ClaimId == null || y.ClaimId == 0)).OrderByDescending(x => x.CreatedOn).Take(4).ToListAsync());
                return activityLog;
            }
            catch (Exception ex)
            {
                _Log.LogError("ActivityLogService - GetActivityLogAsync - " + ex);
                return null;
            }

        }

        public async Task<List<ActivityLogViewModel>> GetUnreadActivityLog(string numberOfRecord = "SOME")
        {
            try
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();
                if (numberOfRecord.ToUpper() == "ALL")
                {
                    var activityLog = mapper.Map<List<ActivityLogViewModel>>(await _db.ActivityLog.Where(y => y.IsRead == false && (y.ClaimId == null || y.ClaimId == 0) && (y.TenantId == tenantid || tenantid == 0)).OrderByDescending(x => x.CreatedOn).ToListAsync());
                    return activityLog;
                }
                var activityLogSome = mapper.Map<List<ActivityLogViewModel>>(await _db.ActivityLog.Where(y => y.IsRead == false && (y.ClaimId == null || y.ClaimId == 0) && (y.TenantId == tenantid || tenantid == 0)).OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync());
                return activityLogSome;
            }
            catch (Exception ex)
            {
                _Log.LogError("ActivityLogService - GetUnreadActivityLogAsync - " + ex);
                return null;
            }
        }

        public async Task<bool> ActivityNotificationRead()
        {
            try
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

                var activityLogSome = _db.ActivityLog.Where(y => y.IsRead == false && (y.TenantId == tenantid || tenantid == 0)).OrderByDescending(x => x.CreatedOn);

                foreach(var item in activityLogSome)
                {
                    _db.ActivityLog.Update(item);
                    item.IsRead = true;
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _Log.LogError("ActivityLogService - GetUnreadActivityLogAsync - " + ex);
                return false;
            }
        }

    }
}
