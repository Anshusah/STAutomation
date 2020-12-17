using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Extensions;
using Cicero.Service.Models;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.Case;
using static Cicero.Service.Enums;
using Hangfire;

namespace Cicero.Service.Services
{

    public interface ICaseService
    {
        Task<CaseViewModel> CreateOrUpdate(CaseViewModel cvm);
        Task<CaseFrontViewModel> CreateOrUpdate(CaseFrontViewModel cvm);
        CaseViewModel GetCaseById(int id);

        IEnumerable<CaseViewModel> GetCaseByUserId(bool all, int year = 0);

        IEnumerable<MediaViewModel> GetImagesByCaseId(int id);

        List<SelectListItem> GetCaseListToSelect();
        List<ClaimListViewModel> GetCaseListToSelectWithUserId();
        //List<QueueListViewModel> GetListOfQueues(string tenantidentifier);
        //QueueListViewModel GetCasesByUserIdAndQueue(int queueid);
        int GetQueueIdByStateId(int stateid);

        string GetQueueNameByQueueIdAndCaseFormId(int queueId, int caseFormId);

        int GetQueueIdByStateIdAndCaseFormId(int stateId, int caseFormId);

        List<KeyValuePair<int, string>> GetQueueNameList(int year = 0);

        List<CaseViewModel> GetListOfCasesByQueueId(int queueId, string queueName, bool all, int year = 0);

        string GetQueueIconByQueueId(int queueId);

        string GetQueueColorByQueueId(int queueId);

        string GetCaseFormTypeNameByCaseFormId(int caseFormId);

        string GenerateCaseId();

        Task<string> HtmlToPdf(int id);

        List<CaseViewModel> GetListofCasesByFormId(int caseFormId);
        List<SelectListItem> FormList(string tenantidentifier);

        CaseFormViewModel LoadForm(int id);
        dynamic GetListOfCasesByStateId(int formid, string searchText);
        List<CaseViewModel> GetListByStateId(int StateId, int formId);
        bool UpdateCase(List<CaseViewModel> caseList);
        bool DeleteCase(CaseViewModel cases);
        //states to which this case can move to
        List<StateViewModel> CaseTasks(int stateid, string roleid, bool isAdmin);

        List<KeyValuePair<int, string>> GetFormIdsByQueue(int queueid, int caseformid);

        Task<bool> CaseStateChangeById(int id, int stateid, string reason, string useraccessid);
        int SynchronizeCase(int caseId, int fromStateId, int toStateId);
        int GetStateIdByCaseId(int caseId);

        List<int> GetDateListsForCasePreview();
        Task<bool> SaveCaseStateHistory(CaseViewModel caseData, string loggedUser, string reason, int prevStateId, int newStateId);
        List<CaseStateHistoryViewModel> GetStateHistory(int caseId);
        List<SelectListItem> GetCaseStates(int caseId, int formid, string roleid);

        bool ResetCaseAssignment(int caseId);
    }

    public class CaseService : ICaseService
    {
        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<CaseService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private readonly IActivityLogService activityLogService;
        private readonly ICommonService commonService;
        private readonly IRazorToStringRender razorToStringRender;
        private readonly ITemplateService templateService;
        private readonly AppSetting _setting;
        private readonly IQueueService _queueService;
        private readonly IMessageService _messageService;
        private readonly IFormBuilderService formbuilderService;
        private readonly IAuditLogService _auditLogService;

        public CaseService(ICommonService _commonService, ApplicationDbContext _db, Utils _utils, ILogger<CaseService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, IActivityLogService _activityLogService, IRazorToStringRender _razorToStringRender, ITemplateService _templateService, AppSetting setting, IQueueService queueService, IMessageService messageService, IFormBuilderService _formbuilderService, IAuditLogService auditLogService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            commonService = _commonService;
            IMapper = _IMapper;
            activityLogService = _activityLogService;
            razorToStringRender = _razorToStringRender;
            templateService = _templateService;
            _setting = setting;
            _queueService = queueService;
            _messageService = messageService;
            formbuilderService = _formbuilderService;
            _auditLogService = auditLogService;
        }

        public CaseViewModel GetCaseById(int id)
        {
            try
            {

                string loggedUser = commonService.getLoggedInUserId();

                string roleid = commonService.GetRoleIdByUserId(loggedUser);

                bool isAdmin = commonService.IsSuperAdmin().Result;

                var caseData = db.Case.Where(x => x.Id == id);

                //var cases = IMapper.Map<CaseViewModel>(caseData);
                CaseViewModel cases = new CaseViewModel();
                foreach (var val in caseData)
                {
                    cases.Id = val.Id;

                    cases.CaseGeneratedId = val.CaseGeneratedId;
                    cases.UpdatedAt = val.UpdatedAt;
                    cases.CreatedAt = val.CreatedAt;
                    cases.UserId = val.UserId;
                    cases.TenantId = val.TenantId;
                    cases.CaseFormId = val.CaseFormId;
                    cases.StateId = val.StateId;
                    cases.AssignedTo = val.AssignedTo;
                    if(val.AssignedAt !=null)
                    {
                        cases.AssignedAt = (DateTime)val.AssignedAt;
                    }
                    if(val.DueDate !=null)
                    {
                        cases.DueDate = (DateTime)val.DueDate;
                    }
                }
                return cases;
            }
            catch (Exception ex)
            {
                Log.LogError("CaseService - GetCaseById - " + ex);
            }

            return new CaseViewModel { };
        }

        public IEnumerable<CaseViewModel> GetCaseByUserId(bool all, int year = 0)
        {
            try
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);

                string currentUser = commonService.getLoggedInUserId();
                dynamic caseData;
                if (all)
                {
                    //  caseData = db.Case.Where(x => x.UserId == currentUser).OrderBy(y => y.UpdatedAt);
                    if (!(year == -1))
                    {
                        caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                                    join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                    join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                    join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                    select c).Distinct().Where(x => x.CreatedAt.Year == year).OrderByDescending(x => x.UpdatedAt).ToList();
                    }
                    else
                    {
                        caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                                    join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                    join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                    join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                    select c).Distinct().OrderByDescending(x => x.UpdatedAt).ToList();
                    }
                }

                else
                {
                    if (!(year == -1))
                    {
                        caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                                    join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                    join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                    join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                    select c).Distinct().Where(x => x.CreatedAt.Year == year).OrderByDescending(x => x.UpdatedAt).Take(4).ToList();
                    }
                    else
                    {
                        caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                                    join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                    join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                    join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                    select c).Distinct().OrderByDescending(x => x.UpdatedAt).Take(4).ToList();
                    }

                }

                var cases = IMapper.Map<IEnumerable<CaseViewModel>>(caseData);

                return cases;
            }
            catch (Exception ex)
            {
                Log.LogError("CaseService - GetCaseById - " + ex);
            }
            List<CaseViewModel> ex1 = new List<CaseViewModel>();
            return ex1;
        }

        private async Task<int> Create(Case model)
        {
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            model.UserId = loggedUser;
            // model.CreatedAt = DateTime.Now;
            model.CaseGeneratedId = GenerateCaseId();
            await db.Case.AddAsync(model);

            await db.SaveChangesAsync();

            await activityLogService.CreateLog(loggedUser, "New Case created <a href ='/admin" + Utils.GetTenantForUrl(false) + "/Case/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.CaseGeneratedId + "</a>. Created By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
            string objectId = Convert.ToString(model.Id);
             await (_auditLogService.LogWriter(model, (int)AuditLogOperation.Insert, objectId, model));
         //   var jobId = BackgroundJob.Enqueue(() =>  _auditLogService.LogWriter(model, (int)AuditLogOperation.Insert, objectId, model,null,null));
            return model.Id;
        }

        private async Task<bool> Update(CaseViewModel cvm)
        {

            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            var currentCase = await db.Case.FindAsync(cvm.Id);
            var temp = JsonConvert.SerializeObject(currentCase, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });

            var oldCase = JsonConvert.DeserializeObject<Case>(temp);
            cvm.CreatedAt = currentCase.CreatedAt;
            cvm.UpdatedAt = DateTime.Now;
            cvm.UserId = currentCase.UserId;
            // cvm.UpdatedAt = currentCase.UpdatedAt;
            cvm.CaseGeneratedId = currentCase.CaseGeneratedId;
            currentCase = IMapper.Map(cvm, currentCase);
            db.Case.Update(currentCase);

            await db.SaveChangesAsync();
            string objectId = Convert.ToString(currentCase.Id);

            await activityLogService.CreateLog(loggedUser, "Case updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/Case/" + Utils.EncryptId(currentCase.Id) + "/edit.html'>" + currentCase.CaseGeneratedId + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
            await (_auditLogService.LogWriter(currentCase, (int)AuditLogOperation.Update, objectId, currentCase, oldCase));
         //   var jobid = BackgroundJob.Enqueue(() => _auditLogService.LogWriter(currentCase, (int)AuditLogOperation.Update, objectId, currentCase, oldCase, null));
            return true;
        }

        private async Task<bool> Update(CaseFrontViewModel cvm)
        {

            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            var currentCase = await db.Case.FindAsync(cvm.Id);
            //var caseform = await db.CaseForm.FindAsync(cvm.CaseFormId);
            cvm.UserId = currentCase.UserId;
            currentCase.UpdatedAt = DateTime.Now;
            //cvm.StateId = currentCase.StateId;
            //cvm.CreatedAt = Utils.GetDefaultDateFormatToDetail(currentCase.CreatedAt);
            cvm.CaseGeneratedId = currentCase.CaseGeneratedId;
            currentCase = IMapper.Map(cvm, currentCase);
            //currentCase.CaseForm = caseform;

            db.Entry(currentCase).State = EntityState.Modified;
            //db.Case.Update(currentCase);

            await db.SaveChangesAsync();

            await activityLogService.CreateLog(loggedUser, "Case updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/Case/" + Utils.EncryptId(currentCase.Id) + "/edit.html'>" + currentCase.CaseGeneratedId + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

            await activityLogService.CreateClaimLog(loggedUser, "Case updated to " + cvm.StateName, cvm.Id, cvm.StateId, cvm.TenantId);

            return true;
        }

        private async Task CaseMediaUpdate(CaseFrontViewModel cvm)
        {
            db.CaseMedia.RemoveRange(db.CaseMedia.Where(e => e.CaseId == cvm.Id));
            foreach (var item in cvm.Images)
            {
                CaseMedia modelMedia = new CaseMedia
                {
                    CaseId = cvm.Id,
                    MediaId = item
                };
                db.CaseMedia.Add(modelMedia);
            }
            await db.SaveChangesAsync();
        }

        public async Task<CaseViewModel> CreateOrUpdate(CaseViewModel cvm)
        {

            var caseData = IMapper.Map<Case>(cvm);

            caseData.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            if (cvm.Id == 0)
            {
                cvm.Id = await Create(caseData);

            }
            else
            {
                await Update(cvm);
            }


            //cvm.Id = caseData.Id;

            return cvm;
        }


        public async Task<CaseFrontViewModel> CreateOrUpdate(CaseFrontViewModel cvm)
        {

            var caseData = IMapper.Map<Case>(cvm);

            caseData.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            caseData.UpdatedAt = DateTime.Now;

            if (cvm.Id == 0)
            {

                cvm.Id = await Create(caseData);

            }
            else
            {
                await Update(cvm);
            }

            return cvm;
        }


        public IEnumerable<MediaViewModel> GetImagesByCaseId(int id)
        {
            var mediaList = db.CaseMedia
                  .Join(db.Media, w => w.MediaId, z => z.Id, (w, z) => new { w, z })
                  .Where(x => x.w.CaseId == id)
                  .Select(b => new MediaViewModel
                  {
                      Id = b.z.Id,
                      CreatedBy = b.z.CreatedBy,
                      Description = b.z.Description,
                      Title = b.z.Title,
                      Url = b.z.Url
                  }).ToList();

            return mediaList;
        }

        public List<SelectListItem> GetCaseListToSelect()
        {
            string loggedUser = commonService.getLoggedInUserId();
            return db.Case.Where(x => x.UserId == loggedUser).Select(y => new SelectListItem
            {
                Value = y.Id.ToString(),
                Text = y.CaseGeneratedId
            }).ToList();

        }

        public List<ClaimListViewModel> GetCaseListToSelectWithUserId()
        {
            string loggedUser = commonService.getLoggedInUserId();
            return db.Case.Select(y => new ClaimListViewModel
            {
                UserId = y.UserId,
                Value = y.Id.ToString(),
                Text = y.CaseGeneratedId
            }).ToList();

        }

        public int GetQueueIdByStateId(int stateid)
        {

            string loggeduser = commonService.getLoggedInUserId();

            string roleid = commonService.GetRoleIdByUserId(loggeduser);

            return db.Queue.Include(a => a.QueueForForm).ThenInclude(b => b.QueuePermissions.Where(a => a.QueueForForm.AllUser == true || a.RoleId == roleid)).SelectMany(x => x.QueueToState).Where(b => b.StateId == stateid).Select(y => y.QueueId).FirstOrDefault();

        }

        public int GetQueueIdByStateIdAndCaseFormId(int stateId, int caseFormId)
        {
            var queueId = db.QueueToState.Where(x => x.StateId == stateId && x.CaseFormId == caseFormId).Select(y => y.QueueId).FirstOrDefault();

            return queueId;
        }

        public string GetQueueNameByQueueIdAndCaseFormId(int queueId, int caseFormId)
        {
            var queueForFormId = db.QueueForForm.Where(x => x.QueueId == queueId && x.CaseFormId == caseFormId).Select(x => x.Id).FirstOrDefault();

            string loggeduser = commonService.getLoggedInUserId();
            var roleId = commonService.GetRoleIdByUserId(loggeduser);

            var queuePermission = db.QueuePermission.Where(x => x.QueueForFormId == queueForFormId && x.RoleId == roleId).FirstOrDefault();

            if (queuePermission != null)
            {
                var queueName = queuePermission.DisplayName;
                return queueName;
            }

            return string.Empty;
        }

        public List<KeyValuePair<int, string>> GetQueueNameList(int year = 0)
        {
            string currentUser = commonService.getLoggedInUserId();
            //dynamic caseData;
            //var caseData = db.Case.Where(x => x.UserId == currentUser).Select(x => new { StateId = x.StateId, CaseFormId = x.CaseFormId, CreatedAt = x.CreatedAt }).OrderByDescending(y => y.CreatedAt).ToList();

            string loggeduser = commonService.getLoggedInUserId();
            var roleid = commonService.GetRoleIdByUserId(loggeduser);

            var caseData = new List<Case>();

            if (!(year == -1))
            {
                caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                            join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                            join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                            join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                            select c).Where(x => x.CreatedAt.Year == year).Distinct().ToList();
            }
            else
            {
                caseData = (from c in db.Case.Where(x => x.UserId == loggeduser)
                            join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                            join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                            join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                            select c).Distinct().ToList();
            }

            List<KeyValuePair<int, string>> queueNameList = new List<KeyValuePair<int, string>>();

            foreach (var item in caseData)
            {

                var queueId = GetQueueIdByStateIdAndCaseFormId(item.StateId, item.CaseFormId);
                var queueForFormId = db.QueueForForm.Where(x => x.QueueId == queueId && x.CaseFormId == item.CaseFormId).Select(x => x.Id).FirstOrDefault();
                var queuePermission = db.QueuePermission.Where(x => x.QueueForFormId == queueForFormId && x.RoleId == roleid).FirstOrDefault();

                if (queuePermission != null)
                {
                    var queueName = queuePermission.DisplayName;
                    queueNameList.Add(new KeyValuePair<int, string>(queueId, queueName));
                }
            }

            var getDefaultQueues = db.Queue.Where(x => x.Name == "Unsubmitted" || x.Name == "Paid" || x.Name == "Rejected").Select(x => x.Name).Distinct().ToList();
            foreach (var item in getDefaultQueues)
            {
                if (!queueNameList.Select(x => x.Value).ToList().Contains(item))
                {
                    queueNameList.Add(new KeyValuePair<int, string>(0, item));
                }
            }
            return queueNameList.OrderBy(x => x.Value).ToList();
        }

        public List<CaseViewModel> GetListOfCasesByQueueId(int queueId, string queueName, bool all, int year = 0)
        {
            var caseFormIdList = new List<int>();
            if (!(year == -1))
            {
                caseFormIdList = db.Case.Where(x => x.CreatedAt.Year == year).Select(x => x.CaseFormId).Distinct().ToList();
            }
            else
            {
                caseFormIdList = db.Case.Select(x => x.CaseFormId).Distinct().ToList();
            }

            var caseDatas = new List<CaseViewModel>();


            string loggeduser = commonService.getLoggedInUserId();
            string roleid = commonService.GetRoleIdByUserId(loggeduser);

            foreach (var item in caseFormIdList)
            {
                var datas = new List<Case>();
                if (all)
                {
                    if (!(year == -1))
                    {
                        datas = (from c in db.Case.Where(x => x.UserId == loggeduser && x.CaseFormId == item)
                                 join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                 join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                 join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                 where qts.QueueId == queueId
                                 select c).Where(x => x.CreatedAt.Year == year).Distinct().OrderByDescending(x => x.UpdatedAt).ToList();
                    }
                    else
                    {
                        datas = (from c in db.Case.Where(x => x.UserId == loggeduser && x.CaseFormId == item)
                                 join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                 join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                 join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                 where qts.QueueId == queueId
                                 select c).Distinct().OrderByDescending(x => x.UpdatedAt).ToList();
                        
                    }
                }
                else
                {
                    if (!(year == -1))
                    {
                        datas = (from c in db.Case.Where(x => x.UserId == loggeduser && x.CaseFormId == item)
                                 join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                 join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                 join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                 where qts.QueueId == queueId
                                 select c).Where(x => x.CreatedAt.Year == year).Distinct().OrderByDescending(x => x.UpdatedAt).Take(4).ToList();
                    }
                    else
                    {
                        datas = (from c in db.Case.Where(x => x.UserId == loggeduser && x.CaseFormId == item)
                                 join qts in db.QueueToState.Where(x => x.IsQueue == true) on new { c.StateId, c.CaseFormId } equals new { qts.StateId, qts.CaseFormId }
                                 join qff in db.QueueForForm on qts.QueueId equals qff.QueueId
                                 join qp in db.QueuePermission.Where(x => x.RoleId == roleid) on qff.Id equals qp.QueueForFormId
                                 where qts.QueueId == queueId
                                 select c).Distinct().OrderByDescending(x => x.UpdatedAt).Take(4).ToList();
                    }
                }

                var cases = IMapper.Map<IEnumerable<CaseViewModel>>(datas);
                caseDatas.AddRange(cases);
            }
            var maincasesDatas = new List<CaseViewModel>();
            foreach (var item in caseDatas)
            {

                var queueForFormId = db.QueueForForm.Where(x => x.QueueId == queueId && x.CaseFormId == item.CaseFormId).Select(x => x.Id).FirstOrDefault();
                var queuePermission = db.QueuePermission.Where(x => x.QueueForFormId == queueForFormId && x.RoleId == roleid).FirstOrDefault();

                if (queuePermission != null)
                {
                    maincasesDatas.Add(item);
                }
            }
            foreach (var item in maincasesDatas)
            {
                var temp = formbuilderService.GetBuilderFormById(item.CaseFormId);

                if (temp != null)
                {
                    item.CaseFormUrl = temp.UrlIdentifier;
                }

                item.QueueId = queueId;
                item.QueueName = queueName;
                var queueIcon = db.Queue.Where(x => x.Id == queueId).Select(x => x.Icon).FirstOrDefault();
                item.QueueIcon = queueIcon;

                var queueColor = db.Queue.Where(x => x.Id == queueId).Select(x => x.Color).FirstOrDefault();
                item.QueueColor = queueColor;
            }

            List<CaseFormViewModel> lst = commonService.GetCaseFormListForActiveTenantId();

            foreach (var item in maincasesDatas)
            {
                item.VisibleInFooterViewModel = new List<VisibleInFooterViewModel>();

                CaseFormViewModel lsts = lst.Where(d => d.Id == item.CaseFormId).FirstOrDefault();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);

                List<FormBuilderViewModel.Form.Table> tables = allfields.Forms.Tables;
                var elementValue = GetTableData(item.Id, tables, allfields);

                foreach (var tab in allfields.Tab)
                {
                    foreach (var row in tab.Row)
                    {
                        foreach (var column in row.Column)
                        {
                            foreach (dynamic element in column.Element)
                            {
                                var isit = element.GetType().GetProperty("VisibleinFooter");
                                if (isit != null)
                                {
                                    if (element.VisibleinFooter != null && element.VisibleinFooter)
                                    {
                                        if (item.VisibleInFooterViewModel.Count != 2)
                                        {
                                            var textValue = string.Empty;
                                            var type = element.GetType().Name.ToLower();
                                            var iconUrl = string.Empty;
                                            if (elementValue["elm" + element.ElementId] != null)
                                            {
                                                if (elementValue["elm" + element.ElementId].Value != null)
                                                {
                                                    textValue = elementValue["elm" + element.ElementId].Value;
                                                }
                                            }

                                            if (type == "selectbox")
                                            {
                                                if (element.SelectOptions != null && element.SelectOptions.Count > 0)
                                                {
                                                    foreach (var soption in element.SelectOptions)
                                                    {
                                                        var soptionValue = soption.Value;
                                                        if (soptionValue != null)
                                                        {
                                                            soptionValue = soptionValue.Trim();
                                                            if (soptionValue == textValue.Trim())
                                                            {
                                                                iconUrl = soption.IconUrl;
                                                            }
                                                        }


                                                    }

                                                }
                                            }

                                            item.VisibleInFooterViewModel.Add(new VisibleInFooterViewModel
                                            {
                                                Text = textValue,
                                                Type = type,
                                                IconUrl = iconUrl
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return maincasesDatas.OrderByDescending(x => x.UpdatedAt).ToList();
        }

        public JObject GetTableData(int CaseId, List<FormBuilderViewModel.Form.Table> tables, FormBuilderViewModel a)
        {


            //var ccvm = _formBuilderService.GetBuilderFormById(CaseId);
            //var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            //ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            //FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            //List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
            string value = String.Empty;
            JObject FormValues = new JObject();
            if (tables != null)
            {
                foreach (var tb in tables)
                {
                    if (Utils.ConvertToString(tb.Name) != "")
                    {
                        bool exists = db.CustomEntities1.FromSql($"SELECT 1 cnt FROM sys.tables AS T  INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id   WHERE  T.Name = '{tb.Name}'", tb.Name).SingleOrDefault() != null;
                        if (exists)
                        {
                            var Forms = db.CustomEntities.FromSql($"SELECT * FROM [{tb.Name}] Where [CaseId] = {CaseId}", tb.Name, CaseId);
                            int j = 0;
                            JObject jsonObj = new JObject();

                            // var temp = Forms.Extras;
                            foreach (var item in Forms)
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                            }
                            foreach (var item in a.Tab)
                            {
                                foreach (var row in item.Row)
                                {
                                    foreach (var column in row.Column)
                                    {
                                        foreach (dynamic element in column.Element)
                                        {
                                            var isit = element.GetType().GetProperty("TableName");
                                            if (isit != null)
                                            {
                                                if (element.TableName == tb.Name)
                                                {

                                                    foreach (var data in jsonObj)
                                                    {
                                                        if (element.FieldName == data.Key)
                                                        {
                                                            //if (data.Key.Count() > 1)
                                                            //{

                                                            //}
                                                            FormValues.Add("elm" + element.ElementId, data.Value);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }

                    }


                }
            }

            return FormValues;
        }

        public string GetQueueIconByQueueId(int queueId)
        {
            var queueIcon = db.Queue.Where(x => x.Id == queueId).Select(x => x.Icon).FirstOrDefault();
            return queueIcon;
        }

        public string GetQueueColorByQueueId(int queueId)
        {
            var queueIcon = db.Queue.Where(x => x.Id == queueId).Select(x => x.Color).FirstOrDefault();
            return queueIcon;
        }

        public string GetCaseFormTypeNameByCaseFormId(int caseFormId)
        {
            var caseFormTypeName = db.CaseForm.Where(x => x.Id == caseFormId).Select(x => x.Name).FirstOrDefault();
            return caseFormTypeName;
        }

        public string GenerateCaseId()
        {
            string loggedUser = commonService.getLoggedInUserId();

            string generatedCaseId = null;

            var caseData = db.Case
                            .OrderByDescending(b => b.CreatedAt)
                            .Select(y => new
                            {
                                y.CaseGeneratedId,
                                y.CreatedAt
                            }).FirstOrDefault();

            if (caseData == null || caseData.CreatedAt.Date != DateTime.Now.Date)
            {
                generatedCaseId = Utils.GetDateForCase() + "100001";

                return generatedCaseId;
            }

            generatedCaseId = Utils.GetDateForCase() + (Convert.ToInt32(caseData.CaseGeneratedId.Substring(14)) + 1).ToString();

            return generatedCaseId;
        }

        public async Task<string> HtmlToPdf(int id)
        {
            try
            {
                // instantiate a html to pdf converter object 
                HtmlToPdf converter = new HtmlToPdf();

                var caseData = GetCaseById(id);
                //string userId = (string)commonService.getLoggedInUserId();
                string loggedUser = await commonService.GetUserFullName();

                if (caseData != null)
                {
                    string filename = caseData.CaseGeneratedId + ".pdf";
                    string folderpath = HostingEnvironment.ContentRootPath;
                    string url = folderpath + "/wwwroot/uploads/";
                    //string url = "/uploads/";

                    var pdfbody = new TemplateViewModel { };
                    string pdfbodyOld = "";
                    pdfbodyOld = templateService.GetTemplateBodyByName("subrogation_letter");

                    //caseData.DamageCategoryList = GetCaseCategoryList();
                    //caseData.DamageTypeList = GetTypeListByCategoryById(caseData.CategoryId);

                    string label = "";
                    string value = "";

                    if (!string.IsNullOrEmpty(pdfbodyOld))
                    {
                        pdfbodyOld = pdfbodyOld.Replace("[manager_name]", loggedUser);
                        pdfbodyOld = pdfbodyOld.Replace("[date_today]", Utils.GetDefaultDateFormatToDetail(DateTime.Now));
                        //pdfbodyOld = pdfbodyOld.Replace("[client_name]", caseData.FirstName + " " + caseData.SurName);
                        //pdfbodyOld = pdfbodyOld.Replace("[user_name]", caseData.FirstName + " " + caseData.SurName);

                        //pdfbodyOld = pdfbodyOld.Replace("[user]", caseData.BillOfLadingNumber);

                        //pdfbodyOld = pdfbodyOld.Replace("[bl_number]", caseData.BillOfLadingNumber);
                        //pdfbodyOld = pdfbodyOld.Replace("[containers]", caseData.NumberOfContainers.ToString());
                        //pdfbodyOld = pdfbodyOld.Replace("[vessel_name]", caseData.Vessel);
                        //pdfbodyOld = pdfbodyOld.Replace("[port_to]", caseData.To);
                        //pdfbodyOld = pdfbodyOld.Replace("[port_from]", caseData.From);
                        //pdfbodyOld = pdfbodyOld.Replace("[delivery_date]", caseData.CargoDeliveryDate);
                        //pdfbodyOld = pdfbodyOld.Replace("[nature_of_loss]", caseData.DamageCategoryList.Where(x => x.Value == caseData.CategoryId.ToString()).Select(d => d.Text).FirstOrDefault() + " " +
                        //                caseData.DamageTypeList.Where(b => b.Value == caseData.DamageTypeId.ToString()).Select(c => c.Text).FirstOrDefault());
                        //pdfbodyOld = pdfbodyOld.Replace("[client_name]", caseData.FirstName + " "  + caseData.SurName);

                        //need signature if implemented
                        pdfbodyOld = pdfbodyOld.Replace("[signature]", "");
                    }


                    pdfbody.Content = pdfbodyOld;

                    //string body = 

                    string htmlString = await razorToStringRender.RenderViewToStringAsync("Areas/Admin/Views/Email/SubrogationLetter.cshtml", pdfbody);


                    //string htmlString = templateService.GetTemplateBodyByName("subrogation_letter");
                    if (htmlString != null)
                    {
                        // create a new pdf document converting an url 
                        PdfDocument doc = converter.ConvertHtmlString(htmlString);

                        // save pdf document 
                        //byte[] pdf = doc.Save();
                        doc.Save(url + caseData.CaseGeneratedId + ".pdf");

                        // close pdf document 
                        doc.Close();

                        return url + caseData.CaseGeneratedId + ".pdf";
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<SelectListItem> FormList(string tenantidentifier)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenantidentifier);

            return db.CaseForm
                       .Where(x => x.TenantId == tenantid && x.Status == true).AsNoTracking()
                       .Select(y => new SelectListItem
                       {
                           Text = y.Name,
                           Value = y.Id.ToString()
                       }).ToList();
        }

        public CaseFormViewModel LoadForm(int id)
        {

            var caseForm = db.CaseForm
                        .Where(x => x.Id == id).AsNoTracking()
                        .FirstOrDefault();

            var vm = IMapper.Map<CaseFormViewModel>(caseForm);

            return vm;

        }

        public List<CaseViewModel> GetListofCasesByFormId(int caseFormId)
        {
            int tenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<Case> cases = db.Case.Where(x => x.TenantId == tenantId && x.CaseFormId == caseFormId).ToList();
            return IMapper.Map<List<CaseViewModel>>(cases);
        }

        public List<CaseViewModel> GetListByStateId(int StateId, int formId)
        {
            int tenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<Case> cases = db.Case.Where(x => x.TenantId == tenantId && x.CaseFormId == formId).ToList();
            return IMapper.Map<List<CaseViewModel>>(cases);
        }

        public dynamic GetListOfCasesByStateId(int formId, string searchText)
        {
            dynamic objects = new JArray();
            List<StateViewModel> sts = _queueService.GetAllStates();


            for (int i = 0; i < sts.Count; i++)
            {
                dynamic states = new JObject();
                states.state = (JsonConvert.SerializeObject(sts[i]));
                List<CaseViewModel> cs = GetListByStateId(sts[i].Id, formId);

                states.cases = new JArray();
                for (int j = 0; j < cs.Count; j++)
                {
                    states.cases.Add(JsonConvert.SerializeObject(cs[j]));

                }

                objects.Add(states);
            }
            return objects;
        }

        public bool DeleteCase(CaseViewModel cases)
        {
            try
            {
                Case caseItem = db.Case.Where(x => x.Id == cases.Id).SingleOrDefault();
                List<CaseMedia> caseMedias = db.CaseMedia.Where(x => x.CaseId == cases.Id).ToList();
                db.Case.Remove(caseItem);
                db.CaseMedia.RemoveRange(caseMedias);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool UpdateCase(List<CaseViewModel> caseList)
        {
            try
            {
                List<Case> cases = IMapper.Map<List<Case>>(caseList);
                foreach (var item in cases)
                {
                    Case caseItem = db.Case.Where(x => x.Id == item.Id).SingleOrDefault();

                    db.Case.Update(caseItem);
                }
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public List<StateViewModel> CaseTasks(int stateid, string roleid, bool isAdmin)
        {
            return IMapper.Map<List<StateViewModel>>(db.StateToState.
                Where(z => stateid == z.FromStateId && (z.FromState.StateForForm.SelectMany(y => y.StatePermissions).Where(x => x.RoleId == roleid) != null || roleid == "all" || isAdmin == true)).Select(w => w.ToState).ToList());

        }

        public List<KeyValuePair<int,string>> GetFormIdsByQueue(int queueid, int caseformid)
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
             var abc = db.QueueToState.Where(x => x.QueueId == queueid && x.IsQueue == true && x.CaseFormId == caseformid)
                .Join(db.Case, qts => qts.StateId, cas => cas.StateId, (x, z) => new { qts = x, cas = z })
                .OrderByDescending(y => y.cas.Id).Select(b => new SelectListItem {
                    Value = b.cas.Id.ToString(),
                    Text = b.cas.CaseGeneratedId
                   });
            foreach(var item in abc)
            {
                var temp = new KeyValuePair<int, string>(Convert.ToInt16(item.Value), item.Text);
                list.Add(temp);
            }
            return list;
        }

        public async Task<bool> CaseStateChangeById(int id, int stateid, string reason, string useraccessid)
        {
            var caseData = await db.Case.FindAsync(id);
            caseData.UpdatedAt = DateTime.Now;
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            if (caseData != null)
            {
                caseData.StateId = stateid;
                //caseData.UserAccessId = useraccessid;
                var result = db.Case.Update(caseData);
                db.SaveChanges();

                await activityLogService.CreateLog(loggedUser, "Case has been changed <a href ='/admin" + Utils.GetTenantForUrl(false) + "/case/" + Utils.EncryptId(caseData.Id) + "/edit.html'>" + caseData.CaseGeneratedId + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                await activityLogService.CreateClaimLog(loggedUser, "Case updated to " + _queueService.GetStateById(caseData.TenantId, caseData.StateId).SystemName, caseData.Id, caseData.StateId, caseData.TenantId);

                return true;
            }

            Log.LogError("CaseService - CaseStateChangeById - " + id + " - : ");
            return false;
        }

        public int SynchronizeCase(int caseId, int fromStateId, int toStateId)
        {
            //    var currentCase = db.Case.FindAsync(id).Result;

            //    int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            //    var syncsetting = _setting.Get("app_case_synchronization");

            //    string policyNumber = currentCase.PolicyNumber;

            //    int tostate = 0;

            //    bool check = false;

            //    SyncSettingViewModel ssvm = JsonConvert.DeserializeObject<SyncSettingViewModel>(syncsetting);
            //    if (ssvm != null)
            //    {
            //        foreach (var configs in ssvm.configs)
            //        {
            //            if (configs.pull.Equals(currentCase.StateId.ToString(), StringComparison.OrdinalIgnoreCase) && configs.pass.Equals(stateid.ToString(), StringComparison.OrdinalIgnoreCase) && configs.destination.Equals(currentCase.CaseFormId.ToString(), StringComparison.OrdinalIgnoreCase))
            //            {
            //                var policycases = db.PolicyManagement.Where(x => x.TenantId == tenantid && x.CaseFormId.ToString().Equals(configs.source, StringComparison.OrdinalIgnoreCase)).Select(y => y.Fields).ToList();

            //                JObject jobj = JObject.Parse(currentCase.Extras);
            //                DynamicFormViewModel ccdfvm = jobj.ToObject<DynamicFormViewModel>();

            //                foreach (var policycase in policycases)
            //                {
            //                    JObject obj = JObject.Parse(policycase);
            //                    DynamicFormViewModel pcdfvm = obj.ToObject<DynamicFormViewModel>();
            //                    if (pcdfvm.Tabs.Any(x => x.element.Any(y => y.name == configs.policyfield && y.value.Equals(policyNumber, StringComparison.OrdinalIgnoreCase))) && configs.process.Equals(process, StringComparison.OrdinalIgnoreCase))
            //                    {
            //                        check = true;

            //                        foreach (var cctab in ccdfvm.Tabs)
            //                        {
            //                            foreach (var ccelement in cctab.element)
            //                            {
            //                                foreach (var pctab in pcdfvm.Tabs)
            //                                {
            //                                    foreach (var pcelement in pctab.element)
            //                                    {
            //                                        for (int i = 0; i < configs.sourcefield.Where(x => x != null).Count(); i++)
            //                                        {
            //                                            if (configs.sourcefield[i].Equals(pcelement.name, StringComparison.OrdinalIgnoreCase) && (configs.destinationfield[i].Equals(ccelement.name, StringComparison.OrdinalIgnoreCase) || !ccdfvm.Tabs.Any(y => y.element.Any(x => x.name == configs.destinationfield[i]))))
            //                                            {
            //                                                if (pcelement.type == "select" || pcelement.type == "targetForm" || pcelement.type == "radio-group" || pcelement.type == "checkbox-group")
            //                                                {
            //                                                    if (pcelement.values != null && ccelement.values != null)
            //                                                    {
            //                                                        if (configs.sourcefield[i].Equals(pcelement.name, StringComparison.OrdinalIgnoreCase) && configs.destinationfield[i].Equals(ccelement.name, StringComparison.OrdinalIgnoreCase))
            //                                                        {
            //                                                            ccelement.values = pcelement.values;
            //                                                        }

            //                                                        foreach (var ccvalue in ccelement.values)
            //                                                        {
            //                                                            foreach (var pcvalue in pcelement.values)
            //                                                            {

            //                                                                switch (configs.destinationfield[i])
            //                                                                {
            //                                                                    case "Country":
            //                                                                        if (pcvalue.selected == true)
            //                                                                        {
            //                                                                            currentCase.Country = pcvalue.label;
            //                                                                        }
            //                                                                        break;
            //                                                                    case "VatRegistered":
            //                                                                        if (pcvalue.selected == true)
            //                                                                        {
            //                                                                            currentCase.VatRegistered = pcvalue.label;
            //                                                                        }
            //                                                                        break;
            //                                                                    case "HasChildren":
            //                                                                        if (pcvalue.selected == true)
            //                                                                        {
            //                                                                            currentCase.HasChildren = pcvalue.label;
            //                                                                        }
            //                                                                        break;
            //                                                                    default:
            //                                                                        break;
            //                                                                }
            //                                                            }

            //                                                        }

            //                                                    }
            //                                                    else if (pcelement.targetformdata != null && ccelement.targetformdata != null)
            //                                                    {
            //                                                        ccelement.targetformdata = pcelement.targetformdata;

            //                                                    }
            //                                                }
            //                                                else
            //                                                {
            //                                                    if (!string.IsNullOrWhiteSpace(pcelement.value))
            //                                                    {
            //                                                        switch (configs.destinationfield[i])
            //                                                        {
            //                                                            case "FirstName":
            //                                                                currentCase.FirstName = pcelement.value;
            //                                                                break;
            //                                                            case "SurName":
            //                                                                currentCase.SurName = pcelement.value;
            //                                                                break;
            //                                                            case "Address1":
            //                                                                currentCase.Address1 = pcelement.value;
            //                                                                break;
            //                                                            case "Address2":
            //                                                                currentCase.Address2 = pcelement.value;
            //                                                                break;
            //                                                            case "PostCode":
            //                                                                currentCase.PostCode = pcelement.value;
            //                                                                break;
            //                                                            case "City":
            //                                                                currentCase.City = pcelement.value;
            //                                                                break;
            //                                                            case "TelephoneNumber":
            //                                                                currentCase.TelephoneNumber = pcelement.value;
            //                                                                break;
            //                                                            case "Email":
            //                                                                currentCase.Email = pcelement.value;
            //                                                                break;
            //                                                            case "PolicyStartDate":
            //                                                                try
            //                                                                {
            //                                                                    currentCase.PolicyStartDate = Convert.ToDateTime(pcelement.value);
            //                                                                }
            //                                                                catch (Exception)
            //                                                                {
            //                                                                    break;
            //                                                                }
            //                                                                break;
            //                                                            case "PolicyEndDate":
            //                                                                try
            //                                                                {
            //                                                                    currentCase.PolicyEndDate = Convert.ToDateTime(pcelement.value);
            //                                                                }
            //                                                                catch (Exception)
            //                                                                {
            //                                                                    break;
            //                                                                }
            //                                                                break;
            //                                                            case "NumberOfChildren":
            //                                                                try
            //                                                                {
            //                                                                    currentCase.NumberOfChildren = Convert.ToInt32(pcelement.value);
            //                                                                }
            //                                                                catch (Exception)
            //                                                                {
            //                                                                    break;
            //                                                                }
            //                                                                break;
            //                                                            default:
            //                                                                if (pcelement.values != null && pcelement.values.Count() > 0)
            //                                                                {
            //                                                                    ccelement.values = new List<Value>();
            //                                                                    ccelement.values = pcelement.values;
            //                                                                }
            //                                                                ccelement.value = pcelement.value;
            //                                                                break;
            //                                                        }
            //                                                    }

            //                                                }
            //                                            }
            //                                        }

            //                                    }

            //                                }
            //                            }

            //                        }
            //                    }

            //                }

            //                if (ccdfvm != null && check == true)
            //                {
            //                    var fieldObject = JsonConvert.SerializeObject(ccdfvm);

            //                    currentCase.Extras = fieldObject;

            //                    try
            //                    {
            //                        var caseToSave = IMapper.Map<Case>(currentCase);

            //                        db.Case.Update(caseToSave);
            //                        db.SaveChanges();
            //                        tostate = Convert.ToInt32(configs.pass);
            //                        return tostate;
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        tostate = -1;
            //                    }

            //                }

            //                try
            //                {
            //                    tostate = Convert.ToInt32(configs.fail);
            //                }
            //                catch (Exception)
            //                {
            //                    tostate = -1;
            //                }


            //            }
            //        }
            //    }

            //    return tostate;
            return 0;
        }
        public int GetStateIdByCaseId(int caseId)
        {
            var stateId = db.Case.Where(x => x.Id == caseId).FirstOrDefault().StateId;
            return stateId;
        }

        public List<int> GetDateListsForCasePreview()
        {
            string loggeduser = commonService.getLoggedInUserId();
            var datas = db.Case.Where(x => x.UserId == loggeduser).Select(x => x.UpdatedAt.Year).Distinct().ToList();
            return datas;
        }

        public async Task<bool> SaveCaseStateHistory(CaseViewModel caseData, string loggedUser, string reason, int prevStateId, int newStateId)
        {
            //caseHistory
            var caseHistory = new CaseStateHistory()
            {
                CaseId = caseData.Id,
                PreviousStateId = prevStateId,
                CurrentStateId = newStateId,
                UpdatedBy = commonService.getLoggedInUserId(),
                Reason = reason,
                UpdatedAt = DateTime.Now
            };
            db.CaseStateHistory.Add(caseHistory);
            db.SaveChanges();
            return true;
        }

        public List<CaseStateHistoryViewModel> GetStateHistory(int caseId)
        {
            var caseDatas = new List<CaseStateHistoryViewModel>();
            var datas = db.CaseStateHistory.Where(x => x.CaseId == caseId).ToList();

            var cases = IMapper.Map<IEnumerable<CaseStateHistoryViewModel>>(datas);
            foreach (var item in cases)
            {
                item.User = commonService.GetUserById(item.UpdatedBy).GetAwaiter().GetResult();
                item.UpdatedByImg = commonService.GetUserMediaById(item.UpdatedBy).GetAwaiter().GetResult();
            }
            caseDatas.AddRange(cases);
            return caseDatas;
        }
        public List<SelectListItem> GetCaseStates(int caseId, int formid, string roleid)
        {
            var caseDatas = new List<int>();
            var datas = db.CaseStateHistory.Where(x => x.CaseId == caseId).ToList();
            foreach (var item in datas)
            {
                caseDatas.Add(Convert.ToInt32(item.PreviousStateId));
            }
            caseDatas.Add((Convert.ToInt32(datas.Last().CurrentStateId)));
            var temp2 = (from c in caseDatas
                         join sff in db.StateForForm.Where(x => x.CaseFormId == formid).Include(x => x.State).Include(y => y.StatePermissions)
                         on c equals sff.StateId
                         select sff)
                        .Select(x => new SelectListItem
                        {
                            Text = (x.StatePermissions.Any(z => z.RoleId == roleid) ? x.StatePermissions.Where(w => w.RoleId == roleid).Select(c => c.DisplayName).FirstOrDefault() : x.State.SystemName),
                            Value = x.StateId.ToString()
                        }).ToList();

            return temp2;
        }

        public bool ResetCaseAssignment(int caseId)
        {
            try
            {
                var caseAssigned = db.Case.Where(x => x.Id == caseId).FirstOrDefault();

                caseAssigned.AssignedAt = null;
                caseAssigned.AssignedTo = null;
                caseAssigned.DueDate = null;

                db.Update(caseAssigned);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
