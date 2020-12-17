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
using static Cicero.Service.Services.ISynchronizeService;

namespace Cicero.Service.Services
{
    public interface IAutomationService
    {
        //bool Automate(dynamic _new, int caseFormId, int fromStateId, int toStateId, string form, int caseId);
        List<Operator> GetOperators();
        List<FormBuilderViewModel.Form.Table> GetTables(int caseFormId);
        bool StartAutomation(dynamic component, int caseId, List<List<bool>> dts);
        int CaseAutomationSystem(dynamic _new, int caseFormId, int fromStateId, int toStateId, string form, int caseId);
        List<List<bool>> SetAutomationValue(dynamic component, int caseId);
    }
    public class Operator
    {
        public string field { get; set; }
        public string value { get; set; }
    }
    public class AutomationService : IAutomationService
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

        public AutomationService(ICommonService commonService, IFormService formService, IFormBuilderService formBuilderService, ApplicationDbContext db, Utils utils, ILogger<SynchronizeService> log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, IActivityLogService activityLogService, IRazorToStringRender razorToStringRender, ITemplateService templateService, AppSetting setting, IQueueService queueService, IMessageService messageService)
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

        public List<Operator> GetOperators()
        {
            string[] operators = { "="
                    , ">"
                    , "<"
                    , "<>"
                    , ">="
                    , "<="
                    , "Contains"
                    , "Does Not Contain"

            };
            List<Operator> ops = new List<Operator>();
            foreach (var item in operators)
            {
                ops.Add(new Operator { field = item, value = item });
            }
            return ops;
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
        public int CaseAutomationSystem(dynamic _new, int caseFormId, int fromStateId, int toStateId, string form, int caseId)
        {
            this.HttpContext = _new.HttpContext as HttpContext;
            Theme _theme = _new.Theme as Theme;
            List<List<bool>> dts = new List<List<bool>>();

            dynamic components = _theme.GetComponentsByType("Themes.Core.Components.CaseAutomation");
            int returnStateId = 0;

            foreach (dynamic component in components)
            {
                if (Convert.ToInt64(component.configs[0].source) == caseFormId && Convert.ToInt32(component.configs[0].pull) == fromStateId && Convert.ToInt32(component.configs[0].pass) == toStateId)
                {
                    //if (Convert.ToInt32(component.configs[0].pull) == fromStateId && Convert.ToInt32(component.configs[0].pass) == toStateId)
                    //{
                    dts = SetAutomationValue(component, caseId);
                    if (dts.Count != 0) //check if returned value consists of datatables for sync
                    {
                        try
                        {
                            if (!StartAutomation(component, caseId, dts))
                            {
                                returnStateId = Convert.ToInt32(component.configs[0].fail);
                            }//start sync process
                            else
                            {
                                returnStateId = toStateId;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    else // fail
                    {
                        returnStateId = Convert.ToInt32(component.configs[0].fail);
                    }
                    break;

                }

            }

            return returnStateId;
        }
        
        public List<List<bool>> SetAutomationValue(dynamic component, int caseId)
        {
            List<List<bool>> ret = new List<List<bool>>();
            int totalCheck = component.configs[0].sourceTable.Count;
            List<bool> results = new List<bool>();

            for (int i = 0; i < totalCheck; i++)
            {
                string tablename = component.configs[0].sourceTable[i];
                string fieldname = component.configs[0].sourceField[i];
                string operatorname = component.configs[0].operators[i];
                string fieldvalues = component.configs[0].valueFields[i];

                var Forms = _db.CustomEntities.FromSql($"SELECT * FROM [{tablename}] Where [CaseId] = {caseId}", tablename, caseId);
                JObject jsonObj = new JObject();
                if (Forms != null && Forms.Count() != 0)
                {
                    foreach (var item in Forms)
                    {
                        jsonObj = (JObject)JsonConvert.DeserializeObject(item.Extras);
                        foreach (var data in jsonObj)
                        {
                            if (data.Key == fieldname)
                            {
                                switch (operatorname)
                                {
                                    case "=":

                                        if (Utils.ConvertToString(data.Value) == Utils.ConvertToString(fieldvalues))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case ">":

                                        if ((data.Value != null && fieldvalues != null) && (Convert.ToInt64(data.Value) > Convert.ToInt64(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case "<":

                                        if ((data.Value != null && fieldvalues != null) && (Convert.ToInt64(data.Value) < Convert.ToInt64(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case ">=":

                                        if ((data.Value != null && fieldvalues != null) && (Convert.ToInt64(data.Value) >= Convert.ToInt64(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case "<=":

                                        if ((data.Value != null && fieldvalues != null) && (Convert.ToInt64(data.Value) <= Convert.ToInt64(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case "Contains":

                                        if (Utils.ConvertToString(data.Value).Contains(Utils.ConvertToString(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                    case "Does Not Contain":
                                        if (!Utils.ConvertToString(data.Value).Contains(Utils.ConvertToString(fieldvalues)))
                                            results.Add(true);
                                        else
                                            results.Add(false);
                                        break;
                                }

                            }
                        }
                    }

                }


                ret.Add(results);
            }


            return ret;
        }
        public bool StartAutomation(dynamic component, int caseId, List<List<bool>> dts)
        {
            bool returnval = true;
            bool chkval = dts[0][0];
            //if (!chkval)
            //{
            //    return false;
            //}
            //else
            //{
            int totalCheck = component.configs[0].sourcepolicyfieldtable.Count;
            for (int i = 1; i < totalCheck; i++)
            {
                string logical = component.configs[0].logical[i];
                if (logical.ToUpper() == "AND")
                {
                    if (!(chkval && dts[i][0]))
                        return false;
                    else chkval = true;

                }
                else if (logical.ToUpper() == "OR")
                {
                    if (!(chkval || dts[i][0]))
                        return false;
                    else chkval = true;
                }
            }
            //}
            return returnval;
        }

        /// <summary>
        /// GetTables and Data from caseId
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public List<FormBuilderViewModel.Form.Table> GetTables(int caseFormId)
        {
            List<FormBuilderViewModel.Form.Table> tablesSource=new List<FormBuilderViewModel.Form.Table>();
            var ccvm = _formBuilderService.GetBuilderFormById(caseFormId);
            if (ccvm.Fields != null)
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                FormBuilderViewModel sourceFormBuilderVM = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                tablesSource = sourceFormBuilderVM.Forms.Tables;
            }
            
            

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
                                    jsonObjDB.Add(tblElm.Name, "");
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




    }
}
