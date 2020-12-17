using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using static Cicero.Service.Enums;
using static Cicero.Service.Models.Core.FormBuilderViewModel;
using static Cicero.Service.Models.Core.FormBuilderViewModel.Form;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FormBuilderController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly Utils _utils;
        private readonly ILogger<FormBuilderController> _log;
        private readonly IStatus _status;
        private readonly ITenantService _tenantService;
        private readonly AppSetting _appSetting;
        private readonly IToastNotification _toastNotification;

        public ApplicationDbContext db = null;

        public FormBuilderController(ICommonService ICs, IUserService userService, AppSetting appSetting, 
            IFormBuilderService formBuilderService, Utils utils, ILogger<FormBuilderController> log, 
            IStatus status, ITenantService tenantService, ApplicationDbContext _db,
            IToastNotification toastNotification) : base(userService)
        {
            _userService = userService;
            _formBuilderService = formBuilderService;
            _utils = utils;
            _log = log;
            _status = status;
            _tenantService = tenantService;
            _appSetting = appSetting;
            _commonService = ICs;
            db = _db;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [Route("admin/builderforms.html")]
        [Route("admin/{tenant_identifier}/builderforms.html")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/builderforms.html")]
        [Route("admin/{tenant_identifier}/builderforms.html")]
        public JsonResult Index(DTPostModel model)
        {
            var caseForm = _formBuilderService.GetBuilderFormListByFilter(model);
            return Json(new
            {
                draw = caseForm.draw,
                recordsTotal = caseForm.recordsTotal,
                recordsFiltered = caseForm.recordsFiltered,
                data = caseForm.data
            });
        }

        [HttpGet]
        [Route("admin/builderform/{encryptedid}/edit2.html")]
        [Route("admin/{tenant_identifier}/builderform/{encryptedid}/edit2.html")]
        public IActionResult Edit2(string encryptedid)
        {
            int id = _utils.DecryptId(encryptedid);
            try
            {
                CaseFormViewModel ccvm = new CaseFormViewModel
                {
                    Id = id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                }
                return View(ccvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [HttpPost]
        [Route("admin/builderform/{encryptedid}/edit2.html")]
        [Route("admin/{tenant_identifier}/builderform/{encryptedid}/edit2.html")]
        public async Task<IActionResult> Edit2(CaseFormViewModel ccvm, string tenant_identifier)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    ccvm.TenantId = _tenantService.GetTenantIdByIdentifier(tenant_identifier);
                    ccvm.UpdatedAt = DateTime.Now;
                    if (ccvm.TenantId != 0)
                    {
                        ccvm = await _formBuilderService.CreateOrUpdate(ccvm);
                        _toastNotification.AddSuccessToastMessage("Formbuilder is saved successfully.");
                        return Redirect("~/admin" + _utils.GetTenantForUrl(false) + "/builderform/" + _utils.EncryptId(ccvm.Id) + "/edit.html");
                    }
                    _toastNotification.AddWarningToastMessage("Please choose a Tenant.");
                }
                _utils.addModelError(ModelState);
                return View(ccvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(ccvm);
            }
        }
        [HttpGet]
        [Route("admin/builderform/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/builderform/{encryptedid}/edit.html")]
        public async Task<IActionResult> Edit(string encryptedid)
        {
            int id = _utils.DecryptId(encryptedid);
            
            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {
                FormBuilderViewModel fvm1 = new FormBuilderViewModel(); 
                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    fvm1 = (FormBuilderViewModel)ccvm.FormBuilder;
                }
                else
                {
                    Cicero.Service.Models.Core.Elements.Tab tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                    tab.ElementId = Utils.GenerateId();
                    tab.ElementIndex = "0";
                    tab.Type = tab.GetType().FullName;
                    tab.Row.Clear();
                    FormBuilderViewModel fvm = new FormBuilderViewModel();
                    Form frm = new Form();
                    Navigation nv = new Navigation();
                    frm.Navigations = nv;
                    frm.ElementId = Utils.GenerateId();
                    fvm.Forms = frm;
                    fvm.Tab.Add(tab);
                    CaseFormViewModel Newccvm = new CaseFormViewModel
                    {
                        Id = id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    Newccvm.FormBuilder = fvm;
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    string jsonsstr = JsonConvert.SerializeObject(fvm, settings).Trim();
                    Newccvm.Name = "UnTitled";
                    Newccvm.TenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                    Newccvm.UserId = _commonService.getLoggedInUserId();
                    Newccvm.Fields = jsonsstr;
                   // ccvm = await _formBuilderService.CreateOrUpdate(Newccvm);
                    ccvm = Newccvm;
                    fvm1 = fvm;
                    //ccvm.FormBuilder =

                    
                }
                if (fvm1.Forms.Tables == null || fvm1.Forms.Tables.Count < 1)
                {
                    _toastNotification.AddWarningToastMessage("No table are created in Database. Please create required Tables.");
                }
                return View(ccvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(ccvm);
            }

        }
        [HttpGet]
        [Route("admin/builderform/getcreatedtable.html")]
        public IActionResult getCreatedTable(int encryptedid)
        {
            int id = encryptedid;

            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {

                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Table> table = a.Forms.Tables;
                    return Json(table);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("");
            }

        }

        [HttpGet]
        [Route("admin/builderform/gettablefields.html")]
        public IActionResult gettablefields(int encryptedid, string tablename)
        {
            int id = encryptedid;

            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {

                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Field> fields = a.Forms.Tables.Where(x => x.Name == tablename).SingleOrDefault().Fields.ToList();
                    return Json(fields);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("");
            }

        }
        public string la(Match match)
        {


            Regex regex = new Regex(@"(?<=\bElementType"":"")[^""]*");
            Match type = regex.Match(match.Value);
            string title = type.Value;
            var regex1 = new Regex(@"Themes\.Element,");
            string text = regex1.Replace(match.Value, "Cicero.Service.Models.Core.Elements." + title + ",");
            return text;
        }
        [HttpPost]
        [Route("admin/builderform/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/builderform/{encryptedid}/edit.html")]
        public async Task<IActionResult> Edit(CaseFormViewModel ccvm, FormBuilderViewModel tb, string tenant_identifier, IFormCollection ifs)
        {
          
            //TryUpdateModelAsync(Tabs)
            try
            {
                if (ModelState.IsValid)
                {
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel fbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    if(ccvm.Id==0)
                    {
                        //Cicero.Service.Models.Core.Elements.Tab tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                        //tab.ElementId = Utils.GenerateId();
                        //tab.ElementIndex = "0";
                        //tab.Type = tab.GetType().FullName;
                        //tab.Row.Clear();
                        //FormBuilderViewModel fvm = new FormBuilderViewModel();
                        //Form frm = new Form();
                        //Navigation nv = new Navigation();
                        //frm.Navigations = nv;
                        //frm.ElementId = Utils.GenerateId();
                        //fvm.Forms = frm;
                        //fvm.Tab.Add(tab);
                        CaseFormViewModel Newccvm = new CaseFormViewModel
                        {
                            Id = ccvm.Id,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        Newccvm.FormBuilder = ccvm.FormBuilder; 
                        Newccvm.Name = ccvm.Name;
                        Newccvm.TenantId = ccvm.TenantId;
                        Newccvm.UserId = ccvm.UserId;
                        Newccvm.Fields = ccvm.Fields;
                        Newccvm.ModelName = fbvm.Forms.Navigations.Name;
                        Newccvm.Status = ccvm.Status;
                        Newccvm.Icon = fbvm.Forms.Navigations.Icon;
                        Newccvm.ModelTitle = fbvm.Forms.Navigations.Title;
                        Newccvm.UrlIdentifier = fbvm.Forms.Navigations.Identifier;
                        Newccvm = await _formBuilderService.CreateOrUpdate(Newccvm);
                        return Redirect("~/admin" + _utils.GetTenantForUrl(false) + "/builderform/" + _utils.EncryptId(Newccvm.Id) + "/edit.html");
                    }
                    else
                    {
                        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        var OLDccvm = db.CaseForm.Where(x => x.Id == ccvm.Id).FirstOrDefault();

                        var oldfields = OLDccvm.Fields;


                        ccvm.TenantId = _tenantService.GetTenantIdByIdentifier(tenant_identifier);

                        if (ccvm.TenantId != 0)
                        {
                            ccvm.Fields = oldfields;
                            ccvm.ModelName = fbvm.Forms.Navigations.Name;
                            ccvm.Icon = fbvm.Forms.Navigations.Icon;
                            ccvm.ModelTitle = fbvm.Forms.Navigations.Title;
                            ccvm.UrlIdentifier = fbvm.Forms.Navigations.Identifier;
                            ccvm = await _formBuilderService.CreateOrUpdate(ccvm);
                            _toastNotification.AddSuccessToastMessage("Form for Case is saved.");
                            return Redirect("~/admin" + _utils.GetTenantForUrl(false) + "/builderform/" + _utils.EncryptId(ccvm.Id) + "/edit.html");
                        }
                    }
                    _toastNotification.AddWarningToastMessage("Please select a Tenant.");
                }
                _utils.addModelError(ModelState);

                return View(ccvm);

            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }
        }

        [HttpPost]
        [Route("admin/builderform/action.html")]
        [Route("admin/{tenant_identifier}/builderform/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page, string tenant_identifier = "")
        {
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
                _toastNotification.AddWarningToastMessage("Please select atleast one item.");
                return Redirect(page);
            }
            int successCount = 0;
            string state = "";
            foreach (var item in Ids)
            {
                bool result = false;

                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await _formBuilderService.DeleteBuilderFormById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await _formBuilderService.ActiveBuilderFormById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription(); 
                        result = await _formBuilderService.InactiveBuilderFormById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " bulderform(s) " + state+".");
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " bulderform(s) " + state + ".");
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " bulderform(s) " + state + ".");
            }
            if (string.IsNullOrEmpty(page))
            {
                return Redirect("~/admin" + _utils.GetTenantForUrl(false) + "/builderforms.html");
            }
            return Redirect(page);
        }


        [HttpPost]
        [Route("admin/builderform/createAction.html")]
        [Route("admin/{tenant_identifier}/builderform/createAction.html")]
        public async Task<IActionResult> CreateNew(string formName, string tenant_identifier,string formIcon)
        {

            try
            {
                CaseFormViewModel ccvm = new CaseFormViewModel();
                if (ModelState.IsValid)
                {
                    ccvm.TenantId = _tenantService.GetTenantIdByIdentifier(tenant_identifier);
                    ccvm.Name = formName;
                    Cicero.Service.Models.Core.Elements.Tab tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                    tab.ElementId = Utils.GenerateId();
                    tab.ElementIndex = "0";
                    tab.Type = tab.GetType().FullName;
                    tab.Row.Clear();
                    FormBuilderViewModel fvm = new FormBuilderViewModel();
                    Form frm = new Form();
                    Navigation nv = new Navigation();
                    nv.Icon = formIcon;
                    nv.Name = formName;
                    frm.Navigations = nv;
                    frm.ElementId = Utils.GenerateId();
                    fvm.Forms = frm;
                    fvm.Tab.Add(tab);
                    CaseFormViewModel Newccvm = new CaseFormViewModel
                    {
                        Id = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    Newccvm.FormBuilder = fvm;
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    string jsonsstr = JsonConvert.SerializeObject(fvm, settings).Trim();
                    Newccvm.Name = formName;
                    Newccvm.ModelName = formName;
                    Newccvm.TenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                    Newccvm.UserId = _commonService.getLoggedInUserId();
                    Newccvm.Fields = jsonsstr;
                    Newccvm.Icon = formIcon;
                    
                    if (ccvm.TenantId != 0)
                    {
                        ccvm = await _formBuilderService.CreateOrUpdate(Newccvm);
                       // ccvm = Newccvm;
                        return Json(_utils.EncryptId(ccvm.Id));
                    }
                }

                return Json(0);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(0);
            }
        }

        ///From builder FrontEnd
        ///
        [HttpGet]
        [Route("admin/case/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/case/{encryptedid}/edit.html")]
        public IActionResult Edit(string encryptedid, string tenant_identifier)
        {
            int id = _utils.DecryptId(encryptedid);

            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {

                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                }
                else
                {
                    Cicero.Service.Models.Core.Elements.Tab tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                    tab.ElementId = Utils.GenerateId();
                    tab.ElementIndex = "0";
                    tab.Type = tab.GetType().FullName;
                    tab.Row.Clear();
                    FormBuilderViewModel fvm = new FormBuilderViewModel();
                    Form frm = new Form();
                    Navigation nv = new Navigation();
                    frm.Navigations = nv;
                    frm.ElementId = Utils.GenerateId();
                    fvm.Forms = frm;
                    fvm.Tab.Add(tab);
                    CaseFormViewModel Newccvm = new CaseFormViewModel
                    {
                        Id = id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    Newccvm.FormBuilder = fvm;
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    string jsonsstr = JsonConvert.SerializeObject(fvm, settings).Trim();
                    Newccvm.Name = "UnTitled";
                    Newccvm.TenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                    Newccvm.UserId = _commonService.getLoggedInUserId();
                    Newccvm.Fields = jsonsstr;
                  //  ccvm = await _formBuilderService.CreateOrUpdate(Newccvm);
                    ccvm = Newccvm;
                    //ccvm.FormBuilder =
                }
                return View(ccvm);
            }
            catch (Exception ex)
            {
                _log.LogError("CaseFormService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(ccvm);
            }

        }

        //[HttpPost]
        //[Route("admin/case/{encryptedid}/edit.html")]
        //[Route("admin/{tenant_identifier}/case/{encryptedid}/edit.html")]
        //public async Task<IActionResult> Edit(CaseViewModel cvm, string tenant_identifier, IFormCollection data)
        //{
        //    try
        //    {
        //        int tenantId = commonService.GetTenantIdByIdentifier(tenant_identifier);

        //        if (ModelState.IsValid)
        //        {

        //            var formSchema = caseFormService.GetCaseFormById(cvm.CaseFormId);
        //            if (formSchema != null)
        //            {
        //                JObject obj = JObject.Parse(formSchema.Fields);
        //                DynamicFormViewModel dfvm = obj.ToObject<DynamicFormViewModel>();

        //                List<string> message = new List<string>();
        //                string msg = string.Empty;

        //                List<string> tempSelector = null;
        //                bool tempTask = false;

        //                foreach (var tab in dfvm.Tabs)
        //                {
        //                    foreach (var element in tab.element)
        //                    {
        //                        if (tempSelector != null)
        //                        {
        //                            //string[] selectedarray = Array.ConvertAll(tempSelector.ToString().Split(','), Convert.ToString);

        //                            foreach (var selecteditem in tempSelector)
        //                            {
        //                                if (element.name == selecteditem)
        //                                {
        //                                    element.required = tempTask;
        //                                }
        //                            }

        //                        }

        //                        foreach (var item in data)
        //                        {
        //                            if (element.name == item.Key || element.name + "[]" == item.Key)
        //                            {
        //                                if (element.type == "select" || element.type == "targetForm" || element.type == "radio-group" || element.type == "checkbox-group")
        //                                {
        //                                    if (element.values != null)
        //                                    {
        //                                        foreach (var Values in element.values)
        //                                        {

        //                                            if (element.multiple == true)
        //                                            {
        //                                                string[] value = Array.ConvertAll(item.Value.ToString().Split(','), Convert.ToString);

        //                                                foreach (var multipleChoice in value)
        //                                                {
        //                                                    if (Values.value == multipleChoice)
        //                                                    {
        //                                                        Values.selected = true;
        //                                                        break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        Values.selected = false;
        //                                                    }
        //                                                }

        //                                            }
        //                                            else
        //                                            {

        //                                                //previous version of code
        //                                                //if (element.type == "select" && element.className.Contains("country") && !element.values.Any(x => x.value == item.Value))
        //                                                //{

        //                                                //    string NewLabel = "";
        //                                                //    if (!string.IsNullOrEmpty(item.Value))
        //                                                //    {
        //                                                //        NewLabel = item.Value.ToString().First().ToString().ToUpper() + item.Value.ToString().Substring(1);
        //                                                //    }

        //                                                //    Values.label = NewLabel;
        //                                                //    Values.value = item.Value;

        //                                                //}

        //                                                //if (Values.value == item.Value)
        //                                                //{
        //                                                //    Values.selected = true;

        //                                                //    if (Values.mapElement != null)
        //                                                //    {

        //                                                //        //if (item.Value == itemactions.value)
        //                                                //        //{
        //                                                //        tempSelector = Values.mapElement;
        //                                                //        tempTask = Values.showHide;
        //                                                //        //}

        //                                                //    }
        //                                                //}
        //                                                //else
        //                                                //{
        //                                                //    Values.selected = false;
        //                                                //}

        //                                                string[] value = Array.ConvertAll(item.Value.ToString().Split(','), Convert.ToString);

        //                                                if (element.type == "select" && element.className.Contains("country") && !element.values.Any(x => x.value == item.Value))
        //                                                {

        //                                                    string NewLabel = "";
        //                                                    if (value.Count() > 1)
        //                                                    {
        //                                                        NewLabel = item.Value.ToString().First().ToString().ToUpper() + item.Value.ToString().Substring(1);
        //                                                        Values.value = value[0];
        //                                                        element.value = item.Value;
        //                                                    }
        //                                                    else if (!string.IsNullOrEmpty(item.Value))
        //                                                    {
        //                                                        NewLabel = item.Value.ToString().First().ToString().ToUpper() + item.Value.ToString().Substring(1);
        //                                                        Values.value = item.Value;
        //                                                    }

        //                                                    Values.label = NewLabel;

        //                                                }

        //                                                if (value.Count() > 1)
        //                                                {
        //                                                    if (Values.value == value[0])
        //                                                    {
        //                                                        Values.selected = true;

        //                                                        if (Values.mapElement != null)
        //                                                        {

        //                                                            //if (item.Value == itemactions.value)
        //                                                            //{
        //                                                            tempSelector = Values.mapElement;
        //                                                            tempTask = Values.showHide;
        //                                                            //}

        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        Values.selected = false;
        //                                                    }
        //                                                }
        //                                                else
        //                                                {
        //                                                    if (Values.value == item.Value)
        //                                                    {
        //                                                        Values.selected = true;

        //                                                        if (Values.mapElement != null)
        //                                                        {

        //                                                            //if (item.Value == itemactions.value)
        //                                                            //{
        //                                                            tempSelector = Values.mapElement;
        //                                                            tempTask = Values.showHide;
        //                                                            //}

        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        Values.selected = false;
        //                                                    }
        //                                                }
        //                                            }

        //                                        }

        //                                    }
        //                                    else if (element.targetformdata != null)
        //                                    {
        //                                        foreach (var multipletargetdata in element.targetformdata)
        //                                        {
        //                                            string[] value = Array.ConvertAll(item.Value.ToString().Split(','), Convert.ToString);

        //                                            if (value != null)
        //                                            {
        //                                                foreach (var multipleChoice in value)
        //                                                {
        //                                                    if (multipletargetdata.name == multipleChoice)
        //                                                    {
        //                                                        multipletargetdata.selected = true;
        //                                                        break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        multipletargetdata.selected = false;
        //                                                    }
        //                                                }
        //                                            }

        //                                            if (multipletargetdata.childrens != null)
        //                                            {
        //                                                foreach (var multipleValue in multipletargetdata.childrens)
        //                                                {
        //                                                    foreach (var multipleChoice in value)
        //                                                    {
        //                                                        if (multipleValue.list == multipleChoice)
        //                                                        {
        //                                                            multipleValue.selected = true;
        //                                                            break;
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            multipleValue.selected = false;
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }

        //                                        }
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    element.value = item.Value;
        //                                    if (element.type == "number")
        //                                    {
        //                                        if (item.Value.Count() > 1)
        //                                        {

        //                                            element.values = new List<Value>();
        //                                            foreach (var numitem in item.Value)
        //                                            {
        //                                                element.values.Add(new Value { value = numitem, mapElement = new List<string>() });

        //                                            }
        //                                        }

        //                                    }
        //                                }

        //                            }
        //                        }
        //                        if (element.show_in_back == true)
        //                        {
        //                            msg = ICaseService.ValidateFormElement(element);
        //                            if (!string.IsNullOrEmpty(msg))
        //                            {
        //                                Status.Show("error", msg, false);
        //                                message.Add(msg);
        //                            }
        //                        }

        //                    }
        //                }

        //                var fieldObject = JsonConvert.SerializeObject(dfvm);

        //                cvm.Extras = fieldObject;

        //                if (message.Count < 1)
        //                {
        //                    cvm.TenantId = tenantId;
        //                    if (cvm.TenantId != 0)
        //                    {
        //                        cvm.Version = cvm.Version + 1;
        //                        //cvm.StateName = appSetting.Get("app_claim_back");
        //                        cvm = await ICaseService.CreateOrUpdate(cvm);

        //                        Status.Show("success", "Case is succesfully saved", false);
        //                        return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/case/" + utils.EncryptId(cvm.Id) + "/edit.html");
        //                    }
        //                    else
        //                    {
        //                        Status.Show("error", "Please choose a Tenant", false);
        //                    }
        //                }
        //            }

        //        }
        //        utils.addModelError(ModelState);

        //        cvm.StateId = queueService.GetStateByName(tenantId, appSetting.Get("app_claim_back"));
        //        cvm.CountryList = ICaseService.CountryListLoad();
        //        cvm.QueueList = queueService.GetQueueListForClaim();
        //        cvm.StateList = queueService.GetStateSelectList();
        //        cvm.QueueName = queueService.GetQueueNameByStateId(cvm.StateId);
        //        return View(cvm);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.LogError("CaseServices:Edit - " + ex.ToString());
        //        Status.Show("error", ex.ToString(), false);
        //        return View(cvm);
        //    }
        //}


        [HttpGet]
        [Route("admin/builderform/checkname")]
        [Route("admin/{tenant_identifier}/builderform/checkname")]
        public IActionResult CheckUniqueElementName(string elementName,int formId, string elementId)
        {
            return Json(_formBuilderService.IsUniqueueElementName(elementName,formId,elementId));
        }
        ///End FrontEnd
        ///
    }
}