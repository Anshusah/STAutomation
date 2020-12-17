using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.User;
using System.Reflection;
using static Cicero.Service.Enums;

namespace Cicero.Service.Services
{
    public interface ICommonService
    {
        List<CoreCaseTable> GetCoreFormFieldsByFormId(int FormId);

        #region User
        List<AssignCaseViewModel> GetBackOfficeUserListWithSkillSet();
        dynamic getLoggedInUserId();
        Task<UserViewModel> GetUserById(string id);
        Task<string> GetUserFullName();
        List<UserMessageListViewModel> GetUserList();
        Task<bool> IsSuperAdmin();
        string GetTenantIdentifierByUserId();
        List<UserViewModel> GetUsersByRole(string roleid);
        Task<string> GetUserMediaById(string id);
        #endregion

        #region Role
        Task<bool> CheckRoleSideClaimForLoggedUser();
        string GetRoleIdByUserId(string id);
        List<SelectListItem> GetRoleList();
        List<SelectListItem> GetBackOfficeRoleList();
        List<SelectListItem> GetRoleListWithSides();
        List<SelectListItem> GetTemplateList(string type);
        #endregion

        #region Tenant
        TenantViewModel GetTenantById(int id);
        Task<List<SelectListItem>> GetTenantListByUserIdAsync();
        Task<List<SelectListItem>> GetTenantList();
        int GetTenantIdByIdentifier(string identifier);
        bool CheckUserInTenant(string tenantIdentifier);
        bool IsTenantAdmin();
        int GetTenantIdByUserId(string userId);

        #endregion

        #region CaseForm
        List<CaseFormViewModel> GetCaseFormListForActiveTenantId();

        List<string> GetTableNameByUrl(string url, int tenantid);

        List<SelectListItem> GetFormListForActiveTenant();

        int GetCaseFormIdByUrl(string url);
        #endregion

        #region Common
        List<SelectListItem> CountryList();
        string GetQueueNameByStateId(int stateId, string form);
        List<KeyValuePair<string, string>> GetQueueListByFormId(int caseformid);
        Task<UatSettingViewModel> CreateOrUpdateUatSetting(UatSettingViewModel model);
        Task<UatSettingViewModel> GetUatSettingByIdAsync();
        #endregion
    }

    public class CommonService : ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly Utils _utils;
        private readonly ILogger<CommonService> _log;
        private readonly IMapper _mapper;
        private readonly SimpleTransferApplicationDbContext stdb;

        public CommonService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ApplicationDbContext db, Utils utils, ILogger<CommonService> log, IMapper mapper,
            SimpleTransferApplicationDbContext _stdb)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _db = db;
            _utils = utils;
            _log = log;
            _mapper = mapper;
            stdb = _stdb;
        }

        #region User
        //unused
        public async Task<UserViewModel> GetUserById(string id)
        {

            var uvm = await (from u in _userManager.Users
                             where u.Id == id
                             select new UserViewModel
                             {
                                 Id = u.Id,
                                 UserId = u.UserId,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Email = u.Email,
                                 Status = u.Status,
                                 CreatedBy = u.CreatedBy,
                                 PhoneNumber = u.PhoneNumber,
                                 Address = u.Address,
                                 CreatedAt = Utils.GetDefaultDateFormat(u.CreatedAt),
                                 UpdatedAt = Utils.GetDefaultDateFormat(u.UpdatedAt)
                             }).FirstOrDefaultAsync();

            var role = _db.UserRoles.Where(x => x.UserId == uvm.Id).FirstOrDefault();

            if (role != null)
            {
                uvm.RoleId = role.RoleId;
            }
            else
            {
                uvm.RoleId = " ";
            }

            return uvm;

        }

        public List<AssignCaseViewModel> GetBackOfficeUserListWithSkillSet()
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var roleList = (from r in _db.Roles
                            join c in _db.RoleClaims on r.Id equals c.RoleId
                            join ur in _db.UserRoles on r.Id equals ur.RoleId
                            join u in _db.Users on ur.UserId equals u.Id
                            join us in _db.UserSkillSet on u.Id equals us.UserId
                            join s in _db.SkillSet on us.SkillSetId equals s.Id
                            where c.ClaimType.Equals("Side", StringComparison.OrdinalIgnoreCase) && r.TenantId == tenantid && c.ClaimValue.Equals("backend", StringComparison.OrdinalIgnoreCase)
                            select new AssignCaseViewModel()
                            {
                                DisplayName = u.FirstName + " " + u.LastName,
                                RoleName = r.DisplayName,
                                SkillSet = s.Title,
                                UserId = u.Id
                            }).ToList();
            return roleList.ToList();
        }

        public dynamic getLoggedInUserId()
        {
            try
            {
                var principal = _httpContextAccessor.HttpContext.User;
                string userId = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                return userId;

            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - " + ex.ToString());
                return false;
            }
        }

        public async Task<string> GetUserFullName()
        {
            var user = await _userManager.FindByIdAsync(this.getLoggedInUserId());

            if (user != null)
            {
                return user.FirstName + " " + user.LastName;
            }

            return "";
        }

        public List<UserMessageListViewModel> GetUserList()
        {
            try
            {
                var userlist = _db.Users
                                .GroupJoin(_db.UserRoles, x => x.Id, z => z.UserId, (x, z) => new { x, z })
                                .SelectMany(y => y.z.DefaultIfEmpty(), (a, b) => new { a, b })
                                .GroupJoin(_db.Roles, d => d.b.RoleId, e => e.Id, (d, e) => new { d, e })
                                .SelectMany(f => f.e.DefaultIfEmpty(), (g, h) => new { g, h })
                                .Where(i => i.h.NormalizedName.ToLower() == "user")
                                .Select(u => new UserMessageListViewModel
                                {
                                    Id = u.g.d.a.x.Id,
                                    Name = u.g.d.a.x.FirstName + " " + u.g.d.a.x.LastName
                                }).ToList();

                return userlist;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<bool> IsSuperAdmin()
        {
            try
            {
                var principal = _httpContextAccessor.HttpContext.User;
                string userId = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                var result = await _db.Users.Where(x => x.Id == userId).Select(b => b.IsSuperAdmin).FirstOrDefaultAsync();

                if (result == true)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - " + ex.ToString());
                return false;
            }
        }

        public string GetTenantIdentifierByUserId()
        {
            string loggedUser = (string)getLoggedInUserId();

            return _db.TenantUser
                            .Where(x => x.UserId == loggedUser)
                            .Include(y => y.TenantForUser)
                            .Select(b => b.TenantForUser.Identifier)
                            .FirstOrDefault();
        }
        public int GetTenantIdByUserId(string userId)
        {
            return _db.TenantUser
                            .Where(x => x.UserId == userId)
                            .Include(y => y.TenantForUser)
                            .Select(b => b.TenantId)
                            .FirstOrDefault();
        }

        public List<UserViewModel> GetUsersByRole(string roleid)
        {
            var users = _db.Users
                             .GroupJoin(_db.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u = u, ur = ur })
                             .SelectMany(y => y.ur.DefaultIfEmpty(), (u, ur) => new { u = u, ur = ur })
                             .Where(x => x.ur.RoleId.Equals(roleid, StringComparison.OrdinalIgnoreCase))
                             .Select(c => c.u.u).ToList();

            var userList = _mapper.Map<List<UserViewModel>>(users);

            return userList;
        }
        public async Task<string> GetUserMediaById(string id)
        {
            var query =
               await (from m in _db.UserMedia
                      join um in _db.Media on m.MediaId equals um.Id
                      where m.UserId == id
                      select new { um }).FirstOrDefaultAsync();

            return query != null ? query.um.Url : "https://www.leasecorp.com/assets/default/images/icons/icon-update-profile.jpg";
        }
        #endregion

        #region Role
        public async Task<bool> CheckRoleSideClaimForLoggedUser()
        {
            var principal = _httpContextAccessor.HttpContext.User;

            var currentUser = await _userManager.GetUserAsync(principal);
            if (principal.HasClaim(x => x.Type == "Side" && x.Value == "backend"))
            {
                return true;
            }

            return false;
        }

        public string GetRoleIdByUserId(string id)
        {
            try
            {
                int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());

                if (!IsSuperAdmin().Result)
                {
                    var result = _db.UserRoles.Where(x => x.UserId == id)
                                .Join(_db.Roles, a => a.RoleId, b => b.Id, (a, b) => new { a, b })
                                .Where(c => c.b.TenantId == tenantid)
                                .Select(y => y.b.Id).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return " ";
                    }
                    return result.ToString();

                }
                return " ";

            }
            catch (Exception)
            {
                return " ";
            }
        }

        public List<SelectListItem> GetRoleList()
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var roleList = _db.Roles
                                .Where(b => b.TenantId == tenantid || tenantid == 0)
                                .Select(x => new SelectListItem
                                {
                                    Text = x.Name,
                                    Value = x.Id
                                });
            return roleList.ToList();
        }

        public List<SelectListItem> GetRoleListWithSides()
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var roleList = (from r in _db.Roles
                            join c in _db.RoleClaims on r.Id equals c.RoleId
                            where c.ClaimType.Equals("Side", StringComparison.OrdinalIgnoreCase) && r.TenantId == tenantid
                            select new SelectListItem()
                            {
                                Value = r.Id,
                                Text = c.ClaimValue
                            }).ToList();
            return roleList.ToList();
        }

        public List<SelectListItem> GetBackOfficeRoleList()
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var roleList = (from r in _db.Roles
                            join c in _db.RoleClaims on r.Id equals c.RoleId
                            where c.ClaimType.Equals("Side", StringComparison.OrdinalIgnoreCase) && r.TenantId == tenantid && c.ClaimValue.Equals("backend", StringComparison.OrdinalIgnoreCase)
                            select new SelectListItem()
                            {
                                Value = r.Id,
                                Text = r.Name
                            }).ToList();
            return roleList.ToList();
        }

        public List<SelectListItem> GetTemplateList(string type = "template")
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            var templateList = _db.Article.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.Type == type).Select(x => new
           SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            });

            return templateList.ToList();
        }
        #endregion

        #region Tenant
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
                _log.LogError("CommonServices - GetTenantById - " + ex);
                return new TenantViewModel { };
            }
        }

        public async Task<List<SelectListItem>> GetTenantListByUserIdAsync()
        {
            try
            {
                string id = (string)getLoggedInUserId();
                if (await IsSuperAdmin() == true)
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
                _log.LogError("CommonServices - GetTenantListByUserIdAsync - " + ex);
                return null;
            }

        }

        public async Task<List<SelectListItem>> GetTenantList()
        {
            try
            {

                var tenants = await _db.Tenant.Select(b => new SelectListItem
                {
                    Value = b.Identifier,
                    Text = b.Name
                }).ToListAsync();
                return tenants;

            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - GetTenantList - " + ex);
                return null;
            }

        }

        public int GetTenantIdByIdentifier(string identifier)
        {
            try
            {
                var result = _db.Tenant
                                .Where(x => x.Identifier == identifier)
                                .Select(b => new { b.Id })
                                .FirstOrDefault();

                if (result != null)
                {
                    return result.Id;
                }
                return 14;
            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - GetTenantIdByIdentifier - " + ex);
                return 14;
            }
        }

        public bool CheckUserInTenant(string tenantIdentifier)
        {
            string loggedUser = (string)getLoggedInUserId();

            bool result = _db.Tenant.Where(x => x.Identifier == tenantIdentifier)
                                .Any(c => c.TenantUsers.Any(b => b.UserId == loggedUser));

            if (IsSuperAdmin().Result == true)
            {
                result = _db.Tenant.Any(x => x.Identifier == tenantIdentifier);
            }
            return result;
        }

        public bool IsTenantAdmin()
        {
            try
            {
                var principal = _httpContextAccessor.HttpContext.User;
                string userId = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                string roleid = GetRoleIdByUserId(userId);
                var result = _db.RolePermission.Where(x => x.PermissionId == 43 && x.RoleId == roleid).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - " + ex.ToString());
                return false;
            }
        }

        #endregion

        #region CaseForm

        public List<CaseFormViewModel> GetCaseFormListForActiveTenantId()
        {

            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            var caseFormList = _db.CaseForm.Where(x => x.TenantId == tenantid && !string.IsNullOrWhiteSpace(x.ModelName) && !string.IsNullOrWhiteSpace(x.UrlIdentifier)).OrderByDescending(x => x.CreatedAt).ToList();
            List<CaseFormViewModel> CaseformViewModelList = _mapper.Map<List<CaseFormViewModel>>(caseFormList);
            return CaseformViewModelList;

        }
        public List<CoreCaseTable> GetCoreFormFieldsByFormId(int FormId)
        {

            
            var caseFormList = _db.CoreCaseTable.Where(d=>d.CaseFormId== FormId).ToList();
            List<CoreCaseTable> CoreCaseTableList = _mapper.Map<List<CoreCaseTable>>(caseFormList);
            return CoreCaseTableList;

        }
        public List<string> GetTableNameByUrl(string url, int tenantid)
        {
            try
            {

                var tablename = new List<string>();

                var casedata = _db.CaseForm
                    .Where(x => !string.IsNullOrWhiteSpace(x.UrlIdentifier) && x.UrlIdentifier.Equals(url, StringComparison.OrdinalIgnoreCase) && x.TenantId == tenantid)
                    .FirstOrDefault();

                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var jsonform = JsonConvert.DeserializeObject<FormBuilderViewModel>(casedata.Fields, settings);


                foreach (var item in jsonform.Forms.Tables)
                {
                    tablename.Add(item.Name);
                }

                if (tablename.Count > 0)
                {
                    return tablename;
                }

                return new List<string>();
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService - GetTableNameByUrl - " + ex);
                return new List<string>();
            }
        }

        public List<SelectListItem> GetFormListForActiveTenant()
        {
            int tenantid = GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            var caseFormList = _db.CaseForm.Where(x => x.TenantId == tenantid && !string.IsNullOrWhiteSpace(x.ModelName)).OrderByDescending(x => x.CreatedAt)
                .Select(y => new SelectListItem
                {
                    Text = y.Name,
                    Value = y.Id.ToString()
                }).ToList();

            return caseFormList;
        }

        public int GetCaseFormIdByUrl(string url)
        {
            return _db.CaseForm.Where(x => x.UrlIdentifier == url).FirstOrDefault().Id;
        }
        #endregion

        #region Common

        public List<SelectListItem> CountryList()
        {
            return _db.CountryList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
        }

        public List<KeyValuePair<string, string>> GetQueueListByFormId(int caseformid)
        {
            string loggedUser = (string)getLoggedInUserId();
            string roleId = GetRoleIdByUserId(loggedUser);

            List<KeyValuePair<string, string>> queueName = new List<KeyValuePair<string, string>>();
            if (roleId.Trim() != "")
            {
                var tempo = _db.Queue
                    .Include(a => a.QueueForForm)
                    .ThenInclude(b => b.QueuePermissions);
                var tempo2 = _db.Queue
                    .Include(a => a.QueueForForm).SelectMany(y => y.QueueForForm.Where(z => z.CaseFormId == caseformid)).Include(e => e.QueuePermissions).OrderBy(x => x.Order);
                foreach (var q in tempo2)
                {
                    var qff = q;
                    if (qff != null)
                    {
                        string dn = "";

                        bool check = qff.AllUser;

                        if (!check)
                        {
                            var qp = qff.QueuePermissions.Where(b => b.RoleId == roleId).FirstOrDefault();
                            if (qp != null)
                                dn = qp.DisplayName;
                            else
                            {
                                continue;
                            }
                        }
                        var d = tempo.Where(x => x.Id == q.QueueId).FirstOrDefault();

                        var temp = new KeyValuePair<string, string>(d.UrlIdentifier, (string.IsNullOrWhiteSpace(dn) ? d.Name : dn));

                        queueName.Add(temp);
                    }
                }

                //            var temp = _db.Queue
                //.Include(a => a.QueueForForm)
                //.ThenInclude(b => b.QueuePermissions).Where(a => a.QueueForForm.AllUser == true || a.RoleId == roleId).Where(a => a.CaseFormId == caseformid);

                //queueName = _db.Queue
                //    .Include(a => a.QueueForForm)
                //    .ThenInclude(b => b.QueuePermissions).Where(a => a.CaseFormId == caseformid).SelectMany( c => c.QueueForForm.FirstOrDefault().QueuePermissions).Select(v => new KeyValuePair<string, string>(v.QueueForForm.Queue.UrlIdentifier, (v.DisplayName == null) ? v.QueueForForm.Queue.Name : v.DisplayName)).ToList();

                //queueName = _db.Queue
                //.Join(_db.QueueForForm, Qs => Qs.Id, qff => qff.QueueId, (c, d) => new { Qs = c, qff = d })
                //.GroupJoin(_db.QueuePermission.Where(e => e.RoleId == roleId), qff => qff.qff.Id, sps => sps.QueueForFormId, (a, b) => new { qff = a, sps = b }).SelectMany(y => y.sps.DefaultIfEmpty(), (a, b) => new { que = a.qff, qp = b })
                //.Where(a => a.que.qff.CaseFormId == caseformid).Select(v => new KeyValuePair<string, string>(v.que.Qs.UrlIdentifier, (  v.qp.DisplayName == null) ? v.que.Qs.Name : v.qp.DisplayName)).ToList();

            }
            else
            {
                if (queueName != null && queueName.Count() < 1)
                    queueName = _db.Queue.Include(x => x.QueueForForm).SelectMany(y => y.QueueForForm)
                    .Where(z => z.CaseFormId == caseformid).OrderBy(x => x.Order).Select(v => new KeyValuePair<string, string>(v.Queue.UrlIdentifier, v.Queue.Name)).ToList();
            }

            return queueName.Distinct().ToList();

            //return _db.Queue.Include(x => x.QueueForForm).SelectMany(y => y.QueueForForm)
            //    .Where(z => z.CaseFormId == caseformid && z.).Select(v => new KeyValuePair<string, string>(v.Queue.UrlIdentifier, v.Queue.Name)).ToList();


        }
        public string GetQueueNameByStateId(int stateId, string form)
        {
            var tempo = _db.Queue.Include(a => a.QueueForForm).ThenInclude(b => b.QueuePermissions).Where(x=>x.Id == stateId);

            return "";
        }
        public async Task<UatSettingViewModel> CreateOrUpdateUatSetting(UatSettingViewModel model)
        {
            try
            {
                char[] trim= new char[] {'-','(',')',' ','+'};
                UatSetting uatSetting = new UatSetting
                {
                    Id = model.Id,
                    PhoneNumber = string.Concat('+', model.PhoneNumber.Trim(trim).TrimStart('0').TrimStart(trim).Replace("(","").Replace(")","")),
                    Status = model.Status
                };
                if (model.Id == 0)
                {                    
                    stdb.UatSetting.Add(uatSetting);
                    await stdb.SaveChangesAsync();
                    return model;
                }
                else
                {
                    var data = uatSetting;
                    stdb.UatSetting.Attach(data).State = EntityState.Modified;
                    await stdb.SaveChangesAsync();
                }
                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public async Task<UatSettingViewModel> GetUatSettingByIdAsync()
        {
            var uatSetting = await (from c in stdb.UatSetting
                                  select c).FirstOrDefaultAsync();
            return _mapper.Map<UatSettingViewModel>(uatSetting);
        }
        #endregion

        #region Case
        public bool AssignCase(int caseid, string user, int timelimit)
        {
            try
            {
                var caseToAssign = _db.Case.Where(x => x.Id == caseid).FirstOrDefault();
                var temp = JsonConvert.SerializeObject(caseToAssign, Formatting.Indented,
                           new JsonSerializerSettings
                           {
                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                           });

                var oldCase = JsonConvert.DeserializeObject<Case>(temp);
                caseToAssign.AssignedAt = DateTime.UtcNow;
                caseToAssign.AssignedTo = user;
                caseToAssign.DueDate = DateTime.UtcNow.AddHours(timelimit);
                _db.Update(caseToAssign);
                _db.SaveChanges();
                bool result = CommonLogWriter(caseToAssign, (int)AuditLogOperation.Update, caseToAssign.Id.ToString(), new Case(), oldCase).Result;

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        private async Task<bool> CommonLogWriter<T>(T newValue, int operation, string objectId, T parentObject, T oldValue = null, string parentId = null) where T : class
        {
            try
            {
                dynamic props = newValue.GetType().GetRuntimeProperties();
                List<AuditLog> logs = new List<AuditLog>();
                string userId = getLoggedInUserId();
                string user = await GetUserFullName();
                int tenantId = GetTenantIdByIdentifier(_utils.GetTenantFromSession());


                foreach (var prop in props)
                {
                    AuditLog log = new AuditLog();
                    log.OperationType = operation;
                    log.FieldName = prop.Name;
                    log.NewValue = Convert.ToString(newValue.GetType().GetProperty(log.FieldName).GetValue(newValue));
                    log.TimeStamp = DateTime.UtcNow;
                    log.User = user;
                    log.UserId = userId;
                    log.TenantId = tenantId;
                    log.ObjectId = objectId;
                    log.Object = newValue.GetType().Name;
                    log.IsManual = false;
                    log.ParentObject = parentObject.GetType().Name;
                    log.IsDeleted = false;
                    if (operation == (int)AuditLogOperation.Update)
                    {
                        var isit = oldValue.GetType().GetProperty(log.FieldName);
                        if (isit != null)
                        {
                            string oVal = Convert.ToString(oldValue.GetType().GetProperty(log.FieldName).GetValue(oldValue));
                            if (oVal != log.NewValue)
                            {
                                log.OldValue = oVal;
                                logs.Add(log);
                            }
                        }
                        else // if update and old value doesn't exist
                        {
                            log.OperationType = (int)AuditLogOperation.Insert;
                            logs.Add(log);
                        }

                    }
                    else
                    {
                        logs.Add(log);
                    }
                }
                await _db.AuditLog.AddRangeAsync(logs);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
