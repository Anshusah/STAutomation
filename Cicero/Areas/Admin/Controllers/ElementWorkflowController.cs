using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Cicero.Service.Library.Toastr;
using Cicero.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ElementWorkflowController : BaseController
    {
        private readonly ILogger<QueueController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly IQueueService queueService;
        private readonly IActionsService actionsService;
        private readonly ICommonService commonService;
        private readonly IWorkflowService workflowService;
        private readonly IToastNotification _toastNotification;
        private readonly IValidationService validationService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IFormService formService;
        private readonly IElementWorkflowService elementWorkflowService;
        private readonly string baseApiUrl;
        private readonly IConfiguration _config;

        public ElementWorkflowController(ILogger<QueueController> _Log, IStatus _status, Utils _utils, IUserService _userService,
            IQueueService _queueService, IActionsService _actionsService, ICommonService _commonService,
            IWorkflowService _workflowService, IToastNotification toastNotification,
            IValidationService _validationService, IFormBuilderService _formBuilderService,
            IFormService _formService,
            IElementWorkflowService _elementWorkflowService,IConfiguration config) : base(_userService)
        {
            Log = _Log;
            status = _status;
            utils = _utils;
            userService = _userService;
            queueService = _queueService;
            actionsService = _actionsService;
            commonService = _commonService;
            workflowService = _workflowService;
            _toastNotification = toastNotification;
            validationService = _validationService;
            formBuilderService = _formBuilderService;
            formService = _formService;
            elementWorkflowService = _elementWorkflowService;
            _config = config;
            baseApiUrl= _config.GetSection("BaseApiUrl").Value;
        }
        [HttpGet]
        [Route("admin/manage/elementworkflow.html")]
        [Route("admin/{tenant_identifier}/manage/elementworkflow.html")]
        public IActionResult ElementWorkflow(string tenant_identifier)
        {
            return View();
        }

        [HttpPost]
        [Route("admin/manage/pullelementworkflow")]
        [Route("admin/{tenantIdentifier}/manage/pullelementworkflow")]
        public JsonResult PullElementWorkflow(int tenantIdentifier, int formId, string elementId,int eventType)
        {
            try
            {
                string jwfpvm = elementWorkflowService.GetElementWorkFlowByCaseFormId(formId, tenantIdentifier,elementId, eventType);
                var AllPoints = (JObject)JsonConvert.DeserializeObject(jwfpvm);
                _toastNotification.AddSuccessToastMessage("");
                return Json(new { status = "success", form_id = formId, json = AllPoints, elementId = elementId, eventType = eventType});
            }
            catch (Exception ex)
            {
                Log.LogError("Worflow:GetData - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = "Error" });
            }
          
        }

        [HttpPost]
        [Route("admin/manage/elementworkflow")]
        [Route("admin/{tenant_identifier}/manage/elementworkflow")]
        public JsonResult StateWorkFlow([FromBody] JObject data)
        {

            int tenantid = Convert.ToInt32(data["tenant_identifier"]);
            int form_id = Convert.ToInt32(data["form_id"]);
            // JObject jObject = (JObject)JsonConvert.DeserializeObject(allPoints);
            JObject jObject = (JObject)data["allPoints"];
            JArray jArray = (JArray)jObject["point"];
            JArray jArryObj = (JArray)jObject["object"];
            string addStateForForm = data["addStateForForm"].ToString();
            string removeStateForForm = data["removeStateForForm"].ToString();
            string elementId = data["elementId"].ToString();
            int eventType = Convert.ToInt16(data["eventType"]);
            
            try
            {
                JObject result = new JObject();
                if (form_id != 0)
                {
                    List<JsonWorkFlowPointsViewModel> jwvmLst = new List<JsonWorkFlowPointsViewModel>();
                    foreach (JObject obj in jArray)
                    {
                        JsonWorkFlowPointsViewModel jwvm = new JsonWorkFlowPointsViewModel();
                        jwvm.CaseFormId = form_id;
                        JArray points = new JArray();
                        points = (JArray)obj["points"];
                        jwvm.Type = Convert.ToString(obj["type"]);
                        bool biDirectional = false;

                        if (points[0]["lineCap"].ToString() == "arrow" && points[1]["lineCap"].ToString() == "arrow")
                        {
                            biDirectional = true;
                        }

                        int i = 1;
                        foreach (JObject point in points)
                        {
                            if (i == 1)
                            {
                                jwvm.First = new FirstPoint();
                                //Type Object on  First Point
                                JObject AObject = (JObject)point["object"];
                                jwvm.First.AObject = new ActionObject();
                                jwvm.First.AObject.Type = Convert.ToString(AObject["type"]).ToUpper();
                                jwvm.First.AObject.TypeId = Convert.ToInt32(AObject["id"]);
                            }
                            else
                            {
                                //Last Point Object
                                jwvm.Last = new LastPoint();
                                //Type Object on  Last Point
                                JObject AObject = (JObject)point["object"];
                                jwvm.Last.AObject = new ActionObject();
                                jwvm.Last.AObject.Type = Convert.ToString(AObject["type"]).ToUpper();
                                jwvm.Last.AObject.TypeId = Convert.ToInt32(AObject["id"]);

                            }
                            i++;
                        }
                        jwvmLst.Add(jwvm);

                        if (biDirectional)
                        {
                            JsonWorkFlowPointsViewModel jwvmbi = new JsonWorkFlowPointsViewModel();
                            jwvmbi.CaseFormId = form_id;
                            i = 2;
                            foreach (JObject point in points)
                            {
                                if (i == 1)
                                {
                                    jwvmbi.First = new FirstPoint();
                                    //Type Object on  First Point
                                    JObject AObject = (JObject)point["object"];
                                    jwvmbi.First.AObject = new ActionObject();
                                    jwvmbi.First.AObject.Type = Convert.ToString(AObject["type"]).ToUpper();
                                    jwvmbi.First.AObject.TypeId = Convert.ToInt32(AObject["id"]);
                                }
                                else
                                {
                                    //Last Point Object
                                    jwvmbi.Last = new LastPoint();
                                    //Type Object on  Last Point
                                    JObject AObject = (JObject)point["object"];
                                    jwvmbi.Last.AObject = new ActionObject();
                                    jwvmbi.Last.AObject.Type = Convert.ToString(AObject["type"]).ToUpper();
                                    jwvmbi.Last.AObject.TypeId = Convert.ToInt32(AObject["id"]);

                                }
                                i--;
                            }
                            jwvmLst.Add(jwvmbi);
                        }
                    }
                    result = elementWorkflowService.SaveElementWorkFlow(form_id, jwvmLst, tenantid, jObject, jArryObj,elementId,eventType);
                    if (removeStateForForm != "")
                    {
                        bool status = elementWorkflowService.RemoveElementState(removeStateForForm,form_id,elementId,eventType);
                    }

                }
                if (result["status"].ToString() == "success")
                {
                    _toastNotification.AddSuccessToastMessage("Workflow is saved.");

                    return Json(new { status = "success", message = "Workflow is Successfully Saved" });
                }
                else
                {
                    return Json(new { status = "error", message = result["message"].ToString() });
                }

            }

            catch (Exception ex)
            {
                Log.LogError("Workflow:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = ex });
            }
        }

        [HttpPost]
        [Route("admin/workflow/run")]
        public JsonResult RunWorkflow([FromBody] WorkRun values)
        {
            //  WorkRun jObject = (WorkRun)JsonConvert.DeserializeObject(values);
            JObject data = new JObject();
            JObject emptyCheck = new JObject();
            data = elementWorkflowService.RunWorkFlowApiForElements(this, values, baseApiUrl);
            object response;
            object Target;
            object Data;
            try
            {

                //workflowService.RunWorflowActionObjectV2();
                // object data = new { calculate = cal };
                if (Convert.ToInt32(data["success"]) == 1)
                {

                    if (data["response"]["target"] != null)
                    {
                        Target = data["response"]["target"];
                    }
                    else
                    {
                        Target = "";
                    }
                    Data = data["data"]["response"];

                }
                else
                {
                    Data = "";
                    Target = "";
                }
                if (data["response"]["target"] != null && values.Targets != null)
                {
                    string tar = data["response"]["target"].ToString();
                    Target = elementWorkflowService.AssembleTarget(values.Targets, (JArray)JsonConvert.DeserializeObject(tar));
                }
                response = new { Success = data["response"]["success"], StatusCode = data["response"]["statusCode"], Message = data["response"]["message"], DataList = data["response"]["dataList"], Data, Target };



            }
            catch (Exception ex)
            {
                if (data["response"] != null)
                {
                    response = new { Success = data["response"]["success"], StatusCode = data["response"]["statusCode"], Message = data["response"]["message"], DataList = data["response"]["dataList"], Data = "", Target = "" };

                }
                else
                {
                    response = new { Success = false, StatusCode = "404", Message = "No Data", DataList = "", Data = "", Target = "" };

                }


            }
            return Json(response);

        }

        [HttpPost]
        [Route("/admin/workflow/elementvalidation.html")]
        public IActionResult ValidateElementData(IFormCollection formData, int formId, List<string> exclude, bool isFrontValidation)
        {
            CaseFormViewModel caseaa = formBuilderService.GetBuilderFormById(formId);
            string form = caseaa.UrlIdentifier;

            List<ElementValidation> elmvalidations = validationService.ValidateFormData(formData, form, exclude, isFrontValidation);
            List<ElementValidation> elmval = elmvalidations.Where(x => x.IsValid == false).ToList();
            bool valid = (elmvalidations.Where(x => x.IsValid == false).Count() > 0) ? false : true;
            Validations validations = new Validations()
            {
                ElementValidations = elmvalidations,
                isFormValid = valid
            };
            return Json(validations);
        }

        [HttpPost]
        [Route("/admin/workflow/manage/element/createelementstate")]
        public JsonResult CreateElementState(int formId, string elementId, int eventType, string stateType)
        {
            int tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            ElementStateViewModel elementStateViewModel = new ElementStateViewModel()
            {
                Name = char.ToUpper(stateType.First()) + stateType.Substring(1).ToLower(),
                ElementId = elementId,
                CreatedAt = DateTime.UtcNow,
                ForEventType = eventType,
                Type = stateType,
                isDefaultEnd = false,
                FormId = formId,
                TenantId = tenantId
            };


            elementStateViewModel = elementWorkflowService.CreateElementState(elementStateViewModel);
            JObject result = new JObject();
            result.Add("id", elementStateViewModel.Id);
            result.Add("title", elementStateViewModel.Name);
            return Json(result);
        }

    }
}