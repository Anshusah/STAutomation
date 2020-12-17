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
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Cicero.Service.Models.User;
using Cicero.Service.Services.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.Customer;
using static Cicero.Data.Enumerations;
using Cicero.Service.Models.API.LexisNexis;

namespace Cicero.Service.Services.SimpleTransfer
{

    public interface ICustomerUserService
    {
        DTResponseModel GetUserListByFilter(DTPostModel model, string side = null);
        Task<CustomerUserViewModel> CreateOrUpdate(CustomerUserViewModel model);
        Task<CustomerUserViewModel> GetUserById(string id);
        Task<List<SanctionPepPersonViewModel>> GetPersonByUserId(string userId);

        ChangePasswordViewModel GetForChangePassword();
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel cpvm);
        Task<SignInResult> Login(string UserName, string Password, bool RememberMe, bool lockout);

        Task<bool> DeleteUserById(string id);
        Task<bool> ActiveUserById(string id);
        Task<bool> InactiveUserById(string id);

        dynamic getLoggedInUserId();
        Task<bool> Logout();
        Task<string> GetUserFullName();
        bool IsLogin();

        bool IsDuplicateEmailInTenant(string Email, string Id, int TenantId);
        bool IsDuplicateEmail(string Email);
        string GetRoleByUserIdAsync(string id);
        IEnumerable<MediaViewModel> GetImagesByUserId(string id);
        Task<string> GetDefaultOrFirstImagesByUserId(string id, string _default);

        List<UserMessageListViewModel> GetUserList();
        List<UserMessageListViewModel> GetBackendUserList();

        Task<bool> IsSuperAdmin();
        Task<bool> IsSuperAdminEmail(string email);
        Task<string> GetTenantIdentifierbyEmail(string email);

        bool CheckUserInTenant(string tenantIdentifier);
        string UserHasPolicy();
        string GetTenantIdentifierByUserId();
        int GetTenantUsersCount();
        List<CustomerUserViewModel> GetAddedUserData(DateTime startDatetime);
        bool RemoveUserById(string userId);
        bool CheckIfActiveUser(string userId);
        bool CheckIfEmailExists(string userId);

    }
    public class CustomerUserService : ICustomerUserService
    {
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly ApplicationDbContext db;
        private readonly Utils utils;
        private readonly ILogger<CustomerUserService> Log;
        private readonly IActivityLogService activityLogService;
        private readonly ICommonService commonService;
        private readonly AppSetting setting;
        private readonly IMapper iMapper;
        private readonly ISmsService smsService;
        private readonly SimpleTransferApplicationDbContext stdb;
        private readonly IBeneficiarySetupService beneficiarySetupService;
        private readonly IMapper mapper;

        public CustomerUserService(UserManager<ApplicationUser> _UserManager, SignInManager<ApplicationUser> _SignInManager, ApplicationDbContext _db, Utils _Utils, IHttpContextAccessor _IHttpContextAccessor,
            ILogger<CustomerUserService> _Log, IActivityLogService _activityLogService, ICommonService _commonService, AppSetting _setting, IMapper _iMapper, ISmsService smsService, SimpleTransferApplicationDbContext _stdb, IBeneficiarySetupService beneficiarySetupService, IMapper mapper)
        {
            UserManager = _UserManager;
            SignInManager = _SignInManager;
            db = _db;
            utils = _Utils;
            IHttpContextAccessor = _IHttpContextAccessor;
            Log = _Log;
            commonService = _commonService;
            activityLogService = _activityLogService;
            setting = _setting;
            iMapper = _iMapper;
            this.smsService = smsService;
            stdb = _stdb;
            this.beneficiarySetupService = beneficiarySetupService;
            this.mapper = mapper;
        }

        public DTResponseModel GetUserListByFilter(DTPostModel model, string side = null)
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
                    sortBy = "name";//model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            var userlist = db.Tenant
                            .Where(a => a.Id == tenantid)
                            .Join(db.TenantUser, t => t.Id, tu => tu.TenantId, (t, tu) => new { t, tu })
                            .GroupJoin(db.UserRoles, tu => tu.tu.UserForTenant.Id, ur => ur.UserId, (tu, ur) => new { tu, ur })
                            .SelectMany(b => b.ur.DefaultIfEmpty(), (tu, ur) => new { tu, ur })
                            .Join(db.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                            .Where(r => r.r.TenantId == tenantid)
                            .Select(x => new UserDataModel
                            {
                                id = x.ur.tu.tu.tu.UserId,
                                name = x.ur.tu.tu.tu.UserForTenant.FirstName + " " + x.ur.tu.tu.tu.UserForTenant.LastName,
                                first_name = x.ur.tu.tu.tu.UserForTenant.FirstName,
                                last_name = x.ur.tu.tu.tu.UserForTenant.LastName,
                                status = (x.ur.tu.tu.tu.UserForTenant.Status == true) ? "Active" : "Inactive",
                                email = x.ur.tu.tu.tu.UserForTenant.Email,
                                address = x.ur.tu.tu.tu.UserForTenant.Address,
                                image = "a",
                                //created_at = Utils.GetDefaultDateFormat(u.CreatedAt),
                                updated_at = Utils.GetDefaultDateFormat(x.ur.tu.tu.tu.UserForTenant.UpdatedAt),
                                role = x.r.DisplayName,
                                side = db.RoleClaims.Where(rc => rc.RoleId == x.r.Id).Select(rc => rc.ClaimValue).FirstOrDefault(),
                                action = "<a href='/st/admin" + utils.GetTenantForUrl(false) + "/customers/" + x.ur.tu.tu.tu.UserForTenant.Id + "/edit.html' title='Edit User' class='btn btn-light btn-icon btn-edit' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span>Edit</span></a>"
                            });




            //<div class='dropdown' title='More' data-toggle='tooltip' data-placement='top'><a data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='ri-more-fill'></i></a><div  class='dropdown-menu dropdown-menu--custom dropdown-menu-right'>" +
            //"<a href='/admin" + utils.GetTenantForUrl(false) + "/user/" + x.ur.tu.tu.tu.UserForTenant.Id + "/edit.html' title='Edit User' class='dropdown-item' data-toggle='tooltip' data-placement='top'><span>Edit</span></a>" +
            //"<a href='' class='dropdown-item d-flex justify-content-between align-items-center'>Status<div class='custom-control custom-switch'>" +
            //"<input type = 'checkbox' class='custom-control-input' id='customSwitch1'>" +
            //"<label class='custom-control-label' for='customSwitch1'></label>" +
            //"</div> </a>" +
            //"<div class='dropdown-divider'></div>" +
            //"<a href = '' class='dropdown-item' id=''>Delete</a>" +
            //"</div></div>

            if (side != null)
            {
                userlist = userlist.Where(x => x.side == side);
            }

            if (!String.IsNullOrEmpty(searchBy))
            {
                userlist = userlist.Where(o => o.name.ToLower().Contains(searchBy.ToLower()) || o.email.ToLower().Contains(searchBy.ToLower()) || o.first_name.ToLower().Contains(searchBy.ToLower()) || o.last_name.ToLower().Contains(searchBy.ToLower()) || o.role.ToLower().Contains(searchBy.ToLower()) || o.updated_at.ToLower().Contains(searchBy.ToLower()));

            }

            totalResultsCount = userlist.Count();
            userlist = userlist.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = userlist.ToList();
            foreach (var item in list)
            {
                var mediaId = db.UserMedia.Where(x => x.UserId == item.id).Select(x => x.MediaId).FirstOrDefault();
                var mediaUrl = db.Media.Where(x => x.Id == mediaId).Select(x => x.Url).FirstOrDefault();
                item.image = mediaUrl;
            }

            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        private async Task AddUserToTenant(int tenantid, string userid)
        {
            var tenantUser = new TenantUser
            {
                TenantId = tenantid,
                UserId = userid
            };

            db.TenantUser.Add(tenantUser);
            await db.SaveChangesAsync();
        }

        private async Task<IdentityResult> ChangePassword(ApplicationUser user, string password)
        {
            await UserManager.RemovePasswordAsync(user);
            return await UserManager.AddPasswordAsync(user, password);
        }

        private async Task AttachRoleToUser(ApplicationUser model, string roleid)
        {
            var role = db.UserRoles.Where(x => x.UserId == model.Id).FirstOrDefault();
            //int id = db.TenantUser.Where(x => x.UserId == model.Id).FirstOrDefault().TenantId;
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            if (role != null && db.TenantUser.Any(x => x.TenantId == tenantid))
            {
                var oldRole = db.Roles.Where(x => x.Id == role.RoleId).FirstOrDefault();
                await UserManager.RemoveFromRoleAsync(model, oldRole.Name);
            }
            var newRole = db.Roles.Where(x => x.Id == roleid).FirstOrDefault();
            await UserManager.AddToRoleAsync(model, newRole.Name);
        }

        private async Task AddImageToUser(CustomerUserViewModel model)
        {
            db.UserMedia.RemoveRange(db.UserMedia.Where(e => e.UserId == model.Id));
            foreach (var item in model.Images.Where(x => x != 0))
            {
                UserMedia modelMedia = new UserMedia
                {
                    UserId = model.Id,
                    MediaId = item
                };
                db.UserMedia.Add(modelMedia);
            }
            await db.SaveChangesAsync();
        }

        public async Task<CustomerUserViewModel> CreateOrUpdate(CustomerUserViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    int tenantid = 0;

                    if (model.TenantId == 0)
                    {
                        tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                    }
                    else
                    {
                        tenantid = model.TenantId;
                    }

                    if (model.Id == "0" && !IsDuplicateEmail(model.Email) && IsSuperAdminEmail(model.Email).Result == false)
                    {
                        ApplicationUser user = new ApplicationUser
                        {
                            UserName = model.Email,
                            DisplayName = model.FirstName + " " + model.FirstName,
                            FirstName = model.FirstName,
                            UserId = model.UserId,
                            Email = model.Email,
                            LastName = model.LastName,
                            Address = model.Address,
                            PhoneNumber = model.PhoneNumber,
                            CreatedBy = model.CreatedBy,
                            Status = model.Status,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            EmailConfirmed = false, //Please remove later
                            LockoutEnd = model.Lockout == true ? DateTimeOffset.Now.AddDays(30) : (DateTimeOffset?)null
                        };
                        if (tenantid != 0)
                        {
                            var result = await UserManager.CreateAsync(user, model.Password);

                            if (model.RoleId == null)
                            {
                                string role = setting.Get("app_user_role");
                                string roleid = db.Roles.Where(x => x.DisplayName == role && x.TenantId == tenantid).SingleOrDefault().Id;
                                await AttachRoleToUser(user, roleid);

                            }
                            else
                            {
                                await AttachRoleToUser(user, model.RoleId);
                            }


                            if (result.Succeeded)
                            {
                                model.Id = user.Id;

                                await AddUserToTenant(tenantid, user.Id);

                                if (model.Images != null)
                                {
                                    await AddImageToUser(model);

                                }

                                await activityLogService.CreateLog(getLoggedInUserId(), "New User Profile Created for  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + user.Id + "/edit.html' data-toggle = 'tooltip' > " + user.FirstName + " " + user.LastName + "</a>. Created By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + getLoggedInUserId() + "/edit.html' data - toggle = 'tooltip' > " + GetUserFullName().Result + " </a> ");
                            }

                        }

                    }
                    else
                    {
                        var user = await UserManager.FindByIdAsync(model.Id);
                        if (user == null)
                        {
                            user = await UserManager.FindByEmailAsync(model.Email);
                        }
                        user.UserName = model.Email;
                        user.UserId = model.UserId;
                        user.DisplayName = model.FirstName + " " + model.FirstName;
                        user.FirstName = model.FirstName;
                        user.UpdatedAt = DateTime.Now;
                        user.Email = model.Email;
                        user.LastName = model.LastName;
                        user.Address = model.Address;
                        user.CreatedBy = model.CreatedBy;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Status = model.Status;
                        user.LockoutEnd = model.Lockout == true ? DateTimeOffset.Now.AddDays(30) : (DateTimeOffset?)null;

                        var result = await UserManager.UpdateAsync(user);
                        //var isSuperAdmin = await IsSuperAdmin();

                        //if (!isSuperAdmin)
                        //{
                        await AttachRoleToUser(user, model.RoleId);
                        //}

                        if (result.Succeeded)
                        {

                            if (!string.IsNullOrEmpty(model.Password))
                            {
                                result = await ChangePassword(user, model.Password);
                            }

                            if (!TenantAndUserExist(user.Id, tenantid))
                            {
                                await AddUserToTenant(tenantid, user.Id);
                            }

                            await activityLogService.CreateLog(getLoggedInUserId(), "User Profile Updated for  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + user.Id + "/edit.html' data-toggle = 'tooltip' >" + user.FirstName + " " + user.LastName + "</a>. Updated By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + getLoggedInUserId() + "/edit.html' data - toggle = 'tooltip' > " + GetUserFullName().Result + " </a> ");
                        }

                        if (model.Images != null)
                        {
                            await AddImageToUser(model);

                        }

                    }

                    transaction.Commit();
                    return model;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return model;
                }
            }

        }

        public async Task<CustomerUserViewModel> GetUserById(string id)
        {
            try
            {
                var uvm = await (from u in UserManager.Users
                                     // join c in stdb.Customer on u.Id equals c.UserId
                                 where u.Id == id
                                 select new CustomerUserViewModel
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
                                     UpdatedAt = Utils.GetDefaultDateFormat(u.UpdatedAt),
                                     Lockout = u.LockoutEnd == null ? false : true,
                                     IsOnfidoVerify = "N/A",
                                     OnfidoChecksResult = "N/A",
                                     OnfidoDocuments = new List<string>(),
                                     BeneficiaryList = new List<SelectListItem>(),
                                     LexisNexisReports = new List<SelectListItem>()
                                 }).FirstOrDefaultAsync();

                uvm.OnfidoDocuments = new List<string>();
                uvm.BeneficiaryList = new List<SelectListItem>();
                uvm.LexisNexisReports = new List<SelectListItem>();
                uvm.SanctionPepPerson = new List<SanctionPepPersonViewModel>();
                var customer = await stdb.Customer.Where(x => x.UserId == id).FirstOrDefaultAsync();
                if (customer != null)
                {
                    uvm.IsOnfidoVerify = (customer.IsOnfidoVerify) ? "completed" : "not verified";
                    uvm.OnfidoChecksResult = (customer.OnfidoChecksResult == null) ? "N/A" : customer.OnfidoChecksResult;

                    var applicantId = await stdb.OnfidoApplicant.Where(x => x.CustomerId == customer.Id).Select(x => x.ApplicantId).FirstOrDefaultAsync();
                    var onfidoDocumentsList = new List<string>();
                    var onfidoDocuments = await stdb.OnfidoApplicantDocument.Where(x => x.applicant_id == applicantId).Select(x => x.Url).ToListAsync();
                    var onfidoLivePhotos = await stdb.OnfidoApplicantLivePhoto.Where(x => x.applicant_id == applicantId).Select(x => x.Url).ToListAsync();

                    if (onfidoDocuments.Count > 0)
                    {
                        onfidoDocumentsList.AddRange(onfidoDocuments);
                    }

                    if (onfidoLivePhotos.Count > 0)
                    {
                        onfidoDocumentsList.AddRange(onfidoLivePhotos);
                    }

                    if (onfidoDocumentsList.Count > 0)
                    {
                        uvm.OnfidoDocuments = onfidoDocumentsList;
                    }

                    var beneficiaryList = beneficiarySetupService.GetBeneficiaryListAsync(id);


                    if (beneficiaryList.Count > 0)
                    {
                        uvm.BeneficiaryList = beneficiaryList;
                    }

                    uvm.DOB = (customer.DOB != DateTime.MinValue) ? Utils.GetDefaultDateFormatToDetail(customer.DOB) : "";
                    uvm.IdType = await stdb.IdentificationType.Where(x => x.Id == customer.IdType).Select(x => x.IdentificationTypeName).FirstOrDefaultAsync();
                    uvm.IdNumber = customer.IdNumber;
                    uvm.IdExpirationDate = (customer.IdExpiryDate != DateTime.MinValue) ? Utils.GetDefaultDateFormatToDetail(customer.IdExpiryDate) : "";
                    uvm.PostCode = customer.PostCode;
                    uvm.City = customer.City;

                    var data = await (from ln in stdb.LexisNexis
                                      where ln.UserId == customer.UserId
                                      select new SanctionDataModel
                                      {
                                          SanctionMatch = ln.SanctionMatch,
                                          PepMatch = ln.PepMatch,
                                          ProfileUrl = ln.ProfileUrl,
                                          Date = ln.CreatedDate,
                                          Reference = ln.Reference
                                      }).FirstOrDefaultAsync();

                    var confirmMatchData = await (from ln in stdb.LexisNexis
                                      join spc in stdb.SanctionPepCustomer on ln.Id equals spc.LexisNexisId
                                      where ln.UserId == customer.UserId
                                      select spc).FirstOrDefaultAsync();

                    if(confirmMatchData != null)
                    {
                        if (!confirmMatchData.IsMatch)
                        {
                            data.SanctionMatch = false;
                            data.PepMatch = false;
                            data.Date = confirmMatchData.CreatedDate;
                        }
                    }

                    if(data != null)
                    {
                        uvm.IsSanctionMatch = data.SanctionMatch.ToString();
                        uvm.IsPepMatch = data.PepMatch.ToString();
                        uvm.LexisNexisReports.Add(new SelectListItem
                        {
                            Value = data.ProfileUrl,
                            Text = data.Date.ToString("yyyy-MM-dd") + "_" + data.Reference
                        });
                    }
                    //if (data.Count > 0)
                    //{
                    //    if (data.Select(x => x.PepsType).ToList().Contains((int)SanctionPep.Sanction))
                    //    {
                    //        sanctionCheck = true;
                    //    }

                    //    if (data.Select(x => x.PepsType).ToList().Contains((int)SanctionPep.Pep))
                    //    {
                    //        pepCheck = true;
                    //    }

                    //    foreach (var item in data)
                    //    {
                    //        uvm.LexisNexisReports.Add(new SelectListItem
                    //        {
                    //            Value = item.ProfileUrl,
                    //            Text = item.Date.ToString("yyyy-MM-dd") + "_" + item.Reference
                    //        });
                    //    }
                    //}


                    //var personData = await (from ln in stdb.LexisNexis
                    //                  join spp in stdb.SanctionPepPerson on ln.Id equals spp.LexisNexisId
                    //                  where ln.UserId == customer.UserId
                    //                  select spp).ToListAsync();

                    uvm.SanctionPepPerson = await GetPersonByUserId(customer.UserId); //mapper.Map<List<SanctionPepPersonViewModel>>(personData);
                }

                var role = db.UserRoles.Where(x => x.UserId == uvm.Id).FirstOrDefault();

                var groupData = db.UserMediaGroup.Where(x => x.UserId == id).Select(y => y.GroupId).FirstOrDefault();
                var groupIds = new List<string>();
                if (groupData != null)
                {
                    groupIds = new List<string>(groupData.Split(','));
                }

                uvm.Ids = groupIds;

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
            catch (Exception ex)
            {
                return new CustomerUserViewModel();
            }

        }

        public ChangePasswordViewModel GetForChangePassword()
        {
            var result = GetUserById(getLoggedInUserId()).Result;

            var details = iMapper.Map<ChangePasswordViewModel>(result);

            return details;

        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel cpvm)
        {
            var user = await UserManager.FindByIdAsync(cpvm.Id);
            return await UserManager.ChangePasswordAsync(user, cpvm.OldPassword, cpvm.NewPassword);

        }

        public async Task<SignInResult> Login(string UserName, string Password, bool RememberMe, bool lockout)
        {
            var result = await SignInManager.PasswordSignInAsync(UserName, Password, RememberMe, lockout);

            return result;
        }

        public async Task<bool> DeleteUserById(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user.Id == getLoggedInUserId())
                return false;
            string FullName = user.FirstName + " " + user.LastName;
            if (CheckUserForDeletion(id))
            {
                if (StepsBeforeUserDelete(id))
                {
                    var result = UserManager.DeleteAsync(user).Result;
                    if (result.Succeeded)
                    {
                        await activityLogService.CreateLog(getLoggedInUserId(), "User Profile Deleted for " + FullName + ". Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + getLoggedInUserId() + "/edit.html' data - toggle = 'tooltip' > " + GetUserFullName().Result + " </a> ");
                        return true;
                    }
                    Log.LogError("UserServices - DeleteUserById - " + id + " - : " + result.Errors.ToString());
                    return false;
                }
            }
            Log.LogError("UserServices - DeleteUserById- " + id);
            return false;
        }
        private bool StepsBeforeUserDelete(string id)
        {
            try
            {
                TenantUser tenantUser = db.TenantUser.Where(x => x.UserId == id).SingleOrDefault();
                db.TenantUser.Remove(tenantUser);
                List<ActivityLog> activities = db.ActivityLog.Where(x => x.UserId == id).ToList();
                activities.ForEach(x => db.ActivityLog.Remove(x));
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private bool CheckUserForDeletion(string id)
        {
            int count = 0;
            count = db.UserMedia.Where(x => x.UserId == id).ToList().Count
                     + db.MessageUser.Where(x => x.UserId == id).ToList().Count
                     + db.Case.Where(x => x.UserId == id).ToList().Count;
            if (count > 0)
            {
                return false;
            }
            else { return true; }
        }

        public async Task<bool> ActiveUserById(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            user.Status = true;
            var result = UserManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                await activityLogService.CreateLog(getLoggedInUserId(), "User Profile changed to Active for " + user.FirstName + " " + user.LastName + ". Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + getLoggedInUserId() + "/edit.html' data - toggle = 'tooltip' > " + GetUserFullName().Result + " </a> ");
                return true;
            }
            Log.LogError("UserServices - ActiveUserById - " + id + " - : " + result.Errors.ToString());
            return false;
        }

        public async Task<bool> InactiveUserById(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            user.Status = false;
            var result = UserManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                await activityLogService.CreateLog(getLoggedInUserId(), "User Profile changed to Inactive for " + user.FirstName + " " + user.LastName + ". Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + getLoggedInUserId() + "/edit.html' data - toggle = 'tooltip' > " + GetUserFullName().Result + " </a> ");
                return true;
            }
            Log.LogError("UserServices - InactiveUserById - " + id + " - : " + result.Errors.ToString());
            return false;
        }

        public dynamic getLoggedInUserId()
        {
            try
            {
                var principal = IHttpContextAccessor.HttpContext.User;
                string userId = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                return userId;

            }
            catch (Exception ex)
            {
                Log.LogError("UserServices - " + ex.ToString());
                return false;
            }
        }

        public async Task<bool> Logout()
        {
            await SignInManager.SignOutAsync();
            return true;
        }

        public bool IsLogin()
        {
            if (SignInManager.IsSignedIn(IHttpContextAccessor.HttpContext.User))
            {
                return true;
            }
            return false;
        }

        public bool IsDuplicateEmailInTenant(string Email, string Id, int TenantId)
        {

            if (Id == "0")
            {
                return (!db.Users.Any(d => d.Email == Email && (d.TenantUsers.Any(x => x.TenantId == TenantId) || d.IsSuperAdmin == true)));
            }
            else
            {
                return (!db.Users.Any(d => d.Email == Email && d.Id != Id && (d.TenantUsers.Any(x => x.TenantId == TenantId) || d.IsSuperAdmin == true)));
            }

        }

        public bool IsDuplicateEmail(string Email)
        {
            return (db.Users.Any(d => d.Email == Email));
        }

        private bool TenantAndUserExist(string id, int TenantId)
        {
            return (db.Users.Any(d => d.Id == id && d.TenantUsers.Any(x => x.TenantId == TenantId)));
        }

        public async Task<string> GetUserFullName()
        {
            var user = await UserManager.FindByIdAsync(this.getLoggedInUserId());

            if (user != null)
            {
                return user.FirstName + " " + user.LastName;
            }

            return "";
        }

        public string GetRoleByUserIdAsync(string id)
        {
            try
            {
                var result = db.UserRoles.Where(x => x.UserId == id)
                .Join(db.Roles, a => a.RoleId, b => b.Id, (a, b) => new { a, b })
                .Select(y => y.b.NormalizedName).FirstOrDefault();
                return result.ToString();
            }
            catch (Exception)
            {
                return " ";
            }
        }

        public IEnumerable<MediaViewModel> GetImagesByUserId(string id)
        {
            var mediaList = db.UserMedia
                  .Join(db.Media, w => w.MediaId, z => z.Id, (w, z) => new { w, z })
                  .Where(x => x.w.UserId == id)
                  .Select(b => new MediaViewModel
                  {
                      Id = b.z.Id,
                      CreatedBy = b.z.CreatedBy,
                      Description = b.z.Description,
                      Title = b.z.Title,
                      Url = b.z.Url
                  }).OrderByDescending(a => a.Id).ToList();

            return mediaList;
        }

        public async Task<string> GetDefaultOrFirstImagesByUserId(string id, string _default = "default-avatar.png")
        {
            MediaViewModel mediaList = await db.UserMedia
                      .Join(db.Media, w => w.MediaId, z => z.Id, (w, z) => new { w, z })
                      .Where(x => x.w.UserId == id)
                      .Select(b => new MediaViewModel
                      {
                          Id = b.z.Id,
                          CreatedBy = b.z.CreatedBy,
                          Description = b.z.Description,
                          Title = b.z.Title,
                          Url = b.z.Url
                      }).OrderByDescending(a => a.Id).FirstOrDefaultAsync();

            if (mediaList != null)
            {
                return mediaList.Url;
            }
            return _default;
        }

        public List<UserMessageListViewModel> GetUserList()
        {
            try
            {

                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

                var userlist = db.Users
                                .Join(db.TenantUser, x => x.Id, z => z.UserId, (x, z) => new { x, z })
                                .Where(i => i.z.TenantId == tenantid)
                                .Select(u => new UserMessageListViewModel
                                {
                                    Id = u.x.Id,
                                    Name = u.x.FirstName + " " + u.x.LastName + " (" + u.x.Email + ")"
                                }).ToList();

                //var userlist = db.Users
                //                .Join(db.UserRoles, x => x.Id, z => z.UserId, (x, z) => new { x, z })
                //                .Join(db.Roles, d => d.z.RoleId, e => e.Id, (d, e) => new { d, e })
                //                //.Where(i => i.h.NormalizedName.ToLower() =="user")
                //                .Select(u => new UserMessageListViewModel
                //                {
                //                    Id = u.d.x.Id,
                //                    Name = u.d.x.FirstName + " " + u.d.x.LastName + " (" + u.d.x.Email + ")"
                //                }).ToList();

                //var userlist = db.Users
                //                .GroupJoin(db.UserRoles, x => x.Id, z => z.UserId, (x, z) => new { x, z })
                //                .SelectMany(y => y.z.DefaultIfEmpty(), (a, b) => new { a, b })
                //                .GroupJoin(db.Roles, d => d.b.RoleId, e => e.Id, (d, e) => new { d, e })
                //                .SelectMany(f => f.e.DefaultIfEmpty(), (g, h) => new { g, h })
                //                //.Where(i => i.h.NormalizedName.ToLower() =="user")
                //                .Select(u => new UserMessageListViewModel
                //                {
                //                    Id = u.g.d.a.x.Id,
                //                    Name = u.g.d.a.x.FirstName + " " + u.g.d.a.x.LastName + " - " + u.h.DisplayName
                //                }).ToList();

                return userlist;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<UserMessageListViewModel> GetBackendUserList()
        {
            try
            {

                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

                //var userlist = db.Users
                //                .GroupJoin(db.UserRoles, x => x.Id, z => z.UserId, (x, z) => new { x, z })
                //                .SelectMany(y => y.z.DefaultIfEmpty(), (a, b) => new { a, b })
                //                .GroupJoin(db.Roles, d => d.b.RoleId, e => e.Id, (d, e) => new { d, e })
                //                .SelectMany(f => f.e.DefaultIfEmpty(), (g, h) => new { g, h })
                //                .Where(i => i.h.NormalizedName.ToLower() == "claim manager")
                //                .Select(u => new ManagerViewModel
                //                {
                //                    Id = u.g.d.a.x.Id,
                //                    Name = u.g.d.a.x.FirstName + " " + u.g.d.a.x.LastName
                //                }).ToList();

                var userlist = db.Users
                                    .Join(db.TenantUser, u => u.Id, tu => tu.UserId, (u, tu) => new { u, tu })
                                    .Where(i => i.tu.TenantId == tenantid)
                                    .GroupJoin(db.UserRoles, u => u.u.Id, ur => ur.UserId, (a, b) => new { u = a.u, ur = b })
                                    .SelectMany(y => y.ur.DefaultIfEmpty(), (a, b) => new { u = a.u, ur = b })
                                    .GroupJoin(db.RoleClaims, ur => ur.ur.RoleId, rc => rc.RoleId, (c, d) => new { ur = c, rc = d })
                                    .SelectMany(f => f.rc.DefaultIfEmpty(), (c, d) => new { ur = c.ur, rc = d })
                                    .Where(i => i.rc.ClaimValue.Equals("backend", StringComparison.OrdinalIgnoreCase) && i.rc.ClaimType.Equals("Side", StringComparison.OrdinalIgnoreCase) || i.ur.u.IsSuperAdmin == true)
                                    .Select(u => new UserMessageListViewModel
                                    {
                                        Id = u.ur.u.Id,
                                        Name = u.ur.u.FirstName + " " + u.ur.u.LastName,
                                        RoleName = db.Roles.Where(x => x.Id == u.ur.ur.RoleId).Select(x => x.DisplayName).FirstOrDefault()
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
                var principal = IHttpContextAccessor.HttpContext.User;
                string userId = principal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault();
                var result = await db.Users.Where(x => x.Id == userId).Select(b => b.IsSuperAdmin).FirstOrDefaultAsync();

                if (result == true)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Log.LogError("UserServices - " + ex.ToString());
                return false;
            }
        }

        public async Task<bool> IsSuperAdminEmail(string email)
        {
            try
            {

                var result = await db.Users.Where(x => x.Email == email).Select(z => z.IsSuperAdmin).FirstOrDefaultAsync();

                if (result == true)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Log.LogError("UserServices - " + ex.ToString());
                return false;
            }
        }

        public async Task<string> GetTenantIdentifierbyEmail(string email)
        {
            var loggedUser = db.Users.Where(x => x.Email == email).Select(z => new { z.Id, z.IsSuperAdmin }).FirstOrDefault();
            if (loggedUser != null)
            {
                string tenantIdentifier = await db.TenantUser
                                            .Where(x => x.UserId == loggedUser.Id)
                                            .Include(y => y.TenantForUser)
                                            .Select(b => b.TenantForUser.Identifier).FirstOrDefaultAsync();

                return tenantIdentifier;
            }
            return null;
        }

        public bool CheckUserInTenant(string tenantIdentifier)
        {
            string loggedUser = (string)getLoggedInUserId();

            bool result = db.Tenant.Where(x => x.Identifier == tenantIdentifier)
                                .Any(c => c.TenantUsers.Any(b => b.UserId == loggedUser));

            if (IsSuperAdmin().Result == true)
            {
                result = db.Tenant.Any(x => x.Identifier == tenantIdentifier);
            }
            return result;
        }

        public string UserHasPolicy()
        {
            var principal = IHttpContextAccessor.HttpContext.User;

            var result = principal.Claims.Where(x => x.Type == "Side" || x.Type == "access").Select(x => x.Value).FirstOrDefault();

            return result;

        }

        public string GetTenantIdentifierByUserId()
        {
            string loggedUser = (string)getLoggedInUserId();

            //superadmin has a selected tenant too
            return db.TenantUser
                            .Where(x => x.UserId == loggedUser || IsSuperAdmin().Result == true)
                            .Include(y => y.TenantForUser).Select(b =>
                            b.TenantForUser.Identifier).FirstOrDefault();
        }
        /// <summary>
        /// returns count of tenant users.
        /// </summary>
        /// <returns></returns>
        public int GetTenantUsersCount()
        {
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            return db.TenantUser.Where(x => x.TenantId == tenantid).ToList().Count();
        }

        public List<CustomerUserViewModel> GetAddedUserData(DateTime startDatetime)
        {
            string loggedUser = commonService.getLoggedInUserId();
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            List<ApplicationUser> listUser = db.Users.Where(x => (x.CreatedBy == loggedUser) && x.CreatedAt >= startDatetime).ToList();
            return iMapper.Map<List<CustomerUserViewModel>>(listUser);
        }

        public bool RemoveUserById(string userId)
        {
            try
            {
                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                ApplicationUser user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                TenantUser tuser = db.TenantUser.Where(x => x.UserId == userId && x.TenantId == tenantid).FirstOrDefault();
                db.TenantUser.Remove(tuser);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckIfActiveUser(string userName)
        {
            var isActive = db.Users.Where(x => x.EmailConfirmed == true && x.UserName == userName).FirstOrDefault();
            if (isActive == null || isActive.EmailConfirmed != true)
                return false;
            return true;
        }
        public bool CheckIfEmailExists(string userName)
        {
            var isActive = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
            if (isActive == null)
                return false;
            return true;
        }

        public async Task<List<SanctionPepPersonViewModel>> GetPersonByUserId(string userId)
        {
            try
            {
                var personData = await(from ln in stdb.LexisNexis
                                       join spp in stdb.SanctionPepPerson on ln.Id equals spp.LexisNexisId
                                       where ln.UserId == userId
                                       select spp).ToListAsync();

                return mapper.Map<List<SanctionPepPersonViewModel>>(personData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class SanctionDataModel
    {
        public bool SanctionMatch { get; set; }
        public bool PepMatch { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
    }
}
