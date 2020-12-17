using Cicero.Service.Services;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using System.Collections.Generic;
using Cicero.Service.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Service.Models.Core;
using Cicero.Service.Library.Toastr;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CaseSyncController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;
        private readonly IStatus status;
        private readonly ITenantService tenantService;
        private readonly Utils utils;
        private readonly IMapper mapper;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly ISynchronizeService synchronizeService;
        private readonly IQueueService queueService;
        private readonly IToastNotification _toastNotification;
        public CaseSyncController(ApplicationDbContext adb, ISynchronizeService _synchronizeService,
            IStatus _status, ICommonService _commonService, IQueueService _queueService, IUserService _userService,
            ITenantService _tenantService, Utils _utils, IMapper _mapper, IFormBuilderService _formBuilderService,
            IToastNotification toastNotification) : base(_userService)
        {
            db = adb;
            status = _status;
            userService = _userService;
            tenantService = _tenantService;
            utils = _utils;
            mapper = _mapper;
            formBuilderService = _formBuilderService;
            commonService = _commonService;
            queueService = _queueService;
            synchronizeService = _synchronizeService;
            _toastNotification = toastNotification;
        }

        [Route("admin/case-sync.html")]
        [Route("admin/{tenant_identifier}/case-sync.html")]
        public ActionResult Index()
        {
            var id = tenantService.GetTenantIdByIdentifier(GetTenantIdentifier());
            if (id == 0)
            {
                return Redirect("~/admin/" + utils.GetTenantForUrl(true) + "case-sync.html");
            }
            // setting = mapper.Map<List<SettingViewModel>>(db.Setting.Where(x => x.TenantId == id).ToList());
            var casesetting = db.Setting.Where(x => x.TenantId == id && x.FieldKey == "app_case_synchronization").FirstOrDefault()?.FieldValue;
            SyncSettingViewModel ssvm = JsonConvert.DeserializeObject<SyncSettingViewModel>(casesetting);
            if (ssvm != null)
            {
                return View(ssvm);
            }
            var defaultValues = new SyncSettingViewModel();
            //defaultValues.configs = new List<Configs>();
            defaultValues.configs.Add(new Configs() { });
            return View(defaultValues);
        }

        [HttpPost]
        [Route("admin/case-sync.html")]
        [Route("admin/{tenant_identifier}/case-sync.html")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SyncSettingViewModel ssvm)
        {
            var id = tenantService.GetTenantIdByIdentifier(GetTenantIdentifier());
            if (id == 0)
            {
                _toastNotification.AddErrorToastMessage("Please choose a Tenant before save.");
                return Redirect("~/admin/" + utils.GetTenantForUrl(true) + "case-sync.html");
            }
            using (db)
            {
                foreach (var item in ssvm.configs)
                {

                    if (item.destinationfield[item.sourcefield.Count() - 1] == item.sourcefield[item.sourcefield.Count() - 1] && item.sourcefield[item.sourcefield.Count() - 1] == null)
                    {
                        item.destinationfield.RemoveAt(item.sourcefield.Count() - 1);
                        item.sourcefield.RemoveAt(item.sourcefield.Count() - 1);
                    }

                }

                var obj = JsonConvert.SerializeObject(ssvm);



                var _SettingModel = db.Setting.Where(x => x.FieldKey.Equals("app_case_synchronization", StringComparison.OrdinalIgnoreCase) && x.TenantId == id).SingleOrDefault();
                if (_SettingModel != null)
                {
                    _SettingModel.FieldValue = obj.ToString();
                    db.SaveChanges();

                }

            }
            _toastNotification.AddSuccessToastMessage("Settings are saved.");
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/case-sync.html");
        }

        [HttpGet]
        [Route("admin/form-list-source")]
        [Route("admin/{tenant_identifier}/form-list-source")]
        public JsonResult GetFormListSource(string selected_tenant_id = null)
        {

            var caseList = formBuilderService.FormList(selected_tenant_id);

            return Json(caseList);
        }




        [HttpGet]
        [Route("admin/form-list-destination")]
        [Route("admin/{tenant_identifier}/form-list-destination")]
        public JsonResult GetFormListDestination(string tenant_identifier = null)
        {
            //string tenant_ide
            var caseList = formBuilderService.FormList(tenant_identifier);
            return Json(caseList);
        }

        [HttpGet]
        [Route("admin/form-fields")]
        [Route("admin/{tenant_identifier}/form-fields")]
        public JsonResult GetFormFields(int caseformid)
        {
            //var caseList = caseFormService.GetCaseFormById(caseformid);
            var caseList = formBuilderService.GetFormFields(caseformid);
            if (caseList != null)
            {
                return Json(caseList);
            }

            return Json(null);
        }


        [HttpGet]
        [Route("admin/sync-source-table-all")]
        [Route("admin/{tenant_identifier}/sync-source-table-all")]
        public JsonResult GetSyncSourceTableAll(int formId)
        {
            return Json(synchronizeService.GetSyncAllTables(formId));
        }
        [HttpGet]
        [Route("admin/sync-source-table-column-all")]
        [Route("admin/{tenant_identifier}/sync-source-table-column-all")]
        public JsonResult GetSyncSourceTableColumnAll(string tableName)
        {
            return Json(synchronizeService.getSyncSourceTableColumnsAll(tableName));
        }

        [HttpGet]
        [Route("admin/sync-source-table")]
        [Route("admin/{tenant_identifier}/sync-source-table")]
        public JsonResult GetSyncSourceTable()
        {
            return Json(synchronizeService.getSyncSourceTable());
        }
        [HttpGet]
        [Route("admin/sync-source-table-column")]
        [Route("admin/{tenant_identifier}/sync-source-table-column")]
        public JsonResult GetSyncSourceTableColumn(string tableName)
        {
            return Json(synchronizeService.getSyncSourceTableColumns(tableName));
        }


        [HttpGet]
        [Route("admin/getcreatedtable")]
        [Route("admin/{tenant_identifier}/getcreatedtable")]
        public IActionResult GetCreatedTable(int caseformid, string tableFor = "")
        {
            int id = caseformid;
            if (tableFor == "source" || tableFor == "policy")
            {
                return Json(synchronizeService.getSyncSourceTable());
            }
            else
            {
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
                        ccvm = formBuilderService.GetBuilderFormById(id);
                        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                        FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                        List<FormBuilderViewModel.Form.Table> table = a.Forms.Tables;
                        return Json(table);
                    }
                    return Json("");
                }
                catch (Exception ex)
                {
                    return Json("");
                }
            }


        }

        [HttpGet]
        [Route("admin/gettablefields")]
        [Route("admin/{tenant_identifier}/gettablefields")]
        public IActionResult GetTableFields(int caseformid, string tablename)
        {
            int id = caseformid;

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
                    ccvm = formBuilderService.GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    List<FormBuilderViewModel.Form.Field> fields = a.Forms.Tables.Where(x => x.Name == tablename).SingleOrDefault().Fields.ToList();
                    return Json(fields);
                }
                return Json("");
            }
            catch (Exception ex)
            {

                return Json("");
            }

        }

        [HttpGet]
        [Route("admin/getStateByFormId")]
        [Route("admin/{tenent_identifier}/getStateByFormId")]
        public IActionResult GetStateByFormId(int caseFormId)
        {
            var loggedUser = commonService.getLoggedInUserId();
            string roleId = commonService.GetRoleIdByUserId(loggedUser);
            return Json(queueService.GetStateSelectListByFormId(caseFormId, roleId));

        }


        [HttpGet]
        [Route("admin/form-table")]
        [Route("admin/{tenant_identifier}/form-table")]
        public JsonResult GetFormTable(int caseformidone, int caseformidtwo)
        {
            var casefieldsone = formBuilderService.GetFormFields(caseformidone);
            var casefieldstwo = formBuilderService.GetFormFields(caseformidtwo);


            if (casefieldstwo.Count > 0 && casefieldsone.Count > 0)
            {
                return Json(new { source = casefieldsone, destination = casefieldstwo });
            }


            return Json(null);
        }

        //[HttpGet]
        //[Route("admin/form-table")]
        //[Route("admin/{tenant_identifier}/form-table")]
        //public JsonResult GetFormTable(int caseformidone, int caseformidtwo)
        //{
        //    //string tenant_ide
        //    var caseListone = caseFormService.GetCaseFormById(caseformidone);
        //    var caseListtwo = caseFormService.GetCaseFormById(caseformidtwo);

        //    List<SelectListItem> casefieldsone = new List<SelectListItem>();
        //    List<SelectListItem> casefieldstwo = new List<SelectListItem>();

        //    if (caseListone != null)
        //    {

        //        JObject obj = JObject.Parse(caseListone.Fields);
        //        DynamicFormViewModel dm = obj.ToObject<DynamicFormViewModel>();



        //        foreach (var tabitem in dm.Tabs)
        //        {
        //            foreach (var elementitem in tabitem.element)
        //            {

        //                SelectListItem cm = new SelectListItem { Text = elementitem.label, Value = elementitem.name };

        //                casefieldsone.Add(cm);
        //            }
        //        }

        //    }

        //    if (caseListtwo != null)
        //    {

        //        JObject obj = JObject.Parse(caseListtwo.Fields);
        //        DynamicFormViewModel dm = obj.ToObject<DynamicFormViewModel>();

        //        foreach (var tabitem in dm.Tabs)
        //        {
        //            foreach (var elementitem in tabitem.element)
        //            {

        //                SelectListItem cv = new SelectListItem { Text = elementitem.label, Value = elementitem.name };

        //                casefieldstwo.Add(cv);
        //            }
        //        }

        //    }

        //    if (casefieldstwo.Count > 0 && casefieldsone.Count > 0)
        //    {
        //        return Json(new { source = casefieldsone, destination = casefieldstwo });
        //    }


        //    return Json(null);
        //}
    }
}