using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Cicero.Service.Extensions;
using System.Threading.Tasks;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Cicero.Service.Models.Core;
using System.Globalization;

namespace Cicero.Service.Services
{

    public interface IFormBuilderService
    {
        DTResponseModel GetBuilderFormListByFilter(DTPostModel model);
        CaseFormViewModel GetBuilderFormById(int id);

        CaseFormViewModel GetBuilderFormByUrl(string url, int tenantid);
        List<CaseFormViewModel> GetActiveTenantForms(int tenantid);

        //check if this is used anywhere if not delete
        List<SelectListItem> FormList(string tenantidentifier);
        Task<CaseFormViewModel> CreateOrUpdate(CaseFormViewModel ccvm);
        CaseFormViewModel CreateOrUpdateModal(CaseFormViewModel ccvm);
        Task<bool> DeleteBuilderFormById(int id);
        Task<bool> ActiveBuilderFormById(int id);
        Task<bool> InactiveBuilderFormById(int id);
        List<CaseFormViewModel> GetAddedBuilderForm(DateTime startDatetime);
        List<SelectListItem> GetFormFields(int caseformid, bool getAll = false, bool forTarget = false, string elementType = null);
        List<CaseFormViewModel> GetBuilderFormListByTenantId(int tenantId);
        bool DeleteBuilderForm(int id);
        List<string> GetBuilderFormsForPermission();
        JObject IsUniqueueElementName(string elementName, int formId, string elementId);

        List<FormBuilderViewModel.Form.Table> GetFormTable(int caseFormId);
        List<FormBuilderViewModel.Form.Field> GetTableField(int caseFormId, string tableName);
    }

    public class FormBuilderService : IFormBuilderService
    {

        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<FormBuilderService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly IPermissionService _permissionService;
        private readonly ICiceroCoreFormService _CoreFormService;

        public FormBuilderService(ApplicationDbContext _db, IPermissionService permissionService, Utils _utils, ILogger<FormBuilderService> _log, ICiceroCoreFormService Iccf, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            IMapper = _IMapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            _permissionService = permissionService;
            _CoreFormService = Iccf;
        }

        public DTResponseModel GetBuilderFormListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "updated_at";
            bool sortDir = true;

            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            int draw = 0;

            if (model != null)
            {
                searchBy = (model.search != null) ? model.search.value : null;
                take = model.length;
                skip = model.start;
                draw = model.draw;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            var caseClaim = db.CaseForm.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                created_at = Utils.GetDefaultDateFormatToDetail(x.CreatedAt),
                updated_at = Utils.GetDefaultDateFormatToDetail(x.UpdatedAt),
                tenant = x.Tenant.Name,
                status = x.Status == true ? "Active" : "Inactive",
                action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/builderform/" + Utils.EncryptId(x.Id) + "/edit.html' title='Edit Form' class='btn btn-light btn-icon datatable__action datatable__edit-btn' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Form</span></a>"
            });

            totalResultsCount = caseClaim.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                caseClaim = caseClaim.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = caseClaim.Count();
            if (sortBy == "created_at" && sortDir == true)
            {
                caseClaim = db.CaseForm.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null).Skip(skip).Take(take).OrderByDescending(x => x.CreatedAt).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    created_at = Utils.GetDefaultDateFormatToDetail(x.CreatedAt),
                    updated_at = Utils.GetDefaultDateFormatToDetail(x.UpdatedAt),
                    tenant = x.Tenant.Name,
                    status = x.Status == true ? "Active" : "Inactive",
                    action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/builderform/" + Utils.EncryptId(x.Id) + "/edit.html' title='Edit Form' class='btn btn-light btn-icon datatable__action datatable__edit-btn' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Form</span></a>"
                });
            }
            else if (sortBy == "created_at" && sortDir == false)
            {
                caseClaim = db.CaseForm.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null).Skip(skip).Take(take).OrderBy(x => x.CreatedAt).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    created_at = Utils.GetDefaultDateFormatToDetail(x.CreatedAt),
                    updated_at = Utils.GetDefaultDateFormatToDetail(x.UpdatedAt),
                    tenant = x.Tenant.Name,
                    status = x.Status == true ? "Active" : "Inactive",
                    action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/builderform/" + Utils.EncryptId(x.Id) + "/edit.html' title='Edit Form' class='btn btn-light btn-icon datatable__action datatable__edit-btn' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Form</span></a>"
                });
            }
            else if (sortBy == "updated_at" && sortDir == true)
            {
                caseClaim = db.CaseForm.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null).Skip(skip).Take(take).OrderByDescending(x => x.UpdatedAt).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    created_at = Utils.GetDefaultDateFormatToDetail(x.CreatedAt),
                    updated_at = Utils.GetDefaultDateFormatToDetail(x.UpdatedAt),
                    tenant = x.Tenant.Name,
                    status = x.Status == true ? "Active" : "Inactive",
                    action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/builderform/" + Utils.EncryptId(x.Id) + "/edit.html' title='Edit Form' class='btn btn-light btn-icon datatable__action datatable__edit-btn' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Form</span></a>"
                });
            }
            else if (sortBy == "updated_at" && sortDir == false)
            {
                caseClaim = db.CaseForm.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null).Skip(skip).Take(take).OrderBy(x => x.UpdatedAt).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    created_at = Utils.GetDefaultDateFormatToDetail(x.CreatedAt),
                    updated_at = Utils.GetDefaultDateFormatToDetail(x.UpdatedAt),
                    tenant = x.Tenant.Name,
                    status = x.Status == true ? "Active" : "Inactive",
                    action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/builderform/" + Utils.EncryptId(x.Id) + "/edit.html' title='Edit Form' class='btn btn-light btn-icon datatable__action datatable__edit-btn' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Form</span></a>"
                });
            }
            else
                caseClaim = caseClaim.Skip(skip).Take(take).OrderBy(sortBy, sortDir);

            var list = caseClaim.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };
        }

        public CaseFormViewModel GetBuilderFormById(int id)
        {
            try
            {

                var caseclaimdata = db.CaseForm
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                if (caseclaimdata == null)
                {
                    return new CaseFormViewModel { };
                }
                var caseClaim = IMapper.Map<CaseFormViewModel>(caseclaimdata);

                return caseClaim;
            }
            catch (Exception ex)
            {
                Log.LogError("FormBuilderService - GetCaseFormById - " + ex);
            }

            return new CaseFormViewModel { };
        }

        public CaseFormViewModel GetBuilderFormByUrl(string url, int tenantid)
        {
            try
            {

                var caseclaimdata = db.CaseForm
                    .Where(x => !string.IsNullOrWhiteSpace(x.UrlIdentifier) && x.UrlIdentifier.Equals(url, StringComparison.OrdinalIgnoreCase) && x.TenantId == tenantid)
                    .FirstOrDefault();
                //var caseclaimdata = db.CaseForm
                //    .Where(x => x.UrlIdentifier == url && x.TenantId == tenantid)
                //    .FirstOrDefault();
                if (caseclaimdata == null)
                {
                    return new CaseFormViewModel { };
                }
                var caseClaim = IMapper.Map<CaseFormViewModel>(caseclaimdata);

                return caseClaim;
            }
            catch (Exception ex)
            {
                Log.LogError("FormBuilderService - GetCaseFormByUrl - " + ex);
                return new CaseFormViewModel { };
            }

        }

        public List<CaseFormViewModel> GetActiveTenantForms(int tenantid)
        {
            try
            {

                var caseclaimdata = db.CaseForm
                                        .Where(x => x.TenantId == tenantid)
                                        .ToList();

                var caseClaim = IMapper.Map<List<CaseFormViewModel>>(caseclaimdata);

                return caseClaim;
            }
            catch (Exception ex)
            {
                Log.LogError("FormBuilderService - GetActiveTenantForms - " + ex);
            }

            return null;
        }

        //check if this is used anywhere
        public List<SelectListItem> FormList(string tenantidentifier)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenantidentifier);

            return db.CaseForm
                       .Where(x => x.TenantId == tenantid).AsNoTracking()
                       .Select(y => new SelectListItem
                       {
                           Text = y.Name,
                           Value = y.Id.ToString(),
                           Disabled = !(y.Status)

                       }).ToList();
        }

        private async Task<CaseForm> Create(CaseForm model)
        {
            try
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await db.CaseForm.AddAsync(model);

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "New Form for Claim created <a href ='/admin" + Utils.GetTenantForUrl(false) + "/caseclaim/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Name + "</a>. Created By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return model;
            }
            catch (Exception)
            {
                return model;
            }

        }

        private async Task<bool> Update(CaseForm model)
        {

            try
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                db.CaseForm.Attach(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "Form for claim updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/caseclaim/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Name + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<CaseFormViewModel> CreateOrUpdate(CaseFormViewModel ccvm)
        {

            //ccvm.UpdatedAt = (DateTime.Now);
            var model = IMapper.Map<CaseForm>(ccvm);

            model.UserId = commonService.getLoggedInUserId();

            model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            if (ccvm.Id == 0)
            {
                model = await Create(model);
            }
            else
            {
                model.UpdatedAt = DateTime.Now;
                await Update(model);
            }

            ccvm.Id = model.Id;
            return ccvm;
        }
        public CaseFormViewModel CreateOrUpdateModal(CaseFormViewModel ccvm)
        {

            //ccvm.UpdatedAt = (DateTime.Now);
            var model = IMapper.Map<CaseForm>(ccvm);

            model.UserId = commonService.getLoggedInUserId();

            model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            if (ccvm.Id == 0)
            {
                db.CaseForm.Add(model);
            }
            else
            {
                model.UpdatedAt = DateTime.Now;
                db.CaseForm.Update(model);
            }
            db.SaveChanges();
            ccvm.Id = model.Id;
            return ccvm;
        }
        public async Task<bool> DeleteBuilderFormById(int id)
        {
            var caseform = await db.CaseForm.FindAsync(id);
            _CoreFormService.DeleteDymanicCoreForm(id);
            string title = caseform.Name;
            if (caseform != null)
            {
                db.CaseForm.Remove(caseform);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "CaseForm Deleted " + title + ". Deleted By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("FormBuilderService - DeleteCaseFormById - " + id + " - : ");
            return false;
        }

        public async Task<bool> ActiveBuilderFormById(int id)
        {
            var caseform = await db.CaseForm.FindAsync(id);
            if (caseform != null)
            {
                caseform.Status = true;
                var result = db.CaseForm.Update(caseform);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "CaseForm changed to Active <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(caseform.Id) + "/edit.html'>" + caseform.Name + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("FormBuilderService - ActiveCaseFormById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveBuilderFormById(int id)
        {
            var caseform = await db.CaseForm.FindAsync(id);
            if (caseform != null)
            {
                caseform.Status = false;
                var result = db.CaseForm.Update(caseform);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "CaseForm changed to InActive <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(caseform.Id) + "/edit.html'>" + caseform.Name + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("FormBuilderService - InactiveCaseFormById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetFormFields(int caseformid, bool getAll = false, bool forTarget = false, string elementType = null)
        {
            var caseList = GetBuilderFormById(caseformid);

            if (caseList != null && caseList.Fields != null)
            {

                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                caseList.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(caseList.Fields, settings);
                FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(caseList.Fields, settings);
                // JObject obj = JObject.Parse(caseList.Fields);
                // DynamicFormViewModel dm = obj.ToObject<DynamicFormViewModel>();

                List<SelectListItem> casefields = new List<SelectListItem>();


                foreach (var tabBody in a.Tab)
                {
                    foreach (var row in tabBody.Row)
                    {
                        if (getAll == true)
                        {
                            if (forTarget)
                            {
                                if (row.Name != null)
                                {
                                    SelectListItem cm = new SelectListItem { Text = "row-" + row.Name, Value = row.ElementId };
                                    casefields.Add(cm);
                                }
                            }

                        }
                        foreach (var col in row.Column)
                        {
                            if (getAll == true)
                            {
                                if (forTarget)
                                {
                                    if (col.Name != null)
                                    {
                                        SelectListItem cm = new SelectListItem { Text = "col-" + col.Name, Value = col.ElementId };
                                        casefields.Add(cm);
                                    }
                                }
                            }
                            foreach (dynamic element in col.Element)
                            {
                                if (getAll == true)
                                {
                                    string type = element.Type.ToString();
                                    if (element.Name != null)
                                    {
                                        SelectListItem cm = new SelectListItem { Text = "elm-" + element.Name, Value = element.ElementId };
                                        if (elementType == null)
                                        {
                                            casefields.Add(cm);
                                        }
                                        else
                                        {
                                            if (type.Contains("Radio"))
                                            {
                                                casefields.Add(cm);
                                            }
                                        }
                                    }
                                    if (type.Contains("Table"))
                                    {
                                        if (element.Column != null)
                                        {
                                            foreach (var tableColumn in element.Column)
                                            {
                                                dynamic tableColElm = tableColumn.ColumnElement;
                                                if (tableColElm != null)
                                                {
                                                    string tableElementType = tableColElm.Type.ToString();
                                                    if (tableColElm.Name != null)
                                                    {
                                                        SelectListItem cm = new SelectListItem { Text = "elm-" + tableColElm.Name, Value = tableColElm.ElementId };
                                                        if (elementType == null)
                                                        {
                                                            casefields.Add(cm);
                                                        }
                                                        else
                                                        {
                                                            if (type.Contains("Radio"))
                                                            {
                                                                casefields.Add(cm);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    var isit = element.GetType().GetProperty("FieldName");
                                    if (isit != null)
                                    {
                                        if (element.FieldName != "" && element.FieldName != null)
                                        {
                                            SelectListItem cm = new SelectListItem { Text = element.FieldName, Value = element.ElementId };
                                            casefields.Add(cm);
                                        }

                                    }
                                    string type = element.Type.ToString();
                                    if (type.Contains("Table"))
                                    {
                                        if (element.Column != null)
                                        {
                                            foreach (var tableColumn in element.Column)
                                            {
                                                dynamic tableColElm = tableColumn.ColumnElement;
                                                if (tableColElm != null)
                                                {
                                                    isit = tableColElm.GetType().GetProperty("FieldName");
                                                    if (isit != null)
                                                    {
                                                        if (tableColElm.FieldName != "" && tableColElm.FieldName != null)
                                                        {
                                                            SelectListItem cm = new SelectListItem { Text = tableColElm.FieldName, Value = tableColElm.ElementId };
                                                            casefields.Add(cm);
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
                if (elementType == null)
                {
                    casefields.Add(new SelectListItem { Value = "loggedinuser", Text = "Get Loggedin User" });
                    casefields.Add(new SelectListItem { Value = "formid", Text = "Get Form Id" });
                    casefields.Add(new SelectListItem { Value = "caseid", Text = "Get Case Id" });
                }
                return casefields;
            }

            return new List<SelectListItem>();
        }

        /// <summary>
        /// returns caseformlist by current tenant id
        /// </summary>
        /// <returns></returns>
        public List<CaseFormViewModel> GetBuilderFormListByTenantId(int tenantId)
        {

            List<CaseForm> caseFormList = db.CaseForm.Where(x => x.TenantId == tenantId && x.ModelName != null).OrderByDescending(x => x.CreatedAt).ToList();
            List<CaseFormViewModel> CaseformViewModelList = IMapper.Map<List<CaseFormViewModel>>(caseFormList);
            return CaseformViewModelList;

        }

        public List<CaseFormViewModel> GetAddedBuilderForm(DateTime startDatetime)
        {
            string loggedUser = commonService.getLoggedInUserId();
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<CaseForm> listCaseForm = db.CaseForm.Where(x => x.UserId == loggedUser && x.CreatedAt >= startDatetime).ToList();
            return IMapper.Map<List<CaseFormViewModel>>(listCaseForm);
        }

        public bool DeleteBuilderForm(int id)
        {
            var caseform = db.CaseForm.Where(x => x.Id == id).FirstOrDefault();
            string title = caseform.Name;
            if (caseform != null)
            {
                db.CaseForm.Remove(caseform);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                activityLogService.CreateLog(loggedUser, "CaseForm Deleted " + title + ". Deleted By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("FormBuilderService - DeleteCaseFormById - " + id + " - : ");
            return false;

        }

        public List<string> GetBuilderFormsForPermission()
        {

            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<string> Names = new List<string>();
            List<CaseFormViewModel> cases = GetBuilderFormListByTenantId(tenantid);
            foreach (var item in cases)
            {
                try
                {
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(item.Fields, settings);
                    string name = a.Forms.Navigations.Name;
                    name = name + "," + _permissionService.CheckForPermissionGroup(name, item.Id).ToString() + "," + item.Id;
                    Names.Add(name);
                }
                catch (Exception ex)
                {

                }
            }
            return Names;
        }

        public List<FormBuilderViewModel.Form.Table> GetFormTable(int caseFormId)
        {
            int id = caseFormId;
            List<FormBuilderViewModel.Form.Table> table = new List<FormBuilderViewModel.Form.Table>();
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
                    ccvm = GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    table = a.Forms.Tables;

                }

            }
            catch (Exception ex)
            {

            }
            return table;
        }

        public List<FormBuilderViewModel.Form.Field> GetTableField(int caseFormId, string tableName)
        {
            int id = caseFormId;
            List<FormBuilderViewModel.Form.Field> fields = new List<FormBuilderViewModel.Form.Field>();
            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            try
            {

                if (id != 0 && !string.IsNullOrWhiteSpace(tableName))
                {
                    ccvm = GetBuilderFormById(id);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                    fields = a.Forms.Tables.Where(x => x.Name == tableName).SingleOrDefault().Fields.ToList();

                }

            }
            catch (Exception ex)
            {


            }
            return fields;
        }

        public JObject IsUniqueueElementName(string elementName, int formId, string elementId)
        {
            CaseFormViewModel ccvm = new CaseFormViewModel
            {
                Id = formId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            bool isQnique = true;
            ccvm = GetBuilderFormById(formId);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            isQnique = CheckforUniqueueNameElement(a, elementName, elementId);
            JObject temp = new JObject();
            if (isQnique)
            {

                temp.Add("unique", true);
                temp.Add("message", "");
            }
            else
            {
                temp.Add("unique", false);
                temp.Add("message", "Name is not unique, please input unique name");
            }
            return temp;
        }

        private bool CheckforUniqueueNameElement(FormBuilderViewModel formBuilderViewModel, string name, string elementId)
        {

            foreach (var tab in formBuilderViewModel.Tab)
            {

                foreach (var row in tab.Row)
                {
                    if (row.ElementId != elementId)
                    {
                        if (name == row.Name)
                        {
                            return false;
                        }

                    }
                    foreach (var column in row.Column)
                    {
                        if (column.ElementId != elementId)
                        {
                            if (name == column.Name)
                            {
                                return false;
                            }

                        }
                        foreach (dynamic element in column.Element)
                        {
                            if (element.ElementId != elementId)
                            {
                                if (name == element.Name)
                                {
                                    return false;
                                }

                            }
                            string type = element.Type.ToString();
                            if (type.Contains("Table"))
                            {
                                if (element.Column != null)
                                {
                                    foreach (var tableColumn in element.Column)
                                    {
                                        dynamic tableColElm = tableColumn.ColumnElement;
                                        if (tableColElm != null)
                                        {
                                            if (tableColElm.ElementId != elementId)
                                            {
                                                if (name == tableColElm.Name)
                                                {
                                                    return false;
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
            return true;
        }
    }
}
