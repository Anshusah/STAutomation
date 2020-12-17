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
    public struct dashboardCounts
    {
        public int roleCount { get; set; }
        public int userCount { get; set; }
        public int claimFormsCount { get; set; }
        public int workflowCount { get; set; }
        public int mediaCount { get; set; }
        public int templateCount { get; set; }
        public int reportCount { get; set; }
    }
    public struct dashboardAddedData
    {
        public string roleData { get; set; }
        public string UserData { get; set; }
        public string workflowData { get; set; }
    }

    public interface ITenantConfig
    {
        dashboardCounts GetTenantConfigCount(int caseFormId);
        List<CaseFormListViewModel> GetTenantForm(bool isActiveOnly);
        bool CheckCaseFrom(string encryptedId);
        dashboardAddedData GetTenantAddedData(DateTime startDatetime);
        Task<bool> RevertProcessAsync(string[] role, string[] user, string[] workflow, string CaseFormId, string delStep);
        AdminConfigViewModel GetAdminConfigByKeyId(string KeyId);
        bool RemoveAdminConfigByKeyId(string KeyId);
        bool AddOrUpdateAdminConfig(string keyId, string value);
    }
    public class TenantConfig : ITenantConfig
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TenantConfig> _log;
        private readonly ICommonService _commonService;
        private readonly Utils _utils;
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;
        private readonly IRoleService _roleService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IQueueService _queueService;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly ITemplateService _templateService;
        private readonly IWorkflowService _workflowService;

        public TenantConfig(ApplicationDbContext db, IMapper mapper,
            ITemplateService templateService, IMediaService mediaService, 
            IQueueService queueService, ILogger<TenantConfig> log, ICommonService commonService,
            Utils utils, IUserService userService, ITenantService tenantService, IRoleService roleService,
            IFormBuilderService formBuilderService, IWorkflowService workflowService)
        {
            _db = db;
            _log = log;
            _utils = utils;
            _templateService = templateService;
            _userService = userService;
            _tenantService = tenantService;
            _roleService = roleService;
            _formBuilderService = formBuilderService;
            _commonService = commonService;
            _queueService = queueService;
            _mapper = mapper;
            _mediaService = mediaService;
            _workflowService = workflowService;
        }
        public bool RemoveAdminConfigByKeyId(string KeyId)
        {
            AdminConfig adminConfig = _db.AdminConfig.Where(x => x.KeyId == KeyId).FirstOrDefault();
            _db.Remove(adminConfig);
            _db.SaveChanges();
            return true;
        }

        public bool AddOrUpdateAdminConfig(string keyId, string value)
        {
            AdminConfig adminConfig = new AdminConfig();
            if (_db.AdminConfig.Where(x => x.KeyId == keyId).FirstOrDefault() != null)
            {
                adminConfig = _db.AdminConfig.Where(x => x.KeyId == keyId).FirstOrDefault();
            }
            adminConfig.KeyId = keyId;
            adminConfig.Value = value;
            if (adminConfig.Id != 0)
            {
                _db.AdminConfig.Update(adminConfig);
            }
            else
            {
                _db.AdminConfig.Add(adminConfig);
            }
            _db.SaveChanges();
            return true;
        }
        public AdminConfigViewModel GetAdminConfigByKeyId(string KeyId)
        {
            AdminConfigViewModel adminConfigViewModel = new AdminConfigViewModel();
            try
            {
                adminConfigViewModel = _mapper.Map<AdminConfigViewModel>(_db.AdminConfig.Where(x => x.KeyId == KeyId).FirstOrDefault());
            }
            catch (Exception ex)
            {

            }
            return adminConfigViewModel;
        }

        public bool CheckCaseFrom(string encryptedId)
        {
            CaseFormViewModel cf = _formBuilderService.GetBuilderFormById(_utils.DecryptId(encryptedId));
            if (cf.UpdatedAt == cf.CreatedAt)
            {
                return true;
            }
            return false;
        }


        public dashboardAddedData GetTenantAddedData(DateTime startDatetime)
        {
            dashboardAddedData data = new dashboardAddedData();
            List<RoleViewModel> roles = _roleService.GetRoleAddedList(startDatetime);
            List<UserViewModel> users = _userService.GetAddedUserData(startDatetime);
            // List<Queue> workflows = _queueService.GetAddedQueue(startDatetime);
            foreach (var item in roles)
            {
                if (data.roleData == null)
                {
                    data.roleData = item.Id;
                }
                else { data.roleData = data.roleData + "," + item.Id; }

            }
            foreach (var item in users)
            {
                if (data.UserData == null)
                { data.UserData = item.Id; }
                else
                {
                    data.UserData = data.UserData + "," + item.Id;
                }

            }
            //foreach (var item in workflows)
            //{
            //    if (data.workflowData == null)
            //    {
            //        data.workflowData = Convert.ToString(item.Id);
            //    }
            //    else { data.workflowData = data.workflowData + "," + Convert.ToString(item.Id); }

            //}
            return data;
        }
        /// <summary>
        /// Gets Tenant configuration counts for dashboard.
        /// </summary>
        /// <returns></returns>
        public dashboardCounts GetTenantConfigCount(int caseFormId)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            dashboardCounts counts = new dashboardCounts()
            {
                roleCount = _roleService.GetRoleList().Count(),
                claimFormsCount = _formBuilderService.GetActiveTenantForms(tenantid).Count(),
                userCount = _userService.GetTenantUsersCount(),
                workflowCount = _queueService.GetCaseFormWorkFlowCount(caseFormId),
                mediaCount = _mediaService.GetImagesByTenantId(tenantid).Count(),
                templateCount = _templateService.GetTemplateCount()
            };
            return counts;
        }
        /// <summary>
        /// Gets tenant forms
        /// </summary>
        /// <returns></returns>
        public List<CaseFormListViewModel> GetTenantForm(bool isActiveOnly)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            List<CaseFormListViewModel> formList = new List<CaseFormListViewModel>();
            if(isActiveOnly)
            {
                formList = _db.CaseForm
                       .Where(x => x.TenantId == tenantid && x.ModelName != null && x.Status==true).OrderByDescending(x => x.CreatedAt)
                       .Select(y => new CaseFormListViewModel
                       {
                           Name = y.Name,
                           Id = y.Id,
                           EncryptedId = _utils.EncryptId(y.Id),
                           Icon = y.Icon,
                           Url = _utils.GetTenantForUrl(true) + y.UrlIdentifier + "/" + _utils.EncryptId(0) + "/edit.html",
                           Status = y.Status
                       }).ToList();

            }
            else
            {
                formList = _db.CaseForm
                       .Where(x => x.TenantId == tenantid && x.ModelName != null).OrderByDescending(x => x.CreatedAt)
                       .Select(y => new CaseFormListViewModel
                       {
                           Name = y.Name,
                           Id = y.Id,
                           EncryptedId = _utils.EncryptId(y.Id),
                           Icon = y.Icon,
                           Url = _utils.GetTenantForUrl(true) + y.UrlIdentifier + "/" + _utils.EncryptId(0) + "/edit.html",
                           Status = y.Status
                       }).ToList();

            }
            return formList;
        }



        /// <summary>
        /// Delete process for reverting back to initial states
        /// </summary>
        /// <param name="role"></param>
        /// <param name="user"></param>
        /// <param name="workflow"></param>
        /// <param name="CaseFormId"></param>
        /// <returns></returns>
        public async Task<bool> RevertProcessAsync(string[] role, string[] user, string[] workflow, string caseFormId, string delStep)
        {
            try
            {
                if (delStep.ToLower() == "all")
                {


                    _queueService.RemoveWorkflowByCaseFormId(_utils.DecryptId(caseFormId));


                    if (user.Length >= 1)
                    {
                        foreach (var item in user)
                        {
                            _userService.RemoveUserById(item);
                        }
                    }
                    if (role.Length >= 1)
                    {
                        foreach (var item in role)
                        {
                            _roleService.RemoveRoleById(item);
                        }
                    }
                    if (caseFormId != null)
                    {
                        await _formBuilderService.DeleteBuilderFormById(_utils.DecryptId(caseFormId));
                    }
                }
                else
                {
                    switch (delStep)
                    {
                        case "step1":
                            {
                                if (caseFormId != null)
                                {
                                    await _formBuilderService.DeleteBuilderFormById(_utils.DecryptId(caseFormId));
                                }
                                break;
                            }
                        case "step2":
                            {
                                if (role.Length >= 1)
                                {
                                    foreach (var item in role)
                                    {
                                        _roleService.RemoveRoleById(item);
                                    }
                                }
                                break;
                            }
                        case "step3":
                            {
                                if (user.Length >= 1)
                                {
                                    foreach (var item in user)
                                    {
                                        _userService.RemoveUserById(item);
                                    }
                                }
                                break;
                            }
                        case "step4":
                            {
                                break;
                            }
                        case "step5":
                            {
                                if (caseFormId != null)
                                {
                                    _workflowService.RemoveWorkFlowByCaseFormId(_utils.DecryptId(caseFormId));
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
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
