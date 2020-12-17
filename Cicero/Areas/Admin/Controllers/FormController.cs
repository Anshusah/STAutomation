using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Cicero.Configuration;
using Cicero.Data;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FormController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly Utils _utils;
        private readonly ILogger<FormController> _log;
        private readonly IStatus _status;
        private readonly ITenantService _tenantService;
        private readonly AppSetting _appSetting;
        private readonly ICommonService _ICommonService;
        private readonly IFormService _formService;
        private readonly IQueueService _queueService;
        private readonly ICaseService _caseService;
        private readonly ISynchronizeService _synchronizeService;
        private readonly IAutomationService _automationService;
        private readonly IWorkflowService _workflowService;
        private readonly IMediaService mediaService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly ISecureTradingService secureTradingService;
        private readonly IConfiguration configuration;
        private readonly IOnfidoService onfidoService;
        private readonly string baseApiUrl;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly ApplicationDbContext _db;
        private readonly ITransactionMgmtService transactionMgmtService;
        private readonly ICustomerService customerService;

        public FormController(IFormService IFormService,
            ISynchronizeService synchronizeService,
            IAutomationService autoService,
            ICaseService caseService,
            IQueueService queueService,
            ICommonService ics,
            IUserService userService,
            AppSetting appSetting,
            IFormBuilderService formBuilderService,
            Utils utils,
            ILogger<FormController> log,
            IStatus status,
            ITenantService tenantService,
            IWorkflowService workflowService,
            IMediaService mediaService,
            IHostingEnvironment hostingEnvironment,
            IToastNotification toastNotification,
            ISecureTradingService secureTradingService,
                        IConfiguration configuration, IOnfidoService onfidoService,
                        IRolePermissionService rolePermissionService,
            ApplicationDbContext db, ITransactionMgmtService transactionMgmtService,
            ICustomerService customerService) : base(userService)
        {
            _userService = userService;
            _formBuilderService = formBuilderService;
            _utils = utils;
            _log = log;
            _status = status;
            _tenantService = tenantService;
            _appSetting = appSetting;
            _ICommonService = ics;
            _formService = IFormService;
            _queueService = queueService;
            _caseService = caseService;
            _synchronizeService = synchronizeService;
            _automationService = autoService;
            _workflowService = workflowService;
            this.mediaService = mediaService;
            this.hostingEnvironment = hostingEnvironment;
            _toastNotification = toastNotification;
            this.secureTradingService = secureTradingService;
            this.configuration = configuration;
            this.onfidoService = onfidoService;
            baseApiUrl = this.configuration.GetSection("BaseApiUrl").Value;
            _rolePermissionService = rolePermissionService;
            _db = db;
            this.transactionMgmtService = transactionMgmtService;
            this.customerService = customerService;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/form/{form}.html")]
        [Route("admin/form/{tenant_identifier}/{form}.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{queue}.html")]
        public IActionResult Index(string tenant_identifier, string form, string queue)
        {
            List<CaseFormViewModel> lst = _ICommonService.GetCaseFormListForActiveTenantId();
            string search_key = "Identifier\":\"" + form;
            CaseFormViewModel lsts = lst.Where(d => d.Fields.Contains(search_key)).FirstOrDefault();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);
            List<KeyValuePair<string, string>> dyn = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, List<string>>> dyn1 = new List<KeyValuePair<string, List<string>>>();
            List<String> TableList = new List<string>();
            foreach (var tab in allfields.Tab)
            {
                foreach (var row in tab.Row)
                {
                    foreach (var column in row.Column)
                    {
                        foreach (dynamic element in column.Element)
                        {
                            var isit = element.GetType().GetProperty("VisibleinGrid");
                            if (isit != null)
                            {
                                if (element.VisibleinGrid)
                                {
                                    TableList.Add(element.TableName);
                                    dyn.Add(new KeyValuePair<string, string>(element.BackendLabel, element.ElementId));
                                    List<String> listr = new List<string>();
                                    listr.Add(element.BackendLabel);
                                    listr.Add(element.FieldName);
                                    dyn1.Add(new KeyValuePair<string, List<string>>(element.TableName, listr));
                                }
                            }
                        }
                    }
                }
            }

            var jsondata = JsonConvert.SerializeObject(dyn);

            ViewBag.JsonData = jsondata;


            ///// 
            //var tempqueue = _queueService.GetQueueByQueueIdentifier(queue);
            //int queueid = 0;
            //if (tempqueue != null)
            //{
            //    queueid = tempqueue.Id;
            //}

            //int caseformid = _ICommonService.GetCaseFormIdByUrl(form); 
            //DTResponseModel jsonFile = _formService.GetJsonData(TableList, dyn1, caseformid, queueid);
            //List<JObject> obj = (List<JObject>)jsonFile.data;
            ////
            //ViewBag.JsonData = obj[0];
            ///////////
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/form/{form}.html")]
        [Route("admin/form/{tenant_identifier}/{form}.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{queue}.html")]
        public JsonResult Index(DTPostModel model, string tenant_identifier, string form, string queue)
        {
            Theme th = ViewData["Theme"] as Theme;
            //Kishan sir Code committed out
            // th.DoAction("before_case_switch", new object[] { "1", "2", "3" });
            List<CaseFormViewModel> lst = _ICommonService.GetCaseFormListForActiveTenantId();
            string search_key = "Identifier\":\"" + form;
            CaseFormViewModel lsts = lst.Where(d => d.Fields.Contains(search_key)).FirstOrDefault();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);
            List<KeyValuePair<string, string>> dyn = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> dtype = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, List<string>>> dyn1 = new List<KeyValuePair<string, List<string>>>();
            List<String> TableList = new List<string>();
            foreach (var tab in allfields.Tab)
            {
                foreach (var row in tab.Row)
                {
                    foreach (var column in row.Column)
                    {
                        foreach (dynamic element in column.Element)
                        {
                            var isit = element.GetType().GetProperty("VisibleinGrid");
                            if (isit != null)
                            {
                                if (element.VisibleinGrid)
                                {
                                    TableList.Add(element.TableName);
                                    switch (element.GetType().Name.ToLower()) //checkfor type;
                                    {
                                        case "number":
                                            if (element.FrontendClass.ToString().Contains("currency") || element.FrontendClass.ToString().Contains("currency") || element.IsCurrency)
                                            {
                                                string cur = "$";
                                                if (element.CurrencyType != "")
                                                {
                                                    cur = element.CurrencyType;
                                                }
                                                dtype.Add(new KeyValuePair<string, string>(element.ElementId, "currency_" + cur));
                                            }
                                            else
                                            {
                                                dtype.Add(new KeyValuePair<string, string>(element.ElementId, "decimal"));
                                            }

                                            break;
                                        case "textbox":
                                            if (element.FrontendClass.ToString().Contains("date") || element.FrontendClass.ToString().Contains("datetime"))
                                            {
                                                dtype.Add(new KeyValuePair<string, string>(element.ElementId, "datetime"));
                                            }
                                            else
                                            {
                                                dtype.Add(new KeyValuePair<string, string>(element.ElementId, "string"));
                                            }
                                            break;
                                        default:
                                            dtype.Add(new KeyValuePair<string, string>(element.ElementId, "string"));
                                            break;
                                    }
                                    dyn.Add(new KeyValuePair<string, string>(element.BackendLabel, element.ElementId));
                                    List<String> listr = new List<string>();
                                    listr.Add(element.ElementId);
                                    listr.Add(element.FieldName);
                                    dyn1.Add(new KeyValuePair<string, List<string>>(element.TableName, listr));
                                }
                            }
                        }
                    }
                }
            }

            var tempqueue = _queueService.GetQueueByQueueIdentifier(queue);

            int queueid = 0;
            if (tempqueue != null)
            {
                queueid = tempqueue.Id;
            }

            int caseformid = _ICommonService.GetCaseFormIdByUrl(form);
            Permission permission = new Permission(_userService, _db, _rolePermissionService);

            bool isAllowed = false;

            //var cases = _caseService.GetFormIdsByQueue(queueid, caseformid);
            var jsonFile = _formService.GetJsonData(model, TableList, dyn1, caseformid, queueid,isAllowed, dtype, queue);
            var formdata = jsonFile;

            return Json(new
            {
                draw = formdata.draw,
                recordsTotal = formdata.recordsTotal,
                recordsFiltered = formdata.recordsFiltered,
                data = formdata.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/form/{encryptedid}/{encryptedCaseid}/edit.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{encryptedCaseid}/edit.html")]
        public IActionResult Edit(string encryptedCaseid, string form, string id = "")
        {
            if (form.ToLower() == "jazzcash")
            {
                HttpContext.Session.Remove("payerPayment");
            }
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            string view = "";
            if (_userService.UserHasPolicy() == "frontend" || _userService.UserHasPolicy() == null)
            {
                string Theme = "~/Themes/" + this.Theme.GetName(false) + "/Shared/_Layout.cshtml";
                ViewBag.ThemeView = Theme;
                view = "/Themes/" + this.Theme.GetName(false) + "/Form/Edit.cshtml";
            }
            int Caseid = _utils.DecryptId(encryptedCaseid);

            try
            {
                CaseViewModel cvm = new CaseViewModel
                {
                    Id = Caseid,
                    PaymentRequestId = _utils.DecryptId(id),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CaseGeneratedId = _formService.GenerateFormId(),
                };
                if (Caseid != 0)
                    cvm.Id = Caseid;
                var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
                int case_id = ccvm.Id;

                if (_userService.UserHasPolicy() == "frontend")
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "front");
                }
                else
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "back");
                }

                if (Caseid != 0 && form != "jazzcash")
                    cvm = _caseService.GetCaseById(Caseid);

                int caseformid = case_id;

                cvm.CaseFormName = _caseService.GetCaseFormTypeNameByCaseFormId(caseformid);

                string roleid = _ICommonService.GetRoleIdByUserId(_ICommonService.getLoggedInUserId());
                if (roleid == " ")
                {
                    roleid = ApplicationConstants.CustomerRoleId;
                }

                cvm.StateList = _queueService.GetStateSelectListByFormId(caseformid, roleid);

                cvm.CaseTasks = _queueService.GetCaseMovement(caseformid, Caseid);
                if (cvm.UserId != null)
                {
                    var userName = _userService.GetUserById(cvm.UserId).Result;
                    cvm.UserFullName = userName.FirstName + " " + userName.LastName;
                    cvm.CaseOwner = _queueService.GetCaseOwner(cvm.CaseFormId, cvm.StateId, cvm.UserId);
                }

                cvm.QueueName = _queueService.GetQueueNameByFormId(caseformid, Convert.ToInt32(cvm.StateId));
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                if (ccvm.Fields != null)
                {
                    ViewBag.formData = new JObject();
                    cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
                    ViewBag.formData = _formService.GetTableData(Caseid, tables, a);
                    var formData = "";
                    string loggedinUser = _ICommonService.getLoggedInUserId();
                    if (loggedinUser != null)
                    {
                        formData = customerService.GetCustomerCountryCode(loggedinUser).Result == "GB"? HttpContext.Session.GetString("formData") : "";
                        HttpContext.Session.Clear();
                    }
                    if (ViewBag.formData.Children == null && formData != "" && formData != null)
                    {
                        var array = JArray.Parse(formData);
                        IDictionary<string, string> dict = array.ToDictionary(k => ((JObject)k).Properties().First().Name, v => v.Values().First().Value<string>());
                        ViewBag.formData = dict;
                        HttpContext.Session.Clear();
                    }
                }
                if (cvm.CaseFormId == 0)
                {
                    cvm.CaseFormId = caseformid;
                }

                if (view != "")
                    return View(view, cvm);
                return View(cvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/form/{encryptedid}/edit.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{encryptedid}/edit.html")]
        public async Task<IActionResult> EditAsync(CaseViewModel cvm, string tenant_identifier, string form, IFormCollection formData)
        {

            try
            {
                var recaptchaData = formData.Where(x => x.Key.Contains("g-recaptcha-response")).FirstOrDefault();
                if (recaptchaData.Key != null)
                {
                    JObject jsonResult;
                    var url = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
                    url = string.Format(url, "6LfOJdgUAAAAADIxX7YGDbVM7QPKwytQPEIcViuW", recaptchaData.Value.ToString());
                    jsonResult = WebApiService.InstanceForExternal.PostAsync<JObject>(url, false, "", null).Result;
                    string googleResult = jsonResult["success"].ToString();
                    if (googleResult == "False")
                    {
                        throw new Exception();
                    }
                }

                int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

                string loggedinUser = _ICommonService.getLoggedInUserId();
                bool isadmin = await _ICommonService.IsSuperAdmin();

                cvm.UserId = loggedinUser;
                bool isNew = false;
                int case_id = _formBuilderService.GetBuilderFormByUrl(form, tenantid).Id;
                int prevStateId = 0;


                if (cvm.Id == 0)
                {
                    isNew = true;
                    cvm.CreatedAt = DateTime.Now;
                    //int case_id = _formBuilderService.GetBuilderFormByUrl(form, tenantid).Id;

                    if (_userService.UserHasPolicy() == "backend" || isadmin == true)
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "back");
                        prevStateId = _queueService.GetFirstState(case_id, "back");
                    }
                    else
                    {
                        prevStateId = _queueService.GetFirstState(case_id, "front");
                    }

                }
                else
                {
                    cvm.StateId = _caseService.GetCaseById(cvm.Id).StateId;
                    prevStateId = cvm.StateId;
                }
                var fromBtn = string.Empty;
                if (_userService.UserHasPolicy() == "frontend")
                {
                    if (formData["btn-type"].ToString().Equals("save", StringComparison.OrdinalIgnoreCase))
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "front");
                    }
                    else if (formData["btn-type"].ToString().Equals("send", StringComparison.OrdinalIgnoreCase))
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "back");
                    }
                    else
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "back");
                        fromBtn = "other";
                    }

                }


                int tenantId = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                cvm.TenantId = tenantId;

                cvm.UpdatedAt = DateTime.Now;
                cvm = await _formService.SaveCaseAsync(cvm, form);
                var formdata1 = formData.Where(x => x.Key.Contains("elm") || x.Key.Contains("Media")).ToList();

                var mediaData = formData.Where(x => x.Key.Contains("Media")).ToList();

                foreach (var item in mediaData)
                {
                    var mediaIdList = item.Value.ToString().Split(",").ToList();
                    var result = _formService.SaveOrUpdateCaseMedia(mediaIdList.Select(int.Parse).ToList(), cvm.Id);
                }

                _formService.UpdateFormData(formdata1, form, cvm.Id, isNew);
                cvm.StateId = _workflowService.RunWorflowActionObject(this, cvm.CaseFormId, prevStateId, cvm.StateId, cvm.Id, baseApiUrl);
                if (cvm.StateId != prevStateId)
                    await _caseService.SaveCaseStateHistory(cvm, loggedinUser, "", prevStateId, cvm.StateId);

                //

                cvm = await _formService.SaveCaseAsync(cvm, form);
                var redirect = string.Empty;
                if (fromBtn == "")
                {
                    redirect = "~/admin/form" + _utils.GetTenantForUrl(false) + "/" + form + "/" + _utils.EncryptId(cvm.Id) + "/edit.html";
                }
                else
                {
                    redirect = "~/admin/form" + _utils.GetTenantForUrl(false) + "/" + form + "/" + _utils.EncryptId(cvm.Id) + "/edit.html";
                }


                if (ModelState.IsValid)
                {
                    _toastNotification.AddSuccessToastMessage(formData["CaseFormName"].ToString() + " case updated.");
                    return Redirect(redirect);
                }
                _utils.addModelError(ModelState);
                return Redirect("~/admin/form" + _utils.GetTenantForUrl(false) + "/" + form + "/" + _utils.EncryptId(cvm.Id) + "/edit.html");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage("Error: " + ex.ToString());
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                return Redirect("~/admin/form" + _utils.GetTenantForUrl(false) + "/" + form + "/" + _utils.EncryptId(cvm.Id) + "/edit.html");

            }
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/form/ajax/edit.html")]
        [Route("admin/form/ajax/{tenant_identifier}/{form}/edit.html")]
        public async Task<IActionResult> AjaxEditAsync(IFormCollection formData, CaseViewModel cvm, string tenant_identifier, string form)
        {
            try
            {
                var recaptchaData = formData.Where(x => x.Key.Contains("g-recaptcha-response")).FirstOrDefault();
                if (recaptchaData.Key != null)
                {
                    JObject jsonResult;
                    var url = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
                    url = string.Format(url, "6LfOJdgUAAAAADIxX7YGDbVM7QPKwytQPEIcViuW", recaptchaData.Value.ToString());
                    jsonResult = WebApiService.InstanceForExternal.PostAsync<JObject>(url, false, "", null).Result;
                    string googleResult = jsonResult["success"].ToString();
                    if (googleResult == "False")
                    {
                        throw new Exception();
                    }
                }

                int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

                string loggedinUser = _ICommonService.getLoggedInUserId();

                if (loggedinUser != null)
                {
                    var checkOnfidoResults = onfidoService.CheckOnfidoVerifyResultByUserId(loggedinUser).Result;
                    if (checkOnfidoResults != null)
                    {
                        if (!checkOnfidoResults.IsOnfidoVerify)
                        {
                            return StatusCode(404, new { cvm = "", formDatas = "", message = "Onfido Not Verified" });
                        }
                        //if (checkOnfidoResults.IsOnfidoVerify)
                        //{
                        //    if (checkOnfidoResults.OnfidoCheckResult != OnfidoCheckResults.clear.ToString())
                        //    {
                        //        return StatusCode(400, new { cvm = "", formDatas = "" });
                        //    }
                        //}
                        //else
                        //{
                        //    return StatusCode(404, new { cvm = "", formDatas = "" });
                        //}
                    }

                    var sanctionPep = await transactionMgmtService.SanctionCheckCustomer(loggedinUser);
                    if (!sanctionPep)
                    {
                        return StatusCode(404, new { cvm = "", formDatas = "", message = "Sanction Pep" });
                    }
                }

                bool isadmin = await _ICommonService.IsSuperAdmin();

                cvm.UserId = loggedinUser;
                bool isNew = false;

                if (cvm.Id == 0)
                {
                    isNew = true;
                }

                int case_id = _formBuilderService.GetBuilderFormByUrl(form, tenantid).Id;


                int tenantId = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                cvm.TenantId = tenantId;

                cvm.UpdatedAt = DateTime.Now;
                if (loggedinUser == null || loggedinUser == "")
                {
                    var list = formData.Select(p => new Dictionary<string, string>() { { p.Key, p.Value } });
                    HttpContext.Session.SetString("formData", list.ToJson());
                    throw new Exception("User not logged in.");
                }
                cvm = await _formService.SaveCaseAsync(cvm, form);
                var formdata1 = formData.Where(x => x.Key.Contains("elm") || x.Key.Contains("Media")).ToList();

                var mediaData = formData.Where(x => x.Key.Contains("Media")).ToList();

                foreach (var item in mediaData)
                {
                    var mediaIdList = item.Value.ToString().Split(",").ToList();
                    var result = _formService.SaveOrUpdateCaseMedia(mediaIdList.Select(int.Parse).ToList(), cvm.Id);
                }

                _formService.UpdateFormData(formdata1, form, cvm.Id, isNew);

                cvm = await _formService.SaveCaseAsync(cvm, form);

                var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);

                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
                var formDatas = _formService.GetTableData(cvm.Id, tables, a);
              
                return Ok(new { cvm = cvm, formDatas = formDatas, encryptedcaseid = _utils.EncryptId(cvm.Id) });
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage("Error: " + ex.ToString());
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                return StatusCode(500, new { cvm = "", formDatas = "" });
            }
        }


        [Area("Admin")]
        [HttpPost]
        [Route("admin/form/{tenant_identifier}/action.html")]
        [Route("admin/form/{tenant_identifier}/{form}/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page, string reason = "", IEnumerable<string> attachments = null, string tenant_identifier = "", string useraccessid = null)
        {
            List<int> toStates = new List<int>();
            if (string.IsNullOrEmpty(tenant_identifier))
            {
                _toastNotification.AddWarningToastMessage("Please select a Tenant.");
                return Redirect(page);
            }
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect(page);
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one item");
                return Redirect(page);
            }
            Theme th = ViewData["Theme"] as Theme;



            int successCount = 0;
            string state = "";
            int stateid = 0;
            int newstateid = 0;

            foreach (var item in Ids)
            {

                //var CaseId = Convert.ToInt32(item);
                bool result = false;

                if (item != 0)
                {
                    //th.DoAction('before_case_switch', new object[]{"ds","fdfd","fdfd" });

                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        CaseViewModel cvm = new CaseViewModel();
                        if (item != 0)
                        {

                            cvm = _caseService.GetCaseById(item);
                            int caseformid = cvm.CaseFormId;

                            var ccvm = _formBuilderService.GetBuilderFormById(caseformid);
                            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                            cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                            FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                            result = _formService.DeleteFromData(a, item);
                            if (result)
                            {
                                result = _caseService.DeleteCase(cvm);
                            }
                        }
                        //result = await ICaseService.DeleteCaseById(item);
                    }

                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        state = ButtonAction.active.ToDescription();
                        //result = await ICaseService.ActiveCaseById(item);
                    }

                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        //result = await ICaseService.InactiveCaseById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.sendsubrogation))
                    {
                        //need shipping company data
                        //state = "sent letter";

                        //var caseData = ICaseService.GetCaseById(item);
                        //string messagebody = templateService.GetTemplateBodyByName("claim-email-notification");
                        //messagebody = templateService.GenerateTemplate(messagebody, caseData);
                        //string newmsg = templateService.GenerateTemplate(messagebody, caseData);
                        //if (messagebody != null)
                        //{
                        //    //replace message details
                        //    //messagebody = messagebody.Replace("[user_name]", caseData.FullName);

                        //    var newMessage = new TemplateViewModel { };
                        //    newMessage.Content = messagebody;

                        //    string body = await razorToStringRender.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", newMessage);

                        //    string url = "wwwroot/uploads/";

                        //    List<string> imagesurl = new List<string>();

                        //    foreach (var itemImages in attachments)
                        //    {
                        //        imagesurl.Add(url + itemImages);
                        //    }

                        //    string letterAttachment = null;

                        //    try
                        //    {
                        //        letterAttachment = await ICaseService.HtmlToPdf(caseData.Id);

                        //        if (letterAttachment == null)
                        //        {
                        //            _status.Show("error", "Pdf not created.", false);
                        //        }
                        //    }
                        //    catch (Exception)
                        //    {
                        //        _status.Show("error", "Pdf not created.", false);
                        //    }

                        //    //string letterAttachment = await HtmlToPdf(caseData.Id);

                        //    if (letterAttachment != null)
                        //    {
                        //        imagesurl.Add(letterAttachment);

                        //        //client email used for now
                        //        await emailSender.SendEmailAttachmentAsync(caseData.Email, "Claim Filed", body, imagesurl);

                        //        _status.Show("success", "Email Sent Successfully.", false);
                        //        result = true;
                        //        break;
                        //    }
                        //    _status.Show("error", "Email not Sent.", false);
                        //    result = false;
                        //}
                    }

                    else
                    {
                        try
                        {
                            stateid = Convert.ToInt32(action);
                        }
                        catch (Exception)
                        {
                            break;
                        }

                        if (stateid != 0)
                        {

                            //case synce when state changed

                            ////var currentstate = ICaseService.GetStateIdByCaseId(item);
                            //try
                            //{
                            //    int synchronizeCheck = ICaseService.SynchronizeCase(item, stateid, "manual");
                            //    if (synchronizeCheck != 0)
                            //    {
                            //        newstateid = synchronizeCheck;
                            //    }

                            //}
                            //catch (Exception ex)
                            //{
                            //    break;
                            //}

                            //try
                            //{
                            //    if (newstateid != 0)
                            //    {
                            //        int synchronizeCheck = ICaseService.SynchronizeCase(item, newstateid, "auto");
                            //        if (synchronizeCheck != 0)
                            //        {
                            //            newstateid = synchronizeCheck;
                            //        }
                            //    }

                            //}
                            //catch (Exception ex)
                            //{
                            //    break;
                            //}

                            //if (newstateid != 0)
                            //{
                            //    state = queueService.GetStateNameById(newstateid);
                            //    stateid = newstateid;
                            //}
                            //else
                            //{
                            //    newstateid = stateid;
                            //}

                            if (string.IsNullOrWhiteSpace(useraccessid))
                            {
                                useraccessid = null;
                            }

                            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                            //if (!string.IsNullOrWhiteSpace(useraccessid))
                            //{
                            //    var loggeduser = _ICommonService.getLoggedInUserId();
                            //    string roleid = _ICommonService.GetRoleIdByUserId(loggeduser);
                            //    string rolecheck = _queueService.GetStateById(tenantid, newstateid).RoleId;

                            //    if (rolecheck.Equals(roleid, StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        useraccessid = loggeduser;
                            //    }
                            //}

                            //For Automation
                            var currentstate = _caseService.GetStateIdByCaseId(item);
                            //  th.DoAction("before_case_switch", new object[] { currentstate, Convert.ToInt32(action), new object[] { item } });

                            //change stateid to new stateid for sync component
                            var caseModel = _caseService.GetCaseById(item);
                            //int newStateId = _synchronizeService.SynchronizeCase(this, caseModel.CaseFormId, caseModel.StateId, stateid, caseModel.CaseFormUrl, item);
                            int newStateId = _workflowService.RunWorflowActionObject(this, caseModel.CaseFormId, caseModel.StateId, stateid, item, baseApiUrl);
                            result = await _caseService.CaseStateChangeById(item, newStateId, reason, useraccessid);
                            if (stateid != caseModel.StateId)
                                await _caseService.SaveCaseStateHistory(caseModel, useraccessid, "", caseModel.StateId, stateid);
                            toStates.Add(newStateId);
                            if (result == true)
                            {
                                //    int newautoState = _automationService.CaseAutomationSystem(this, caseModel.CaseFormId, caseModel.StateId, stateid, caseModel.CaseFormUrl, item);
                                //    if (newautoState!=0 && newautoState != stateid && newautoState!= caseModel.StateId)
                                //    {
                                //        result = await _caseService.CaseStateChangeById(item, newautoState, "Case Automation!!", useraccessid);
                                //    }
                                //    //int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
                                //    var stateForPermission = _queueService.GetStatePermissionsById(tenantid, stateid);



                                //    if (stateForPermission.NotifyUser == true)
                                //    {
                                //        //string message = _templateService.GetTemplateBodyByName("claim-email-notification");
                                //        //message = _templateService.GenerateTemplate(message, caseModel);

                                //        //if (message != null)
                                //        //{
                                //        //    //replace message details
                                //        //    //message = message.Replace("[user_name]", caseModel.FullName);

                                //        //    var messageNew = new TemplateViewModel { };
                                //        //    messageNew.Content = message;

                                //        //    string body = await razorToStringRender.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew);

                                //        //    string url = "wwwroot/uploads/";

                                //        //    List<string> imagesurl = new List<string>();

                                //        //    foreach (var itemImages in attachments)
                                //        //    {
                                //        //        imagesurl.Add(url + itemImages);
                                //        //    }

                                //        //    await emailSender.SendEmailAttachmentAsync(caseModel.Email, "Claim Filed", body, imagesurl);

                                //        //    _status.Show("success", "Email Sent Successfully.", false);


                                //        //}

                                //    }

                                if (_queueService.IsStateInRole(stateid) == false && page == null)
                                {
                                    return Json("state");
                                }
                            }
                        }
                    }
                }
                if (result)
                {
                    successCount++;
                }
            }

            if (state == "")
            {
                var b = toStates
                          .GroupBy(a => a)
                          .Select(g => new { g.Key, Count = g.Count(), Name = _queueService.GetStateNameByStateId(g.Key) });

                foreach (var item in b)
                {
                    state = state + item.Count + " case(s) moved to " + item.Name + ". ";
                }
            }

            if (state != "")
            {
                if (successCount == Ids.Count())
                {
                    _toastNotification.AddSuccessToastMessage(state);
                }
                else if (successCount > 0)
                {
                    _toastNotification.AddInfoToastMessage(state);
                }
                else
                {
                    _toastNotification.AddInfoToastMessage(state);
                }
            }

            if (!string.IsNullOrEmpty(page))
            {
                return Redirect(page);
                // return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/cases.html");
            }
            else
            {
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    _toastNotification.AddSuccessToastMessage("Case deleted.");
                }
                return Json("success");
            }

            //return Redirect(page);
        }

        /// <summary>
        /// View
        /// </summary>
        /// <returns></returns>
        /// To View Case Form 
        [Area("Admin")]
        [HttpGet]
        [Route("admin/formview/{encryptedid}/view1.html")]
        [Route("admin/formview/{tenant_identifier}/{form}/{encryptedid}/view1.html")]
        public IActionResult View1(string encryptedid, string tenant_identifier, string form)
        {
            CaseFrontViewModel cvm = new CaseFrontViewModel();

            string vp = "/Themes/" + this.Theme.GetName(false) + "/form/View.cshtml";
            try
            {
                int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

                string loggedinUser = _ICommonService.getLoggedInUserId();
                cvm.UserId = loggedinUser;
                bool isNew = false;
                if (cvm.Id == 0)
                {
                    isNew = true;
                    int case_id = _formBuilderService.GetBuilderFormByUrl(form, tenantid).Id;

                    if (_userService.UserHasPolicy() == "frontend")
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "front");
                    }
                    else
                    {
                        cvm.StateId = _queueService.GetFirstState(case_id, "back");
                    }

                }
                else
                {
                    cvm.StateId = _caseService.GetCaseById(cvm.Id).StateId;
                }


                int tenantId = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                cvm.TenantId = tenantId;
                //cvm =   _formService.SaveCaseAsync(cvm, form);
            }
            catch (Exception ex)
            {

            }

            return View(vp, cvm);
        }

        /// <summary>
        /// View
        /// </summary>
        /// <returns></returns>
        /// To View Case Form 
        [Area("Admin")]
        [HttpGet]
        [Route("admin/formview/{encryptedid}/view.html")]
        [Route("admin/formview/{tenant_identifier}/{form}/{encryptedid}/view.html")]
        [Route("admin/formview/{encryptedid}/{queue}/view.html")]
        public async Task<IActionResult> View(string encryptedid, string tenant_identifier, string form, string queue)
        {
            string vp = "/Themes/" + this.Theme.GetName(false) + "/form/View.cshtml";
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            string view = "";
            if (_userService.UserHasPolicy() == "frontend")
            {
                string Theme = "~/Themes/" + this.Theme.GetName(false) + "/Shared/_Layout.cshtml";
                ViewBag.ThemeView = Theme;
                view = "/Themes/" + this.Theme.GetName(false) + "/Form/Edit.cshtml";
            }
            else
            {
                vp = "/Areas/Admin/Views/Form/View.cshtml";
            }


            int Caseid = _utils.DecryptId(encryptedid);
            try
            {
                CaseViewModel cvm = new CaseViewModel();
                if (Caseid != 0)
                {
                    if (_userService.UserHasPolicy() == "frontend")
                    {
                        cvm.StateId = _queueService.GetFirstState(Caseid, "front");
                    }
                    else
                    {
                        cvm.StateId = _queueService.GetFirstState(Caseid, "back");
                    }


                    cvm = _caseService.GetCaseById(Caseid);

                    int caseformid = cvm.CaseFormId;

                    string roleid = _ICommonService.GetRoleIdByUserId(_ICommonService.getLoggedInUserId());

                    cvm.StateList = _queueService.GetStateSelectListByFormId(caseformid, roleid);

                    cvm.CaseTasks = _queueService.GetCaseMovement(caseformid, Caseid);

                    cvm.QueueId = _caseService.GetQueueIdByStateIdAndCaseFormId(cvm.StateId, cvm.CaseFormId);

                    var queueName = string.Empty;
                    var isSuperAdmin = await _ICommonService.IsSuperAdmin();
                    queueName = _caseService.GetQueueNameByQueueIdAndCaseFormId(cvm.QueueId, cvm.CaseFormId);

                    if (queueName != null)
                    {
                        //   item.DisplayPermission = true;
                        cvm.QueueName = queueName.ToString();
                    }

                    if (isSuperAdmin)
                    {
                        queueName = _queueService.GetQueueNameByFormId(caseformid, Convert.ToInt32(cvm.StateId));
                        cvm.QueueName = queueName;
                    }

                    var queueIcon = _caseService.GetQueueIconByQueueId(cvm.QueueId);
                    cvm.QueueIcon = queueIcon;

                    var queueColor = _caseService.GetQueueColorByQueueId(cvm.QueueId);
                    cvm.QueueColor = queueColor;

                    cvm.CaseFormName = _caseService.GetCaseFormTypeNameByCaseFormId(cvm.CaseFormId);

                    var temp = _formBuilderService.GetBuilderFormById(caseformid);
                    if (temp != null)
                    {
                        cvm.CaseFormUrl = temp.UrlIdentifier;
                    }

                    var ccvm = _formBuilderService.GetBuilderFormByUrl(temp.UrlIdentifier, tenantid);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
                    ViewBag.formData = _formService.GetTableData(Caseid, tables, a);
                    ViewBag.Queue = queue;
                }

                if (view != "")
                    return View(vp, cvm);
                return View(vp, cvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/country-list")]
        [Route("admin/{tenant_identifier}/country-list")]
        public JsonResult GetCountryList()
        {

            var caseList = _ICommonService.CountryList();

            return Json(caseList);
        }


        ///front End Setting 
        [Area("Admin")]
        [HttpGet]
        [Authorize]
        [Route("/admin/form/changecaseform.html")]
        public IActionResult changecaseform()
        {

            string Theme = "~/Themes/" + this.Theme.GetName(false) + "/Shared/_Layout.cshtml";
            //ViewBag.Count = tenantConfig.GetTenantConfigCount();
            //var dashboardData = _dashboardService.GetDashboard();
            //dashboardData.LastFourActivities = await activityLogService.GetActivityLogAsync();
            ViewBag.ThemeView = Theme;
            return View();
        }
        ///End Front End Setting
        ///
        /// Delete Case
        [Area("Admin")]
        [HttpGet]
        [Route("admin/form/{encryptedid}/{encryptedCaseid}/delete.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{encryptedCaseid}/delete.html")]
        public IActionResult Delete(string encryptedCaseid, string form)
        {
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            string view = "";
            if (_userService.UserHasPolicy() == "frontend")
            {
                string Theme = "~/Themes/" + this.Theme.GetName(false) + "/Shared/_Layout.cshtml";
                ViewBag.ThemeView = Theme;
                view = "/Themes/" + this.Theme.GetName(false) + "/Form/Edit.cshtml";
            }


            int Caseid = _utils.DecryptId(encryptedCaseid);
            try
            {
                CaseViewModel cvm = new CaseViewModel();
                if (Caseid != 0)
                {

                    cvm = _caseService.GetCaseById(Caseid);
                    int caseformid = cvm.CaseFormId;

                    var ccvm = _formBuilderService.GetBuilderFormById(caseformid);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    _formService.DeleteFromData(a, Caseid);
                    _caseService.DeleteCase(cvm);
                }
                return Redirect("~/user/userdashboard.html");
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Redirect("~/user/userdashboard.html");
            }

        }

        [Route("admin/getImageDetails.html")]
        public List<MediaViewModel> GetImageDetails(string values)
        {
            try
            {
                var mediaIdList = values.Split(",").ToList();
                var mediaDatas = mediaService.GetImagesByIds(mediaIdList.Select(int.Parse).ToList());
                return mediaDatas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// 
        [Route("user/uploadFiles.html")]
        [HttpPost]
        public async Task<List<MediaViewModel>> Index(IList<IFormFile> files)
        {
            try
            {
                var mvmList = new List<MediaViewModel>();
                foreach (IFormFile source in files)
                {
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                    filename = this.EnsureCorrectFilename(filename);

                    //using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    //    await source.CopyToAsync(output);
                    var extension = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
                    var parentId = 0;
                    var tenantId = 0;
                    tenantId = _tenantService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                    var imageExtensions = new List<string>(new string[] { "jpg", "jpeg", "png", "gif" });
                    if (imageExtensions.Contains(extension))
                    {
                        var mediaDatas = await mediaService.GetMediaGroup(tenantId);
                        parentId = mediaDatas.Where(x => x.Title == "Pictures").Select(x => x.Id).FirstOrDefault();
                    }
                    else
                    {
                        var mediaDatas = await mediaService.GetMediaGroup(tenantId);
                        parentId = mediaDatas.Where(x => x.Title == "Documents").Select(x => x.Id).FirstOrDefault();
                    }

                    mvmList.Add(new MediaViewModel
                    {
                        ParentId = parentId,
                        TenantId = tenantId,
                        Title = filename,
                        Description = filename,
                        Type = 2,
                        Url = this.GetPathAndFilename(filename),
                        UploadImage = source,
                        CreatedAt = DateTime.Now,
                        CreatedBy = _userService.getLoggedInUserId()
                    });


                }

                foreach (var item in mvmList)
                {
                    item.Id = await mediaService.CreateOrUpdate(item, true);
                }

                return mvmList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [Route("admin/uploadFiles.html")]
        [HttpPost]
        public async Task<List<MediaViewModel>> BackIndex(IList<IFormFile> files)
        {
            try
            {
                var mvmList = new List<MediaViewModel>();
                foreach (IFormFile source in files)
                {
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                    filename = this.EnsureCorrectFilename(filename);

                    //using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    //    await source.CopyToAsync(output);
                    var tenantId = 0;
                    tenantId = _tenantService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

                    mvmList.Add(new MediaViewModel
                    {
                        ParentId = 0,
                        TenantId = tenantId,
                        Title = filename,
                        Description = filename,
                        Type = 3,
                        Url = this.GetPathAndFilename(filename),
                        UploadImage = source,
                        CreatedAt = DateTime.Now,
                        CreatedBy = _userService.getLoggedInUserId()
                    });


                }

                foreach (var item in mvmList)
                {
                    item.Id = await mediaService.CreateOrUpdate(item, true);
                }

                return mvmList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [Route("admin/deleteFile.html")]
        [HttpPost]
        public async Task<bool> Delete(int id)
        {
            try
            {
                var result = await mediaService.DeleteById(id);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return this.hostingEnvironment.WebRootPath + "\\uploads\\" + filename;
        }
        [Area("Admin")]
        [HttpGet]
        [Route("admin/form/{encryptedid}/{encryptedCaseid}/payment.html")]
        [Route("admin/form/{tenant_identifier}/{form}/{encryptedCaseid}/payment.html")]
        public IActionResult PaymentPage(string encryptedCaseid, string form)
        {

            //Response.Cookies.Delete("cbtpx");
            //var cookieOptions = new CookieOptions
            //{
            //    // Set the secure flag, which Chrome's changes will require for SameSite none.
            //    // Note this will also require you to be running on HTTPS.
            //    Secure = true,

            //    // Set the cookie to HTTP only which is good practice unless you really do need
            //    // to access it client side in scripts.
            //    //HttpOnly = true,

            //    // Add the SameSite attribute, this will emit the attribute with a value of none.
            //    // To not emit the attribute at all set
            //     //SameSite = (SameSiteMode)(-1)
            //    SameSite = SameSiteMode.None,
            //    Domain= "https://webservices.securetrading.net/js/v2/st.js",
            //    IsEssential=true,
            //    HttpOnly=false
            //};
            //cookieOptions.SameSite = SameSiteMode.None;

            //// Add the cookie to the response cookie collection
            //Response.Cookies.Append("SimpleTransferCookie","none",cookieOptions);
            //cookieOptions.SameSite = SameSiteMode.None;
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            string view = "";
            if (_userService.UserHasPolicy() == "frontend" || _userService.UserHasPolicy() == null)
            {
                string Theme = "~/Themes/" + this.Theme.GetName(false) + "/Shared/_Layout.cshtml";
                ViewBag.ThemeView = Theme;
                view = "/Themes/" + this.Theme.GetName(false) + "/Form/PaymentPage.cshtml";
            }
            int Caseid = _utils.DecryptId(encryptedCaseid);
            try
            {
                CaseViewModel cvm = new CaseViewModel
                {
                    Id = Caseid,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CaseGeneratedId = _formService.GenerateFormId(),
                };
                var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
                int case_id = ccvm.Id;

                if (_userService.UserHasPolicy() == "frontend")
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "front");
                }
                else
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "back");
                }

                if (Caseid != 0)
                    cvm = _caseService.GetCaseById(Caseid);

                int caseformid = case_id;

                cvm.CaseFormName = _caseService.GetCaseFormTypeNameByCaseFormId(caseformid);

                string roleid = _ICommonService.GetRoleIdByUserId(_ICommonService.getLoggedInUserId());
                if (roleid == "")
                {
                    roleid = ApplicationConstants.CustomerRoleId;
                }

                cvm.StateList = _queueService.GetStateSelectListByFormId(caseformid, roleid);

                cvm.CaseTasks = _queueService.GetCaseMovement(caseformid, Caseid);
                if (cvm.UserId != null)
                {
                    var userName = _userService.GetUserById(cvm.UserId).Result;
                    cvm.UserFullName = userName.FirstName + " " + userName.LastName;
                    //cvm.CaseOwner = _queueService.GetCaseOwner(cvm.CaseFormId, cvm.StateId, cvm.UserId);
                }

                cvm.QueueName = _queueService.GetQueueNameByFormId(caseformid, Convert.ToInt32(cvm.StateId));
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                if (ccvm.Fields != null)
                {
                    cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
                    ViewBag.formData = _formService.GetTableData(Caseid, tables, a);
                }
                if (cvm.CaseFormId == 0)
                {
                    cvm.CaseFormId = caseformid;
                }
                if (view != "")
                    return View(view);
                return View(cvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }
    }
}