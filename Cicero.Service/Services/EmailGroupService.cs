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
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Cicero.Service.Services
{
    public interface IEmailGroupService
    {
        List<SelectListItem> GetAllEmailGroupForTenant();
        List<EmailGroupViewModel> GetAllEmailGroupsByTenant();
        List<string> GetEmailsByEmailGroupId(int id);
        bool CreateOrUpdateUserGroup(EmailGroupViewModel egvm);
        JObject DeleteUserGroup(int skillId);
        List<string> GetEmailByText(string search);
    }

    public class EmailGroupService : IEmailGroupService
    {
        private readonly ApplicationDbContext _db;
        private readonly Utils _utils;
        private readonly ILogger<EmailGroupService> _log;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _iMapper;
        private readonly ICommonService _commonService;
        private readonly IActivityLogService _activityLogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSetting _appSetting = null;

        public EmailGroupService(ApplicationDbContext db, Utils utils, AppSetting appSetting, UserManager<ApplicationUser> userManager,
           ILogger<EmailGroupService> log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper iMapper,
           ICommonService commonService, IActivityLogService activityLogService)
        {
            _db = db;
            _utils = utils;
            _appSetting = appSetting;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _iMapper = iMapper;
            _activityLogService = activityLogService;
            _commonService = commonService;
            _userManager = userManager;
        }

        public bool CreateOrUpdateUserGroup(EmailGroupViewModel egvm)
        {
            try
            {
                EmailGroup emailGroup = _iMapper.Map<EmailGroup>(egvm);

                if (egvm.Id != 0)
                {
                    var oldEmails = _db.Emails.Where(x => x.EmailGroupId == egvm.Id).ToList();

                    _db.Emails.RemoveRange(oldEmails);

                    _db.EmailGroup.Update(emailGroup);

                }
                else
                {
                    _db.EmailGroup.Add(emailGroup);
                    //_db.SaveChanges();
                    //emailGroup.Emails.ToList().ForEach(x => x.EmailGroupId = emailGroup.Id);
                    //_db.Emails.AddRange(emailGroup.Emails);
                    //_db.SaveChanges();
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public JObject DeleteUserGroup(int id)
        {
            try
            {
                EmailGroup emailGroup = _db.EmailGroup.Where(x => x.Id == id).FirstOrDefault();

                    //SkillSet skillSet = _db.SkillSet.Find(id);
                    _db.EmailGroup.Remove(emailGroup);
                    _db.SaveChanges();

                return _utils.ReturnResult("success", "Email Group deleted successfully.");

            }
            catch (Exception ex)
            {
                return _utils.ReturnResult("error", "Couldn't be deleted", ex.ToString());
            }
        }

        public List<SelectListItem> GetAllEmailGroupForTenant()
        {
            var tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in _db.EmailGroup.Where(x => x.TenantId == tenantid).OrderBy(x=> x.Id).ToList())
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.Title;
                selectListItem.Value = item.Id.ToString();
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        public List<EmailGroupViewModel> GetAllEmailGroupsByTenant()
        {
            var tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var emailGroups = _iMapper.Map<List<EmailGroupViewModel>>(_db.EmailGroup.Include(x => x.Emails).Where(x => x.TenantId == tenantid).OrderBy(x => x.Id).ToList());

            return emailGroups;
        }

        public List<string> GetEmailsByEmailGroupId(int id)
        {
            //var tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            List<string> emailGroups = new List<string>();
            foreach (var item in _db.Emails.Where(x => x.EmailGroupId == id).OrderBy(x => x.Id).ToList())
            {
                emailGroups.Add(item.Emailstring.ToString());
            }
            return emailGroups;
        }

        public List<string> GetEmailByText(string search)
        {
            try
            {
                return _db.Users.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search) || x.Email.Contains(search)).Select(x => x.Email).Take(5).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }

        }
    }
}
