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

namespace Cicero.Service.Services
{

    public interface IElementWorkflowService
    {
        JObject SaveElementWorkFlow(int caseformId, List<JsonWorkFlowPointsViewModel> wfvm, int tenantId, JObject allPoints, JArray workflowObjj, string elementId, int eventType);
        string GetElementWorkFlowByCaseFormId(int caseFormId, int tenentId, string elementId, int eventType);
        JObject RunWorkFlowApiForElements(dynamic _new, WorkRun workData, string baseApiUrl);
        bool RemoveElementState(string stateIds, int caseFormId, string elementId, int eventType);
        bool RemoveWorkFlowByElementId(string elementId, int caseformId);
        JArray AssembleTarget(List<Target> targets, JArray values);
        ElementStateViewModel CreateElementState(ElementStateViewModel elementStateViewModel);
       
    }
    public class ElementWorkflowComparer : IEqualityComparer<ElementWorkflowPoint>
    {
        public bool Equals(ElementWorkflowPoint x, ElementWorkflowPoint y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ElementWorkflowPoint obj)
        {
            return (int)obj.TenantId;
        }
    }
    

    public class ElementWorkflowService : IElementWorkflowService
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

        public ElementWorkflowService(ApplicationDbContext db, ILogger<QueueService> Log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService, IActivityLogService activityLogService, Utils utils, ISynchronizeService synchronizeService, ITemplateService templateService, IEmailSender emailSender, IRazorToStringRender renderRazorToString, ICaseService caseService)
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
        }


        public string GetElementWorkFlowByCaseFormId(int caseFormId, int tenentId, string elementId, int eventType)
        {
            var elementWorkflow = _db.ElementWorkflow.Where(x => x.CaseFormId == caseFormId && x.TenantId == tenentId && x.ElementId == elementId && x.EventType == eventType).FirstOrDefault();
            string JsonData = "";
            if (elementWorkflow!=null)
            {
                JsonData = elementWorkflow.JsonData;
            }
            return JsonData;

        }

        public JObject DefineWorkFlow(int caseformId, int tenantId, string elementId, int eventType)
        {
            bool success = false;

            List<Flow> finalWorkFlow = new List<Flow>();
            try
            {
                List<ActionAfter> actionAfters = new List<ActionAfter>();
                List<ElementWorkflowObject> wfObj = _db.ElementWorkflowObject.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId && x.ElementId == elementId && x.EventType == eventType).ToList();
                List<ElementWorkflowPoint> wfPoints = _db.ElementWorkflowPoint.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId && x.ElementId == elementId && x.EventType == eventType).ToList();
                List<ElementWorkflowPoint> wfSelectedPoints = new List<ElementWorkflowPoint>();
                List<ElementWorkflowPoint> wfStartStates = wfPoints.Where(x => x.Type == "normal").ToList();
                List<ElementWorkflowState> wfStateList = new List<ElementWorkflowState>();
                List<string> allAfter = new List<string>();

                foreach (ElementWorkflowPoint startPoint in wfStartStates)
                {
                    int finalState = 0;
                    ElementWorkflowObject startObject = wfObj.Where(x => x.Id == startPoint.FWObjectId).SingleOrDefault();
                    ElementWorkflowObject endObject = wfObj.Where(x => x.Id == startPoint.LWObjectId).SingleOrDefault();
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
                        ElementWorkflowState elementWorkflowState = new ElementWorkflowState();
                        elementWorkflowState.Id = tenantId.ToString() + startObject.TypeId.ToString() + finalState.ToString() + caseformId.ToString() + eventType.ToString() + elementId;
                        elementWorkflowState.FromStateId = startObject.TypeId;
                        elementWorkflowState.ToStateId = finalState;
                        elementWorkflowState.TenantId = tenantId;
                        elementWorkflowState.ElementId = elementId;
                        elementWorkflowState.EventType = eventType;
                        if (before.Count > 0)
                        {
                            elementWorkflowState.BeforeChangeActionsId = before.First();
                        }
                        elementWorkflowState.CaseFormId = caseformId;
                        wfStateList.Add(elementWorkflowState);
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
                _db.RemoveRange(_db.ElementWorkflowState.Where(x => x.CaseFormId == caseformId && x.TenantId == tenantId && x.ElementId == elementId && x.EventType == eventType));

                _db.ElementWorkflowState.AddRange(wfStateList);
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

        public JObject SaveElementWorkFlow(int caseformId, List<JsonWorkFlowPointsViewModel> wfvm, int tenantId, JObject allPoints, JArray workflowObjs, string elementId, int eventType)
        {
            JObject success = new JObject();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.ElementWorkflowPoint.RemoveRange(_db.ElementWorkflowPoint.Where(e => e.CaseFormId == caseformId && e.TenantId == tenantId && e.EventType == eventType && e.ElementId == elementId));
                    _db.SaveChanges();
                    #region Workflow
                    ElementWorkflow elementWorkflow = new ElementWorkflow();
                    elementWorkflow = _db.ElementWorkflow.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId && x.EventType == eventType && x.ElementId == elementId).FirstOrDefault();
                    if (elementWorkflow == null) { elementWorkflow = new ElementWorkflow(); }
                    elementWorkflow.TenantId = tenantId;
                    elementWorkflow.JsonData = JsonConvert.SerializeObject(allPoints);
                    elementWorkflow.CaseFormId = caseformId;
                    elementWorkflow.ElementId = elementId;
                    elementWorkflow.EventType = eventType;
                    if (elementWorkflow.Id != 0)
                    {
                        _db.ElementWorkflow.Update(elementWorkflow);
                    }
                    else
                    {
                        _db.ElementWorkflow.Add(elementWorkflow);
                    }
                    _db.SaveChanges();
                    #endregion
                    _db.ElementWorkflowObject.RemoveRange(_db.ElementWorkflowObject.Where(x => x.TenantId == tenantId && x.CaseFormId == caseformId && x.EventType == eventType && x.ElementId == elementId));
                    _db.SaveChanges();
                    List<ElementWorkflowObject> workflowObjects = new List<ElementWorkflowObject>();

                    foreach (var item in workflowObjs)
                    {
                        ElementWorkflowObject elementWorkflowObject = new ElementWorkflowObject();
                        elementWorkflowObject.CaseFormId = caseformId;
                        elementWorkflowObject.TenantId = tenantId;
                        elementWorkflowObject.Type = (Convert.ToString(item["type"]).ToUpper().Contains("STATE")?"STATE": Convert.ToString(item["type"]).ToUpper());
                        elementWorkflowObject.TypeId = Convert.ToInt32(item["id"]);
                        elementWorkflowObject.ElementId = elementId;
                        elementWorkflowObject.EventType = eventType;
                        elementWorkflowObject.Id = eventType.ToString() + elementId + caseformId.ToString() + tenantId.ToString() + elementWorkflowObject.TypeId.ToString() + elementWorkflowObject.Type;
                        workflowObjects.Add(elementWorkflowObject);

                    }
                    _db.ElementWorkflowObject.AddRange(workflowObjects);
                    _db.SaveChanges();

                    List<ElementWorkflowPoint> elementWorkflowPoints = new List<ElementWorkflowPoint>();

                    foreach (var item in wfvm)
                    {
                        #region workflowObject

                        string firstObjId = eventType.ToString() + elementId + caseformId.ToString() + tenantId.ToString() + item.First.AObject.TypeId.ToString() + (Convert.ToString(item.First.AObject.Type).ToUpper().Contains("STATE") ? "STATE" : Convert.ToString(item.First.AObject.Type).ToUpper()); // firstObjId
                        string lastObjId = eventType.ToString() + elementId + caseformId.ToString() + tenantId.ToString() + item.Last.AObject.TypeId.ToString() + (Convert.ToString(item.Last.AObject.Type).ToUpper().Contains("STATE") ? "STATE" : Convert.ToString(item.Last.AObject.Type).ToUpper());  // firstObjId

                        #endregion
                        #region ElementWorkflowPoint
                        ElementWorkflowPoint elementWorkflowPoint = new ElementWorkflowPoint();
                        elementWorkflowPoint.CaseFormId = caseformId;
                        elementWorkflowPoint.TenantId = tenantId;
                        elementWorkflowPoint.Id = eventType.ToString() + elementId + caseformId.ToString() + tenantId.ToString() + firstObjId.ToString() + lastObjId.ToString();
                        elementWorkflowPoint.FWObjectId = firstObjId;
                        elementWorkflowPoint.LWObjectId = lastObjId;
                        elementWorkflowPoint.Type = item.Type;
                        elementWorkflowPoint.ElementId = elementId;
                        elementWorkflowPoint.EventType = eventType;
                        elementWorkflowPoints.Add(elementWorkflowPoint);

                        #endregion

                    }
                    List<ElementWorkflowPoint> wp1 = elementWorkflowPoints.Distinct(new ElementWorkflowComparer()).ToList();
                    _db.ElementWorkflowPoint.AddRange(wp1);
                    _db.SaveChanges();
                    success = DefineWorkFlow(caseformId, tenantId, elementId, eventType);
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

        public bool RemoveWorkFlowByElementId(string elementId, int caseformId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    List<ElementWorkflowState> workFlowStates = _db.ElementWorkflowState.Where(x => x.CaseFormId == caseformId && x.ElementId == elementId).ToList();
                    if (workFlowStates.Count > 0)
                    { _db.ElementWorkflowState.RemoveRange(workFlowStates); }
                    List<ElementWorkflowPoint> workflowPoints = _db.ElementWorkflowPoint.Where(x => x.CaseFormId == caseformId && x.ElementId == elementId).ToList();
                    if (workflowPoints.Count > 0)
                    {
                        _db.ElementWorkflowPoint.RemoveRange(workflowPoints);
                    }
                    List<ElementWorkflowObject> workflowObjects = _db.ElementWorkflowObject.Where(x => x.CaseFormId == caseformId && x.ElementId == elementId).ToList();
                    if (workflowObjects.Count > 0)
                    {
                        _db.ElementWorkflowObject.RemoveRange(workflowObjects);
                    }
                    ElementWorkflow workflow = _db.ElementWorkflow.Where(x => x.CaseFormId == caseformId && x.ElementId == elementId).FirstOrDefault();
                    if (workflow != null)
                    {
                        _db.ElementWorkflow.Remove(workflow);
                    }
                    ElementComponent elementComponent = _db.ElementComponent.Where(x => x.FormId == caseformId && x.ElementId == elementId).SingleOrDefault();
                    if (elementComponent != null)
                    {
                        _db.ElementComponent.Remove(elementComponent);
                    }
                    List<ElementState> elementStates = _db.ElementState.Where(x => x.FormId == caseformId && x.ElementId == elementId).ToList();
                    if(elementStates!=null)
                    {
                        _db.ElementState.RemoveRange(elementStates);
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

        public JObject RunWorkFlowApiForElements(dynamic _new, WorkRun workData, string baseApiUrl)
        {
            try
            {
                int finalState = 0;
                JObject finalRes = new JObject();
                object switchSuccess = new object();
                List<ElementWorkflowPoint> elementWorkflowPoints = _db.ElementWorkflowPoint.Where(x => x.CaseFormId == workData.Work.CaseFormId && x.EventType == workData.Work.EventType && x.ElementId == workData.Work.ElementId).ToList();
                List<ElementWorkflowObject> elementWorkflowObjects = _db.ElementWorkflowObject.Where(x => x.CaseFormId == workData.Work.CaseFormId && x.EventType == workData.Work.EventType && x.ElementId == workData.Work.ElementId).ToList();
                List<ElementWorkflowState> elementWorkflowStates = _db.ElementWorkflowState.Where(x => x.CaseFormId == workData.Work.CaseFormId && x.EventType == workData.Work.EventType && x.ElementId == workData.Work.ElementId).ToList();
                ElementWorkflowState elementWorkflowState = _db.ElementWorkflowState.Where(x => x.CaseFormId == workData.Work.CaseFormId && x.ElementId == workData.Work.ElementId && x.EventType == workData.Work.EventType).SingleOrDefault();

                ElementWorkflowObject startObject = new ElementWorkflowObject();
                ElementWorkflowPoint startPoint = new ElementWorkflowPoint();
                ElementWorkflowObject endObject = new ElementWorkflowObject();
                ElementWorkflowObject afterObject = new ElementWorkflowObject();
                if (elementWorkflowState != null)
                {
                    startObject = elementWorkflowObjects.Where(x => x.TypeId == elementWorkflowState.FromStateId && x.Type == "STATE").SingleOrDefault();
                    if (elementWorkflowState.BeforeChangeActionsId != null)
                    {
                        endObject = elementWorkflowObjects.Where(x => x.Id == elementWorkflowState.BeforeChangeActionsId).SingleOrDefault();
                        startPoint = elementWorkflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == elementWorkflowState.BeforeChangeActionsId).SingleOrDefault();
                    }
                    else
                    {
                        endObject = elementWorkflowObjects.Where(x => x.TypeId == elementWorkflowState.ToStateId && x.Type == "STATE").SingleOrDefault();
                        startPoint = elementWorkflowPoints.Where(x => x.FWObjectId == startObject.Id && x.LWObjectId == endObject.Id).SingleOrDefault();
                    }

                    do
                    {
                        if (endObject.Type == "STATE")
                        {
                            finalState = endObject.TypeId;
                            string actionAfter = null;
                            if (elementWorkflowStates.Where(x => x.ToStateId == finalState).ToList().Count > 0)
                            {
                                actionAfter = elementWorkflowStates.Where(x => x.ToStateId == finalState).ToList().First().AfterChangeActionsId;
                            }

                            if (actionAfter != null)
                            {

                            }

                        }
                        else
                        {
                            switchSuccess = SwitchAndRunForElements(_new, workData, endObject, baseApiUrl);
                            finalRes = (JObject)JsonConvert.DeserializeObject(switchSuccess.ToJson());
                            startObject = endObject;
                            ElementWorkflowPoint nextPoint = new ElementWorkflowPoint();
                            switch (Convert.ToInt16(finalRes["success"]))
                            {
                                case 0:
                                    //fail;
                                    nextPoint = elementWorkflowPoints.Where(x => x.Type == "fail" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                    endObject = elementWorkflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
                                    break;
                                case 1:
                                    //pass
                                    nextPoint = elementWorkflowPoints.Where(x => x.Type == "pass" && x.FWObjectId == startObject.Id).SingleOrDefault();
                                    endObject = elementWorkflowObjects.Where(x => x.Id == nextPoint.LWObjectId).SingleOrDefault();
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
            catch (Exception ex)
            {
                JObject finalRes = new JObject();
                return finalRes;
            }
           
        }

        private object SwitchAndRunForElements(dynamic _new, WorkRun work, ElementWorkflowObject wfObj, string baseApiUrl)
        {

            object success = new object();
            switch (wfObj.Type)
            {
                case "API":
                    {
                        try
                        {
                            success = _synchronizeService.CallAPI(_new, work, wfObj, baseApiUrl);

                        }
                        catch (Exception ex)
                        {
                            success = new { success = 0 };
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

                for (int i = 0; i < values.Count; i++)
                {
                    var temp = tar.Where(x => x.SelectValue == values[i]["value"].ToString()).FirstOrDefault();
                    if (temp != null)
                    {
                        tar1.Add(temp);
                    }
                }
                foreach (var item1 in tar1)
                {
                    if (targetElm.TargetId == null)
                    {
                        targetElm = item1;

                    }
                    else
                    {
                        targetElm.Option = GetTargetPriority(targetElm.Option, item1.Option);
                        if (item1.HasSubTarget)
                        {
                            if (item1.SubTarget != null && targetElm.SubTarget.Count > 0)
                            {
                                int j = 0;
                                if (targetElm.SubTarget != null && targetElm.SubTarget.Count > 0)
                                {
                                    foreach (var subTar in item1.SubTarget)
                                    {
                                        SubTarget subTarget = targetElm.SubTarget.Where(x => x.Value == subTar.Value).SingleOrDefault();
                                        if (subTarget != null)
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
                if (targetElm.TargetId != null)
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
            if (getPriorityValue(nextOption) > getPriorityValue(prevOption))
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
            switch (option)
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

        public ElementStateViewModel CreateElementState(ElementStateViewModel elementStateViewModel)
        {
            ElementState elementState = _mapper.Map<ElementState>(elementStateViewModel);
            _db.ElementState.Add(elementState);
            _db.SaveChanges();
            elementStateViewModel.Id = elementState.Id;
            return elementStateViewModel;
        }

        public bool RemoveElementState(string stateIds,int caseFormId, string elementId, int eventType)
        {
            try
            {
                if (stateIds != "")
                {
                    var ids = stateIds.Split(",");
                    List<ElementState> previousState = _db.ElementState.Where(x => x.FormId == caseFormId && x.ElementId == elementId && x.ForEventType == eventType).ToList();
                    List<ElementState> stateForForms = new List<ElementState>();
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            ElementState stateForForm = previousState.Where(x => x.Id == Convert.ToInt32(id)).SingleOrDefault();
                            if (stateForForm != null)
                            {
                                stateForForms.Add(stateForForm);
                            }

                        }
                    }
                    if (stateForForms.Count > 0)
                    {
                        _db.ElementState.RemoveRange(stateForForms);
                        _db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
          
        }
    }

}
