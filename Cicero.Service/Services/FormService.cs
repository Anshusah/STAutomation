using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Extensions;
using Cicero.Service.Models;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Cicero.Service.Models.Core;
using System.Data.SqlClient;
using Cicero.Service.Models.Core.Elements;
using System.Reflection;
using System.Reflection.Emit;
using static Cicero.Service.Enums;
using System.Data;
using Hangfire;
using Permission = Cicero.Service.Helpers.Permission;

namespace Cicero.Service.Services
{

    public interface IFormService
    {
        DTResponseModel GetFormListByFilter(DTPostModel model, List<string> form, int id);
        DTResponseModel GetJsonData(DTPostModel model, List<string> TableList, List<KeyValuePair<string, List<string>>> lstValue, int caseformid, int queueid, bool IsAllowed, List<KeyValuePair<string, string>> dtype, string queueName = "");
        string GenerateFormId();
        bool UpdateFormData(dynamic formCollection, string form, int caseId, bool isNew);
        bool DeleteFromData(FormBuilderViewModel a, int caseId);
        Task<CaseViewModel> SaveCaseAsync(CaseViewModel cvm, string form);
        JObject GetTableData(int caseId, List<FormBuilderViewModel.Form.Table> tables, FormBuilderViewModel a);
        bool SaveOrUpdateTable(string table, bool isCreate, dynamic extraValues, int caseId);
        bool SaveOrUpdateCaseMedia(List<int> mediaIds, int caseId);


    }

    public class FormService : IFormService
    {

        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<FormService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private readonly IActivityLogService activityLogService;
        private readonly ICommonService commonService;
        private readonly IRazorToStringRender razorToStringRender;
        private readonly ITemplateService templateService;
        private readonly AppSetting _setting;
        private readonly IQueueService _queueService;
        private readonly IMessageService _messageService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly ICaseService _caseService;
        private readonly ICiceroCoreFormService _ciceroCoreFormService;
        private readonly IAuditLogService _auditLogService;
        public FormService(ICommonService _commonService, ICiceroCoreFormService ciceroCoreFormService, IFormBuilderService formBuilderService, ICaseService caseService, ApplicationDbContext _db, Utils _utils, ILogger<FormService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, IActivityLogService _activityLogService, IRazorToStringRender _razorToStringRender, ITemplateService _templateService, AppSetting setting, IQueueService queueService, IMessageService messageService,
            IAuditLogService auditLogService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            commonService = _commonService;
            IMapper = _IMapper;
            activityLogService = _activityLogService;
            razorToStringRender = _razorToStringRender;
            templateService = _templateService;
            _setting = setting;
            _queueService = queueService;
            _messageService = messageService;
            _formBuilderService = formBuilderService;
            _caseService = caseService;
            _ciceroCoreFormService = ciceroCoreFormService;
            _auditLogService = auditLogService;
        }

        public JObject GetTableData(int CaseId, List<FormBuilderViewModel.Form.Table> tables, FormBuilderViewModel a)
        {
            string value = String.Empty;
            JObject FormValues = new JObject();
            if (tables != null)
            {
                foreach (var tb in tables)
                {
                    if (Utils.ConvertToString(tb.Name) != "")
                    {
                        bool exists = db.CustomEntities1.FromSql($"SELECT 1 cnt FROM sys.tables AS T  INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id   WHERE  T.Name = '{tb.Name}'", tb.Name).SingleOrDefault() != null;
                        if (exists)
                        {
                            var Forms = db.CustomEntities.FromSql($"SELECT * FROM [{tb.Name}] Where [CaseId] = {CaseId}", tb.Name, CaseId);
                            int j = 0;
                            JObject jsonObj = new JObject();

                            // var temp = Forms.Extras;
                            foreach (var item in Forms)
                            {
                                jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                            }
                            foreach (var item in a.Tab)
                            {
                                foreach (var row in item.Row)
                                {
                                    foreach (var column in row.Column)
                                    {
                                        foreach (dynamic element in column.Element)
                                        {
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
                                                            var isit = tableColElm.GetType().GetProperty("TableName");
                                                            if (isit != null)
                                                            {
                                                                if (tableColElm.TableName == tb.Name)
                                                                {
                                                                    foreach (var data in jsonObj)
                                                                    {
                                                                        if (tableColElm.FieldName == data.Key)
                                                                        {
                                                                            //if (data.Key.Count() > 1)
                                                                            //{

                                                                            //}
                                                                            if (tableColElm.GetType().Name.ToLower() == "media")
                                                                            {
                                                                                FormValues.Add("Media" + tableColElm.ElementId, data.Value);
                                                                            }
                                                                            else
                                                                            {
                                                                                FormValues.Add("elm" + tableColElm.ElementId, data.Value);
                                                                            }

                                                                            break;
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
                                                var isit = element.GetType().GetProperty("TableName");
                                                if (isit != null)
                                                {
                                                    if (element.TableName == tb.Name)
                                                    {

                                                        foreach (var data in jsonObj)
                                                        {
                                                            if (element.FieldName == data.Key)
                                                            {
                                                                //if (data.Key.Count() > 1)
                                                                //{

                                                                //}
                                                                if (element.GetType().Name.ToLower() == "media")
                                                                {
                                                                    FormValues.Add("Media" + element.ElementId, data.Value);
                                                                }
                                                                else
                                                                {
                                                                    FormValues.Add("elm" + element.ElementId, data.Value);
                                                                }

                                                                break;
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


                }
            }

            return FormValues;
        }

        public dynamic GetTableValue(int caseId, string tableName)
        {
            bool exists = db.CustomEntities1.FromSql($"SELECT 1 cnt FROM sys.tables AS T  INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id   WHERE  T.Name = '{tableName}'", tableName).SingleOrDefault() != null;
            if (exists)
            {
                var Forms = db.CustomEntities.FromSql($"SELECT * FROM [{tableName}] Where [CaseId] = {caseId}", tableName, caseId);
                int j = 0;
                JObject jsonObj = new JObject();

                // var temp = Forms.Extras;
                foreach (var item in Forms)
                {
                    jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                }
                ClassBuilder CB = new ClassBuilder(tableName);
                string[] property = new string[jsonObj.Count];
                Type[] propertyType = new Type[jsonObj.Count];
                int k = 0;
                foreach (var item in jsonObj)
                {
                    property[k] = item.Key;
                    propertyType[k] = typeof(string);
                    k++;
                }
                dynamic myclass = CB.CreateObject(property, propertyType);
                foreach (var item in jsonObj)
                {
                    myclass.GetType().GetProperty(item.Key).SetValue(myclass, item.Value.ToString());

                }
                return myclass;
            }
            else
            {
                return null;
            }
        }

        public JObject GetTableData2(int CaseId, string Tablename)
        {
            JObject jsonObj = new JObject();

            if (Utils.ConvertToString(Tablename) != "")
            {
                bool exists = db.CustomEntities1.FromSql($"SELECT 1 cnt FROM sys.tables AS T  INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id   WHERE  T.Name = '{Tablename}'", Tablename).SingleOrDefault() != null;
                if (exists)
                {
                    var Forms = db.CustomEntities.FromSql($"SELECT * FROM [{Tablename}] Where [CaseId] = {CaseId}", Tablename, CaseId);
                    int j = 0;


                    // var temp = Forms.Extras;
                    foreach (var item in Forms)
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                    }

                }
            }
            return jsonObj;

        }
        public DTResponseModel GetFormListByFilter(DTPostModel model, List<string> tablename, int id)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "createdOrder";
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
                    if (sortBy == "createdAt")
                    {
                        sortBy = "createdOrder";
                    }
                }
            }

            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            string loggedUser = commonService.getLoggedInUserId();

            string roleid = commonService.GetRoleIdByUserId(loggedUser);

            bool isAdmin = commonService.IsSuperAdmin().Result;

            var tbl = tablename[0];
            var Forms = db.CustomEntities.FromSql($"SELECT [t3].[Id], [t3].[Status], [t3].[CreatedAt], [t3].[UpdatedAt], [t3].[UserId], [t3].[Extras], [t3].[Order], [t3].[CaseId], [t3].[CoreCaseTableId], [t3].[TenantId], [t3].[JsonExtras] FROM {tbl} AS [t3]", tbl);


            for (int i = 1; i < tablename.Count(); i++)
            {
                tbl = tablename[i];

                Forms = Forms.Concat(db.CustomEntities.FromSql($"SELECT [t3].[Id], [t3].[Status], [t3].[CreatedAt], [t3].[UpdatedAt], [t3].[UserId], [t3].[Extras], [t3].[Order], [t3].[CaseId], [t3].[CoreCaseTableId], [t3].[TenantId], [t3].[JsonExtras] FROM {tbl} AS [t3]", tbl));

            }

            foreach (var item in Forms)
            {
                var temp = item.Extras;
            }



            if (!String.IsNullOrEmpty(searchBy))
            {
                //Forms = Forms.Where(o => o.CreatedBy.ToLower().Contains(searchBy.ToLower()) || o.fullName.Contains(searchBy.ToLower()));
            }

            totalResultsCount = Forms.Count();
            //Forms = Forms.OrderBy(sortBy, sortDir).Skip(skip).Take(take);

            var list = Forms.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public string GenerateFormId()
        {

            string generatedCaseId = string.Empty;
            Random rand = new Random();
            generatedCaseId = Utils.GetDateForCase() + rand.Next(100001, 999999);
            return generatedCaseId;

        }

        public async Task<CaseViewModel> SaveCaseAsync(CaseViewModel cvm, string form)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
            cvm.CaseFormId = ccvm.Id;
            cvm = await _caseService.CreateOrUpdate(cvm);
            return cvm;
        }


        public bool UpdateFormData(dynamic formCollection, string form, int caseId, bool isNew)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
            string value = String.Empty;
            try
            {
                foreach (var tb in tables)
                {
                    // dynamic finalValues  ;
                    JObject tableRowValues = new JObject();
                    if (isNew == false)
                    {
                        tableRowValues = GetTableData2(caseId, tb.Name);
                    }
                    foreach (var item in a.Tab)
                    {
                        foreach (var row in item.Row)
                        {
                            bool isRepeatRow = false;
                            if(row.SetAsRepeatItem)
                            {
                                isRepeatRow = row.SetAsRepeatItem;
                            }
                            foreach (var column in row.Column)
                            {
                                foreach (dynamic element in column.Element)
                                {
                                    string key = "elm" + element.ElementId;
                                    string key1 = "Media" + element.ElementId;
                                    //List<FormBuilderViewModel.Form.Table> elementTable = element.ModelData.Forms.Tables;
                                    string type = element.Type.ToString();
                                    if (type.Contains("Table")) // if element is a table element
                                    {
                                        if (element.Column != null)
                                        {
                                            foreach (var tableColumn in element.Column)
                                            {
                                                dynamic tableColElm = tableColumn.ColumnElement;
                                                key = "elm" + tableColElm.ElementId;
                                                key1 = "Media" + tableColElm.ElementId;
                                                foreach (var form1 in formCollection)
                                                {
                                                    if (form1.Key.Contains("[]"))
                                                    {
                                                        if (form1.Key == key + "[]" || form1.Key == key1 + "[]")
                                                        {
                                                            JObject abc = new JObject();
                                                            int i = 0;
                                                            foreach (var v in form1.Value)
                                                            {

                                                                var isit = tableColElm.GetType().GetProperty("TableName");
                                                                if (isit != null)
                                                                {
                                                                    if (tableColElm.TableName == tb.Name)
                                                                    {
                                                                        abc.Add(i.ToString(), v);
                                                                    }
                                                                }

                                                                i++;
                                                            }
                                                            // value = form1.Value;
                                                            if (tableColElm.TableName == tb.Name)
                                                            {
                                                                if (tableColElm.FieldName != null)
                                                                {
                                                                    if (tableRowValues.ContainsKey(tableColElm.FieldName))
                                                                    {
                                                                        tableRowValues[tableColElm.FieldName] = abc;
                                                                    }
                                                                    else
                                                                    {
                                                                        tableRowValues.Add(tableColElm.FieldName, abc);
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else if (form1.Key.Contains("["))
                                                    {
                                                        string keyStr = form1.Key.ToString();
                                                        string checkStr = keyStr.Substring(0, keyStr.IndexOf("["));
                                                        value = form1.Value;
                                                        if (checkStr == key)
                                                        {

                                                            int start = keyStr.IndexOf("[");
                                                            int end = keyStr.IndexOf("]");
                                                            string indexVal = keyStr.Substring(start + 1, end - (start + 1));
                                                            JObject abc = new JObject();
                                                            var isit = tableColElm.GetType().GetProperty("TableName");
                                                            if (isit != null)
                                                            {
                                                                if (tableColElm.TableName == tb.Name)
                                                                {
                                                                    if (tableColElm.FieldName != null)
                                                                    {
                                                                        if (tableRowValues.ContainsKey(tableColElm.FieldName))//check if contains Field name
                                                                        {
                                                                            abc = tableRowValues[tableColElm.FieldName]; //value to JObject;
                                                                            if (abc.ContainsKey(indexVal)) //check if Jobject contains currentindex
                                                                            {
                                                                                abc[indexVal] = value;
                                                                            }
                                                                            else
                                                                            {
                                                                                abc.Add(indexVal, value);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            abc.Add(indexVal.ToString(), value);
                                                                            tableRowValues.Add(tableColElm.FieldName, abc);
                                                                        }
                                                                    }


                                                                }
                                                            }


                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (form1.Key == key || form1.Key == key1)
                                                        {
                                                            value = form1.Value;
                                                            var isit = tableColElm.GetType().GetProperty("TableName");
                                                            if (isit != null)
                                                            {
                                                                if (tableColElm.TableName == tb.Name)
                                                                {
                                                                    if (tableRowValues.ContainsKey(tableColElm.FieldName))
                                                                    {
                                                                        tableRowValues[tableColElm.FieldName] = value;
                                                                    }
                                                                    else
                                                                    {
                                                                        tableRowValues.Add(tableColElm.FieldName, value);
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }

                                                }


                                            }

                                        }

                                    }

                                    else if(isRepeatRow)
                                    {
                                        foreach (var form1 in formCollection)
                                        {
                                            if (form1.Key.Contains("[]"))
                                            {
                                                if (form1.Key == key + "[]" || form1.Key == key1 + "[]")
                                                {
                                                    JObject abc = new JObject();
                                                    int i = 0;
                                                    foreach (var v in form1.Value)
                                                    {

                                                        var isit = element.GetType().GetProperty("TableName");
                                                        if (isit != null)
                                                        {
                                                            if (element.TableName == tb.Name)
                                                            {
                                                                abc.Add(i.ToString(), v);
                                                            }
                                                        }

                                                        i++;
                                                    }
                                                    // value = form1.Value;
                                                    if (element.TableName == tb.Name)
                                                    {
                                                        if (element.FieldName != null)
                                                        {
                                                            if (tableRowValues.ContainsKey(element.FieldName))
                                                            {
                                                                tableRowValues[element.FieldName] = abc;
                                                            }
                                                            else
                                                            {
                                                                tableRowValues.Add(element.FieldName, abc);
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            else if (form1.Key.Contains("["))
                                            {
                                                string keyStr = form1.Key.ToString();
                                                string checkStr = keyStr.Substring(0, keyStr.IndexOf("["));
                                                value = form1.Value;
                                                if (checkStr == key || checkStr == key1)
                                                {

                                                    int start = keyStr.IndexOf("[");
                                                    int end = keyStr.IndexOf("]");
                                                    string indexVal = keyStr.Substring(start + 1, end - (start + 1));
                                                    JObject abc = new JObject();
                                                    var isit = element.GetType().GetProperty("TableName");
                                                    if (isit != null)
                                                    {
                                                        if (element.TableName == tb.Name)
                                                        {
                                                            if (element.FieldName != null)
                                                            {
                                                                if (tableRowValues.ContainsKey(element.FieldName))//check if contains Field name
                                                                {
                                                                    abc = tableRowValues[element.FieldName]; //value to JObject;
                                                                    if (abc.ContainsKey(indexVal)) //check if Jobject contains currentindex
                                                                    {
                                                                        abc[indexVal] = value;
                                                                    }
                                                                    else
                                                                    {
                                                                        abc.Add(indexVal, value);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    abc.Add(indexVal.ToString(), value);
                                                                    tableRowValues.Add(element.FieldName, abc);
                                                                }
                                                            }


                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                if (form1.Key == key || form1.Key == key1)
                                                {
                                                    value = form1.Value;
                                                    var isit = element.GetType().GetProperty("TableName");
                                                    if (isit != null)
                                                    {
                                                        if (element.TableName == tb.Name)
                                                        {
                                                            if (tableRowValues.ContainsKey(element.FieldName))
                                                            {
                                                                tableRowValues[element.FieldName] = value;
                                                            }
                                                            else
                                                            {
                                                                tableRowValues.Add(element.FieldName, value);
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var form1 in formCollection)
                                        {
                                            if (form1.Key.Contains("[]"))
                                            {
                                                if (form1.Key == key + "[]" || form1.Key == key1 + "[]")
                                                {
                                                    JObject abc = new JObject();
                                                    int i = 0;
                                                    foreach (var v in form1.Value)
                                                    {

                                                        var isit = element.GetType().GetProperty("TableName");
                                                        if (isit != null)
                                                        {
                                                            if (element.TableName == tb.Name)
                                                            {
                                                                abc.Add(i.ToString(), v);
                                                            }
                                                        }

                                                        i++;
                                                    }
                                                    // value = form1.Value;
                                                    if (element.TableName == tb.Name)
                                                    {
                                                        if (element.FieldName != null)
                                                        {
                                                            if (tableRowValues.ContainsKey(element.FieldName))
                                                            {
                                                                tableRowValues[element.FieldName] = abc;
                                                            }
                                                            else
                                                            {
                                                                tableRowValues.Add(element.FieldName, abc);
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            else if (form1.Key.Contains("["))
                                            {
                                                string keyStr = form1.Key.ToString();
                                                string checkStr = keyStr.Substring(0, keyStr.IndexOf("["));
                                                if (checkStr == key)
                                                {

                                                    int start = keyStr.IndexOf("[");
                                                    int end = keyStr.IndexOf("]");
                                                    string indexVal = keyStr.Substring(start + 1, end - (start + 1));
                                                    JObject abc = new JObject();
                                                    var isit = element.GetType().GetProperty("TableName");
                                                    if (isit != null)
                                                    {
                                                        if (element.TableName == tb.Name)
                                                        {
                                                            if (element.FieldName != null)
                                                            {
                                                                if (tableRowValues.ContainsKey(element.FieldName))//check if contains Field name
                                                                {
                                                                    abc = tableRowValues[element.FieldName]; //value to JObject;
                                                                    if (abc.ContainsKey(indexVal)) //check if Jobject contains currentindex
                                                                    {
                                                                        abc[indexVal] = form1.Value;
                                                                    }
                                                                    else
                                                                    {
                                                                        abc.Add(indexVal, form1.Value);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    abc.Add(indexVal, form1.Value);
                                                                    tableRowValues.Add(element.FieldName, abc);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (form1.Key == key || form1.Key == key1)
                                                {
                                                    value = form1.Value;
                                                    var isit = element.GetType().GetProperty("TableName");
                                                    if (isit != null)
                                                    {
                                                        if (element.TableName == tb.Name)
                                                        {
                                                            if (tableRowValues.ContainsKey(element.FieldName))
                                                            {
                                                                tableRowValues[element.FieldName] = value;
                                                            }
                                                            else
                                                            {
                                                                tableRowValues.Add(element.FieldName, value);
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
                    dynamic oldClass = GetTableValue(caseId, tb.Name);
                    SaveOrUpdateTable(tb.Name, isNew, tableRowValues, caseId);
                    ClassBuilder CB = new ClassBuilder(tb.Name);
                    string[] property = new string[tableRowValues.Count];
                    Type[] propertyType = new Type[tableRowValues.Count];
                    int k = 0;
                    foreach (var item in tableRowValues)
                    {
                        property[k] = item.Key;
                        propertyType[k] = typeof(string);
                        k++;
                    }
                    dynamic myclass = CB.CreateObject(property, propertyType);
                    Case casedata = db.Case.Where(x => x.Id == caseId).SingleOrDefault();
                    foreach (var item in tableRowValues)
                    {
                        myclass.GetType().GetProperty(item.Key).SetValue(myclass, item.Value.ToString());
                    }
                    if (isNew)
                    {
                        try
                        {
                            var result = _auditLogService.LogWriter(myclass, (int)AuditLogOperation.Insert, caseId.ToString(), new Case(), oldClass).Result;
                        }
                        catch(Exception ex)
                        {

                        }
                       
                    }
                    else
                    {
                        try
                        {
                            var result = _auditLogService.LogWriter(myclass, (int)AuditLogOperation.Update, caseId.ToString(), new Case(), oldClass).Result;
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return true;
        }

        public bool SaveOrUpdateTable(string table, bool isCreate, dynamic extraValues, int caseId)
        {
            bool success = false;
            string sqlQuery = string.Empty;
            string loggedUser = commonService.getLoggedInUserId();
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            var coreCaseTable = _ciceroCoreFormService.GetCoreCaseTableByName(table, tenantid);
            var abc = JsonConvert.SerializeObject(extraValues);
            try
            {
                if (isCreate)
                {
                    sqlQuery = CreateNew(table);
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@p1", DateTime.Now),
                new SqlParameter("@p2",DateTime.Now),
                new SqlParameter("@p3",loggedUser),
                new SqlParameter("@p4",tenantid),
                new SqlParameter("@p5",(int)coreCaseTable.Id),
                new SqlParameter("@p6",abc),
                new SqlParameter("@p7",'0'),
                new SqlParameter("@p8",'0'),
                new SqlParameter("@p9",(int)caseId),
            };
                    db.Database.ExecuteSqlCommand(sqlQuery, parameters);
                    db.SaveChanges();
                }
                else
                {
                    sqlQuery = UpdateData(table, caseId);
                    SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@p1", DateTime.Now),
                new SqlParameter("@p2",abc),
                new SqlParameter("@p3",loggedUser)
                };
                    db.Database.ExecuteSqlCommand(sqlQuery, parameters);
                    db.SaveChanges();
                }
                success = true;
                return success;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private string CreateNew(string table)
        {
            string query = string.Format(@"INSERT INTO[dbo].[{0}]
                ([CreatedAt]
                ,[UpdatedAt]     
                ,[UserId]
                ,[TenantId]
                ,[CoreCaseTableId]
                ,[Extras]
                ,[Order]
                ,[Status],
                [CaseId])
                 VALUES
                 (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8,@p9)", table);
            return query;
        }

        private string UpdateData(string table, int id)
        {
            string query = string.Format(@"
                                UPDATE [dbo].[{0}]
                                SET 
                                [UpdatedAt] = @p1
                                ,[Extras] = @p2
                                ,[UserId] = @p3
                                WHERE [CaseId] = {1}
                                ", table, id);
            return query;
        }
        private string DeleteData(string table, int id)
        {
            string query = string.Format(@"Delete from [dbo].[{0}]
               WHERE [CaseId] = {1}
                                ", table, id);
            return query;
        }

        public DTResponseModel GetJsonData(DTPostModel model, List<string> TableList, List<KeyValuePair<string, List<string>>> lstValue, int caseformid, int queueid, bool IsAllowed, List<KeyValuePair<string, string>> dtype, string queueName = "")
        {

            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = true;
            string sortOrder = "asc";

            int draw = 0;
            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            var loggeduser = commonService.getLoggedInUserId();
            string roleid = commonService.GetRoleIdByUserId(loggeduser);
            var isadmin = commonService.IsSuperAdmin().Result;
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
                    sortOrder = model.order[0].dir.ToLower();
                }
            }
            List<JObject> lsttableRowValues = new List<JObject>();
            List<JObject> finalList = new List<JObject>();
            JArray finalData = new JArray();
            int i = 0;
            try
            {
                foreach (string st in TableList.Distinct())
                {

                    if (st != null)
                    {
                        var result = from val in lstValue where val.Key == st select val.Value.ToList();

                        var cases = _caseService.GetFormIdsByQueue(queueid, caseformid);
                        var tempcases = string.Join(",", cases.Select(x => x.Key).ToArray());

                        var Forms = db.CustomEntities.FromSql($"SELECT * FROM {st} AS [t3] ORDER BY id OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY", st, skip, take);

                        if (queueid != 0)
                        {
                            Forms = db.CustomEntities.FromSql($"SELECT * FROM {st} AS [t3] WHERE CaseId IN({tempcases})", st, tempcases);
                        }
                        var Forms1 = Forms.OrderByDescending(x => x.UpdatedAt).ToList();
                        if (sortBy == "updatedAt" && sortOrder == "asc")
                        {
                            Forms1 = Forms.OrderBy(x => x.UpdatedAt).ToList();
                        }
                        else if (sortBy == "createdAt" && sortOrder == "asc")
                        {
                            Forms1 = Forms.OrderBy(x => x.CreatedAt).ToList();
                        }
                        else if (sortBy == "createdAt" && sortOrder == "desc")
                        {
                            Forms1 = Forms.OrderByDescending(x => x.CreatedAt).ToList();
                        }
                        else
                        {

                        }
                        totalResultsCount = cases.Count;
                        filteredResultsCount = Forms.Count();
                        if (i == 0)
                        {
                            foreach (var item in Forms1)
                            {
                                JObject tableRowValues = new JObject();
                                var temp = item.Extras;
                                dynamic jsonObj = JsonConvert.DeserializeObject(temp);
                                tableRowValues.Add("id", Utils.ConvertToString(item.CaseId));
                                string caseGeneratedId = cases.Where(x => x.Key == Convert.ToInt16(item.CaseId)).SingleOrDefault().Value;
                                tableRowValues.Add("caseId", Utils.ConvertToString(caseGeneratedId));
                                Int32.TryParse(item.CaseId, out int intcaseid);
                                string assignedTo = db.Case.Where(x => x.Id == intcaseid).Select(x => x.AssignedTo).FirstOrDefault();
                                if (string.IsNullOrWhiteSpace(assignedTo))
                                {
                                    tableRowValues.Add("assignedTo", "");
                                }
                                else
                                {
                                    var userDetails = commonService.GetUserById(assignedTo).Result;
                                    tableRowValues.Add("assignedTo", userDetails == null ? "" : userDetails.FirstName + " " + userDetails.LastName);
                                }
                                tableRowValues.Add("createdAt", Utils.GetDefaultDateFormatToDetail(item.CreatedAt));
                                tableRowValues.Add("updatedAt", Utils.GetDefaultDateFormatToDetail(item.UpdatedAt));
                                Int32.TryParse(item.CaseId, out int caseid_int);
                                List<SelectListItem> states = _queueService.GetStateSelectListByFormId(caseformid, roleid);
                                dynamic state1 = db.WorkFlowState
                                                .Where(y => y.CaseFormId == caseformid)
                                                .Join(db.Case, sts => sts.FromStateId, cas => cas.StateId, (a, b) => new { sts = a, cas = b })
                                                .Where(z => z.cas.Id == caseid_int)
                                                .Join(db.StateForForm, sts => sts.sts.FromStateId, sff => sff.StateId, (c, d) => new { sts = c, sff = d })
                                                .GroupJoin(db.StatePermission, sff => sff.sff.Id, sp => sp.StateForFormId, (e, f) => new { sff = e, sp = f })
                                                .SelectMany(v => v.sp.DefaultIfEmpty(), (e, f) => new { sff = e.sff, sp = f })
                                                .Where(x => x.sff.sff.CaseFormId == caseformid && (isadmin == true || x.sff.sff.AllUser == true || x.sff.sff.StatePermissions.Any(u => u.RoleId == roleid)))
                                                .Select(w => new { id = w.sff.sts.sts.ToStateId, name = "" })
                                                .Distinct().ToList();
                                List<JObject> state = new List<JObject>();
                                foreach (var a1 in state1)
                                {
                                    var statename = states.Where(x => x.Value.Equals(a1.id.ToString(), StringComparison.OrdinalIgnoreCase)).Select(x => x.Text).SingleOrDefault();

                                    JObject tem1 = new JObject();
                                    tem1.Add("id", a1.id);
                                    tem1.Add("name", statename + " : Selected");
                                    state.Add(tem1);
                                }
                                tableRowValues.Add("states", JsonConvert.SerializeObject(state));
                                foreach (var val in result)
                                {
                                    string chkval = "";
                                    int icnt = 0;
                                    foreach (var obj in jsonObj)
                                    {
                                        icnt++;
                                        chkval = val[1];
                                        if (obj.Name == val[1])
                                        {
                                            tableRowValues.Add(val[0], obj.Value);
                                            chkval = "";
                                            break;
                                        }
                                    }
                                    if (chkval != "" || icnt == 0)
                                    {
                                        tableRowValues.Add(val[0], "");
                                    }
                                }
                                var rows = "";
                                if (IsAllowed)
                                {
                                    rows = "<div class='dropdown'><a class='btn btn-light' type='button' id='dropdownMenuButton' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='ri-more-2-fill'></i></a><div class='dropdown-menu dropdown-menu-right' aria-labelledby='dropdownMenuButton'><a href='/admin/form/" + Utils.GetTenantForUrl(true) + Utils.GetParams("form") + "/" + Utils.EncryptId(Convert.ToInt32(item.CaseId)) + "/edit.html' class='dropdown-item' title='Edit Form'><i class='ri-pencil-line'></i> Edit</a>" +
                                    "<a href='/admin/formview" + "/" + Utils.EncryptId(Convert.ToInt32(item.CaseId)) + "/" + queueName + "/view.html' class='dropdown-item' title='View'><i class='ri-eye-line'></i> View</a></div></div>";
                                }
                                else
                                {
                                    rows = "<div class='dropdown'><a class='btn btn-light' type='button' id='dropdownMenuButton' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='ri-more-2-fill'></i></a><div class='dropdown-menu dropdown-menu-right' aria-labelledby='dropdownMenuButton'>"+
                                   "<a href='/admin/formview" + "/" + Utils.EncryptId(Convert.ToInt32(item.CaseId)) + "/" + queueName + "/view.html' class='dropdown-item' title='View'><i class='ri-eye-line'></i> View</a></div></div>";
                                }
                                tableRowValues.Add("edit", rows);
                                lsttableRowValues.Add(tableRowValues);
                            }
                        }

                        else
                        {
                            int j = 0;
                            foreach (var item in Forms1)
                            {
                                JObject tableRowValues = new JObject();
                                var temp = item.Extras;
                                dynamic jsonObj = JsonConvert.DeserializeObject(temp);

                                foreach (var val in result)
                                {
                                    string chkval = "";
                                    int icnt = 0;
                                    foreach (var obj in jsonObj)
                                    {
                                        icnt++;
                                        chkval = val[1];
                                        if (obj.Name == val[1])
                                        {
                                            lsttableRowValues[j].Add(val[0], obj.Value);
                                            chkval = "";
                                            break;
                                        }
                                    }
                                    if (chkval != "" || icnt == 0)
                                    {
                                        lsttableRowValues[j].Add(val[0], "");
                                    }
                                }
                                j++;
                            }

                        }
                        i++;
                    }
                }

                if (searchBy != null) // search
                {
                    Parallel.ForEach(lsttableRowValues, (item) =>
                    {
                        foreach (var key in item)
                        {
                            if (key.Key.ToString() != "edit" && key.Key.ToString() != "states")
                            {
                                if (key.Value.ToString().ToLower().Contains(searchBy.ToLower()))
                                {
                                    finalList.Add(item);
                                }
                            }
                        }
                    }
                    );
                }
                else
                {
                    finalList = lsttableRowValues;
                }
                List<string> dateCols = new List<string>();
                finalList = finalList.Distinct().ToList();
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(finalList.ToJson());
                DataTable dtCloned = dt.Clone();
                DataTable dtFinal = dt.Clone();
                foreach (DataColumn col in dtCloned.Columns)
                {
                    foreach (var item in dtype)
                    {
                        if (col.Caption == item.Key.ToString())
                        {
                            switch (item.Value)
                            {
                                case string a when a.Contains("currency"):
                                case "decimal":
                                    dtCloned.Columns[col.Caption].DataType = typeof(Decimal);
                                    break;
                                case "datetime":
                                    dtCloned.Columns[col.Caption].DataType = typeof(DateTime);
                                    dtCloned.Columns[col.Caption].AllowDBNull = true;
                                    dateCols.Add(col.Caption);
                                    break;
                            }
                        }
                    }
                }
                foreach (DataRow row in dt.Rows)
                {
                    DataRow row1 = dtCloned.NewRow();
                    for (int j = 0; j < row.ItemArray.Count(); j++)
                    {
                        bool found = false;
                        foreach (var item in dtype)
                        {
                            if (dtCloned.Columns[j].Caption == item.Key.ToString())
                            {
                                found = true;
                                switch (item.Value)
                                {
                                    case string a when a.Contains("currency"):
                                    case "decimal":
                                        if (row[j] != null && row[j].ToString() != "" && row[j].ToString() != string.Empty)
                                        {
                                            row1[j] = Convert.ToDecimal(row[j]);
                                        }

                                        break;
                                    case "datetime":
                                        if (row[j] != null && row[j].ToString() != "" && row[j].ToString() != string.Empty)
                                        {
                                            try
                                            {
                                                row1[j] = DateTime.ParseExact(row[j].ToString(), "d/M/yyyy HH:mm:ss tt", null);
                                            }
                                            catch (Exception ex)
                                            {
                                                row1[j] = Convert.ToDateTime(row[j]);
                                            }
                                        }
                                        break;
                                    default:
                                        row1[j] = row[j];
                                        break;
                                }
                                break;
                            }

                        }
                        if (!found)
                        {
                            row1[j] = row[j];
                        }

                    }
                    dtCloned.Rows.Add(row1);
                }
                if (sortBy != "updatedAt" && sortBy != "createdAt") // sorting
                {
                    dtCloned.DefaultView.Sort = sortBy + " " + sortOrder;
                    dtCloned = dtCloned.DefaultView.ToTable();
                }

                //final format
                foreach (DataRow row in dtCloned.Rows)
                {
                    DataRow row1 = dtFinal.NewRow();
                    for (int j = 0; j < row.ItemArray.Count(); j++)
                    {
                        bool found = false;
                        foreach (var item in dtype)
                        {
                            if (dtCloned.Columns[j].Caption == item.Key.ToString())
                            {
                                found = true;
                                switch (item.Value)
                                {
                                    case string a when a.Contains("currency"):
                                        if (row[j] != null && row[j].ToString() != "" && row[j].ToString() != string.Empty)
                                        {
                                            string cur = item.Value.Split("_")[1];
                                            row1[j] = cur + " " + Convert.ToDecimal(row[j]).ToString("#,##0.00");
                                        }
                                        else
                                        {
                                            row1[j] = row[j].ToString();
                                        }
                                        break;
                                    case "decimal":

                                        row1[j] = row[j].ToString();
                                        break;
                                    case "datetime":
                                        if (row[j] != null && row[j].ToString() != "" && row[j].ToString() != string.Empty)
                                        {
                                            row1[j] = Utils.GetDefaultDateFormatToDetail(Convert.ToDateTime(row[j]));
                                        }
                                        break;
                                    default:
                                        row1[j] = row[j];
                                        break;
                                }
                                break;
                            }

                        }
                        if (!found)
                        {
                            row1[j] = row[j];
                        }

                    }
                    dtFinal.Rows.Add(row1);
                }
                List<DataRow> RowsToDelete = new List<DataRow>();

                for (int l = 0; l < dtFinal.Rows.Count; l++)
                {
                    if (dtFinal.Rows[l].ItemArray[0].ToString() == "")
                    {
                        RowsToDelete.Add(dtFinal.Rows[l]);
                    }
                }
                foreach (var dr in RowsToDelete)
                {
                    dtFinal.Rows.Remove(dr);
                }


                string tmp = JsonConvert.SerializeObject(dtFinal);
                finalData = (JArray)JsonConvert.DeserializeObject(tmp);


            }
            catch (Exception ex)
            {
                return new DTResponseModel
                {
                    draw = draw,
                    recordsTotal = lsttableRowValues.Count,
                    recordsFiltered = finalList.Count,
                    data = finalList.Skip(skip).Take(take)
                };
            }

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = lsttableRowValues.Count,
                recordsFiltered = finalList.Count,
                data = finalData.Skip(skip).Take(take),

            };
        }

        ///Delete Case 
        ///

        public bool DeleteFromData(FormBuilderViewModel a, int caseId)
        {

            List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
            try
            {
                foreach (var tb in tables)
                {

                    DeleteTable(tb.Name, caseId);
                }
            }
            catch (Exception ex)
            {

            }

            return true;
        }

        private bool DeleteTable(string table, int caseId)
        {
            bool success = false;
            string sqlQuery = string.Empty;
            sqlQuery = DeleteData(table, caseId);
            db.Database.ExecuteSqlCommand(sqlQuery);
            db.SaveChanges();

            success = true;
            return success;
        }

        public bool SaveOrUpdateCaseMedia(List<int> mediaIds, int caseId)
        {
            try
            {
                var checkCaseMediaDatas = db.CaseMedia.Where(x => x.CaseId == caseId).ToList();
                if (checkCaseMediaDatas.Count > 0)
                {
                    db.CaseMedia.RemoveRange(checkCaseMediaDatas);
                }

                var newCaseMediaDatas = new List<CaseMedia>();
                foreach (var item in mediaIds)
                {
                    newCaseMediaDatas.Add(new CaseMedia
                    {
                        MediaId = item,
                        CaseId = caseId
                    });
                }

                if (newCaseMediaDatas.Count > 0)
                {
                    db.CaseMedia.AddRange(newCaseMediaDatas);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
