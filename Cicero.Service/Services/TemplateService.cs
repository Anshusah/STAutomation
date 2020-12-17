using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Cicero.Service.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Cicero.Service;
using static Cicero.Service.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Cicero.Service.Models.Core;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using static Cicero.Service.Extensions.Extensions;
using Cicero.Service.Models.General;
using System.Text.Encodings.Web;

namespace Cicero.Service.Services
{
    public interface ITemplateService
    {
        DTResponseModel GetTemplateListByFilter(DTPostModel model);
        TemplateViewModel GetTemplateById(int id);
        string GetTemplateBodyByName(string title);
        Task<TemplateViewModel> UpdateTemplate(TemplateViewModel tvm);
        List<SelectListItem> GetTemplateListForWorkflow(int? formId);
        string GenerateTemplate(string messagebody, CaseViewModel cvm);
        JObject GetFieldsByTemplate(int type, int formid = 0);
        List<string> GetTablesForMatch(bool isForm, int formId);
        List<string> GetFieldsForMatch(string table, bool isForm, int formId);
        List<string> GetSourceTableFields(string table);
        bool CheckFieldName(string fieldName, int Id);
        bool SaveOrUpdateField(MailMergeFieldViewModel mailMergeFieldViewModel);
        bool DeleteMailField(int fieldId);
        MailMergeObjectViewModel SaveOrUpdateMailObject(MailMergeObjectViewModel mailMergeObjectViewModel);
        MailMergeObject GetMergeObjectById(int mailMergeObjectId);
        string CreateEmailTemplate(string messageBody, int caseFormId = 0, int caseId = 0, int stateId = 0, string linkUrl = "", string userId = "", bool external = false);
        MailMergeFieldViewModel GetFieldById(int fieldId);
        MailMergeObjectViewModel GetMailObjectById(int objectId);
        bool SetMailObjectStatus(int objectId, bool status);
        List<EnumViewModel> DefaulEmailSettingsFor();
        string GetEmailGeneralSetting();
        bool updateGeneralEmailSetting(JObject data);
        Task<bool> DeleteTemplateById(int templateId);
        int GetTemplateCount();


    }

    public class TemplateService : ITemplateService
    {
        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<TemplateService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly IFormBuilderService formBuilderService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppSetting appSetting=null;
       

        public TemplateService(ApplicationDbContext _db, Utils _utils, AppSetting _appSetting,UserManager<ApplicationUser> _userManager, IFormBuilderService _formBuilderService, ILogger<TemplateService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _utils;
            appSetting = _appSetting;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            IMapper = _IMapper;
            activityLogService = _activityLogService;
            commonService = _commonService;
            formBuilderService = _formBuilderService;
            userManager = _userManager;
        }

        public DTResponseModel GetTemplateListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
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

            var template = db.Article.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.Type == "template").Select(x => new
            {
                id = x.Id,
                title = x.Title,
                tenant = x.Tenant.Name,
                created_at = Utils.GetDefaultDateFormat(x.CreatedAt),
                updated_at = Utils.GetDefaultDateFormat(x.UpdatedAt),
                version = x.Version,
                action = "<a href='/admin" + Utils.GetTenantForUrl(false) + "/template/" + Utils.EncryptId(x.Id) + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Templates' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Templates</span></a>"
            });

            totalResultsCount = template.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                template = template.Where(o => o.title.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = template.Count();
            template = template.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = template.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };
        }

        public TemplateViewModel GetTemplateById(int id)
        {
            try
            {

                var templatedata = db.Article
                                    .Where(x => x.Id == id && x.Type == "template")
                                    .FirstOrDefault();
                if (templatedata == null)
                {
                    return new TemplateViewModel { };
                }
                var template = IMapper.Map<TemplateViewModel>(templatedata);
                template.TemplateType = Convert.ToInt32(templatedata.Template);
                return template;
            }
            catch (Exception ex)
            {
                Log.LogError("TemplateService - GetTemplateById - " + ex);
                return new TemplateViewModel { };
            }

        }

        public string GetTemplateBodyByName(string title)
        {
            try
            {

                int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

                string messagebody = db.Article
                                    .Where(x => x.Title.ToLower() == title.ToLower() && x.Type == "template" && x.TenantId == tenantid)
                                    .Select(y => y.Content)
                                    .FirstOrDefault();

                return messagebody;
            }
            catch (Exception ex)
            {
                Log.LogError("TemplateService - GetTemplateBodyByName - " + ex);
                return null;
            }
        }

        public async Task<TemplateViewModel> UpdateTemplate(TemplateViewModel tvm)
        {
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            var model = IMapper.Map<Article>(tvm);
            model.Type = "template";
            model.Template = tvm.TemplateType.ToString();
            model.UpdatedAt = DateTime.Now;
            if (model.TenantId == null || model.TenantId == 0)
            {
                int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

                if (tenantid == 0)
                {
                    return null;
                }

                model.TenantId = tenantid;
            }

            if (tvm.Id != 0)
            {
                db.Article.Attach(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "Template updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/template/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Title + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
            }
            else
            {
                db.Article.Add(model);
                await db.SaveChangesAsync();
                await activityLogService.CreateLog(loggedUser, "Template created <a href ='/admin" + Utils.GetTenantForUrl(false) + "/template/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Title + "</a>. Updated By <a href='/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
            }
            db.SaveChanges();
            tvm.Id = model.Id;
            return tvm;
        }

        public List<SelectListItem> GetTemplateListForWorkflow(int? formId)
        {

            var tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            return db.Article.Where(y => y.TenantId == tenantid && y.Type == "template" && (y.FormId == formId || formId==null)).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }).ToList();
        }

        public string GenerateTemplate(string messagebody, CaseViewModel cvm)
        {
            string loggeduser = commonService.GetUserFullName().Result;
            if (messagebody != null)
            {
                string messageNew = messagebody.Replace("[manager_name]", loggeduser);
                messageNew = messagebody.Replace("[date_today]", Utils.GetDefaultDateFormat(DateTime.Now));
                //messageNew = messagebody.Replace("[client_name]", cvm.FirstName + " " + cvm.SurName);
                //messageNew = messagebody.Replace("[user_name]", cvm.FirstName + " " + cvm.SurName);
                //messageNew = messagebody.Replace("[manager_name]", loggeduser);
                //messageNew = messagebody.Replace("[manager_name]", loggeduser);
                //messageNew = messagebody.Replace("[manager_name]", loggeduser);
                //messageNew = messagebody.Replace("[manager_name]", loggeduser);

                return messageNew;
            }
            return null;
        }



        public JObject GetFieldsByTemplate(int type, int formid = 0)
        {
            List<JObject> fields = new List<JObject>();
            List<MailMergeField> mailMergeField = db.MailMergeField.Where(x => x.TemplateType == type && x.isDeleted == false && (x.FormId == formid || x.FormId == 0)).ToList();
           
            JObject item = new JObject();
            foreach (MailMergeField field in mailMergeField)
            {
                JObject obj = new JObject();
                obj.Add("name", field.FieldName);
                obj.Add("alias", field.Alias);
                if (field.FormId == 0)
                {
                    obj.Add("class", "form-general");
                    obj.Add("source", "General");
                }
                else
                {
                    obj.Add("class", "form-specific");
                    obj.Add("source", "Form");
                }

                item.Add(field.Id.ToString(), obj);
            }
            List<MailMergeFieldViewModel> list = getDefaults();
            foreach(MailMergeFieldViewModel field in list)
            {
                JObject obj = new JObject();
                obj.Add("name", field.FieldName);
                obj.Add("alias", field.Alias);
                obj.Add("class", "form-general");
                obj.Add("source", "General");
                item.Add(field.Id.ToString(), obj);
            }
            

            return item;
        }
        public List<string> GetTablesForMatch(bool isForm, int formId = 0)
        {
            List<string> tables = new List<string>();
            if (isForm)
            {
                if (formId != 0)
                {
                    List<FormBuilderViewModel.Form.Table> table = formBuilderService.GetFormTable(formId);
                    foreach (FormBuilderViewModel.Form.Table item in table)
                    {
                        tables.Add(item.Name);
                    }
                }
            }
            else
            {
                tables = Enum.GetNames(typeof(TablesToShow)).ToList();
            }
            return tables;
        }

        public List<string> GetFieldsForMatch(string table, bool isForm, int formId = 0)
        {
            List<string> tableFields = new List<string>();
            if (isForm)
            {
                if (formId != 0)
                {
                    List<FormBuilderViewModel.Form.Field> fields = formBuilderService.GetTableField(formId, table);
                    foreach (FormBuilderViewModel.Form.Field item in fields)
                    {
                        tableFields.Add(item.Name);
                    }
                }
            }
            else
            {
                tableFields = GetSourceTableFields(table);
            }
            return tableFields;
        }

        public List<string> GetSourceTableFields(string tableName)
        {
            List<string> abc = new List<string>();
            var columns = db.CustomEntities3.FromSql($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{tableName}'", tableName);

            foreach (var item in columns)
            {
                abc.Add(item.Column_Name);
            }
            return abc;
        }

        public bool CheckFieldName(string fieldName, int Id)
        {
            var check = db.MailMergeField.Where(x => x.FieldName.ToUpper() == fieldName.ToUpper() && x.isDeleted == false && x.TemplateType == Id).SingleOrDefault();
            if (check == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveOrUpdateField(MailMergeFieldViewModel mailMergeFieldViewModel)
        {
            try
            {
                MailMergeField mailMergeField = new MailMergeField()
                {
                    Id = mailMergeFieldViewModel.Id,
                    FieldName = mailMergeFieldViewModel.FieldName,
                    Alias = mailMergeFieldViewModel.Alias,
                    FormId = mailMergeFieldViewModel.FormId,
                    isDeleted = false,
                    DbSourceField = mailMergeFieldViewModel.DbSourceField,
                    DbSourceTable = mailMergeFieldViewModel.DbSourceTable,
                    TemplateType = mailMergeFieldViewModel.TemplateType,
                    TenantId = mailMergeFieldViewModel.TenantId

                };

                if (mailMergeFieldViewModel.Id != 0)
                {

                    db.MailMergeField.Update(mailMergeField);
                }
                else
                {
                    db.MailMergeField.Add(mailMergeField);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool DeleteMailField(int fieldId)
        {
            try
            {
                MailMergeField mailMergeField = db.MailMergeField.Where(x => x.Id == fieldId).SingleOrDefault();
                if (mailMergeField != null)
                {
                    mailMergeField.isDeleted = true;
                    db.MailMergeField.Update(mailMergeField);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public MailMergeObjectViewModel SaveOrUpdateMailObject(MailMergeObjectViewModel mailMergeObjectViewModel)
        {
            MailMergeObject mailMergeObject = new MailMergeObject()
            {
                Id = mailMergeObjectViewModel.Id,
                Title = mailMergeObjectViewModel.Title,
                FormId = mailMergeObjectViewModel.FormId,
                IsActive = true,
                IsDeleted = false,
                TemplateId = mailMergeObjectViewModel.TemplateId,
                TenantId = mailMergeObjectViewModel.TenantId,
                CreatedDate = mailMergeObjectViewModel.CreatedDate
            };
            if(mailMergeObjectViewModel.Id != 0)
            {
                db.MailMergeObject.Update(mailMergeObject);
            }
            else
            {
                db.MailMergeObject.Add(mailMergeObject);
            }
           
            db.SaveChanges();
            mailMergeObjectViewModel.Id = mailMergeObject.Id;
            return mailMergeObjectViewModel;
        }

        public MailMergeObject GetMergeObjectById(int mailMergeObjectId)
        {
            MailMergeObject mailMergeObject = db.MailMergeObject.Where(x => x.Id == mailMergeObjectId).SingleOrDefault();
            if (mailMergeObject == null)
            {
                return (new MailMergeObject());
            }
            return mailMergeObject;
        }
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public string CreateEmailTemplate(string messageBody, int caseFormId = 0, int caseId = 0, int stateId = 0, string linkUrl = "",string userId="", bool external = false)
        {

            List<string> lst = new List<string>();
            List<string> values = new List<string>();
            List<string> dbTables = Enum.GetNames(typeof(TablesToShow)).ToList();
            MatchCollection mcol = Regex.Matches(messageBody, @"([[A-Z])\w+]");
            List<FormBuilderViewModel.Form.Table> caseFormTables = new List<FormBuilderViewModel.Form.Table>();
            List<JObject> caseFormData = new List<JObject>();
            if (caseFormId != 0 && caseId != 0)
            {
                caseFormTables = GetTables(caseFormId);
                caseFormData = GetTableData(caseId, caseFormTables);
            }
            foreach (Match m in mcol)
            {
                lst.Add(m.ToString());
            }
            List<MailMergeField> mailMergeFields = new List<MailMergeField>();
            List<MailMergeFieldViewModel> Defaults = getDefaults();
            foreach (string field in lst)
            {
                MailMergeField mailMergeField = db.MailMergeField.Where(x => x.Alias == field && x.isDeleted == false).SingleOrDefault();
                if (mailMergeField != null)
                {
                    mailMergeFields.Add(mailMergeField);
                }
                else {
                    MailMergeFieldViewModel text = Defaults.Where(x => x.Alias == field).FirstOrDefault();
                    if(text!=null)
                    {
                        mailMergeField = new MailMergeField()
                        {
                            Id=text.Id,
                            Alias = text.Alias,
                            FieldName = text.FieldName
                        };
                        mailMergeFields.Add(mailMergeField);
                    }
                    
                }
            }
            foreach (MailMergeField item in mailMergeFields)
            {
                string value = "";
                try
                {
                    if (item.FormId != 0)
                    {
                        foreach (JObject jObject in caseFormData)
                        {
                            if (jObject["tableName"].ToString() == item.DbSourceTable)
                            {
                                foreach (var obj in jObject)
                                {
                                    if (obj.Key == item.DbSourceField)
                                    {
                                        value = obj.Value.ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        value = CheckForDefaults(item.Alias, linkUrl);
                        if (!external && value == "")
                        {
                            switch (item.DbSourceTable)
                            {
                                case string a when a == TablesToShow.CaseForm.ToDescription():
                                    CaseForm caseForm = db.CaseForm.Where(x => x.Id == caseFormId).SingleOrDefault();
                                    var propertiesCaseForm = GetProperties(caseForm);

                                    foreach (var p in propertiesCaseForm)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(caseForm, null);
                                            if (value1 != null) { value = Convert.ToString(value1); }
                                            else { value = ""; }
                                            break;
                                        }

                                    }
                                    break;
                                case string a when a == TablesToShow.Case.ToDescription():
                                    Case caseData = db.Case.Where(x => x.Id == caseId).SingleOrDefault();
                                    var propertiesCase = GetProperties(caseData);

                                    foreach (var p in propertiesCase)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {

                                            var value1 = p.GetValue(caseData, null);
                                            if (p.Name == "UserId" || p.Name=="Id")
                                            {
                                                ApplicationUser user1 = db.Users.Where(x => x.Id == value1.ToString()).SingleOrDefault();
                                                string FullName = user1.FirstName + " " + user1.LastName;
                                                value = FullName;
                                            }
                                            else
                                            {
                                                if (value1 != null) { value = Convert.ToString(value1); }
                                                else { value = ""; }
                                            }

                                            break;

                                        }
                                    }

                                    break;
                                case string a when a == TablesToShow.Queue.ToDescription():
                                    QueueToState queueToState = db.QueueToState.Where(x => x.StateId == stateId && x.CaseFormId == caseFormId).FirstOrDefault();
                                    Queue queue = db.Queue.Where(x => x.Id == queueToState.QueueId).FirstOrDefault();
                                    var propertiesQueue = GetProperties(queue);

                                    foreach (var p in propertiesQueue)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(queue, null);
                                            if (value1 != null) { value = Convert.ToString(value1); }
                                            else { value = ""; }
                                            break;
                                        }
                                    }


                                    break;
                                case string a when a == TablesToShow.Role.ToDescription():
                                    if(userId == "")
                                    {
                                        userId = commonService.getLoggedInUserId();
                                    }
                                     
                                    string roleid = commonService.GetRoleIdByUserId(userId);

                                    ApplicationRole role = db.Roles.Where(x => x.Id == roleid).SingleOrDefault();

                                    var propertiesRole = GetProperties(role);

                                    foreach (var p in propertiesRole)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(role, null);
                                            if (value1 != null) { value = Convert.ToString(value1); }
                                            else { value = ""; }
                                            break;
                                        }
                                    }
                                    break;
                                case string a when a == TablesToShow.State.ToDescription():
                                    State state = db.State.Where(x => x.Id == stateId).SingleOrDefault();
                                    var propertiesState = GetProperties(state);

                                    foreach (var p in propertiesState)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(state, null);
                                            if (value1 != null) { value = Convert.ToString(value1); }
                                            else { value = ""; }
                                            break;
                                        }
                                    }
                                    break;
                                case string a when a == TablesToShow.User.ToDescription():
                                    if(userId=="")
                                    {
                                        userId = commonService.getLoggedInUserId();
                                    }
                                    
                                    ApplicationUser user = db.Users.Where(x => x.Id == userId).SingleOrDefault();

                                    var propertiesUser = GetProperties(user);

                                    foreach (var p in propertiesUser)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(user, null);
                                            if (p.Name == "UserId" || p.Name=="Id")
                                            {
                                                string FullName = user.FirstName + " " + user.LastName;
                                                value = FullName;
                                            }
                                            else
                                            {
                                                if (value1 != null) { value = Convert.ToString(value1); }
                                                else { value = ""; }
                                            }

                                            break;


                                            
                                        }
                                    }
                                    break;
                                case string a when a == TablesToShow.Tenant.ToDescription():

                                    int tenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                                    Tenant tenant = db.Tenant.Where(x => x.Id == tenantId).SingleOrDefault();

                                    var propertiesTenant = GetProperties(tenant);

                                    foreach (var p in propertiesTenant)
                                    {
                                        if (item.DbSourceField == p.Name)
                                        {
                                            var value1 = p.GetValue(tenant, null);
                                            if (value1 != null) { value = Convert.ToString(value1); }
                                            else { value = ""; }
                                            break;
                                        }
                                    }
                                    break;

                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                }

                values.Add(value);

            }
            string messageNew = messageBody;
            for (int i = 0; i < mailMergeFields.Count; i++)
            {
                messageNew = messageNew.Replace(mailMergeFields[i].Alias, values[i]);
            }
            return messageNew;
        }

        private List<FormBuilderViewModel.Form.Table> GetTables(int caseFormId)
        {
            var ccvm = formBuilderService.GetBuilderFormById(caseFormId);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            FormBuilderViewModel sourceFormBuilderVM = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            List<FormBuilderViewModel.Form.Table> tablesSource = sourceFormBuilderVM.Forms.Tables;
            return tablesSource;
        }


        /// <summary>
        /// Get Case Tables and Values
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<JObject> GetTableData(int caseId, List<FormBuilderViewModel.Form.Table> table)
        {
            List<JObject> tablesA = new List<JObject>();
            try
            {
                if (table != null)
                {
                    foreach (var tb in table)
                    {
                        if (Utils.ConvertToString(tb.Name) != "")
                        {
                            var Forms = db.CustomEntities.FromSql($"SELECT * FROM [{tb.Name}] Where [CaseId] = {caseId}", tb.Name, caseId);
                            int j = 0;
                            JObject jsonObjDB = new JObject();
                            // var temp = Forms.Extras;
                            foreach (var item in Forms)
                            {
                                jsonObjDB = (JObject)JsonConvert.DeserializeObject(item.Extras);

                            }
                            // JObject jsonObj = new JObject();
                            foreach (var tblElm in tb.Fields)
                            {
                                if (!jsonObjDB.ContainsKey(tblElm.Name))
                                {
                                    jsonObjDB.Add(tblElm.Name, "Null");
                                }

                            }
                            jsonObjDB.Add("tableName", tb.Name);
                            tablesA.Add(jsonObjDB);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return tablesA;
        }


        private List<MailMergeFieldViewModel> getDefaults()
        {
            List<MailMergeFieldViewModel> list = new List<MailMergeFieldViewModel>();
            MailMergeFieldViewModel mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                FieldName = "Password Reset Link",
                Id = -1,
                Alias = "[Password_Rest_Link]"
            };
            list.Add(mailMergeFieldViewModel);

            mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                FieldName = "Confirmation Link",
                Id = -2,
                Alias = "[Confirmation_Link]"
            };
            list.Add(mailMergeFieldViewModel);

            mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                FieldName = "Today Date",
                Id = -3,
                Alias = "[Today_Date]"
            };
            list.Add(mailMergeFieldViewModel);

            mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                FieldName = "Today DateTime",
                Id = -4,
                Alias = "[Today_DateTime]"
            };
            list.Add(mailMergeFieldViewModel);

            mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                FieldName = "Payment Request Link",
                Id = -5,
                Alias = "[PaymentRequest_Link]"
            };
            list.Add(mailMergeFieldViewModel);

            return list;
        }
        private string CheckForDefaults(string alias, string linkUrl = "")
        {
            string value = "";
            switch (alias)
            {
                case "[Password_Rest_Link]":
                    if (linkUrl != "")
                    {
                        value = "<a href='" + HtmlEncoder.Default.Encode(linkUrl) + "'>Click here</a>";
                    }
                    break;
                case "[Confirmation_Link]":
                    if (linkUrl != "")
                    {
                        value = "<a href='"+HtmlEncoder.Default.Encode(linkUrl)+"'>Click here</a>";
                      
                    }
                    break;
                case "[PaymentRequest_Link]":
                    if (linkUrl != "")
                    {
                        value = "<a href='" + HtmlEncoder.Default.Encode(linkUrl) + "'>Click here</a>";

                    }
                    break;
                case "[Today_Date]":
                    value = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                    break;
                case "[Today_DateTime]":
                    value = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                    break;

            }
            return value;
        }

        public MailMergeFieldViewModel GetFieldById(int fieldId)
        {
            MailMergeField mailMergeField = db.MailMergeField.Where(x => x.Id == fieldId && x.isDeleted == false).FirstOrDefault();
            MailMergeFieldViewModel mailMergeFieldViewModel = new MailMergeFieldViewModel()
            {
                    Id = mailMergeField.Id,
                    FieldName = mailMergeField.FieldName,
                    Alias = mailMergeField.Alias,
                    FormId = mailMergeField.FormId,
                    isDeleted = mailMergeField.isDeleted,
                    DbSourceField = mailMergeField.DbSourceField,
                    DbSourceTable = mailMergeField.DbSourceTable,
                    TemplateType = mailMergeField.TemplateType,
                    TenantId = mailMergeField.TenantId

                };
            return mailMergeFieldViewModel;
        }

        public MailMergeObjectViewModel GetMailObjectById(int objectId)
        {
            MailMergeObject mailMergeObject = db.MailMergeObject.Where(x => x.Id == objectId).FirstOrDefault();

            if(mailMergeObject != null)
            {
                MailMergeObjectViewModel mailMergeObjectViewModel = new MailMergeObjectViewModel()
                {
                    Id = mailMergeObject.Id,
                    FormId = mailMergeObject.FormId,
                    CreatedDate = mailMergeObject.CreatedDate,
                    IsActive = mailMergeObject.IsActive,
                    IsDeleted = mailMergeObject.IsDeleted,
                    TemplateId = mailMergeObject.TemplateId,
                    TenantId = mailMergeObject.TenantId,
                    Title = mailMergeObject.Title

                };
                return mailMergeObjectViewModel;
            }
            else
            {
                return new MailMergeObjectViewModel();
            }
        }

        public bool SetMailObjectStatus(int objectId, bool status)
        {
            try
            {

                MailMergeObject mailMergeObject = db.MailMergeObject.Where(x => x.Id == objectId).SingleOrDefault();
                if (mailMergeObject != null)
                {
                    mailMergeObject.IsActive = status;
                    db.MailMergeObject.Update(mailMergeObject);
                    db.SaveChanges();
                    return true;
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

        public List<EnumViewModel> DefaulEmailSettingsFor()
        {
            List<EnumViewModel> foremails = (List<EnumViewModel>)EnumModel<EmailSettingFor>.List();
            return foremails;

        }
        public string GetEmailGeneralSetting()
        {
            return appSetting.Get("app_email");
        }

        public bool updateGeneralEmailSetting(JObject data)
        {
            try
            {
                appSetting.Update("app_email", data.ToString());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTemplateById(int templateId)
        {
            int count = 0;
            count = db.MailMergeObject.Where(x => x.TemplateId == templateId).ToList().Count();
            string mails = appSetting.Get("app_email");
            JObject mailObject = (JObject)JsonConvert.DeserializeObject(mails);
            foreach(var item in mailObject)
            {
                if(item.Value.ToString() == templateId.ToString())
                {
                    count++;
                }
            }
            if (count == 0)
            {
                Article mailTemplate = db.Article.Where(x => x.Id == templateId).FirstOrDefault();
                string Name = mailTemplate.Title;
                db.Article.Remove(mailTemplate);
                db.SaveChanges();
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;
                await activityLogService.CreateLog(loggedUser, "Temlate Deleted " + Name + ". Deleted By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }
            
            return false;
        }

        public int GetTemplateCount()
        {
            return db.Article.Where(x => x.Type == "template").Count();
        }
    }
}
