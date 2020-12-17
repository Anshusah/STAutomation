using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Cicero.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Cicero.Service.Enums;
using System.Diagnostics;

namespace Cicero.Service.Services
{

    public interface IWorkflowService
    {
        JObject SaveWorkFlow(int caseformId, List<JsonWorkFlowPointsViewModel> wfvm, int tenantId, JObject allPoints, JArray workflowObj);
        string GetWorkFlowByCaseFormId(int caseFormId, int tenentId);
        int RunWorflowActionObject(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId, string baseApiUrl);
        JObject RunWorkFlowApiForElements(dynamic _new, WorkRun workData);
        bool RemoveQueueOrStateForForm(string type, int id);
        bool RemoveWorkFlowByCaseFormId(int caseFormId);
        JArray AssembleTarget(List<Target> targets, JArray values);       
    } 
    public class Comparer : IEqualityComparer<WorkflowPoint>
    {
        public bool Equals(WorkflowPoint x, WorkflowPoint y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(WorkflowPoint obj)
        {
            return (int)obj.TenantId;
        }
    }
    public class Flow
    {
        public string StateFrom { get; set; }
        public string StateTo { get; set; }
        public string StateToOld { get; set; }
        public List<string> Actions { get; set; }
        public List<string> ActionsAfter { get; set; }
    }
    public class ActionAfter
    {
        public string StateId { get; set; }
        public List<string> Action { get; set; }
    }
    public class WorkRun
    {
        public List<WorkParams> Parameter { get; set; }
        public List<WorkParams> Response { get; set; }
        public WorkState Work { get; set; }
        public List<Target> Targets { get; set; }
    }
    public class WorkState
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int CaseFormId { get; set; }
        public string ElementId { get; set; }
        public int EventType { get; set; }
    }

    public class WorkParams
    {
        public string Name { get; set; }
        public string ElementId { get; set; }
        public string Value { get; set; }
        public string IsTriggerEvent { get; set; }
    }

    public class Target
    {
        public string SelectValue { get; set; }
        public string TargetId { get; set; }
        public bool HasSubTarget { get; set; }
        public string Option { get; set; }
        public List<SubTarget> SubTarget { get; set; }
    }
    public class SubTarget
    {
        public string Value { get; set; }
        public string Option { get; set; }
    }
   

    public class WorkflowService : IWorkflowService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<QueueService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;
        private readonly Utils _utils;
        private readonly ICommonService _commonService;
        private readonly ISynchronizeService _synchronizeService;
        private readonly ITemplateService _templateService;
        private readonly IEmailSender _emailSender;
        private readonly IRazorToStringRender _renderRazorToString;
        private readonly ICaseService _caseService;
        private readonly IEmailGroupService _emailGroupService;

        public WorkflowService(ApplicationDbContext db, ILogger<QueueService> Log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService, IActivityLogService activityLogService, Utils utils, ISynchronizeService synchronizeService, ITemplateService templateService, IEmailSender emailSender, IRazorToStringRender renderRazorToString, ICaseService caseService, IEmailGroupService emailGroupService)
        {
            _db = db;
            _Log = Log;
            _IHttpContextAccessor = httpContextAccessor;
            _HostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _commonService = commonService;
            _activityLogService = activityLogService;
            _utils = utils;
            _synchronizeService = synchronizeService;
            _templateService = templateService;
            _emailSender = emailSender;
            _renderRazorToString = renderRazorToString;
            _caseService = caseService;
            _emailGroupService = emailGroupService;
        }


        public string GetWorkFlowByCaseFormId(int caseFormId, int tenentId)
        {
            string JsonData = _db.Workflow.Where(x => x.CaseFormId == caseFormId && x.TenantId == tenentId).FirstOrDefault().JsonData;

            return JsonData;

        }

        public JObject DefineWorkFlow(int caseformId, int tenantId)
        {
            bool success = false;

            List<Flow> finalWorkFlow = new List<Flow>();
            try
            {
                List<ActionAfter> actionAfters = new List<ActionAfter>();
                List<WorkflowObject> wfObj = _db.WorkflowObject.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId).ToList();
                List<WorkflowPoint> wfPoints = _db.WorkflowPoint.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId).ToList();
                List<WorkflowPoint> wfSelectedPoints = new List<WorkflowPoint>();
                List<WorkflowPoint> wfStartStates = wfPoints.Where(x => x.Type == "normal").ToList();
                List<WorkFlowState> wfStateList = new List<WorkFlowState>();
                List<string> allAfter = new List<string>();

                foreach (WorkflowPoint startPoint in wfStartStates)
                {
                    int finalState = 0;
                    WorkflowObject startObject = wfObj.Where(x => x.Id == startPoint.FWObjectId).SingleOrDefault();
                    WorkflowObject endObject = wfObj.Where(x => x.Id == startPoint.LWObjectId).SingleOrDefault();
                    List<string> before = new List<string>();
                    List<string> after = new List<string>();

                    do
                    {
                        if (endObject.Type == "STATE")
                        {
                            finalState = endObject.TypeId;
                        }
                        else
                        {
                            before.Add(endObject.Id);
                            if (wfPoints.Where(x => x.FWObjectId == endObject.Id && x.Type == "pass").FirstOrDefault() != null)
                            {
                                endObject = wfObj.Where(y => y.Id == wfPoints.Where(x => x.FWObjectId == endObject.Id && x.Type == "pass").FirstOrDefault().LWObjectId).FirstOrDefault();
                            }
                            else
                            {
                                endObject = null;
                            }

                            if (endObject == null)
                            {
                                if (wfObj.Where(x => x.Id == before.First()).FirstOrDefault().Type == "EMAIL")
                                {
                                    after.Add(before.First());
                                    break;
                                }
                                else
                                {
                                    JObject obj = new JObject();

                                    return new JObject(new JProperty("status", "error"), new JProperty("message", "Only Email can be attached at the end of the state. Please try again."));
                                }

                            }
                        }

                    } while (finalState == 0);
                    if (after.Count == 0)
                    {
                        WorkFlowState workFlowState = new WorkFlowState();
                        workFlowState.Id = String.Concat(tenantId.ToString(), startObject.TypeId.ToString(),finalState.ToString(),caseformId.ToString());
                        workFlowState.FromStateId = startObject.TypeId;
                        workFlowState.ToStateId = finalState;
                        workFlowState.TenantId = tenantId;
                        if (before.Count > 0)
                        {
                            workFlowState.BeforeChangeActionsId = before.First();
                        }
                        workFlowState.CaseFormId = caseformId;
                        wfStateList.Add(workFlowState);
                    }
                    else
                    {
                        allAfter.Add(startObject.TypeId + "," + after.First());
                    }
                }
                if (allAfter.Count > 0)
                {
                    foreach (string forAfter in allAfter)
                    {
                        int toStateId = Convert.ToInt16(forAfter.Split(",")[0]);
                        string a = forAfter.Split(",")[1].ToString();
                        foreach (var item in wfStateList.Where(x => x.ToStateId == toStateId).ToList())
                        {
                            item.AfterChangeActionsId = a;
                        }
                    }
                }
                _db.RemoveRange(_db.WorkFlowState.Where(x => x.CaseFormId == caseformId && x.TenantId == tenantId));

                _db.WorkFlowState.AddRange(wfStateList);
                _db.SaveChanges();
                success = true;

            }
            catch (Exception ex)
            {
                return new JObject(new JProperty("status", "error"), new JProperty("message", "Workflow couldnot be saved."));
            }
            if (success)
            {
                return new JObject(new JProperty("status", "success"), new JProperty("message", "Workflow is saved."));
            }
            else
            {
                return new JObject(new JProperty("status", "error"), new JProperty("message", "Workflow couldnot be saved."));
            }
        }

        public JObject SaveWorkFlow(int caseformId, List<JsonWorkFlowPointsViewModel> wfvm, int tenantId, JObject allPoints, JArray workflowObjs)
        {
            JObject success = new JObject();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.WorkflowPoint.RemoveRange(_db.WorkflowPoint.Where(e => e.CaseFormId == caseformId && e.TenantId == tenantId));
                    _db.SaveChanges();
                    #region Workflow
                    Workflow workflow = new Workflow();
                    workflow = _db.Workflow.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId).FirstOrDefault();
                    if (workflow == null) { workflow = new Workflow(); }
                    workflow.TenantId = tenantId;
                    workflow.JsonData = JsonConvert.SerializeObject(allPoints);
                    workflow.CaseFormId = caseformId;
                    if (workflow.Id != 0)
                    {
                        _db.Workflow.Update(workflow);
                    }
                    else
                    {
                        _db.Workflow.Add(workflow);
                    }
                    _db.SaveChanges();
                    #endregion
                    _db.WorkflowObject.RemoveRange(_db.WorkflowObject.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId));
                    _db.SaveChanges();
                    List<WorkflowObject> workflowObjects = new List<WorkflowObject>();

                    foreach (var item in workflowObjs)
                    {
                        WorkflowObject workflowObject = new WorkflowObject();
                        workflowObject.CaseFormId = caseformId;
                        workflowObject.TenantId = tenantId;
                        workflowObject.Type = Convert.ToString(item["type"]).ToUpper();
                        workflowObject.TypeId = Convert.ToInt32(item["id"]);
                        workflowObject.Id = caseformId.ToString() + tenantId.ToString() + workflowObject.TypeId.ToString() + workflowObject.Type;
                        workflowObjects.Add(workflowObject);

                    }
                    _db.WorkflowObject.AddRange(workflowObjects);
                    _db.SaveChanges();

                    List<WorkflowPoint> workflowPoints = new List<WorkflowPoint>();

                    foreach (var item in wfvm)
                    {
                        #region workflowObject

                        string firstObjId = caseformId.ToString() + tenantId.ToString() + item.First.AObject.TypeId.ToString() + item.First.AObject.Type; // firstObjId
                        string lastObjId = caseformId.ToString() + tenantId.ToString() + item.Last.AObject.TypeId.ToString() + item.Last.AObject.Type;  // firstObjId

                        #endregion
                        #region workflowPoint
                        WorkflowPoint workflowPoint = new WorkflowPoint();
                        workflowPoint.CaseFormId = caseformId;
                        workflowPoint.TenantId = tenantId;
                        workflowPoint.Id = caseformId.ToString() + tenantId.ToString() + firstObjId.ToString() + lastObjId.ToString();
                        workflowPoint.FWObjectId = firstObjId;
                        workflowPoint.LWObjectId = lastObjId;
                        if (workflowObjects.Where(x => x.Id == firstObjId).SingleOrDefault().Type != "STATE" && item.Type =="normal")
                        {
                            workflowPoint.Type = "pass";
                        }
                        else
                        {
                            workflowPoint.Type = item.Type;
                        }
                        workflowPoints.Add(workflowPoint);

                        #endregion

                    }
                    List<WorkflowPoint> wp1 = workflowPoints.Distinct(new Comparer()).ToList();
                    _db.WorkflowPoint.AddRange(wp1);
                    _db.SaveChanges();
                    success = DefineWorkFlow(caseformId, tenantId);
                    if (success["status"].ToString() == "success")
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }

                    return success;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JObject(new JProperty("status", "error"), new JProperty("message", "Workflow couldnot be saved."));
                }
            }
        }

        private int switchAndRun(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId, WorkflowObject wfObj, string baseApiUrl)
        {
            int success = 0;

            switch (wfObj.Type)
            {
                case "API":
                    {
                        try
                        {
                            success = _synchronizeService.SynchronizeCaseAsync(_new, caseFormId, fromStateId, toStateId, caseId, wfObj, baseApiUrl);
                        }
                        catch (Exception ex)
                        {
                            success = 0;
                        }

                        break;
                    }
                case "AUTOMATION":
                    {
                        try
                        {
                            Type _type = Type.GetType("Themes.Core.Components.CaseAutomation, Cicero");
                            dynamic objs = Activator.CreateInstance(_type);
                            MethodInfo MethodInfo = _type.GetMethod("RunRules");
                            objs.HttpContext = _new.HttpContext;
                            objs.Theme = _new.Theme;
                            //TODO: GetComponentId From TypeID object
                            var final = MethodInfo.Invoke(objs, new object[] { new object[] { caseId }, wfObj.TypeId.ToString() });
                            if (final[0].Case.Count > 0)
                            {
                                success = 1;
                            }

                        }
                        catch (Exception ex)
                        {
                            success = 0;
                        }
                        break;
                    }
                case "EMAIL":
                    {
                        try
                        {
                            WorkflowObject obj = _db.WorkflowPoint.Where(x => x.FWObjectId == wfObj.Id).SingleOrDefault().LastWorkflowObject;
                            int stateId = 0;
                            if (obj.Type == "STATE")
                            {
                                stateId = obj.TypeId;
                            }
                            MailMergeObject mailMergeObject = _templateService.GetMergeObjectById(wfObj.TypeId);
                            if (mailMergeObject.IsActive)
                            {
                                TemplateViewModel templateViewModel = _templateService.GetTemplateById(mailMergeObject.TemplateId);
                                string message = _templateService.CreateEmailTemplate(templateViewModel.Content, caseFormId, caseId, stateId);
                                try
                                {
                                    if (message != null)
                                    {
                                        var messageNew = new TemplateEmailViewModel { };
                                        messageNew.Content = message;
                                        string body = _renderRazorToString.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                                        List<string> imagesurl = new List<string>();
                                        var caseModel = _caseService.GetCaseById(caseId);
                                        ApplicationUser user = _db.Users.Where(x => x.Id == caseModel.UserId).FirstOrDefault();
                                        _emailSender.SendEmailAsync(user.Email, "Claim Notification", body);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            else { success = 1; }
                        }
                        catch (Exception ex)
                        {

                        }
                        success = 1;
                        break;
                    }
                case "ASSIGN":
                    {
                        try
                        {
                            Type _type = Type.GetType("Themes.Core.Components.AssignValue, Cicero");
                            dynamic objs = Activator.CreateInstance(_type);
                            MethodInfo MethodInfo = _type.GetMethod("RunAssignValue");
                            objs.HttpContext = _new.HttpContext;
                            objs.Theme = _new.Theme;
                            //TODO: GetComponentId From TypeID object
                            success = MethodInfo.Invoke(objs, new object[] { caseId, wfObj.TypeId.ToString() });
                        }
                        catch (Exception ex)
                        {

                        }
                        break;
                    }
                case "CASEASSIGNMENT":
                    {
                        try
                        {
                            Type _type = Type.GetType("Themes.Core.Components.CaseAssignment, Cicero");
                            dynamic objs = Activator.CreateInstance(_type);
                            MethodInfo MethodInfo = _type.GetMethod("RunRules");
                            objs.HttpContext = _new.HttpContext;
                            objs.Theme = _new.Theme;
                            //TODO: GetComponentId From TypeID object
                            var final = MethodInfo.Invoke(objs, new object[] { new object[] { caseId }, wfObj.TypeId.ToString()});
                            //if (final[0].Case.Count > 0)
                            //{
                                success = 1;
                            //}

                        }
                        catch (Exception ex)
                        {
                            success = 1;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            return success;
        }

        public int RunWorflowActionObject(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId, string baseApiUrl)
        {
            int finalState = 0;
            int switchSuccess = 0;
            List<WorkflowPoint> workflowPoints = _db.WorkflowPoint.Where(x => x.CaseFormId == caseFormId).ToList();
            List<WorkflowObject> workflowObjects = _db.WorkflowObject.Where(x => x.CaseFormId == caseFormId).ToList();
            List<WorkFlowState> workflowStates = _db.WorkFlowState.Where(x => x.CaseFormId == caseFormId).ToList();
            WorkFlowState workFlowState = _db.WorkFlowState.Where(x => x.FromStateId == fromStateId && x.ToStateId == toStateId && x.CaseFormId == caseFormId).FirstOrDefault();
            WorkflowObject startObject = new WorkflowObject();
            WorkflowPoint startPoint = new WorkflowPoint();
            WorkflowObject endObject = new WorkflowObject();
            WorkflowObject afterObject = new WorkflowObject();
            if (workFlowState != null)
            {
                startObject = workflowObjects.Where(x => x.TypeId == fromStateId && x.Type == "STATE").SingleOrDefault();
                if (workFlowState.BeforeChangeActionsId != null)
                {
                    endObject = workflowObjects.Where(x => x.Id == workFlowState.BeforeChangeActionsId).SingleOrDefault();
                    startPoint = workflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == workFlowState.BeforeChangeActionsId).SingleOrDefault();
                }
                else
                {
                    endObject = workflowObjects.Where(x => x.TypeId == workFlowState.ToStateId && x.Type == "STATE").SingleOrDefault();
                    startPoint = workflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == endObject.Id).SingleOrDefault();
                }

                do
                {
                    if (endObject.Type == "STATE")
                    {
                        finalState = endObject.TypeId;
                        string actionAfter = null;
                        if (workflowStates.Where(x => x.ToStateId == finalState).ToList().Count > 0)
                        {
                            actionAfter = workflowStates.Where(x => x.ToStateId == finalState).ToList().First().AfterChangeActionsId;
                        }
                        else //if fail state
                        {
                            if(workflowPoints.Where(x=>x.LWObjectId == endObject.Id && x.Type == "fail").ToList().Count > 0)
                            {
                                List<WorkflowPoint> tempPoint = workflowPoints.Where(x => x.FWObjectId == endObject.Id).ToList();
                                foreach(WorkflowPoint wfPoint in tempPoint)
                                {
                                    WorkflowObject tempObj = workflowObjects.Where(x => x.Id == wfPoint.LWObjectId).SingleOrDefault();
                                    if(tempObj!=null)
                                    {
                                        if(tempObj.Type == "EMAIL")
                                        {
                                            actionAfter = tempObj.Id;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (actionAfter != null)
                        {
                            afterObject = workflowObjects.Where(x => x.Id == actionAfter).SingleOrDefault();

                            //for Mail Object only//
                            MailMergeObject mailMergeObject = _templateService.GetMergeObjectById(afterObject.TypeId);
                            if (mailMergeObject.IsActive)
                            {
                                TemplateViewModel templateViewModel = _templateService.GetTemplateById(mailMergeObject.TemplateId);
                                string message = _templateService.CreateEmailTemplate(templateViewModel.Content, caseFormId, caseId, finalState);
                                try
                                {
                                    if (message != null)
                                    {
                                        var messageNew = new TemplateEmailViewModel { };
                                        messageNew.Content = message;
                                        string body = _renderRazorToString.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                                        List<string> imagesurl = new List<string>();
                                        var caseModel = _db.Case.Where(x => x.Id == caseId).FirstOrDefault();
                                        ApplicationUser user = _db.Users.Where(x => x.Id == caseModel.UserId).FirstOrDefault();

                                        List<string> emails = new List<string>();

                                        switch (templateViewModel.RecipientType){
                                            case (int)RecipientType.CaseField:
                                                if (!string.IsNullOrWhiteSpace(templateViewModel.RecipientField))
                                                {
                                                    var table = templateViewModel.RecipientDatabaseTable;

                                                    var data = _db.CustomEntities.FromSql($"SELECT * FROM {table} AS [t3] WHERE CaseId ={caseId}", table, caseId).FirstOrDefault();

                                                    if (data != null)
                                                    {
                                                        dynamic jsonObj = JsonConvert.DeserializeObject(data.Extras);
                                                        foreach (var obj in jsonObj)
                                                        {
                                                            if (obj.Name == templateViewModel.RecipientField)
                                                            {
                                                                emails.Add(obj.Value);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                                break;
                                            case (int)RecipientType.AssignedUser:
                                                var assigneduser = _db.Users.Where(x => x.Id == caseModel.AssignedTo).FirstOrDefault();
                                                if (assigneduser != null)
                                                {
                                                    emails.Add(assigneduser.Email);
                                                }
                                                break;
                                            case (int)RecipientType.EmailGroup:

                                                int emailGroupId;
                                                Int32.TryParse(templateViewModel.EmailGroupId, out emailGroupId);

                                                if (emailGroupId > 0)
                                                {
                                                    var emailGroup = _emailGroupService.GetEmailsByEmailGroupId(emailGroupId);

                                                    if (emailGroup.Count > 0)
                                                    {
                                                        emails.AddRange(emailGroup);
                                                    }
                                                }
                                                break;
                                            case (int)RecipientType.UsersInRole:
                                                if (!string.IsNullOrWhiteSpace(templateViewModel.RoleId))
                                                {
                                                    var userList = _commonService.GetUsersByRole(templateViewModel.RoleId);

                                                    foreach (var userItem in userList)
                                                    {
                                                        emails.Add(userItem.Email);
                                                    }
                                                }
                                                break;
                                            default:
                                                emails.Add(user.Email);
                                                break;
                                        }


                                        //remove
                                        //if (templateViewModel.RecipientType == Convert.ToInt16(RecipientType.CaseField) && !string.IsNullOrWhiteSpace(templateViewModel.RecipientField))
                                        //{
                                        //    var table = templateViewModel.RecipientDatabaseTable;

                                        //    var data = _db.CustomEntities.FromSql($"SELECT * FROM {table} AS [t3] WHERE CaseId ={caseId}", table, caseId).FirstOrDefault();

                                        //    if (data != null)
                                        //    {
                                        //        dynamic jsonObj = JsonConvert.DeserializeObject(data.Extras);
                                        //        foreach (var obj in jsonObj)
                                        //        {
                                        //            if (obj.Name == templateViewModel.RecipientField)
                                        //            {
                                        //                email = obj.Value;
                                        //                break;
                                        //            }

                                        //        }
                                        //    }
                                        //}
                                        //else if (templateViewModel.RecipientType == Convert.ToInt16(RecipientType.AssignedUser))
                                        //{
                                        //    var assigneduser = _db.Users.Where(x => x.Id == caseModel.AssignedTo).FirstOrDefault();
                                        //    if (assigneduser != null)
                                        //    {
                                        //        email = assigneduser.Email;
                                        //    }
                                        //}

                                        foreach (var emailItem in emails)
                                        {
                                            _emailSender.SendEmailAsync(emailItem, templateViewModel.Subject, body);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            //end Mail Object//
                        }

                    }
                    else
                    {
                        switchSuccess = switchAndRun(_new, caseFormId, fromStateId, toStateId, caseId, endObject,baseApiUrl);
                        startObject = endObject;
                        WorkflowPoint nextPoint = new WorkflowPoint();
                        switch (switchSuccess)
                        {
                            case 0:
                                //fail;
                                nextPoint = workflowPoints.Where(x => x.Type == "fail" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                endObject = workflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
                                break;
                            case 1:
                                //pass
                                nextPoint = workflowPoints.Where(x => x.Type == "pass" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                endObject = workflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
                                break;
                        }
                    }

                } while (finalState == 0);
            }
            else
            {
                finalState = toStateId;
            }

            return finalState;


        }

        public bool RemoveQueueOrStateForForm(string type, int id)
        {
            try
            {
                if (type == "queue")
                {
                    var data = _db.QueueForForm.Where(x => x.QueueId == id).FirstOrDefault();
                    _db.QueueForForm.Remove(data);
                }

                if (type == "statee")
                {
                    var data = _db.StateForForm.Where(x => x.StateId == id).FirstOrDefault();
                    _db.StateForForm.Remove(data);
                }

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveWorkFlowByCaseFormId(int caseFormId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    List<WorkFlowState> workFlowStates = _db.WorkFlowState.Where(x => x.CaseFormId == caseFormId).ToList();
                    if (workFlowStates.Count > 0)
                    { _db.WorkFlowState.RemoveRange(workFlowStates); }
                    List<WorkflowPoint> workflowPoints = _db.WorkflowPoint.Where(x => x.CaseFormId == caseFormId).ToList();
                    if (workflowPoints.Count > 0)
                    {
                        _db.WorkflowPoint.RemoveRange(workflowPoints);
                    }
                    List<WorkflowObject> workflowObjects = _db.WorkflowObject.Where(x => x.CaseFormId == caseFormId).ToList();
                    if (workflowObjects.Count > 0)
                    {
                        _db.WorkflowObject.RemoveRange(workflowObjects);
                    }
                    Workflow workflow = _db.Workflow.Where(x => x.CaseFormId == caseFormId).FirstOrDefault();
                    if (workflow != null)
                    {
                        _db.Workflow.Remove(workflow);
                    }
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public JObject RunWorkFlowApiForElements(dynamic _new, WorkRun workData)
        {
            int finalState = 0;
            JObject finalRes = new JObject();
            object switchSuccess = new object();
            List<WorkflowPoint> workflowPoints = _db.WorkflowPoint.Where(x => x.CaseFormId == workData.Work.CaseFormId).ToList();
            List<WorkflowObject> workflowObjects = _db.WorkflowObject.Where(x => x.CaseFormId == workData.Work.CaseFormId).ToList();
            List<WorkFlowState> workflowStates = _db.WorkFlowState.Where(x => x.CaseFormId == workData.Work.CaseFormId).ToList();
            WorkFlowState workFlowState = _db.WorkFlowState.Where(x => x.FromStateId == workData.Work.Start && x.ToStateId == workData.Work.End && x.CaseFormId == workData.Work.CaseFormId).FirstOrDefault();
            WorkflowObject startObject = new WorkflowObject();
            WorkflowPoint startPoint = new WorkflowPoint();
            WorkflowObject endObject = new WorkflowObject();
            WorkflowObject afterObject = new WorkflowObject();
            if (workFlowState != null)
            {
                startObject = workflowObjects.Where(x => x.TypeId == workData.Work.Start && x.Type == "STATE").SingleOrDefault();
                if (workFlowState.BeforeChangeActionsId != null)
                {
                    endObject = workflowObjects.Where(x => x.Id == workFlowState.BeforeChangeActionsId).SingleOrDefault();
                    startPoint = workflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == workFlowState.BeforeChangeActionsId).SingleOrDefault();
                }
                else
                {
                    endObject = workflowObjects.Where(x => x.TypeId == workFlowState.ToStateId && x.Type == "STATE").SingleOrDefault();
                    startPoint = workflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == endObject.Id).SingleOrDefault();
                }

                do
                {
                    if (endObject.Type == "STATE")
                    {
                        finalState = endObject.TypeId;
                        string actionAfter = null;
                        if (workflowStates.Where(x => x.ToStateId == finalState).ToList().Count > 0)
                        {
                            actionAfter = workflowStates.Where(x => x.ToStateId == finalState).ToList().First().AfterChangeActionsId;
                        }

                        if (actionAfter != null)
                        {

                        }

                    }
                    else
                    {
                        switchSuccess = SwitchAndRunForElements(_new, workData, endObject);
                        finalRes= (JObject)JsonConvert.DeserializeObject(switchSuccess.ToJson());
                        startObject = endObject;
                        WorkflowPoint nextPoint = new WorkflowPoint();
                        switch (Convert.ToInt16(finalRes["success"]))
                        {
                            case 0:
                                //fail;
                                nextPoint = workflowPoints.Where(x => x.Type == "fail" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                endObject = workflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
                                break;
                            case 1:
                                //pass
                                nextPoint = workflowPoints.Where(x => x.Type == "pass" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                endObject = workflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
                                break;
                        }
                    }

                } while (finalState == 0);
            }
            else
            {
                //  finalState = toStateId;
            }

            return finalRes;

        }

        private object SwitchAndRunForElements(dynamic _new, WorkRun work, WorkflowObject wfObj)
        {

            object success = new object();
            //switch (wfObj.Type)
            //{
            //    case "API":
            //        {
            //            try
            //            {
            //                success = _synchronizeService.CallAPI(_new, work, wfObj);

            //            }
            //            catch (Exception ex)
            //            {
            //                success = new { success = 0, response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" } };
            //            }


            //            break;
            //        }

            //    default:
            //        {
            //            break;
            //        }
            //}


            return success;
        }

        public JArray AssembleTarget(List<Target> targets, JArray values)
        {
            var grouped = targets
                          .GroupBy(s => s.TargetId)
                          .Select(g => new { field = g.Key });
            JArray Targets = new JArray();
            foreach (var item in grouped)
            {
                List<Target> tar = targets.Where(x => x.TargetId == item.field).ToList();
                List<Target> tar1 = new List<Target>();
                Target targetElm = new Target();
                
                for (int i=0;i<values.Count;i++)
                {
                    var temp = tar.Where(x => x.SelectValue == values[i]["value"].ToString()).FirstOrDefault();
                    if(temp!=null)
                    {
                        tar1.Add(temp);
                    }
                }
                foreach(var item1 in tar1)
                {
                 if(targetElm.TargetId==null)
                    {
                        targetElm = item1;

                    }
                    else
                    {
                        targetElm.Option = GetTargetPriority(targetElm.Option, item1.Option);
                        if(item1.HasSubTarget)
                        {
                            if(item1.SubTarget!=null&& targetElm.SubTarget.Count>0)
                            {
                                int j = 0;
                                if(targetElm.SubTarget!=null && targetElm.SubTarget.Count>0)
                                {
                                    foreach(var subTar in item1.SubTarget)
                                    {
                                        SubTarget subTarget = targetElm.SubTarget.Where(x => x.Value == subTar.Value).SingleOrDefault();
                                        if(subTarget!=null)
                                        {
                                            targetElm.SubTarget.Where(x => x.Value == subTar.Value).SingleOrDefault().Option = GetTargetPriority(subTarget.Option, subTar.Option);
                                        }
                                        else
                                        {
                                            targetElm.SubTarget.Add(subTar);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                JObject target = new JObject();
                if(targetElm.TargetId !=null)
                {
                    target.Add("elmName", targetElm.TargetId.ToString());

                    if (targetElm.HasSubTarget)
                    {
                        target.Add("type", "1");
                        if (targetElm.SubTarget != null && targetElm.SubTarget.Count > 0)
                        {
                            JArray subTarget = new JArray();
                            foreach (var sub in targetElm.SubTarget)
                            {
                                JObject subTarg = new JObject();
                                subTarg.Add("value", sub.Value);
                                subTarg.Add("setAs", sub.Option);
                                subTarget.Add(subTarg);
                            }
                            target.Add("subTarget", subTarget);
                        }
                        else
                        {
                            target["type"] = "0";
                            target.Add("setAs", targetElm.Option);
                        }
                    }
                    else
                    {
                        target.Add("type", "0");
                        target.Add("setAs", targetElm.Option);
                    }
                    Targets.Add(target);

                }
               
            }
            return Targets;
        }

        private string GetTargetPriority(string prevOption, string nextOption)
        {
            if(getPriorityValue(nextOption)> getPriorityValue(prevOption))
            {
                return nextOption;
            }
            else
            {
                return prevOption;
            }
        }

        private int getPriorityValue(string option)
        {
            switch(option)
            {
                case "enable":
                    return 4;
                case "disable":
                    return 2;
                case "hide":
                    return 1;
                case "show":
                    return 3;
                default:
                    return 1;
            }
        }
    }

}
