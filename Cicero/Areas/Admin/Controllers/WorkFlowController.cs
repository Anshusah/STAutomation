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

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WorkFlowController : BaseController
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

        public WorkFlowController(ILogger<QueueController> _Log, IStatus _status, Utils _utils, IUserService _userService,
            IQueueService _queueService, IActionsService _actionsService, ICommonService _commonService,
            IWorkflowService _workflowService, IToastNotification toastNotification,
            IValidationService _validationService, IFormBuilderService _formBuilderService,
            IFormService _formService) : base(_userService)
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
        }

        [HttpGet]
        [Route("admin/manage/workflow.html")]
        [Route("admin/{tenant_identifier}/manage/workflow.html")]
        public IActionResult WorkFlow(string tenant_identifier)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {

                WorkFlowViewModel wfvm = new WorkFlowViewModel
                {
                    CaseFormList = commonService.GetFormListForActiveTenant(),
                    StateList = queueService.GetSelectStateList(tenantid),
                    QueueList = queueService.GetSelectQueueList(tenantid),
                    RoleList = commonService.GetRoleList(),
                    ActionList = queueService.GetSelectActionList(tenantid)
                };
                return View(wfvm);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        [Route("admin/manage/workflow.html")]
        [Route("admin/{tenant_identifier}/manage/workflow.html")]
        public JsonResult StateWorkFlow([FromBody] JObject data)
        {

            int tenantid = Convert.ToInt32(data["tenant_identifier"]);
            int form_id = Convert.ToInt32(data["form_id"]);
            // JObject jObject = (JObject)JsonConvert.DeserializeObject(allPoints);
            JObject jObject = (JObject)data["allPoints"];
            JArray jArray = (JArray)jObject["point"];
            JArray jArryObj = (JArray)jObject["object"];
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
                    result = workflowService.SaveWorkFlow(form_id, jwvmLst, tenantid, jObject, jArryObj);
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
        [Route("admin/manage/pull-workflow.html")]
        [Route("admin/{tenant_identifier}/manage/pull-workflow.html")]
        public JsonResult PullStateWorkFlow(int tenant_identifier, int form_id)
        {
            try
            {
                string jwfpvm = workflowService.GetWorkFlowByCaseFormId(form_id, tenant_identifier);
                var AllPoints = (JObject)JsonConvert.DeserializeObject(jwfpvm);
                _toastNotification.AddSuccessToastMessage("");
                return Json(new { status = "success", form_id = form_id, json = AllPoints });
            }
            catch (Exception ex)
            {
                Log.LogError("Worflow:GetData - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = "Error" });
            }

        }

        [HttpPost]
        [Route("admin/testapi/postmethod")]
        public async Task<IActionResult> TestApiAsync(IFormCollection data)
        {
            int isGet = Convert.ToInt16(data["configs[0][isGetMethod]"]);
            data.TryGetValue("configs[0][ApiUrl]", out StringValues api_url);
            List<string> param = data["configs[0][ApiParameterList][]"].ToString().Split(',').ToList<string>();
            List<string> param_format = data["configs[0][ApiParameterFormatList][]"].ToString().Split(',').ToList<string>();
            List<string> param_value = data["configs[0][ApiParameterValue][]"].ToString().Split(',').ToList<string>();
            List<string> key = data["configs[0][ApiKeyList][]"].ToString().Split(',').ToList<string>();
            List<string> value = data["configs[0][ApiValueList][]"].ToString().Split(',').ToList<string>();
            JObject keyValuePair = new JObject();
            if (key.Count > 0)
            {
                for (int i = 0; i < key.Count; i++)
                {
                    keyValuePair.Add(key[i], value[i]);
                }
            }

            try
            {
                if (isGet > 0)
                {
                    string parameters = "?";
                    for (int i = 0; i < param.Count; i++)
                    {
                        if (i != param.Count && i != 0)
                            parameters = parameters + "&";
                        parameters = parameters + param[i] + "=" + param_value[i];
                    }
                    var jsonResult =
                        await WebApiService.InstanceForExternal.GetAsync<object>(api_url + parameters, true, "externalHeaders/mirs");
                    return Ok(jsonResult);
                }
                else
                {
                    JObject postData = new JObject();
                    if (key.Count > 0)
                    {
                        for (int i = 0; i < param.Count; i++)
                        {
                            postData.Add(param[i], param_value[i]);
                        }
                    }
                    if (key.Count > 0)
                    {
                        for (int i = 0; i < param.Count; i++)
                        {
                            postData.Add(param[i], param_value[i]);
                        }
                    }

                    var jsonResult =
                    await
                        WebApiService.InstanceForExternal.PostAsync<object>(api_url, true,
                                "externalHeaders/mirs", postData);
                    return Ok(jsonResult);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("MIRS API Failed");

            }

        }

        [HttpPost]
        [Route("admin/stateworkflow/remove.html")]
        public JsonResult Remove(string id)
        {
            var idSplit = id.Split('-');
            string type = idSplit[0];
            int idValue = Convert.ToInt32(idSplit[idSplit.Count() - 1]);
            try
            {
                workflowService.RemoveQueueOrStateForForm(type, idValue);
                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                Log.LogError("Worflow:Remove - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = "Error" });
            }

        }

        [HttpGet]
        [Route("admin/testapi/getcountries")]
        public JsonResult GetCountries()
        {
            object response;
            try
            {
                var caseList = commonService.CountryList();
                object country = new { country = caseList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Json(response);
        }

        [HttpGet]
        [Route("admin/testapi/calulate")]
        public JsonResult Calculate(int number1, int number2)
        {
            object response;
            try
            {
                float cal = number1 * number2;
                object da = new { DataCal = cal + " is the calculated value." };
                object data = new { calculate = da };
                response = new { Success = true, StatusCode = 200, Message = "Calculated Successfully", DataList = "", Data = data, Target = "[{\"value\":\"Building\"},{\"value\":\"Content\"}]" };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 404, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Json(response);
        }
       
    }

}