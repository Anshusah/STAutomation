using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Themes.Core.Components
{
    public class AssignValue : Component
    {
        private IFormBuilderService _formBuilderService;
        private ApplicationDbContext _db;
        private IFormService _formService;

        public AssignValue()
        {

        }

        public ComponentResponse OnUpdate(AssignValue _new, AssignValue _old)
        {
            return Update<AssignValue>(_new) as ComponentResponse;
        }
        public ComponentResponse OnUpdateElm(AssignValue _new, AssignValue _old, int formId, string elementId, int eventType)
        {
            return Update<AssignValue>(_new, eventType, elementId, formId) as ComponentResponse;
        }

        public List<string> SourceField { get; set; }
        public List<string> SourceTable { get; set; }
        public List<string> Values { get; set; }

        public int RunAssignValue(int caseId = 0, string componentId = "")
        {
            int isSuccess = 0;
            if (caseId != 0 && componentId != "")
            {
                AssignValue component = this.Theme.GetComponent(componentId);
                string formid = component.FormId;
                JArray Values = GetSetterValue(component, caseId, Convert.ToInt32(formid));
                List<FormBuilderViewModel.Form.Table> tablesBVM = GetTables(Convert.ToInt32(formid));
                List<JObject> tableA = GetTableData(caseId, tablesBVM);
                List<JObject> tablesToSave = new List<JObject>();
                foreach (JObject jObjects in Values)
                {

                    foreach (JObject table in tableA)
                    {
                        if (table["tableName"].ToString() == jObjects["tableName"].ToString())
                        {
                            JObject objTable = new JObject();
                            objTable = table;
                            foreach (var fromSet in jObjects)
                            {
                                foreach (var toSet in objTable)
                                {
                                    if (toSet.Key == fromSet.Key)
                                    {
                                        JObject ob = new JObject();
                                        try
                                        {
                                            ob = (JObject)toSet.Value;
                                            var newOb = new JObject();
                                            foreach (var itm in ob)
                                            {

                                                dynamic val = itm.Value;
                                                if (val == "Null")
                                                {
                                                    val = "";
                                                }

                                                val = fromSet.Value;

                                                newOb.Add(itm.Key, val);
                                            }
                                            objTable[toSet.Key] = newOb;
                                        }
                                        catch (Exception ex)
                                        {
                                            objTable[toSet.Key] = fromSet.Value;
                                        }
                                    }
                                    
                                }
                            }
                            tablesToSave.Add(objTable);
                        }
                      
                    }
                   
                }
                if (this._formService == null) this._formService = this.Theme.HttpContext.RequestServices.GetService(typeof(IFormService)) as IFormService;
                List<string> tablesSaved = new List<string>();
                foreach (var item in tablesToSave)
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
                if (tablesSaved.Count == tablesToSave.Count)
                {
                    isSuccess = 1;
                }
                else
                {
                    isSuccess = 0;
                }
            }
            return isSuccess;
        }
        /// <summary>
        /// GetTables and Data from caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        private List<FormBuilderViewModel.Form.Table> GetTables(int caseFormId)
        {
            if (this._formBuilderService == null) this._formBuilderService = this.Theme.HttpContext.RequestServices.GetService(typeof(IFormBuilderService)) as IFormBuilderService;
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
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
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

        /// <summary>
        /// check Available conditions before sync process
        /// </summary>
        /// <param name="component"></param>
        /// <param name="form"></param>
        /// <param name="caseId"></param>
        /// <returns></returns>
        private JArray GetSetterValue(AssignValue component, int caseId, int formId)
        {
            int totalCheck = component.SourceTable.Count;
            List<string> tables = new List<string>();
            for (int i = 0; i < totalCheck; i++)
            {
                tables.Add(component.SourceTable[i]);
            }

            var grouped = tables
                          .GroupBy(s => s)
                          .Select(g => new { table = g.Key });
            JArray checkCon = new JArray();
            foreach (var item in grouped)
            {
               
                JObject temp1 = new JObject();
                temp1.Add("tableName", item.table);
                for (int i = 0; i < totalCheck; i++)
                {
                   
                    if (tables[i] == item.table)
                    {

                        temp1.Add(component.SourceField[i], component.Values[i]);
                        //temp.Add(item.table, component.configs[0].sourcepolicyfield[i]);
                        //temp.Add(component.configs[0].destpolicyfieldtable[i], component.configs[0].destpolicyfield[i]);
                        
                    }
                }
                checkCon.Add(temp1);
                //checkCon1.Add(item.table, value1);
            }
            return checkCon;
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

        public List<JObject> SetSyncValue(List<DataTable> tablesA, dynamic component, List<JObject> tablesB, string checkTableA, string checkFieldA, string checkTableB, string checkFieldB, string syncCondition)
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
                                string index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
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
                                string index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);
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
                                string index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);

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
                                string index = getSyncItemIndex(component, tablesB, dt, checkTableB, checkFieldA, checkFieldB, item);

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

        public bool StartSync(dynamic component, int caseId, List<DataTable> dts)
        {
            if (this._formService == null) this._formService = this.Theme.HttpContext.RequestServices.GetService(typeof(IFormService)) as IFormService;
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
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
    }



}
