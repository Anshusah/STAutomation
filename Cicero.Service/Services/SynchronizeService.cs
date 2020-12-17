using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cicero.Service.Services.Core.Themes.Services;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Models.Core.FormBuilderViewModel;
using static Cicero.Service.Models.Core.FormBuilderViewModel.Form;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Cicero.Data.Entities;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;
using System.Threading.Tasks;
using Cicero.Service.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cicero.Service.Services
{

    public interface ISynchronizeService
    {
        int SynchronizeCaseAsync(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId, WorkflowObject obj, string baseApiUrl);
        List<SelectListItem> getSyncSourceTable();
        List<SelectListItem> getSyncSourceTableColumns(string tableName);
        string getComponets(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId);
        List<SelectListItem> GetSyncAllTables(int formId);
        List<SelectListItem> getSyncSourceTableColumnsAll(string tableName);
        object CallAPI(dynamic _new, WorkRun work, ElementWorkflowObject obj, string baseApiUrl);

    }
    public class SynchronizeService : ISynchronizeService
    {
        [JsonIgnore]
        public HttpContext HttpContext;
        public Theme Theme;
        private readonly ApplicationDbContext _db;
        private readonly Utils _utils;
        private readonly ILogger<SynchronizeService> _log;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;
        private readonly ICommonService _commonService;
        private readonly IRazorToStringRender _razorToStringRender;
        private readonly ITemplateService _templateService;
        private readonly AppSetting _setting;
        private readonly IQueueService _queueService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IMessageService _messageService;
        private readonly IFormService _formService;

        public SynchronizeService(ICommonService commonService, IFormService formService, IFormBuilderService formBuilderService, ApplicationDbContext db, Utils utils, ILogger<SynchronizeService> log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, IActivityLogService activityLogService, IRazorToStringRender razorToStringRender, ITemplateService templateService, AppSetting setting, IQueueService queueService, IMessageService messageService)
        {
            _db = db;
            _utils = utils;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _commonService = commonService;
            _mapper = mapper;
            _activityLogService = activityLogService;
            _razorToStringRender = razorToStringRender;
            _templateService = templateService;
            _setting = setting;
            _queueService = queueService;
            _messageService = messageService;
            _formBuilderService = formBuilderService;
            _formService = formService;
        }


        /// <summary>
        /// GetTables and Data from caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        private List<FormBuilderViewModel.Form.Table> GetTables(int caseFormId)
        {
            var ccvm = _formBuilderService.GetBuilderFormById(caseFormId);
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
                            var Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tb.Name}] Where [CaseId] = {caseId}", tb.Name, caseId);
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

        private Tuple<List<JObject>, List<CustomEntity>> GetTableDataAll(int caseId, List<FormBuilderViewModel.Form.Table> table)
        {
            List<JObject> tablesA = new List<JObject>();
            List<CustomEntity> Forms = new List<CustomEntity>();

            try
            {
                if (table != null)
                {
                    foreach (var tb in table)
                    {
                        if (Utils.ConvertToString(tb.Name) != "")
                        {
                            Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tb.Name}] Where [CaseId] = {caseId}", tb.Name, caseId).ToList();
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
            return new Tuple<List<JObject>, List<CustomEntity>>(tablesA, Forms);
        }

        private DataTable GetDataTableFromParams(JArray item)
        {
            string table = string.Empty;
            DataTable dataTable = null;
            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            string CommandText = "SELECT * FROM ";
            try
            {
                foreach (JObject obj in item)
                {

                    int i = 1;
                    foreach (var data in obj)
                    {
                        if (i == 1)
                        {
                            table = data.Key.ToString();
                            dataTable = new DataTable(table);
                            fields.Add(data.Value.ToString());
                        }
                        else
                        {
                            values.Add(data.Value.ToString());
                        }
                        i++;
                        // break;
                    }
                }
                //generate query
                CommandText = CommandText + table + " WHERE ";
                for (int i = 0; i < fields.Count; i++)
                {
                    JObject ob = new JObject();
                    string temp = "(";
                    if (i == fields.Count - 1)
                    {
                        try
                        {
                            ob = (JObject)JsonConvert.DeserializeObject(values[i]);

                            for (int j = 0; j < ob.Count; j++)
                            {
                                string key = Convert.ToString(j);
                                if (j == ob.Count - 1)
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " )";
                                }
                                else
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " OR ";
                                }
                            }
                            CommandText = CommandText + temp;

                        }
                        catch (Exception ex)
                        {
                            if (values[i] != "Null" && values[i] != null)
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "=" + "'" + values[i] + "'";
                            }
                            else
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "Is Not Null";
                            }
                        }

                    }
                    else
                    {
                        try
                        {
                            ob = (JObject)JsonConvert.DeserializeObject(values[i]);
                            for (int j = 0; j < ob.Count; j++)
                            {
                                string key = Convert.ToString(j);
                                if (j == ob.Count - 1)
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " )";
                                }
                                else
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " OR ";
                                }
                            }
                            CommandText = CommandText + temp + " AND ";

                        }
                        catch (Exception ex)
                        {

                            if (values[i] != "Null" && values[i] != null)
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "=" + "'" + values[i] + "'" + " AND ";
                            }
                            else
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "Is Not Null AND";
                            }
                        }

                    }

                }
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = CommandText;
                    _db.Database.OpenConnection();
                    SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
                    SqlCommand sql = new SqlCommand(command.CommandText, connection);
                    using (SqlDataAdapter result = new SqlDataAdapter(sql))
                    {
                        result.Fill(dataTable);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable;
            }
            else
            {
                return new DataTable();
            }
        }

        public DataTable CheckTableFieldValues(int id, List<JObject> tablesB, JArray item)
        {

            string table = string.Empty;
            DataTable dataTable = null;
            List<string> fields = new List<string>();
            List<string> values = new List<string>();

            string CommandText = "SELECT * FROM ";
            try
            {
                foreach (JObject obj in item)
                {
                    foreach (var data in obj)
                    {
                        table = data.Key.ToString();
                        dataTable = new DataTable(table);
                        fields.Add(data.Value.ToString());
                        break;
                    }

                    foreach (var item1 in tablesB)
                    {
                        if (obj.ContainsKey(item1["tableName"].ToString()))
                        {
                            string key = item1["tableName"].ToString();
                            foreach (var obj1 in item1)
                            {
                                if (obj1.Key == obj[key].ToString())
                                {
                                    values.Add(obj1.Value.ToString());
                                    break;
                                }
                            }
                        }
                    }

                }

                //generate query
                CommandText = CommandText + table + " WHERE ";
                for (int i = 0; i < fields.Count; i++)
                {
                    JObject ob = new JObject();
                    string temp = "(";
                    if (i == fields.Count - 1)
                    {
                        try
                        {
                            ob = (JObject)JsonConvert.DeserializeObject(values[i]);

                            for (int j = 0; j < ob.Count; j++)
                            {
                                string key = Convert.ToString(j);
                                if (j == ob.Count - 1)
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " )";
                                }
                                else
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " OR ";
                                }
                            }
                            CommandText = CommandText + temp;

                        }
                        catch (Exception ex)
                        {
                            if (values[i] != "Null" && values[i] != null)
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "=" + "'" + values[i] + "'";
                            }
                            else
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "Is Not Null";
                            }
                        }

                    }
                    else
                    {
                        try
                        {
                            ob = (JObject)JsonConvert.DeserializeObject(values[i]);
                            for (int j = 0; j < ob.Count; j++)
                            {
                                string key = Convert.ToString(j);
                                if (j == ob.Count - 1)
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " )";
                                }
                                else
                                {
                                    temp = temp + "[" + fields[i] + "]" + "=" + "'" + ob[key] + "'" + " OR ";
                                }
                            }
                            CommandText = CommandText + temp + " AND ";

                        }
                        catch (Exception ex)
                        {

                            if (values[i] != "Null" && values[i] != null)
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "=" + "'" + values[i] + "'" + " AND ";
                            }
                            else
                            {
                                CommandText = CommandText + "[" + fields[i] + "]" + "Is Not Null AND";
                            }
                        }

                    }

                }
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = CommandText;
                    _db.Database.OpenConnection();
                    SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
                    SqlCommand sql = new SqlCommand(command.CommandText, connection);
                    using (SqlDataAdapter result = new SqlDataAdapter(sql))
                    {
                        result.Fill(dataTable);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable;
            }
            else
            {
                return new DataTable();
            }

        }

        //check unique condition for 
        public List<DataTable> CheckTableFieldFormValues(int id, List<JObject> tablesB, JArray item)
        {

            string table = string.Empty;
            List<DataTable> dataTables = new List<DataTable>();
            DataTable dataTable = null;

            List<string> fields = new List<string>();
            List<string> values = new List<string>();

            try
            {
                foreach (JObject obj in item)
                {
                    foreach (var data in obj)
                    {
                        table = data.Key.ToString();
                        dataTable = new DataTable(table);
                        fields.Add(data.Value.ToString());
                    }

                    foreach (var item1 in tablesB)
                    {
                        if (obj.ContainsKey(item1["tableName"].ToString()))
                        {
                            string key = item1["tableName"].ToString();
                            foreach (var obj1 in item1)
                            {
                                if (obj1.Key.Trim().Replace(" ", "") == obj[key].ToString())
                                {
                                    JObject myvalue = null;
                                    try
                                    {
                                        myvalue = (JObject)JsonConvert.DeserializeObject(obj1.Value.ToString());
                                    }
                                    catch (Exception e)
                                    {
                                        var myobject = JsonConvert.SerializeObject(new { PropertyA = obj1.Value.ToString() });
                                        myvalue = (JObject)JsonConvert.DeserializeObject(myobject);
                                    }
                                    if (myvalue.Count > 0)
                                    {
                                        foreach (var item2 in myvalue)
                                        {
                                            values.Add(item2.Value.ToString());
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                //get properties of table 
                var results = _db.CustomEntities4.FromSql($"select column_name, DATA_TYPE, IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = {table}").ToList();


                for (int i = 0; i < values.Count; i++)
                {
                    string CommandText = "SELECT * FROM ";
                    CommandText = CommandText + table + " WHERE ";
                    JObject ob = new JObject();
                    var type = results.Where(x => x.column_name == fields[1].Trim().Replace(" ", "")).Select(x => x.DATA_TYPE).FirstOrDefault();
                    if (values[i] != "Null" && values[i] != null)
                    {
                        if (type == "int")
                        {
                            CommandText = CommandText + "[" + fields[1] + "]" + "=" + values[i];
                        }
                        else
                        {
                            CommandText = CommandText + "[" + fields[1] + "]" + "=" + "'" + values[i] + "'";
                        }
                    }
                    else
                    {
                        CommandText = CommandText + "[" + fields[1] + "]" + "Is Not Null";
                    }
                    using (var command = _db.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = CommandText;
                        _db.Database.OpenConnection();
                        SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
                        SqlCommand sql = new SqlCommand(command.CommandText, connection);
                        using (SqlDataAdapter result = new SqlDataAdapter(sql))
                        {
                            result.Fill(dataTable);
                        }
                        _db.Database.CloseConnection();
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTables.Add(dataTable);
                    }
                }
                //generate query

            }
            catch (Exception ex)
            {

            }
            if (dataTables.Count > 0)
            {
                return dataTables;
            }
            else
            {
                return new List<DataTable>();
            }

        }

        /// <summary>
        /// check Available conditions before sync process
        /// </summary>
        /// <param name="component"></param>
        /// <param name="form"></param>
        /// <param name="caseId"></param>
        /// <returns></returns>
        private List<DataTable> CheckConditionBeforeSync(dynamic component, int caseId)
        {
            int totalCheck = component.configs[0].sourcepolicyfieldtable.Count;
            List<string> tables = new List<string>();
            for (int i = 0; i < totalCheck; i++)
            {
                tables.Add(component.configs[0].sourcepolicyfieldtable[i]);
            }

            var grouped = tables
                          .GroupBy(s => s)
                          .Select(g => new { table = g.Key });
            JArray checkCon = new JArray();
            foreach (var item in grouped)
            {
                JArray groupItem = new JArray();
                for (int i = 0; i < totalCheck; i++)
                {
                    JObject temp = new JObject();
                    if (tables[i] == item.table)
                    {

                        temp.Add(item.table, component.configs[0].sourcepolicyfield[i]);
                        temp.Add(component.configs[0].destpolicyfieldtable[i], component.configs[0].destpolicyfield[i]);
                        groupItem.Add(temp);
                    }
                }
                checkCon.Add(groupItem);
                //checkCon1.Add(item.table, value1);
            }
            List<DataTable> check = new List<DataTable>();
            bool isValid = true; //set default value for all condition  as true
            List<SelectListItem> tablesAVM = getSyncSourceTable();

            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));

            List<JObject> tablesB = GetTableData(caseId, tablesBVM);

            foreach (JArray item in checkCon)
            {
                DataTable dt = (CheckTableFieldValues(caseId, tablesB, item));

                if (dt.Rows.Count > 0)
                {
                    check.Add(dt);
                }

            }
            return check;
        }
        private List<DataTable> CheckConditionFormBeforeSync(dynamic component, int caseId)
        {
            int totalCheck = component.configs[0].sourceuniquetable.Count;
            List<string> tables = new List<string>();
            for (int i = 0; i < totalCheck; i++)
            {
                tables.Add(component.configs[0].sourceuniquetable[i]);
            }
            JArray checkCon = new JArray();
            foreach (var item in tables)
            {
                JArray groupItem = new JArray();
                for (int i = 0; i < totalCheck; i++)
                {
                    JObject temp = new JObject();
                    temp.Add(item, component.configs[0].sourceuniquefield[i]);
                    temp.Add(component.configs[0].destuniquetable[i], component.configs[0].destuniquefield[i]);
                    groupItem.Add(temp);
                }
                checkCon.Add(groupItem);
            }

            List<DataTable> check = new List<DataTable>();
            List<SelectListItem> tablesAVM = getSyncSourceTable();

            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].typesource));

            List<JObject> tablesB = GetTableData(caseId, tablesBVM);

            foreach (JArray item in checkCon)
            {
                List<DataTable> dt = (CheckTableFieldFormValues(caseId, tablesB, item));
                if (dt.Count > 0)
                {
                    check.AddRange(dt);
                }
            }
            return check;
        }


        private string getSyncItemIndex(dynamic component, List<JObject> tableB, DataTable tablesA, string checkTableB, string checkFieldA, string checkFieldB, dynamic data)
        {

            if (component.configs[0].destpolicyfieldtable.IndexOf(checkTableB) != -1)
            {

                foreach (var item in tableB)
                {

                    if (item["tableName"].ToString() == checkTableB)
                    {
                        int i = 0;
                        foreach (string fieldName in component.configs[0].destpolicyfield)
                        {
                            foreach (var obj in item)
                            {
                                if (fieldName == obj.Key)
                                {
                                    try
                                    {
                                        JObject ob = new JObject();
                                        ob = (JObject)obj.Value;
                                        string field = component.configs[0].sourcepolicyfield[i];
                                        foreach (var itm in ob)
                                        {
                                            if (itm.Value == data[field].ToString())
                                            {
                                                return itm.Key;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            i++;
                        }

                    }
                }

            }

            return "";
        }

        public List<JObject> SetSyncValue(List<DataTable> tablesA, dynamic component, List<JObject> tablesB, string checkTableA, string checkFieldA, string checkTableB, string checkFieldB, string syncCondition, bool isTableElm = true)
        {
            string valueB = string.Empty;
            List<JObject> tables = new List<JObject>();
            DataTable dt = new DataTable();
            try
            {
                dynamic valueA = null;
                dt = tablesA.Where(x => x.TableName == checkTableA).SingleOrDefault();
                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                    {
                        int i = 0;
                        valueA = new JObject();

                        foreach (DataRow item in dt.Rows)
                        {

                            if (item[checkFieldA].GetType().Name.ToString() == "DateTime")
                            {
                                dynamic data = Convert.ToDateTime(item[checkFieldA].ToString().TrimStart().TrimEnd()).ToString("dd/MM/yyyy HH:mm:ss tt");
                                string index = string.Empty;
                                if (isTableElm)
                                {
                                    index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
                                }
                                else
                                {
                                    index = "";
                                }
                                if (index != "")
                                {
                                    valueA.Add(index, data);
                                }
                                else
                                {
                                    valueA.Add(i.ToString(), data);
                                }
                            }

                            else
                            {
                                dynamic data = item[checkFieldA].ToString().TrimStart().TrimEnd();
                                string index = string.Empty;
                                if (isTableElm)
                                {
                                    index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
                                }
                                else
                                {
                                    index = "";
                                }
                                if (index != "")
                                {
                                    valueA.Add(index, data);
                                }
                                else
                                {
                                    valueA.Add(i.ToString(), data);
                                }
                            }
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (item[checkFieldA].GetType().Name.ToString() == "DateTime")
                            {
                                dynamic data = Convert.ToDateTime(item[checkFieldA].ToString().TrimStart().TrimEnd()).ToString("dd/MM/yyyy HH:mm:ss tt");
                                string index = string.Empty;
                                if (isTableElm)
                                {
                                    index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
                                }
                                else
                                {
                                    index = "";
                                }

                                if (index != "")
                                {
                                    valueA = new JObject();
                                    valueA.Add(index, data);
                                }
                                else
                                {
                                    valueA = string.Empty;
                                    valueA = valueA + "," + Convert.ToDateTime(item[checkFieldA].ToString().TrimStart().TrimEnd()).ToString("dd/MM/yyyy HH:mm:ss tt");
                                    valueA = valueA.Substring(1, valueA.Length - 1);
                                }
                            }

                            else
                            {
                                dynamic data = item[checkFieldA].ToString().TrimStart().TrimEnd();
                                string index = string.Empty;
                                if (isTableElm)
                                {
                                    index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
                                }
                                else
                                {
                                    index = "";
                                }


                                if (index != "")
                                {
                                    valueA = new JObject();
                                    valueA.Add(index, data);
                                }
                                else
                                {
                                    valueA = string.Empty;
                                    valueA = valueA + "," + item[checkFieldA].ToString().TrimStart().TrimEnd();
                                    valueA = valueA.Substring(1, valueA.Length - 1);
                                }
                            }
                        }
                    }
                }
                //setdata
                foreach (var item in tablesB)
                {
                    if (item["tableName"].ToString() == checkTableB)
                    {
                        JObject objTable = new JObject();
                        foreach (var obj in item)
                        {
                            // JObject setObj = new JObject();

                            if (obj.Key == checkFieldB)
                            {
                                if (dt != null)
                                {
                                    JObject ob = new JObject();
                                    try
                                    {
                                        ob = (JObject)obj.Value;
                                        var newOb = new JObject();
                                        foreach (var itm in ob)
                                        {

                                            dynamic val = itm.Value;
                                            if (val == "Null")
                                            {
                                                val = "";
                                            }
                                            if (CheckSyncCondition(syncCondition, val.ToString()))
                                            {
                                                foreach (var itm1 in valueA)
                                                {
                                                    if (itm.Key == itm1.Name)
                                                    {
                                                        val = itm1.Value;
                                                    }
                                                }
                                            }

                                            newOb.Add(itm.Key, val);
                                        }
                                        objTable.Add(obj.Key, newOb);
                                    }
                                    catch (Exception ex)
                                    {
                                        //check condition
                                        if (CheckSyncCondition(syncCondition, obj.Value.ToString()))
                                        {
                                            objTable.Add(obj.Key, valueA);
                                        }
                                        else
                                        {
                                            if (obj.Value.ToString() == "Null")
                                            {
                                                objTable.Add(obj.Key, "");
                                            }
                                            else
                                            {
                                                objTable.Add(obj.Key, obj.Value);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (obj.Value.ToString() == "Null")
                                    {
                                        objTable.Add(obj.Key, "");
                                    }
                                    else
                                    {
                                        objTable.Add(obj.Key, obj.Value);
                                    }
                                }

                            }
                            else
                            {
                                if (obj.Value.ToString() == "Null")
                                {
                                    objTable.Add(obj.Key, "");
                                }
                                else
                                {
                                    objTable.Add(obj.Key, obj.Value);
                                }
                            }

                        }
                        tables.Add(objTable);
                    }
                    else
                    {
                        tables.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return tables;
        }

        private bool CheckSyncCondition(string con, string value)
        {
            bool isTrue = false;
            if (con == "always")
            {
                isTrue = true;
            }
            else if (con == "if empty")
            {
                string val = value.TrimStart().TrimEnd();
                if (val == "" || val == null || val == string.Empty)
                {
                    isTrue = true;
                }
            }
            else if (con == "if exist")
            {
                string val = value.TrimStart().TrimEnd().ToUpper();
                if (val == "" || val == null || val == string.Empty || val == "NULL")
                {
                    isTrue = false;
                }
                else
                {
                    isTrue = true;
                }

            }

            else
            {
                isTrue = false;
            }
            return isTrue;

        }

        private JObject GetResponseData(dynamic component, int caseId, List<DataTable> dts)
        {
            JObject FormValues = new JObject();
            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));
            List<JObject> tablesB = GetTableData(caseId, tablesBVM);
            //List<JObject> tablesA = GetTableData(sourceId, tablesAVM);
            // dts.Where(x => x.TableName == );
            List<string> tablesSaved = new List<string>();
            int count = 0;

            int totalFields = component.configs[0].destinationfield.Count;
            for (int i = 0; i < totalFields; i++)
            {
                tablesB = SetSyncValue(dts, component, tablesB, component.configs[0].sourcetable[i], component.configs[0].sourcefield[i], component.configs[0].destinationtable[i], component.configs[0].destinationfield[i], component.configs[0].synccondition[i], false);

            }
            count = tablesB.Count;
            var ccvm = _formBuilderService.GetBuilderFormById(Convert.ToInt32(component.configs[0].destination));
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            FormBuilderViewModel sourceFormBuilderVM = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            foreach (JObject jsonObj in tablesB)
            {
                foreach (var item in sourceFormBuilderVM.Tab)
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
                                                    if (tableColElm.TableName == jsonObj["tableName"].ToString())
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
                                        if (element.TableName == jsonObj["tableName"].ToString())
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






            //foreach (var item in tablesB)
            //{
            //    string tableName = item["tableName"].ToString();
            //    item.Remove("tableName");
            //    bool success = true;
            //    if (item.Count > 0)
            //    {
            //        var Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tableName}] Where [CaseId] = {caseId}", tableName, caseId);
            //        if (Forms.Count() == 0)
            //        {
            //            success = _formService.SaveOrUpdateTable(tableName, true, item, caseId);
            //        }
            //        else
            //        {
            //            success = _formService.SaveOrUpdateTable(tableName, false, item, caseId);
            //        }

            //    }
            //    if (success) tablesSaved.Add(tableName);
            //}
            if (tablesSaved.Count == count)
            {
                return FormValues;
            }
            else
            {
                return FormValues;
            }
        }
        public bool StartSync(dynamic component, int caseId, List<DataTable> dts)
        {

            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));
            List<JObject> tablesB = GetTableData(caseId, tablesBVM);
            //List<JObject> tablesA = GetTableData(sourceId, tablesAVM);
            // dts.Where(x => x.TableName == );
            List<string> tablesSaved = new List<string>();
            int count = 0;

            int totalFields = component.configs[0].destinationfield.Count;
            for (int i = 0; i < totalFields; i++)
            {
                tablesB = SetSyncValue(dts, component, tablesB, component.configs[0].sourcetable[i], component.configs[0].sourcefield[i], component.configs[0].destinationtable[i], component.configs[0].destinationfield[i], component.configs[0].synccondition[i]);

            }
            count = tablesB.Count;
            foreach (var item in tablesB)
            {
                string tableName = item["tableName"].ToString();
                item.Remove("tableName");
                bool success = true;
                if (item.Count > 0)
                {
                    var Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tableName}] Where [CaseId] = {caseId}", tableName, caseId);
                    if (Forms.Count() == 0)
                    {
                        success = _formService.SaveOrUpdateTable(tableName, true, item, caseId);
                    }
                    else
                    {
                        success = _formService.SaveOrUpdateTable(tableName, false, item, caseId);
                    }

                }
                if (success) tablesSaved.Add(tableName);
            }
            if (tablesSaved.Count == count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //startsync from form to tanentbase
        public bool StartFormSync(dynamic component, int caseId, List<DataTable> dts)
        {
            try
            {
                List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].typesource));
                var tuple = GetTableDataAll(caseId, tablesBVM);

                List<JObject> tablesB = tuple.Item1;
                var tabledata = tuple.Item2;

                int totalFields = component.configs[0].sourcefield.Count;

                List<string> sourcefield = new List<string>();
                List<string> destfield = new List<string>();
                List<List<string>> value = new List<List<string>>();

                string table = component.configs[0].destinationtable[0].ToString();

                string uniquefiled = component.configs[0].destuniquefield[0].ToString();
                List<string> whereValue = new List<string>();

                //get properties of table 
                var results = _db.CustomEntities4.FromSql($"select column_name, DATA_TYPE, IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = {table}").ToList();
                // var results = _db.CustomEntities4.FromSql($"exec sp_columns {table}").ToList();

                for (int i = 0; i < totalFields; i++)
                {
                    destfield.Add(component.configs[0].destinationfield[i]);
                    sourcefield.Add(component.configs[0].sourcefield[i]);
                }
                destfield = destfield.Distinct().ToList();
                sourcefield = sourcefield.Distinct().ToList();



                //count for reperts iteam 
                int count = 0;
                foreach (JObject obj in tablesB)
                {
                    foreach (var data in obj)
                    {
                        List<string> test2 = new List<string>();
                        if (!string.IsNullOrEmpty(data.Value.ToString()))
                        {
                            if (destfield.Contains(data.Key.Trim().Replace(" ", "")) || sourcefield.Contains(data.Key.Trim().Replace(" ", "")))
                            {
                                JObject myvalue = null;
                                try
                                {
                                    myvalue = (JObject)JsonConvert.DeserializeObject(data.Value.ToString());
                                }
                                catch (Exception e)
                                {
                                    var myobject = JsonConvert.SerializeObject(new { PropertyA = data.Value.ToString() });
                                    myvalue = (JObject)JsonConvert.DeserializeObject(myobject);
                                }
                                if (count < myvalue.Count)
                                {
                                    count = myvalue.Count;
                                }
                                break;
                            }
                        }
                    }
                }
                List<List<keypair>> keypairss = new List<List<keypair>>();
                List<keypair> colMapping = new List<keypair>();
                foreach (var item in sourcefield.Select((val, i) => new { i, val }))
                {
                    colMapping.Add(new keypair { key = item.val, val = destfield[item.i] });
                }
                for (int i = 0; i < count; i++)
                {
                    List<keypair> keypairs = new List<keypair>();
                    foreach (JObject obj in tablesB)
                    {
                        foreach (var data in obj)
                        {

                            JObject myvalue = null;
                            if (!string.IsNullOrEmpty(data.Value.ToString()))
                            {
                                if (destfield.Contains(data.Key.Trim().Replace(" ", "")) || sourcefield.Contains(data.Key.Trim().Replace(" ", "")))
                                {
                                    try
                                    {
                                        myvalue = (JObject)JsonConvert.DeserializeObject(data.Value.ToString());
                                    }
                                    catch (Exception e)
                                    {
                                        var myobject = JsonConvert.SerializeObject(new { PropertyA = data.Value.ToString() });
                                        myvalue = (JObject)JsonConvert.DeserializeObject(myobject);
                                    }
                                    var col = colMapping.FirstOrDefault(c => c.key.Trim().Replace(" ", "") == data.Key.Trim().Replace(" ", "")).val;
                                    var type = results.Where(x => x.column_name.ToLower() == col.ToLower().Trim().Replace(" ", "")).Select(x => new { x.DATA_TYPE, x.IS_NULLABLE }).FirstOrDefault();
                                    foreach (var item2 in myvalue)
                                    {
                                        keypair keypair = new keypair();
                                        int findindex = 0;
                                        findindex = destfield.FindIndex(x => x == data.Key.Trim().Replace(" ", ""));
                                        if (findindex == -1)
                                        {
                                            findindex = sourcefield.FindIndex(x => x == data.Key.Trim().Replace(" ", ""));
                                        }

                                        if (item2.Key == i.ToString())
                                        {
                                            keypair.key = destfield[findindex];
                                            if (type.DATA_TYPE == "int")
                                            {
                                                keypair.val = item2.Value.ToString() == "" ? (type.IS_NULLABLE.ToUpper() == "NO" ? "0" : "NULL") : item2.Value.ToString();
                                            }
                                            else if (type.DATA_TYPE == "datetime2")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }

                                            }
                                            else if (type.DATA_TYPE == "date")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }

                                            }
                                            else if (type.DATA_TYPE == "datetime")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }
                                            }
                                            else
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }
                                            }
                                            keypairs.Add(keypair);
                                            break;
                                        }
                                        else if (myvalue.Count == 1)
                                        {
                                            keypair.key = destfield[findindex];
                                            if (type.DATA_TYPE == "int")
                                            {
                                                keypair.val = item2.Value.ToString();
                                            }
                                            else if (type.DATA_TYPE == "datetime2")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }

                                            }
                                            else if (type.DATA_TYPE == "date")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }

                                            }
                                            else if (type.DATA_TYPE == "datetime")
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }
                                            }
                                            else
                                            {
                                                if (item2.Value.ToString() == "Null")
                                                {
                                                    keypair.val = item2.Value.ToString();
                                                }
                                                else
                                                {
                                                    keypair.val = "'" + item2.Value.ToString() + "'";
                                                }
                                            }
                                            keypairs.Add(keypair);
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }

                    //for mapping between two tables
                    int totalmapping = component.configs[0].sourcemappingtable.Count;
                    for (int j = 0; j < totalmapping; j++)
                    {
                        foreach (var item in tabledata)
                        {
                            string type = results.Where(x => x.column_name == component.configs[0].destmappingfield[j]).Select(x => x.DATA_TYPE).FirstOrDefault();
                            keypair pair = new keypair();
                            pair.key = component.configs[0].destmappingfield[j];

                            if (component.configs[0].sourcemappingfield[j] == "Id")
                            {
                                if (type == "int")
                                {
                                    pair.val = item.Id.ToString();
                                }
                                else
                                {
                                    pair.val = "'" + item.Id.ToString() + "'";
                                }
                            }
                            else if (component.configs[0].sourcemappingfield[j] == "UserId")
                            {
                                if (type == "int")
                                {
                                    pair.val = item.UserId.ToString();
                                }
                                else
                                {
                                    pair.val = "'" + item.UserId.ToString() + "'";
                                }
                            }
                            else if (component.configs[0].sourcemappingfield[j] == "TenantId")
                            {
                                if (type == "int")
                                {
                                    pair.val = item.TenantId.ToString();
                                }
                                else
                                {
                                    pair.val = "'" + item.TenantId.ToString() + "'";
                                }
                            }
                            else if (component.configs[0].sourcemappingfield[j] == "CaseId")
                            {
                                if (type == "int")
                                {
                                    pair.val = item.CaseId.ToString();
                                }
                                else
                                {
                                    pair.val = "'" + item.CaseId.ToString() + "'";
                                }
                            }
                            keypairs.Add(pair);
                        }
                    }
                    keypairss.Add(keypairs);
                }


                if (dts.Count() > 0)
                {
                    //first find where condtion to update table 
                    foreach (JObject obj in tablesB)
                    {
                        foreach (var data in obj)
                        {
                            JObject myvalue = null;
                            try
                            {
                                myvalue = (JObject)JsonConvert.DeserializeObject(data.Value.ToString());
                            }
                            catch (Exception e)
                            {
                                var myobject = JsonConvert.SerializeObject(new { PropertyA = data.Value.ToString() });
                                myvalue = (JObject)JsonConvert.DeserializeObject(myobject);
                            }
                            if (data.Key.Trim().Replace(" ", "") == uniquefiled.Trim().Replace(" ", ""))
                            {
                                var type = results.Where(x => x.column_name == uniquefiled.Trim().Replace(" ", "")).Select(x => x.DATA_TYPE).FirstOrDefault();
                                foreach (var item2 in myvalue)
                                {
                                    if (type == "int")
                                    {
                                        whereValue.Add(item2.Value.ToString());
                                    }
                                    else
                                    {
                                        whereValue.Add("'" + item2.Value.ToString() + "'");
                                    }
                                }
                                break;
                            }
                        }
                    }


                    int j = 0;
                    foreach (var item1 in dts)
                    {
                        bool condtiontoupdate = false;
                        var test4 = JsonConvert.SerializeObject(item1.Rows[j].ItemArray).ToString();
                        var datamy = JArray.Parse(test4);
                        for (int i = 0; i < datamy.Count; i++)
                        {
                            JToken elem = datamy[i];
                            string jtokenStr = elem.ToString().Trim();
                            var condtion = whereValue[j].ToString().Trim().Replace("'", "");
                            if (jtokenStr == condtion)
                            {
                                condtiontoupdate = true;
                            }
                        }
                        if (condtiontoupdate)
                        {

                            var insertval = keypairss[j].Select(x => x.val).ToList();
                            var insertfiled = keypairss[j].Select(x => x.key.Trim().Replace(" ", "")).ToList();
                            List<string> querymaker = new List<string>();
                            for (int i = 0; i < destfield.Count; i++)
                            {
                                var valuebind = insertfiled[i] + "=" + insertval[i];
                                querymaker.Add(valuebind);
                            }
                            var commandText = string.Format(@"UPDATE [dbo].[{0}] SET " + String.Join(",", querymaker) + " WHERE " + uniquefiled + " = " + whereValue[j] + ";", table);
                            _db.Database.ExecuteSqlCommand(commandText);
                            _db.SaveChanges();

                        }
                        else
                        {

                            var insertval = keypairss[j].Select(x => x.val).ToList();
                            var insertfiled = keypairss[j].Select(x => x.key.Trim().Replace(" ", "")).ToList();
                            var commandText = string.Format(@"INSERT into [dbo].[{0}] (" + String.Join(",", insertfiled) + ") VALUES (" + String.Join(",", insertval) + ")", table);
                            _db.Database.ExecuteSqlCommand(commandText);
                            _db.SaveChanges();
                        }
                        j++;
                    }

                    return true;
                }
                else
                {
                    try
                    {
                        foreach (var item in keypairss)
                        {
                            var insertval = item.Select(x => x.val).ToList();
                            var insertfiled = item.Select(x => x.key.Trim().Replace(" ", "")).ToList();
                            var commandText = string.Format(@"INSERT into [dbo].[{0}] (" + String.Join(",", insertfiled) + ") VALUES (" + String.Join(",", insertval) + ")", table);
                            _db.Database.ExecuteSqlCommand(commandText);
                            _db.SaveChanges();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;

                    }

                }
            }
            catch (Exception e)
            {

                return false;
            }
        }


        /// <summary>
        /// Case Synchronization Process
        /// </summary>
        /// <param name="_new">Current scope</param>
        /// <param name="caseFormId">FormId</param>
        /// <param name="fromStateId">When Case State Moved From</param>
        /// <param name="toStateId">When Case state moved to</param>
        /// <param name="form">form url</param>
        /// <param name="caseId">case Id</param>
        /// <returns></returns>
        public int SynchronizeCaseAsync(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId, WorkflowObject obj, string baseApiUrl)
        {
            this.HttpContext = _new.HttpContext as HttpContext;
            Theme _theme = _new.Theme as Theme;
            List<DataTable> dts = new List<DataTable>();
            //TODO: GetComponentId From TypeID object
            dynamic component = _theme.GetComponent(obj.TypeId.ToString());
            int returnStateId = 0;
            if (Convert.ToString(component.configs[0].typesource) == Typesource.RestAPI.ToDescription())
            {
                try
                {
                    List<DataTable> dt = new List<DataTable>();
                    bool isGet = Convert.ToBoolean(component.configs[0].isGetMethod);
                    //Convert.ToInt16(component.configs[0].isGetMethod);
                    string api_url = baseApiUrl + component.configs[0].ApiUrl;
                    List<string> param = component.configs[0].ApiParameterList;
                    List<string> param_format = component.configs[0].ApiParameterFormatList;
                    List<string> param_source_table = component.configs[0].ApiParameterSourceTable;
                    List<string> param_source_field = component.configs[0].ApiParameterSourceField;
                    List<string> param_default_value = component.configs[0].ApiParameterDefaultValue;
                    List<string> key = component.configs[0].ApiKeyList;
                    List<string> value = component.configs[0].ApiValueList;
                    List<string> param_destination_table = component.configs[0].ApiResponseDestTable;
                    List<string> param_destination_field = component.configs[0].ApiResponseDestField;
                    List<string> param_response_list = component.configs[0].ApiResponseList;
                    List<string> param_value = new List<string>();
                    List<string> responseList = new List<string>();
                    param_value = GetResponseParamValues(component.configs[0].destination, component.configs[0].ApiParameterSourceTable,
                    component.configs[0].ApiParameterSourceField, component.configs[0].ApiParameterDefaultValue, caseId);
                    var response = GetApiAsync(param, param_value, key, value, Convert.ToInt32(isGet), api_url);
                    bool success = false;

                    //var result = response.Descendants().OfType<JProperty>().Select(p => new KeyValuePair<string, object>(p.Path, p.Value.Type == JTokenType.Array || p.Value.Type == JTokenType.Object ? null : p.Value));
                    if (response != null)
                    {
                        foreach (var itm in param_response_list)
                        {
                            foreach (var item in response)
                            {
                                if (item.Key == itm)
                                {
                                    responseList.Add(item.Value.ToString());
                                }
                            }
                        }
                        var isit = response.GetType().GetProperty("success");
                        if(isit !=null)
                        {
                            success = Convert.ToBoolean(response["success"]);
                        }
                    }
                    StartAPISync(component, caseId, response, param_destination_table, param_destination_field, responseList); //start sync process
                    returnStateId = success ? 1:0 ; //syc success }
                }
                catch (Exception ex)
                {
                    returnStateId = 0;

                }
            }
            else if ((Convert.ToString(component.configs[0].destination).Trim() == caseFormId.ToString().Trim()))
            {
                //if (Convert.ToInt32(component.configs[0].pull) == fromStateId && Convert.ToInt32(component.configs[0].pass) == toStateId)
                //{
                dts = CheckConditionBeforeSync(component, caseId);
                if (dts.Count != 0) //check if returned value consists of datatables for sync
                {
                    try
                    {
                        StartSync(component, caseId, dts); //start sync process
                    }
                    catch (Exception ex)
                    {

                    }
                    returnStateId = 1; //syc success 
                }
                else // fail
                {
                    returnStateId = 0;
                }

                //}
                //else
                //{
                //    returnStateId = toStateId;
                //}
            }
            //form to tenantdatabase
            else if ((Convert.ToString(component.configs[0].typesource)).Trim() == caseFormId.ToString().Trim())
            {
                dts = CheckConditionFormBeforeSync(component, caseId);
                try
                {
                    StartFormSync(component, caseId, dts); //update process
                }
                catch (Exception ex)
                {

                }
                returnStateId = 1; //syc success 
            }
            else
            {
                returnStateId = 0;
            }
            return returnStateId;
        }

        private List<string> GetResponseParamValues(string destination, List<string> apiParameterSourceTable, List<string> apiParameterSourceField, List<string> apiParameterDefaultValue, int caseId)
        {
            List<string> param_values = new List<string>();
            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(destination));
            List<JObject> tablesB = GetTableData(caseId, tablesBVM);

            for (int i = 0; i < apiParameterSourceTable.Count; i++)
            {
                foreach (var item in tablesB)
                {
                    string tableName = item["tableName"].ToString();
                    if (i == (apiParameterSourceTable.Count - 1))
                    {
                        item.Remove("tableName");
                    }
                    if (tableName == apiParameterSourceTable[i])
                    {
                        foreach (var obj in item)
                        {
                            if (apiParameterSourceField[i] == obj.Key)
                            {
                                param_values.Add(obj.Value.ToString());
                            }
                        }
                    }
                }
            }


            //for (int i = 0; i < apiParameterSourceTable.Count; i++)
            //{
            //    if (apiParameterSourceTable[i] != null && apiParameterSourceField[i] != null)
            //    {
            //        var CommandText = "SELECT " + apiParameterSourceField[i] + " FROM " + apiParameterSourceTable[i] + " WHERE CASEID = " + caseId;
            //        using (var command = _db.Database.GetDbConnection().CreateCommand())
            //        {
            //            command.CommandText = CommandText;
            //            _db.Database.OpenConnection();
            //            SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
            //            connection.Open();
            //            SqlCommand sql = new SqlCommand(command.CommandText, connection);
            //            using (SqlDataReader dataReader = sql.ExecuteReader())
            //            {
            //                if (dataReader.HasRows)
            //                {
            //                    while (dataReader.Read())
            //                    {
            //                        param_values.Add(dataReader.GetValue(0).ToString());
            //                    }
            //                }
            //                else
            //                {
            //                    param_values.Add(apiParameterDefaultValue[i].ToString());
            //                }
            //            }
            //        }
            //        //var param = _db.CustomEntities.FromSql($"SELECT {apiParameterSourceField[i]}  FROM   { apiParameterSourceTable[i]}  WHERE CASEID =  {caseId}", apiParameterSourceField[i], apiParameterSourceTable[i],caseId);
            //        //param_values.Add(param.ToString());
            //    }
            //}
            return param_values;

        }

        #region rahul
        public List<SelectListItem> getSyncSourceTable()
        {
            List<SelectListItem> dbTables = new List<SelectListItem>();
            var tables = _db.CustomEntities2.FromSql($"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'");
            foreach (var item in tables)
            {
                string tName = item.Table_Name;
                SelectListItem selectListItemForTable = new SelectListItem()
                {
                    Value = tName,
                    Text = tName
                };
                dbTables.Add(selectListItemForTable);
            }
            return dbTables;
        }

        public List<SelectListItem> getSyncSourceTableColumns(string tableName)
        {
            List<SelectListItem> dbColumns = new List<SelectListItem>();
            string tName = tableName;
            var columns = _db.CustomEntities3.FromSql($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{tName}'", tName);
            foreach (var item in columns)
            {
                SelectListItem selectListItemForColumn = new SelectListItem()
                {
                    Value = item.Column_Name,
                    Text = item.Column_Name
                };
                dbColumns.Add(selectListItemForColumn);
            }
            return dbColumns;

        }


        public List<SelectListItem> getSyncSourceTableColumnsAll(string tableName)
        {
            List<SelectListItem> dbColumns = new List<SelectListItem>();
            if (tableName.Split(',')[1] == "db")
            {
                string tName = tableName.Split(',')[0];
                var columns = _db.CustomEntities3.FromSql($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{tName}'", tName);

                foreach (var item in columns)
                {
                    SelectListItem selectListItemForColumn = new SelectListItem()
                    {
                        Value = item.Column_Name,
                        Text = item.Column_Name
                    };
                    dbColumns.Add(selectListItemForColumn);
                }
                return dbColumns;
            }
            else if (tableName.Split(',')[1] == "form")
            {

                int id = Convert.ToInt32(tableName.Split(",")[2]);
                string tName = Convert.ToString(tableName.Split(",")[0]);

                CaseFormViewModel ccvm = new CaseFormViewModel
                {
                    Id = id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                ccvm = _formBuilderService.GetBuilderFormById(id);
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                List<FormBuilderViewModel.Form.Field> fields = a.Forms.Tables.Where(x => x.Name == tName).SingleOrDefault().Fields.ToList();
                foreach (var item in fields)
                {
                    SelectListItem selectListItemForColumn = new SelectListItem()
                    {
                        Value = item.Name,
                        Text = item.Name
                    };
                    dbColumns.Add(selectListItemForColumn);
                }
                return dbColumns;

            }
            else
            {
                return dbColumns;
            }
        }

        #endregion

        public string getComponets(dynamic _new, int caseFormId, int fromStateId, int toStateId, int caseId)
        {
            string st = null;
            this.HttpContext = _new.HttpContext as HttpContext;
            Theme _theme = _new.Theme as Theme;
            List<DataTable> dts = new List<DataTable>();

            dynamic components = _theme.GetComponentsByType("Themes.Core.Components.CaseAutomation");

            foreach (dynamic component in components)
            {

                if (Convert.ToInt64(component.configs[0].source) == caseFormId && Convert.ToInt32(component.configs[0].pull) == fromStateId && Convert.ToInt32(component.configs[0].pass) == toStateId)
                {
                    st = component.ComponentId;

                }

            }
            return st;
        }

        private List<DataTable> CheckConditionBeforeAPISync(dynamic component, int caseId)
        {
            int totalCheck = component.configs[0].ApiParameterSourceField.Count;
            List<string> fields = new List<string>();
            for (int i = 0; i < totalCheck; i++)
            {
                fields.Add(component.configs[0].ApiParameterSourceField[i]);
            }

            var grouped = fields
                          .GroupBy(s => s)
                          .Select(g => new { field = g.Key });
            JArray checkCon = new JArray();
            foreach (var item in grouped)
            {
                if (item.field != null)
                {
                    JArray groupItem = new JArray();
                    for (int i = 0; i < totalCheck; i++)
                    {
                        JObject temp = new JObject();
                        if (fields[i] == item.field)
                        {
                            if (component.configs[0].ApiParameterSourceField[i] != null && component.configs[0].ApiResponseDestField[i] != null)
                            {
                                //temp.Add(item.field, component.configs[0].sourcefield[i]);
                                temp.Add(component.configs[0].ApiResponseDestTable[i], component.configs[0].ApiResponseDestField[i]);
                                groupItem.Add(temp);
                            }
                        }
                    }
                    checkCon.Add(groupItem);
                }
                //checkCon1.Add(item.table, value1);
            }
            List<DataTable> check = new List<DataTable>();
            bool isValid = true; //set default value for all condition  as true

            List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));

            List<JObject> tablesB = GetTableData(caseId, tablesBVM);

            foreach (JArray item in checkCon)
            {
                //DataTable dt = (CheckTableFieldValues(caseId, tablesB, item));

                //if (dt.Rows.Count > 0)
                //{
                //    check.Add(dt);
                //}
            }
            return check;
        }

        public bool StartAPISync(dynamic component, int caseId, object dts, List<string> destinationTable, List<string> destinationField, List<string> responseList)
        {
            try
            {
                List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));
                List<JObject> tablesB = GetTableData(caseId, tablesBVM);
                // List<JObject> tables = new List<JObject>();
                for (int i = 0; i < destinationTable.Count; i++)
                {
                    foreach (var item in tablesB)
                    {
                        string tableName = item["tableName"].ToString();

                        if (i == (destinationTable.Count - 1))
                        {
                            item.Remove("tableName");
                        }
                        if (tableName == destinationTable[i])
                        {
                            JObject objTable = new JObject();
                            foreach (var obj in item)
                            {
                                if (destinationField[i] == obj.Key)
                                {
                                    objTable.Add(obj.Key, responseList[i]);
                                }
                                else
                                {
                                    objTable.Add(obj.Key, obj.Value);
                                }
                            }
                            var result = _formService.SaveOrUpdateTable(tableName, false, objTable, caseId);
                            //   tables.Add(objTable);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public JObject GetApiAsync(List<string> param, List<string> param_value, List<string> key, List<string> value, int isGet, string api_url)
        {
            isGet = 1;
            JObject keyValuePair = new JObject();
            if (key.Count > 0)
            {
                for (int i = 0; i < key.Count; i++)
                {
                    if (key[i] != null)
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
                        if (param[i] != null)
                        {
                            if (i != param.Count && i != 0)
                                parameters = parameters + "&";
                            parameters = parameters + param[i] + "=" + param_value[i];
                        }
                    }
                    JObject jsonResult;
                    try
                    {
                        jsonResult =
                            WebApiService.InstanceForExternal.GetAsync<JObject>(api_url + parameters, true, "externalHeaders/cicero").GetAwaiter().GetResult();
                        return jsonResult;
                    }
                    catch (Exception ex)
                    {

                    }
                    return jsonResult = null;

                }
                else
                {
                    JObject postData = new JObject();
                    if (param.Count > 0)
                    {
                        for (int i = 0; i < param.Count; i++)
                        {
                            postData.Add(param[i], param_value[i]);
                        }
                    }
                    JObject jsonResult =
                    WebApiService.InstanceForExternal.PostAsync<JObject>(api_url, true,
                            "externalHeaders/mirs", postData).GetAwaiter().GetResult();
                    return jsonResult = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get response from API");

            }

        }

        //Rahul
        public List<SelectListItem> GetSyncAllTables(int formId)
        {
            List<SelectListItem> dbTables = new List<SelectListItem>();
            var tables = _db.CustomEntities2.FromSql($"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'");
            foreach (var item in tables)
            {
                string tName = item.Table_Name + ",db";
                SelectListItem selectListItemForTable = new SelectListItem()
                {
                    Value = tName,
                    Text = item.Table_Name
                };
                dbTables.Add(selectListItemForTable);
            }

            List<FormBuilderViewModel.Form.Table> formTables = GetTables(formId);
            foreach (var item in formTables)
            {
                string tName = item.Name + ",form," + formId;
                SelectListItem exist = dbTables.Where(x => x.Text == item.Name).FirstOrDefault();
                if (exist != null)
                {
                    dbTables.Remove(exist);
                }
                exist = new SelectListItem()
                {
                    Value = tName,
                    Text = item.Name
                };
                dbTables.Add(exist);
            }
            return dbTables;
        }


        private List<DataTable> CheckConditionBeforeSync1(dynamic component, WorkRun work)
        {
            int totalCheck = component.configs[0].sourcepolicyfieldtable.Count;
            List<string> tables = new List<string>();
            for (int i = 0; i < totalCheck; i++)
            {
                tables.Add(component.configs[0].sourcepolicyfieldtable[i]);
            }

            var grouped = tables
                          .GroupBy(s => s)
                          .Select(g => new { table = g.Key });
            JArray checkCon = new JArray();
            foreach (var item in grouped)
            {
                JArray groupItem = new JArray();
                for (int i = 0; i < totalCheck; i++)
                {
                    JObject temp = new JObject();
                    if (tables[i] == item.table)
                    {

                        temp.Add(item.table, component.configs[0].sourcepolicyfield[i]);
                        var value = work.Parameter.Where(x => x.ElementId == component.configs[0].destFormElm[i]).SingleOrDefault().Value;
                        temp.Add(component.configs[0].destFormElm[i], value);
                        groupItem.Add(temp);
                    }
                }
                checkCon.Add(groupItem);
                //checkCon1.Add(item.table, value1);
            }
            List<DataTable> check = new List<DataTable>();
            bool isValid = true; //set default value for all condition  as true
            List<SelectListItem> tablesAVM = getSyncSourceTable();

            //List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(component.configs[0].destination));

            //List<JObject> tablesB = GetTableData(caseId, tablesBVM);

            foreach (JArray item in checkCon)
            {
                DataTable dt = (GetDataTableFromParams(item));

                if (dt.Rows.Count > 0)
                {
                    check.Add(dt);
                }

            }
            return check;
        }


        public object CallAPI(dynamic _new, WorkRun work, ElementWorkflowObject obj, string baseApiUrl)
        {
            this.HttpContext = _new.HttpContext as HttpContext;
            Theme _theme = _new.Theme as Theme;
            object success = new object();
            List<DataTable> dts = new List<DataTable>();
            //TODO: GetComponentId From TypeID object
            dynamic component = _theme.GetElementComponentById(obj.TypeId.ToString(), work.Work.CaseFormId, work.Work.ElementId, work.Work.EventType);
            if (Convert.ToString(component.configs[0].typesource) == Typesource.RestAPI.ToDescription())
            {
                try
                {
                    List<DataTable> dt = new List<DataTable>();
                    bool isGet = Convert.ToBoolean(component.configs[0].isGetMethod);
                    //Convert.ToInt16(component.configs[0].isGetMethod);
                    string apiUrl = baseApiUrl + component.configs[0].ApiUrl;
                    List<string> param = component.configs[0].ApiParameterElementName;
                    List<string> paramFormat = component.configs[0].ApiParameterFormatList;
                    List<string> paramSourceElement = component.configs[0].ApiParameterElement;
                    List<string> paramDefaultValue = component.configs[0].ApiParameterElementDefaultValue;
                    List<string> key = component.configs[0].ApiKeyList;
                    List<string> value = component.configs[0].ApiValueList;
                    List<string> responseElement = component.configs[0].ApiResponseElement;
                    List<string> responseList = component.configs[0].ApiResponseElementName;
                    List<string> paramValue = new List<string>();
                    List<string> responseListValue = new List<string>();
                    if (param != null)
                    {
                        paramValue = MapForParameter(param, paramSourceElement, paramDefaultValue, work);
                    }
                    else
                    {
                        param = new List<string>();
                        paramFormat = new List<string>();
                        paramSourceElement = new List<string>();
                        paramDefaultValue = new List<string>();
                    }
                    if (key == null && value == null)
                    {
                        key = new List<string>();
                        value = new List<string>();
                    }
                    var response = GetApiAsync(param, paramValue, key, value, Convert.ToInt32(isGet), apiUrl);
                    //var result = response.Descendants().OfType<JProperty>().Select(p => new KeyValuePair<string, object>(p.Path, p.Value.Type == JTokenType.Array || p.Value.Type == JTokenType.Object ? null : p.Value));
                    if (response != null)
                    {
                        foreach (var itm in responseList)
                        {
                            if (itm != null)
                            {
                                var itmItem = itm.Split("-");
                                int i = 0;
                                var temp = response[itmItem[i]];
                                while (i < itmItem.Count() - 1)
                                {
                                    i++;
                                    temp = temp[itmItem[i]];
                                }
                                responseListValue.Add(temp.ToString());
                            }

                        }
                    }
                    WorkRun NewWork = MapToResponse(responseList, responseListValue, responseElement, work);
                    success = new { success = 1, Data = NewWork, response = response };

                }
                catch (Exception ex)
                {

                    return new { success = 0, response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" } };
                }
            }
            else if (Convert.ToString(component.configs[0].typesource) == Typesource.TenantDatabase.ToDescription())
            {
                if (Convert.ToString(component.configs[0].policyConCheck) == "formElm")
                {
                    dts = CheckConditionBeforeSync1(component, work);
                    if (dts.Count != 0) //check if returned value consists of datatables for sync
                    {
                        try
                        {
                            JObject data = GetResponseData(component, work.Work.CaseFormId, dts); //start sync process
                            WorkParams res = new WorkParams();
                            res.ElementId = "formData";
                            res.IsTriggerEvent = "false";
                            res.Name = "data-formdata";
                            res.Value = JsonConvert.SerializeObject(data);
                            work.Response = new List<WorkParams>();
                            work.Response.Add(res);
                            success = new { success = 1, Data = work, response = new { Success = true, StatusCode = 200, Message = "Successfull", DataList = "", Data = "" } };
                        }
                        catch (Exception ex)
                        {
                            return new { success = 0, response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" } };
                        }

                    }
                    else // fail
                    {
                        return new { success = 0, response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" } };
                    }
                }

            }

            return success;
        }
        private List<string> MapForParameter(List<string> paramList, List<string> paramSourceElement, List<string> paramDefaultValue, WorkRun work)
        {
            List<string> paramValue = new List<string>();
            for (int i = 0; i < paramSourceElement.Count; i++)
            {
                WorkParams par = work.Parameter.Where(x => x.ElementId == paramSourceElement[i]).FirstOrDefault();
                if (par != null)
                {
                    paramValue.Add(par.Value);
                }
                else
                {
                    paramValue.Add("");
                }
            }
            return paramValue;
        }
        private WorkRun MapToResponse(List<string> responseList, List<string> responseListValue, List<string> responseElement, WorkRun work)
        {
            for (int i = 0; i < responseElement.Count; i++)
            {
                if (work.Response.Where(x => x.ElementId == responseElement[i]).FirstOrDefault() != null)
                {
                    work.Response.Where(x => x.ElementId == responseElement[i]).FirstOrDefault().Value = responseListValue[i];
                }
            }
            return work;
        }

        private class keypair
        {
            public string key { get; set; }
            public string val { get; set; }
        }

    }


}
