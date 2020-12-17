using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TemplateController : BaseController
    {

        private readonly ILogger<TemplateController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly ITemplateService templateService;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IToastNotification _toastNotification;
        private readonly IEmailGroupService _emailGroupService;
        public TemplateController(ITemplateService _templateService, IUserService _userService, ILogger<TemplateController> _Log, IEmailGroupService emailGroupService,
            IStatus _status, Utils _utils, ICommonService _commonService, IFormBuilderService _formBuilderService, IToastNotification toastNotification) : base(_userService)
        {
            templateService = _templateService;
            userService = _userService;
            Log = _Log;
            status = _status;
            utils = _utils;
            commonService = _commonService;
            formBuilderService = _formBuilderService;
            _toastNotification = toastNotification;
            _emailGroupService = emailGroupService;
        }

        [HttpGet]
        [Route("admin/templates.html")]
        [Route("admin/{tenant_identifier}/templates.html")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [Route("admin/templates.html")]
        [Route("admin/{tenant_identifier}/templates.html")]
        public JsonResult Index(DTPostModel model)
        {

            var template = templateService.GetTemplateListByFilter(model);
            return Json(new
            {
                draw = template.draw,
                recordsTotal = template.recordsTotal,
                recordsFiltered = template.recordsFiltered,
                data = template.data
            });
        }

        [HttpGet]
        [Route("admin/template/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/template/{encryptedid}/edit.html")]
        public IActionResult Edit(string encryptedid, string tenant_identifier)
        {
            int id = utils.DecryptId(encryptedid);
            try
            {

                int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

                TemplateViewModel tvm = new TemplateViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    Version = 1
                };
                if (id != 0)
                {
                    tvm = templateService.GetTemplateById(id);
                    return View(tvm);
                }

                //status.Show("error", "Invaild Id", false);
                return View(tvm);

            }
            catch (Exception ex)
            {
                Log.LogError("TemplateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [HttpPost]
        [Route("admin/template/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/template/{encryptedid}/edit.html")]
        public async Task<IActionResult> Edit(TemplateViewModel tvm, string tenant_identifier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tvm.TenantId = commonService.GetTenantIdByIdentifier(tenant_identifier);
                    if (tvm.TenantId != 0)
                    {
                        tvm.Version = tvm.Version + 1;
                        tvm = await templateService.UpdateTemplate(tvm);
                        if (tvm != null)
                        {
                            _toastNotification.AddSuccessToastMessage("Template is saved.");
                            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/template/" + utils.EncryptId(tvm.Id) + "/edit.html");
                        }
                        _toastNotification.AddErrorToastMessage("Invaild Template.");
                        return View(tvm);
                    }
                    _toastNotification.AddWarningToastMessage("Please choose a Tenant.");
                }
                utils.addModelError(ModelState);

                return View(tvm);
            }
            catch (Exception ex)
            {
                Log.LogError("TemplateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(tvm);
            }
        }

        [HttpPost]
        [Route("admin/{tenent_identifier}/template/load_fields")]
        public JsonResult GetTemplateFields(int type, int formid = 0)
        {
            JObject result = new JObject();
            try
            {
                result = templateService.GetFieldsByTemplate(type, formid);
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/template/loadTablesForFields")]
        [Route("admin/template/loadTablesForFields")]
        public JsonResult LoadTablesForField(bool isForm, int formId)
        {
            List<string> tables = templateService.GetTablesForMatch(isForm, formId);
            return Json(tables);
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/template/loadFields")]
        [Route("admin/template/loadFields")]
        public JsonResult LoadTableFields(string table, bool isForm, int formId)
        {
            List<string> fields = templateService.GetFieldsForMatch(table, isForm, formId);
            return Json(fields);
        }
        [HttpPost]
        [Route("admin/template/checkfieldname")]
        public JsonResult CheckFieldName(string Field,int Id)
        {
            bool check = templateService.CheckFieldName(Field,Id);
            return Json(check);
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/template/load_template_options")]
        public JsonResult GetActiveTenantForms(string tenant_identifier)
        {

            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

            if (!string.IsNullOrWhiteSpace(tenant_identifier))
            {

                var caseList = formBuilderService.GetActiveTenantForms(tenantid);

                List<DynamicFormViewModel> dfvm = new List<DynamicFormViewModel>();

                foreach (var caseitem in caseList)
                {
                    JObject obj = JObject.Parse(caseitem.Fields);
                    DynamicFormViewModel dm = obj.ToObject<DynamicFormViewModel>();
                    dfvm.Add(dm);
                }

                List<string> labels = new List<string>();

                foreach (var listitem in dfvm)
                {
                    if (listitem.Tabs != null)
                    {
                        foreach (var tabitem in listitem.Tabs)
                        {
                            foreach (var elementitem in tabitem.element)
                            {
                                elementitem.label = "[" + elementitem.label.Replace("?", "");

                                elementitem.label = elementitem.label.Replace(" ", "_") + "]";

                                labels.Add(elementitem.label);
                            }
                        }
                    }

                }

                return Json(labels);
            }

            return Json(null);
        }

        [HttpPost]
        [Route("admin/template/updatefield")]
        public JsonResult UpdateField(IFormCollection data)
        {
            int tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());


            MailMergeFieldViewModel mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                Id = Convert.ToInt32(data["Id"]),
                FieldName = Convert.ToString(data["FieldName"]),
                Alias = "None",
                DbSourceTable = Convert.ToString(data["DbSourceTable"]),
                DbSourceField = Convert.ToString(data["DbSourceField"]),
                TemplateType = Convert.ToInt32(data["TemplateType"]),
                TenantId = TenantId,
                FormId = Convert.ToInt32(data["FormId"]),
                isDeleted = false
            };
            string alias = "[" + mailMergeFieldViewModel.FieldName.Replace("?", "");
            alias = alias.Replace(" ", "_") + "]";
            mailMergeFieldViewModel.Alias = alias;
            return Json(templateService.SaveOrUpdateField(mailMergeFieldViewModel));
        }
        [HttpPost]
        [Route("admin/template/deleteField")]
        public JsonResult DeleteField(int fieldId)
        {
            return Json(templateService.DeleteMailField(fieldId));
        }

        [HttpPost]
        [Route("admin/template/createmailobject")]
        public JsonResult CreateMailObject(string eTitle, string eId, int formId)
        {
            int tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            MailMergeObjectViewModel mailMergeObjectViewModel = new MailMergeObjectViewModel()
            {
                Title = Convert.ToString(eTitle),
                TemplateId = Convert.ToInt32(eId),
                FormId = Convert.ToInt32(formId),
                TenantId = tenantId,
                CreatedDate = DateTime.Now
            };
            mailMergeObjectViewModel = templateService.SaveOrUpdateMailObject(mailMergeObjectViewModel);
            JObject result = new JObject();
            result.Add("id", mailMergeObjectViewModel.Id);
            result.Add("title", mailMergeObjectViewModel.Title);
            return Json(result);

        }

        [HttpGet]
        [Route("admin/template/getMailObjectById")]
        public IActionResult GetMailObjectById(int id)
        {
            MailMergeObjectViewModel mailMergeObjectViewModel = new MailMergeObjectViewModel();
            try
            {
                mailMergeObjectViewModel = templateService.GetMailObjectById(id);
                return PartialView("SelectedTemplateData", mailMergeObjectViewModel);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        [Route("admin/template/setmailobjectstatus")]
        public JsonResult SetMailObjectStatus(int objectId, bool status)
        {
            return Json(templateService.SetMailObjectStatus(objectId, status));
        }

        [HttpGet]
        [Route("admin/{tenant_identifier}/template/loademailtemplates")]
        [Route("admin/template/loademailtemplates")]
        public IActionResult LoadEmailTemplates(string removeObj,int formId)
        {

            List<SelectListItem> items = templateService.GetTemplateListForWorkflow(formId);
            if (removeObj != null || removeObj != "null")
            {
                var objects = removeObj.Split(",");
                foreach (string id in objects)
                {
                    foreach (SelectListItem item in items)
                        if (item.Value == id)
                        {
                            items.Remove(item);
                            break;
                        }
                }
            }

            return PartialView("_TemplateModal", items);
        }

        [HttpGet]
        [Route("admin/template/getField")]
        public JsonResult GetField(int fieldId)
        {
            return Json(templateService.GetFieldById(fieldId));
        }

        [HttpGet]
        [Route("admin/template/getgeneralemailsetting")]
        public JsonResult GetGeneralEmailSetting()
        {
            return Json(templateService.GetEmailGeneralSetting());
        }
        [HttpPost]
        [Route("admin/template/setgeneralemailSetting")]
        public JsonResult SetGeneralEmailSetting(string data)
        {
            JObject jObject = (JObject)JsonConvert.DeserializeObject(data);
            bool success=templateService.updateGeneralEmailSetting(jObject);
            return Json(success);
        }


        [Area("Admin")]
        [HttpPost]
        [Route("admin/template/action.html")]
        [Route("admin/{tenant_identifier}/template/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var status = "";
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/templates.html");
            }
            if (string.IsNullOrEmpty(Ids.ToString()) || Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one template.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/templates.html");
            }
            int successCount = 0;
            foreach (int item in Ids)
            {
                bool result = false;
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    status = ButtonAction.delete.ToDescription();
                    result = await templateService.DeleteTemplateById(item);

                }
               
                if (result)
                {
                    successCount++;
                }
            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " template(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " template(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " template(s) " + status +" Please make sure selected template is not used anywhere.");
            }
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/templates.html");
        }

        [HttpGet]
        [Route("admin/template/getRoles")]
        public JsonResult GetRoles()
        {
            return Json(commonService.GetBackOfficeRoleList());
        }

        [HttpGet]
        [Route("admin/template/getJsonEmailGroups")]
        public JsonResult GetJsonEmailGroups()
        {
            return Json(_emailGroupService.GetAllEmailGroupForTenant());
        }
    }
}