using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Models.Core.Automation;
using Cicero.Service.Helpers;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Enums;

namespace Themes.Core.Components
{
    public class CaseAssignment : Cicero.Service.Models.Core.Component
    {
        //private IFormBuilderService _formBuilderService;
        private ApplicationDbContext _db;
        //private IFormService _formService;
        public IAutomationService automationService;
        public ISkillSetService _skillSetService;
        public ICommonService commonService;
        public IAuditLogService auditLogService;

        public class QueryBuilderFilterForCaseAssignment
        {
            public string id { get; set; }
            public string label { get; set; }
            public string type { get; set; }
            public string optgroup { get; set; }
            public string default_value { get; set; }
            public string size { get; set; }
            public bool unique { get; set; } = true;
            public List<string> operators { get; set; }

            //public List<SelectListItem> skillset { get; set; }

        }

        public class CaseAssignmentResult
        {
            public object Case { get; set; }
            public CaseAssignment Component { get; set; }

        }

        //public List<string> SourceField { get; set; }
        //public List<string> SourceTable { get; set; }
        //public List<string> Values { get; set; }
        public string json { get; set; }

        public string RoleId { get; set; }

        public List<int> TimeLimit { get; set; }

        public List<string> SkillSetId { get; set; }

        //public List<SelectListItem> SkillSets { get; }

        //public CaseAssignment(ISkillSetService skillSetService)
        //{
        //    _skillSetService = skillSetService;

        //    SkillSets = GetSkillSet();
        //}

        //public List<SelectListItem> GetSkillSet(){

        //    //if (this._skillSetService == null) this._skillSetService = this.Theme.HttpContext.RequestServices.GetService(typeof(ISkillSetService)) as ISkillSetService;
        //    return _skillSetService.GetAllSkillSet().ToList();
        //}


        public ComponentResponse OnUpdate(CaseAssignment _new, CaseAssignment _old)
        {
            return Update<CaseAssignment>(_new) as ComponentResponse;
        }

        public List<string> getOperatorsByType(string type)
        {
            if (type == "string")
            {
                List<string> stringOperators = new List<string>();
                stringOperators.Add("equal");
                stringOperators.Add("not_equal");
                stringOperators.Add("is_null");
                stringOperators.Add("is_not_null");
                stringOperators.Add("contains");
                stringOperators.Add("not_contains");
                stringOperators.Add("begins_with");
                stringOperators.Add("not_begins_with");
                stringOperators.Add("ends_with");
                stringOperators.Add("not_ends_with");
                stringOperators.Add("equal_field");
                stringOperators.Add("not_equal_field");

                return stringOperators;
            }
            if (type == "integer")
            {
                List<string> integerOperators = new List<string>();
                integerOperators.Add("equal");
                integerOperators.Add("not_equal");
                integerOperators.Add("less");
                integerOperators.Add("less_or_equal");
                integerOperators.Add("greater");
                integerOperators.Add("greater_or_equal");
                integerOperators.Add("is_null");
                integerOperators.Add("is_not_null");
                integerOperators.Add("equal_field");
                integerOperators.Add("not_equal_field");
                integerOperators.Add("less_field");
                integerOperators.Add("less_or_equal_field");
                integerOperators.Add("greater_field");
                integerOperators.Add("greater_or_equal_field");
                return integerOperators;
            }
            if (type == "date")
            {
                List<string> datetimeOperators = new List<string>();
                datetimeOperators.Add("equal");
                datetimeOperators.Add("not_equal");
                datetimeOperators.Add("less");
                datetimeOperators.Add("less_or_equal");
                datetimeOperators.Add("greater");
                datetimeOperators.Add("greater_or_equal");
                datetimeOperators.Add("is_null");
                datetimeOperators.Add("is_not_null");
                datetimeOperators.Add("equal_field");
                datetimeOperators.Add("not_equal_field");
                datetimeOperators.Add("less_field");
                datetimeOperators.Add("less_or_equal_field");
                datetimeOperators.Add("greater_field");
                datetimeOperators.Add("greater_or_equal_field");
                return datetimeOperators;
            }
            if (type == "double")
            {
                List<string> decimalOperators = new List<string>();
                decimalOperators.Add("equal");
                decimalOperators.Add("not_equal");
                decimalOperators.Add("less");
                decimalOperators.Add("less_or_equal");
                decimalOperators.Add("greater");
                decimalOperators.Add("greater_or_equal");
                decimalOperators.Add("is_null");
                decimalOperators.Add("is_not_null");
                decimalOperators.Add("equal_field");
                decimalOperators.Add("not_equal_field");
                decimalOperators.Add("less_field");
                decimalOperators.Add("less_or_equal_field");
                decimalOperators.Add("greater_field");
                decimalOperators.Add("greater_or_equal_field");
                return decimalOperators;
            }
            List<string> defaultOperators = new List<string>();
            defaultOperators.Add("equal");
            defaultOperators.Add("not_equal");
            defaultOperators.Add("is_null");
            defaultOperators.Add("is_not_null");
            defaultOperators.Add("contains");
            defaultOperators.Add("not_contains");
            defaultOperators.Add("begins_with");
            defaultOperators.Add("not_begins_with");
            defaultOperators.Add("ends_with");
            defaultOperators.Add("not_ends_with");
            defaultOperators.Add("equal_field");
            defaultOperators.Add("not_equal_field");
            return defaultOperators;

        }

        public string GetFormFilters(string FormId)
        {
            if (this.automationService == null) this.automationService = this.Theme.HttpContext.RequestServices.GetService(typeof(IAutomationService)) as IAutomationService;
            //if (this._skillSetService == null) this._skillSetService = this.Theme.HttpContext.RequestServices.GetService(typeof(ISkillSetService)) as ISkillSetService;
            var list = this.automationService.GetTables(Convert.ToInt32(FormId));
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            List<QueryBuilderFilterForCaseAssignment> fd = new List<QueryBuilderFilterForCaseAssignment>();
            //var skillsets = _skillSetService.GetAllSkillSet().ToList();

            //string optionValues = string.Empty;

            //foreach (var items in skillsets)
            //{
            //    optionValues += "<option value = '"+ items.Value +"' selected = '"+ items.Selected +"'>" + items.Text + "</option>";
            //}

            //var temp = "function(rule, name) {var $container = rule.$el.find('.rule-value-container');" +
            //    "var h = " + optionValues  + ";}";

            foreach (var tm in list)
            {
                foreach (var li in tm.Fields)
                {
                    string PropName = ("fld" + list.IndexOf(tm) + "_" + tm.Fields.IndexOf(li) + "_" + tm.Name + "_" + li.Name).Replace(" ", "");
                    string typestr = string.Empty;
                    //string type = null;
                    if (string.IsNullOrEmpty(li.Name))
                    {
                        PropName = li.Name;
                    }

                    if (!string.IsNullOrEmpty(li.Type))
                    {//"support for previously saved forms remove later"
                        switch (li.Type)
                        {
                            case "Text":
                                typestr = "string";
                                break;
                            case "Binary":
                                typestr = "string";
                                break;
                            case "Date":
                                typestr = "date";
                                break;
                            case "Float":
                                typestr = "double";
                                break;
                            case "Int":
                                typestr = "integer";
                                break;
                            case "Image":
                                typestr = "string";
                                break;
                            case "BigInt":
                                typestr = "integer";
                                break;
                            case "decimal":
                                typestr = "double";
                                break;
                            case "number":
                                typestr = "integer";
                                break;
                            default:
                                typestr = li.Type;
                                break;
                        }
                        //end support
                        //typestr=li.Type; //uncomment later
                    }



                    string id = ("fld" + list.IndexOf(tm) + "_" + tm.Fields.IndexOf(li) + "_" + tm.Name + "_" + li.Name).Replace(" ", "");
                    var flds = new QueryBuilderFilterForCaseAssignment() { id = id, type = typestr, optgroup = tm.Name, label = li.Name, unique = true, default_value = li.Default, size = li.Size, operators = this.getOperatorsByType(typestr) };
                    fd.Add(flds);
                }
            }
            if (fd.Count() > 0)
            {
                return JsonConvert.SerializeObject(fd);
            }
            else
            {
                return "[{\"id\": \"name\",\"label\": \"Name\",\"type\": \"string\",\"unique\":\"true\",\"description\": \"This filter is \"}]";
            }


        }

        public List<DataField> GetFormDatas(int FormId, int CaseId)
        {
            if (this.automationService == null) this.automationService = this.Theme.HttpContext.RequestServices.GetService(typeof(IAutomationService)) as IAutomationService;
            var list = this.automationService.GetTables(FormId);
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            List<DataField> fd = new List<DataField>();
            foreach (var tm in list)
            {
                var Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tm.Name}] Where [CaseId] = {CaseId}", tm.Name, CaseId).FirstOrDefault();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                if (Forms != null && !string.IsNullOrEmpty(Forms.Extras) && Forms.Extras != "{}")
                {
                    JObject jsonObj = JObject.Parse(Forms.Extras);
                    foreach (var li in tm.Fields)
                    {
                        var ex = jsonObj.Properties();
                        var FieldValue = "";
                        foreach (JProperty property in ex)
                        {
                            if (property.Name.ToString() == li.Name)
                            {
                                FieldValue = property.Value.ToString();
                                break;
                            }

                        }

                        string id = ("fld" + list.IndexOf(tm) + "_" + tm.Fields.IndexOf(li) + "_" + tm.Name + "_" + li.Name).Replace(" ", "");
                        var flds = new DataField() { Id = id, TableName = tm.Name, FieldName = li.Name, FieldValue = FieldValue };
                        fd.Add(flds);
                    }
                }
            }
            return fd;
        }
        public Type BuildDynamicTypeWithProperties(int FormId)
        {
            AppDomain Domain = Thread.GetDomain();
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "Cicero";

            if (this.commonService == null) this.commonService = this.Theme.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            // Generate a persistable single-module assembly.
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("FieldData", TypeAttributes.Public);

            List<CoreCaseTable> lst = this.commonService.GetCoreFormFieldsByFormId(FormId);

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            foreach (var item in lst)
            {
                dynamic tables = JsonConvert.DeserializeObject(item.Fields, settings);
                if (tables.Fields != null)
                {
                    foreach (var field in tables.Fields)
                    {
                        string PropName;
                        string typestr = string.Empty;
                        Type type = null;
                        if (!string.IsNullOrEmpty(field.Name))
                        {
                            PropName = field.Name;
                        }
                        if (!string.IsNullOrEmpty(field.Type))
                        {
                            typestr = field.Type;
                        }
                        else
                        {
                            typestr = "";
                        }
                        switch (typestr)
                        {
                            case "string":
                                type = typeof(string);
                                break;
                            case "integer":
                                type = typeof(Int32);
                                break;
                            case "date":
                                type = typeof(DateTime);
                                break;
                            case "decimal":
                                type = typeof(decimal);
                                break;
                            //"support for previously saved forms remove later"

                            case "Text":
                                type = typeof(string);
                                break;
                            case "Binary":
                                type = typeof(string);
                                break;
                            case "Date":
                                type = typeof(DateTime);
                                break;
                            case "Float":
                                type = typeof(decimal);
                                break;
                            case "Int":
                                type = typeof(Int32);
                                break;
                            case "Image":
                                type = typeof(string);
                                break;
                            case "BigInt":
                                type = typeof(Int64);
                                break;
                            //end support
                            //typestr=li.Type; //uncomment later
                            default:
                                type = typeof(string);
                                break;
                        }
                        PropName = ("fld" + lst.IndexOf(item) + "_" + tables.Fields.IndexOf(field) + "_" + item.Name + "_" + field.Name).Replace(" ", "");

                        FieldBuilder FieldNameBuilder = typeBuilder.DefineField(PropName, type, FieldAttributes.Private);
                        // Use an array with no elements: new Type[] {})
                        PropertyBuilder FieldNamebuilder = typeBuilder.DefineProperty(PropName, PropertyAttributes.HasDefault, type, null);
                        // set of attributes.
                        MethodAttributes GetSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                        // Define the "get" accessor method for Field.
                        MethodBuilder GetPropertyMethodBuilder = typeBuilder.DefineMethod("get_" + PropName, GetSetAttr, type, Type.EmptyTypes);
                        ILGenerator GetField = GetPropertyMethodBuilder.GetILGenerator();
                        GetField.Emit(OpCodes.Ldarg_0);
                        GetField.Emit(OpCodes.Ldfld, FieldNameBuilder);
                        GetField.Emit(OpCodes.Ret);
                        // Define the "get" accessor method for Field.
                        MethodBuilder SetPropertyMethodBuilder = typeBuilder.DefineMethod("set_" + PropName, GetSetAttr, null, new Type[] { type });
                        ILGenerator SetField = SetPropertyMethodBuilder.GetILGenerator();
                        SetField.Emit(OpCodes.Ldarg_0);
                        SetField.Emit(OpCodes.Ldarg_1);
                        SetField.Emit(OpCodes.Stfld, FieldNameBuilder);
                        SetField.Emit(OpCodes.Ret);
                        // Their behaviors, "get" and "set" respectively. 
                        FieldNamebuilder.SetGetMethod(GetPropertyMethodBuilder);
                        FieldNamebuilder.SetSetMethod(SetPropertyMethodBuilder);
                    }
                }

            }

            Type retval = typeBuilder.CreateType();

            return retval;
        }
        public string GetComponentIdByCaseId(int id)
        {
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            return _db.Case.Where(d => d.Id == id).FirstOrDefault().CaseFormId.ToString();
        }
        public object RunRules(object[] CaseIds = null, string ComponentId = "")
        {

            List<object> results = new List<object>();
            foreach (var item in CaseIds)
            {
                int id = Convert.ToInt32(item);
                string formid = GetComponentIdByCaseId(id);
                List<DataField> data = GetFormDatas(Convert.ToInt32(formid), id);
                //string ComponentId = data.Where(x => x.FieldName.Contains("ComponentId")).FirstOrDefault().FieldValue;
                if (!String.IsNullOrEmpty(ComponentId) && ComponentId != "")
                {
                    //int skillset = 0;

                    CaseAssignment component = this.Theme.GetComponent(ComponentId);
                    List<FilterRule> filterRuleList = JsonConvert.DeserializeObject<List<FilterRule>>(WebUtility.UrlDecode(component.json)) ?? new List<FilterRule>();

                    int i = 0;

                    var caseToAssign = _db.Case.Where(x => x.Id == id).FirstOrDefault();
                    var temp = JsonConvert.SerializeObject(caseToAssign, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });

                    var oldCase = JsonConvert.DeserializeObject<Case>(temp);
                    caseToAssign.AssignedAt = null;
                    caseToAssign.AssignedTo = null;
                    if (this.auditLogService == null) this.auditLogService = this.Theme.HttpContext.RequestServices.GetService(typeof(IAuditLogService)) as IAuditLogService;
                    foreach (var filterRule in filterRuleList)
                    {
                        if (filterRule != null)
                        {
                            Type custDataType = BuildDynamicTypeWithProperties(Convert.ToInt32(component.FormId));
                            MethodInfo method = typeof(CaseAssignment).GetMethod("_RunRules");
                            MethodInfo generic = method.MakeGenericMethod(custDataType);

                            object res = generic.Invoke(this, new object[] { filterRule, Convert.ToInt32(component.FormId), data });

                            var property = typeof(ICollection).GetProperty("Count");

                            int count = (int)property.GetValue(res, null);

                            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

                            if (string.IsNullOrWhiteSpace(caseToAssign.AssignedTo))
                            {

                                if (count > 0)
                                {

                                    Int32.TryParse(component.SkillSetId[i], out int skillSetId);
                                    //int caseLimit = _db.SkillSet.Where(x => x.Id == skillSetId).Select(x => x.CaseLimit).FirstOrDefault();

                                    var userList = _db.UserSkillSet.Where(x => x.SkillSetId == skillSetId).ToList();

                                    if (string.IsNullOrWhiteSpace(component.RoleId))
                                    {

                                        var user = (from userSkill in _db.UserSkillSet
                                                     where userSkill.SkillSetId == skillSetId
                                                     join cases in
                                                     (from c in _db.Case
                                                      group c by c.AssignedTo into caseGroup
                                                      select new
                                                      {
                                                          AssignedTo = caseGroup.Key,
                                                          Count = caseGroup.Select(x => x.Id).Count()
                                                      }) on userSkill.UserId equals cases.AssignedTo into usercasjoinskill
                                                     from usercases in usercasjoinskill.DefaultIfEmpty()
                                                     select new { userid = userSkill.UserId, count = usercases == null ? 0 : usercases.Count } into userbeforeorder
                                                     orderby userbeforeorder.count
                                                     select userbeforeorder).FirstOrDefault();

                                        user = null;
                                        if (user != null && !string.IsNullOrWhiteSpace(user.userid))
                                        {
                                            caseToAssign.AssignedAt = DateTime.UtcNow;
                                            caseToAssign.AssignedTo = user.userid;
                                            caseToAssign.DueDate = DateTime.UtcNow.AddHours(component.TimeLimit[i]);

                                            _db.Update(caseToAssign);
                                            _db.SaveChanges();
                                            bool result = auditLogService.LogWriter(caseToAssign, (int)AuditLogOperation.Update, caseToAssign.Id.ToString(), new Case(), oldCase).Result;
                                        }
                                    }
                                    else
                                    {

                                        var user = (from userSkill in _db.UserSkillSet
                                                     where userSkill.SkillSetId == skillSetId
                                                     join ur in _db.UserRoles on userSkill.UserId equals ur.UserId
                                                     where ur.RoleId == component.RoleId 
                                                     join cases in
                                                     (from c in _db.Case
                                                      group c by c.AssignedTo into caseGroup
                                                      select new
                                                      {
                                                          AssignedTo = caseGroup.Key,
                                                          Count = caseGroup.Select(x => x.Id).Count()
                                                      }) on userSkill.UserId equals cases.AssignedTo into usercasjoinskill
                                                     from usercases in usercasjoinskill.DefaultIfEmpty()
                                                     select new { userid = userSkill.UserId, count = usercases == null ? 0 : usercases.Count } into userbeforeorder
                                                     orderby userbeforeorder.count
                                                     select userbeforeorder).FirstOrDefault();

                                        if (user != null && !string.IsNullOrWhiteSpace(user.userid))
                                        {
                                            caseToAssign.AssignedAt = DateTime.UtcNow;
                                            caseToAssign.AssignedTo = user.userid;
                                            caseToAssign.DueDate = DateTime.UtcNow.AddHours(component.TimeLimit[i]);

                                            _db.Update(caseToAssign);
                                            _db.SaveChanges();
                                            bool result = auditLogService.LogWriter(caseToAssign, (int)AuditLogOperation.Update, caseToAssign.Id.ToString(), new Case(), oldCase).Result;
                                        }
                                    }

                                }

                            }

                            results.Add(new CaseAssignmentResult() { Case = res, Component = component });
                        }

                        i++;
                    }

                }
            }
            return results;
        }
        public List<T> _RunRules<T>(FilterRule filterRule = null, int FormId = 0, List<DataField> data = null)
        {
            if (this._db == null) this._db = this.Theme.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            Type DataType = typeof(T);
            IList<object> DList = new List<object>();
            object custData = Activator.CreateInstance(DataType);
            PropertyInfo[] custDataPropInfo = DataType.GetProperties();
            foreach (PropertyInfo pInfo in custDataPropInfo)
            {
                try
                {
                    string value = data.Where(d => d?.Id == pInfo?.Name?.ToString())?.FirstOrDefault()?.FieldValue;
                    var propType = pInfo.PropertyType;
                    var converter = TypeDescriptor.GetConverter(propType);
                    var convertedObject = converter.ConvertFromString(value);
                    DataType.InvokeMember(pInfo.Name, BindingFlags.SetProperty, null, custData, new object[] { convertedObject });
                    DataType.InvokeMember(pInfo.Name, BindingFlags.GetProperty, null, custData, new object[] { });
                }

                catch (Exception ex)
                {

                }
            }
            Type t = typeof(List<>).MakeGenericType(DataType);
            IList res = Activator.CreateInstance(t) as IList;
            res.Add(custData);
            List<T> myList = new List<T>();
            foreach (T item in res)
            {
                myList.Add(item);
            }
            return myList.BuildQuery(filterRule).ToList();
        }
    }

}
