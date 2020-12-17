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

namespace Cicero.Service.Services
{
    public interface ISkillSetService
    {
        List<SelectListItem> GetAllSkillSetForTenant();
        string GetAllSkillSetJsonForTenant();
        bool SetSkillSetToUser(string userId, int skillSetId);
        bool CreateOrUpdateSkillSet(SkillSetViewModel skillSetViewModel);
        JObject ExchangeSkillSet(int fromSkillSet, int toSkillSet);
        List<SkillSetViewModel> GetSkillSets(int tenantid);
        JObject DeleteSkill(int skillId);
    }

    public class SkillSetService : ISkillSetService
    {
        private readonly ApplicationDbContext _db;
        private readonly Utils _utils;
        private readonly ILogger<SkillSetService> _log;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _iMapper;
        private readonly ICommonService _commonService;
        private readonly IActivityLogService _activityLogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSetting _appSetting = null;

        public SkillSetService(ApplicationDbContext db, Utils utils, AppSetting appSetting, UserManager<ApplicationUser> userManager,
           ILogger<SkillSetService> log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper iMapper,
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

        public bool CreateOrUpdateSkillSet(SkillSetViewModel skillSetViewModel)
        {
            try
            {
                SkillSet skillSet = _iMapper.Map<SkillSet>(skillSetViewModel);

                if (skillSetViewModel.Id != 0)
                {
                    _db.SkillSet.Update(skillSet);
                }
                else
                {
                    _db.SkillSet.Add(skillSet);
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public JObject DeleteSkill(int skillId)
        {
            try
            {
                List<UserSkillSet> check = _db.UserSkillSet.Where(x => x.SkillSetId == skillId).ToList();
                if (check.Count == 0)
                {
                    SkillSet skillSet = _db.SkillSet.Find(skillId);
                    _db.SkillSet.Remove(skillSet);
                    _db.SaveChanges();
                    return _utils.ReturnResult("success", "Skill deleted successfully.");
                }
                else
                {
                    return _utils.ReturnResult("error", "There are " + check.Count() + " user associated with this skill. Please replace the skill with another one before deleting.");
                }

            }
            catch (Exception ex)
            {
                return _utils.ReturnResult("error", "Couldn't be deleted", ex.ToString());
            }
        }


        public JObject ExchangeSkillSet(int fromSkillSet, int toSkillSet)
        {
            try
            {
                List<UserSkillSet> userSkillSets = _db.UserSkillSet.Where(x => x.SkillSetId == fromSkillSet).ToList();
                List<UserSkillSet> newUserSkillSets = new List<UserSkillSet>();
                if(userSkillSets.Count > 0)
                {
                    foreach (var item in userSkillSets)
                    {
                        UserSkillSet temp = new UserSkillSet();
                        temp.UserId = item.UserId;
                        temp.SkillSetId = toSkillSet;
                        newUserSkillSets.Add(temp);
                    }
                    _db.UserSkillSet.RemoveRange(userSkillSets);
                    _db.UserSkillSet.AddRange(newUserSkillSets);
                    _db.SaveChanges();
                    return _utils.ReturnResult("success", "Skill set exchanged successfully.");
                }
                else
                {
                    return _utils.ReturnResult("info", "No user associated with for exchange.");
                }
                
                

            }
            catch (Exception ex)
            {
                return _utils.ReturnResult("error", "Couldn't be exchanged successfully.", ex.ToString());
            }
        }

        public List<SelectListItem> GetAllSkillSetForTenant()
        {
            var tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in _db.SkillSet.Where(x => x.IsActive == true && x.TenantId == tenantid).OrderBy(x=> x.Id).ToList())
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.Title;
                selectListItem.Value = item.Id.ToString();
                selectListItems.Add(selectListItem);
            }
            return selectListItems;
        }

        public string GetAllSkillSetJsonForTenant()
        {
            var tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            List<string> skillListItems = new List<string>();
            foreach (var item in _db.SkillSet.Where(x => x.IsActive == true && x.TenantId == tenantid).OrderBy(x => x.Id).ToList())
            {
                skillListItems.Add(item.Id.ToString());
            }
            return JsonConvert.SerializeObject(skillListItems);
        }

        public List<SkillSetViewModel> GetSkillSets(int tenantid)
        {
            return _iMapper.Map<List<SkillSetViewModel>>(_db.SkillSet.Where(x => x.TenantId == tenantid).ToList());
        }

        public bool SetSkillSetToUser(string userId, int skillSetId)
        {
            try
            {
                UserSkillSet userSkill = _db.UserSkillSet.Where(x => x.UserId == userId).FirstOrDefault();

                if (userSkill != null)
                {
                    if (userSkill.SkillSetId != skillSetId)
                    {
                        _db.UserSkillSet.Remove(userSkill);
                        _db.SaveChanges();
                    }
                    else
                    {
                        return true;
                    }
                }
                if (skillSetId != 0)
                {
                    userSkill = new UserSkillSet()
                    {
                        UserId = userId,
                        SkillSetId = skillSetId
                    };
                    _db.UserSkillSet.Add(userSkill);
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
