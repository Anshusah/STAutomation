using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Models.Core.FormBuilderViewModel;
using static Cicero.Service.Models.Core.FormBuilderViewModel.Form;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]

    public class Ajax : Cicero.Areas.Admin.Controllers.BaseController
    {
        private readonly ICiceroCoreFormService _CoreFormService;
        public IFormBuilderService _formBuilderService;
        public IUserService _IUserService;
        public ApplicationDbContext db = null;
        public AppSetting appSetting;
        private readonly Utils _utils;
        public Ajax(IUserService __IUserService, IFormBuilderService icf, ApplicationDbContext _db, Utils utils, ICiceroCoreFormService Iccf) : base(__IUserService)
        {
            _IUserService = __IUserService;
            _formBuilderService = icf;
            _CoreFormService = Iccf;
            _utils = utils;
            db = _db;

        }
        [HttpGet]
        [HttpPost]
        [Route("~/ajax.html")]
        public IActionResult Index(IFormCollection data)
        {
            //int tenantid = 1;// please set it dynamically
            string content = string.Empty;
            Dictionary<string, string> arg = new Dictionary<string, string>();
            var Forms = data.Keys;
            foreach (var _Form in Forms)
            {
                arg.Add(_Form.ToString(), Request.Form[_Form].ToString());

            }
            arg.TryGetValue("action", out string action);

            string ac = "";
            if (string.IsNullOrEmpty(action))
            {
                ac = Request.Query["action"].ToString();
            }
            else
            {
                ac = action.ToString();
            }
            if (!String.IsNullOrEmpty(ac))
            {
                if (appSetting == null) appSetting = HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
                string tenant_theme = appSetting.Get("app_theme");
                Type _type = Type.GetType("Themes." + tenant_theme + ".Setup");
                var o = Activator.CreateInstance(_type, HttpContext);
                object p = Convert.ChangeType(o, Type.GetType("Themes." + tenant_theme + ".Setup"));
                MethodInfo mi = _type.GetMethod("Init");
                mi.Invoke(p, null);
                MethodInfo _mi = _type.GetMethod("DoAction");
                content = content + _mi.Invoke(p, new object[] { ac, new object[] { arg } });


            }
            Request.Headers.TryGetValue("Accept", out StringValues accept);
            return Content(content);
        }

    }
    [Area("Admin")]
    public class AjaxController : Cicero.Areas.Admin.Controllers.BaseController
    {
        private readonly ICiceroCoreFormService _CoreFormService;
        public IFormBuilderService _formBuilderService;
        public IUserService _IUserService;
        public ApplicationDbContext db = null;
        public AppSetting appSetting;
        private readonly Utils _utils;
        public AjaxController(IUserService __IUserService, IFormBuilderService icf, ApplicationDbContext _db, Utils utils, ICiceroCoreFormService Iccf) : base(__IUserService)
        {
            _IUserService = __IUserService;
            _formBuilderService = icf;
            _CoreFormService = Iccf;
            _utils = utils;
            db = _db;

        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("admin/ajax/update-widget.html")]
        public JsonResult UpdateWidget(IFormCollection data)
        {
            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            data.TryGetValue("widget_id", out StringValues WidgetId);
            data.TryGetValue("widget_location", out StringValues WidgetLocation);
            data.TryGetValue("widget_type", out StringValues WidgetType);
            data.TryGetValue("action", out StringValues Action);
            Widgets wg = _theme.GetWidget((string)WidgetLocation.ToString(), (string)WidgetId.ToString());
            string type = (string)WidgetType.ToString();
            JObject widget_data = new JObject();
            if ((string)Action.ToString() == "update")
            {
                type = wg.WidgetType;
                widget_data = wg.Data as JObject;
            }

            var _object_instance = Activator.CreateInstance(Type.GetType(type));


            if ((string)Action.ToString() == "create")
            {
                MethodInfo mia = _object_instance.GetType().GetMethod("GetJson");
                string json = (string)mia.Invoke(_object_instance, null);
                widget_data = JObject.Parse(json) as JObject;
            }


            JObject _jobj = widget_data;

            if ((string)Action.ToString() == "update")
            {
                foreach (var _jval in _jobj)
                {
                    var _name = _jval.Key.ToString();
                    if (_name == "HttpContext" || _name == "Theme" || _name == "Data" || _name == "Action" || _name == "WidgetId" || _name == "WidgetLocation" || _name == "Name") continue;


                    PropertyInfo pef = _object_instance.GetType().GetProperty(_jval.Key.ToString());
                    if (pef != null)
                    {
                        //pef.SetValue(_object_instance, _jval.Value as _jval.Value.get));
                        Type type__ = _jval.Value.GetType();
                        if (type__ == typeof(JValue))
                        {
                            pef.SetValue(_object_instance, _jval.Value as JObject);
                        }
                        if (type__ == typeof(String))
                        {
                            pef.SetValue(_object_instance, (string)_jval.Value.ToString());
                        }
                    }

                }

            }
            _object_instance.GetType().GetProperty("WidgetId").SetValue(_object_instance, (string)WidgetId.ToString());
            _object_instance.GetType().GetProperty("Data").SetValue(_object_instance, _jobj);
            _object_instance.GetType().GetProperty("WidgetType").SetValue(_object_instance, type);
            _object_instance.GetType().GetProperty("WidgetLocation").SetValue(_object_instance, (string)WidgetLocation.ToString());
            _object_instance.GetType().GetProperty("Action").SetValue(_object_instance, (string)Action.ToString());
            _object_instance.GetType().GetProperty("HttpContext").SetValue(_object_instance, HttpContext);
            _object_instance.GetType().GetProperty("Theme").SetValue(_object_instance, _theme);
            var __object_instance = Activator.CreateInstance(Type.GetType(type));
            if ((string)Action.ToString() == "update")
            {
                foreach (KeyValuePair<string, StringValues> __jval in data)
                {
                    string ty = __jval.Key.ToString();
                    if (__jval.Key.Contains("[") || __jval.Key.Contains("]"))
                    {
                        ty = ty.Replace("[", "").Replace("]", "");
                    }
                    PropertyInfo e = __object_instance.GetType().GetProperty(ty);

                    if (e != null)
                    {
                        List<string> a = new List<string>();
                        if (__jval.Key.Contains("["))
                        {
                            var val_ = __jval.Value.ToArray();
                            foreach (var p in val_)
                            {
                                a.Add(p);
                            }
                            e.SetValue(__object_instance, a);
                        }
                        else
                        {
                            //==============
                            Type type__ = __jval.Value.GetType();

                            try
                            {
                                //var c = JsonConvert.DeserializeObject<List<string>>(__jval.Value.ToString());

                                e.SetValue(__object_instance, __jval.Value);


                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    e.SetValue(__object_instance, __jval.Value.ToString());
                                }
                                catch (Exception ex1)
                                {
                                    e.SetValue(__object_instance, null);
                                }

                            }

                        }


                    }


                }
            }
            __object_instance.GetType().GetProperty("WidgetId").SetValue(__object_instance, (string)WidgetId.ToString());
            __object_instance.GetType().GetProperty("Data").SetValue(__object_instance, _jobj);
            __object_instance.GetType().GetProperty("WidgetType").SetValue(__object_instance, type);
            __object_instance.GetType().GetProperty("WidgetLocation").SetValue(__object_instance, (string)WidgetLocation.ToString());
            __object_instance.GetType().GetProperty("Action").SetValue(__object_instance, (string)Action.ToString());

            __object_instance.GetType().GetProperty("HttpContext").SetValue(__object_instance, HttpContext);
            __object_instance.GetType().GetProperty("Theme").SetValue(__object_instance, _theme);
            MethodInfo mi = Type.GetType(type).GetMethod("OnUpdate");
            WidgetResponse wr = mi.Invoke(_object_instance, new object[] { __object_instance, _object_instance }) as WidgetResponse; ;
            if (wr != null && !string.IsNullOrEmpty(wr.Content))
            {
                str = str + wr.Content.ToString();
            }


            object obs = new { content = str, subtitle = wr.SubTitle, title = wr.Title + "".ToString(), widget_id = (string)WidgetId + "".ToString(), widget_location = (string)WidgetLocation + "".ToString(), widget_type = (string)WidgetType + "".ToString(), action = (string)Action + "".ToString() };


            return Json(obs);


        }
        [HttpPost]
        [Route("admin/ajax/update-component.html")]
        public JsonResult UpdateComponent(IFormCollection data)
        {

            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            data.TryGetValue("component_id", out StringValues ComponentId);
            data.TryGetValue("component_type", out StringValues ComponentType);
            data.TryGetValue("action", out StringValues Action);
            data.TryGetValue("form_id", out StringValues FormId);
            data.TryGetValue("element_id", out StringValues ElementId);
            data.TryGetValue("event_type", out StringValues EventType);
            dynamic UpdatedModel = Activator.CreateInstance(Type.GetType(ComponentType.FirstOrDefault()));

            IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(UpdatedModel);
            if (Action.FirstOrDefault() == "update" && IsFormUpdated.IsCompleted == false)
            {
                object error = new { content = str, subtitle = "", title = "Error", component_id = ComponentId.FirstOrDefault(), component_type = ComponentType.FirstOrDefault(), action = Action.FirstOrDefault(), form_id = FormId.FirstOrDefault() };
                return Json(error);
            }

            dynamic wg;

            if(ElementId.Count>0 && EventType.Count>0)
            {
                wg = _theme.GetElementComponentById(ComponentId.FirstOrDefault(), Convert.ToInt32(FormId.FirstOrDefault()), ElementId.FirstOrDefault(), Convert.ToInt32(EventType.FirstOrDefault()));
            }
            else
            {
                wg = _theme.GetComponent(ComponentId.FirstOrDefault());
            }
            string type = ComponentType.FirstOrDefault();
            JObject component_data = new JObject();
            if (Action.FirstOrDefault().ToString() == "update")
            {
                type = wg.ComponentType;
                if (string.IsNullOrEmpty(type))
                {
                    type = ComponentType.FirstOrDefault();
                }
                component_data = wg.Data as JObject;
            }

            var _object_instance = Activator.CreateInstance(Type.GetType(type));


            if ((string)Action.ToString() == "create")
            {
                MethodInfo mia = _object_instance.GetType().GetMethod("GetJson");
                string json = (string)mia.Invoke(_object_instance, null);
                component_data = JObject.Parse(json) as JObject;
                _object_instance.GetType().GetProperty("Data").SetValue(_object_instance, component_data);
            }


            JObject _jobj = component_data;

            //if (Action.FirstOrDefault() == "update" && _jobj != null)
            //{
            //    foreach (var _jval in _jobj)
            //    {
            //        var _name = _jval.Key.ToString();
            //        // var obj =  JsonConvert.SerializeObject(ssvm);
            //        if (_name == "HttpContext" || _name == "Theme" || _name == "Data" || _name == "Action" || _name == "ComponentId" || _name == "Name") continue;

            //        //System.Reflection.PropertyInfo
            //        PropertyInfo pef = _object_instance.GetType().GetProperty(_jval.Key.ToString());
            //        if (pef != null)
            //        {
            //            //pef.SetValue(_object_instance, _jval.Value as _jval.Value.get));
            //            Type type__ = _jval.Value.GetType();
            //            var xe = pef.PropertyType;
            //            if (pef.PropertyType == typeof(List<string>))
            //            {
            //                pef.SetValue(_object_instance, _jval.Value.FirstOrDefault());
            //            }
            //            else if (type__ == typeof(JValue))
            //            {
            //                pef.SetValue(_object_instance, _jval.Value.FirstOrDefault());
            //            }
            //            else if (type__ == typeof(String))
            //            {
            //                pef.SetValue(_object_instance, (string)_jval.Value.ToString());
            //            }
            //        }

            //    }

            //}
            _object_instance.GetType().GetProperty("ComponentId").SetValue(_object_instance, ComponentId.FirstOrDefault().ToString());
            _object_instance.GetType().GetProperty("Data").SetValue(_object_instance, _jobj);
            _object_instance.GetType().GetProperty("ComponentType").SetValue(_object_instance, type);
            _object_instance.GetType().GetProperty("FormId").SetValue(_object_instance, FormId.FirstOrDefault());
            _object_instance.GetType().GetProperty("Action").SetValue(_object_instance, Action.FirstOrDefault());
            _object_instance.GetType().GetProperty("HttpContext").SetValue(_object_instance, HttpContext);
            _object_instance.GetType().GetProperty("Theme").SetValue(_object_instance, _theme);
            var __object_instance = new Object();

            if ((Action.FirstOrDefault() == "update" || Action.FirstOrDefault() == "create") && IsFormUpdated.IsCompleted == true)
            {
                __object_instance = UpdatedModel;
            }
            else
            {
                __object_instance = Activator.CreateInstance(Type.GetType(type));
            }

            __object_instance.GetType().GetProperty("ComponentId").SetValue(__object_instance, (string)ComponentId.FirstOrDefault().ToString());
            __object_instance.GetType().GetProperty("Data").SetValue(__object_instance, _jobj);
            __object_instance.GetType().GetProperty("ComponentType").SetValue(__object_instance, type);
            __object_instance.GetType().GetProperty("Action").SetValue(__object_instance, (string)Action.ToString());
            __object_instance.GetType().GetProperty("FormId").SetValue(__object_instance, (string)FormId.FirstOrDefault());
            __object_instance.GetType().GetProperty("HttpContext").SetValue(__object_instance, HttpContext);
            __object_instance.GetType().GetProperty("Theme").SetValue(__object_instance, _theme);
            
            ComponentResponse wr;
            if (ElementId.Count > 0 && EventType.Count > 0)
            {
                MethodInfo mi = __object_instance.GetType().GetMethod("OnUpdateElm");
                wr = mi.Invoke(_object_instance, new object[] { __object_instance, _object_instance, Convert.ToInt32(FormId.FirstOrDefault()),ElementId.FirstOrDefault(), Convert.ToInt32(EventType.FirstOrDefault()) }) as ComponentResponse;
            }
            else
            {
                MethodInfo mi = __object_instance.GetType().GetMethod("OnUpdate");
                wr = mi.Invoke(_object_instance, new object[] { __object_instance, _object_instance }) as ComponentResponse;
            }
            
            if (wr != null && !string.IsNullOrEmpty(wr.Content))
            {
                str = str + wr.Content.ToString();
            }
            object obs = new { content = str, subtitle = wr.SubTitle, title = wr.Title + "".ToString(), component_id = (string)ComponentId + "".ToString(), component_type = (string)ComponentType + "".ToString(), action = (string)Action + "".ToString(), form_id = (string)FormId + "".ToString() };
            return Json(obs);
        }
        [HttpPost]
        [Route("admin/ajax/delete-widget.html")]
        public JsonResult DeleteWidget(IFormCollection data)
        {
            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            data.TryGetValue("widget_id", out StringValues WidgetId);
            data.TryGetValue("widget_location", out StringValues WidgetLocation);
            data.TryGetValue("widget_type", out StringValues WidgetType);
            data.TryGetValue("action", out StringValues Action);
            Widgets wg = _theme.GetWidget((string)WidgetLocation.ToString(), (string)WidgetId.ToString());
            string type = (string)WidgetType.ToString();
            wg.GetType().GetProperty("WidgetId").SetValue(wg, (string)WidgetId.ToString());
            wg.GetType().GetProperty("Data").SetValue(wg, wg.Data as JObject);
            wg.GetType().GetProperty("WidgetType").SetValue(wg, type);
            wg.GetType().GetProperty("WidgetLocation").SetValue(wg, (string)WidgetLocation.ToString());
            wg.GetType().GetProperty("Action").SetValue(wg, (string)Action.ToString());
            wg.GetType().GetProperty("HttpContext").SetValue(wg, HttpContext);
            wg.GetType().GetProperty("Theme").SetValue(wg, _theme);
            object rew = new { };
            WidgetResponse wr = wg.Delete<Widgets>(wg);
            if (wr.Success)
            {
                rew = new { status = "", widget_id = (string)WidgetId + "".ToString(), widget_location = (string)WidgetLocation + "".ToString(), widget_type = (string)WidgetType + "".ToString(), action = (string)Action + "".ToString() };
            }
            return Json(rew);

        }
        [HttpPost]
        [Route("admin/ajax/delete-component.html")]
        public JsonResult DeleteComponentt(IFormCollection data)
        {
            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            data.TryGetValue("component_id", out StringValues ComponentId);
            data.TryGetValue("component_type", out StringValues ComponentType);
            data.TryGetValue("action", out StringValues Action);
            data.TryGetValue("form_id", out StringValues FormId);
            data.TryGetValue("element_id", out StringValues ElementId);
            data.TryGetValue("event_type", out StringValues EventType);
            Cicero.Service.Models.Core.Component wg;
            if (ElementId.Count> 0 && EventType.Count>0)
            {
                wg = _theme.GetElementComponentById((string)ComponentId.ToString(),Convert.ToInt32(FormId.FirstOrDefault()),ElementId,Convert.ToInt32(EventType));
            }
            else
            {
                wg = _theme.GetComponent((string)ComponentId.ToString());
            }
            
            string type = (string)ComponentType.ToString();
            wg.GetType().GetProperty("ComponentId").SetValue(wg, (string)ComponentId.ToString());
            wg.GetType().GetProperty("Data").SetValue(wg, wg.Data as JObject);
            wg.GetType().GetProperty("ComponentType").SetValue(wg, type);
            wg.GetType().GetProperty("Action").SetValue(wg, (string)Action.ToString());
            wg.GetType().GetProperty("HttpContext").SetValue(wg, HttpContext);
            wg.GetType().GetProperty("Theme").SetValue(wg, _theme);
            object rew = new { };
            ComponentResponse wr;
            if (ElementId.Count > 0 && EventType.Count > 0)
            {
                wr = wg.Delete<Cicero.Service.Models.Core.Component>(wg, Convert.ToInt32(EventType) , ElementId, Convert.ToInt32(FormId.FirstOrDefault()));
            }
            else
            {
                wr = wg.Delete<Cicero.Service.Models.Core.Component>(wg);
            }
               
            if (wr.Success)
            {
                rew = new { status = "", component_id = (string)ComponentId + "".ToString(), component_type = (string)ComponentType + "".ToString(), action = (string)Action + "".ToString() };
            }
            return Json(rew);

        }
        [HttpPost]
        [Route("admin/ajax/file-tree.html")]
        public ActionResult fileTree(IFormCollection data)
        {
            //string location = "";
            data.TryGetValue("dir", out StringValues dir);
            string location = dir.ToString();
            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            string ex = _theme.GetFileTreeAsHtml(location, "");
            return Content(ex);

        }
        [HttpGet]
        [Route("admin/ajax/file-content.html")]
        public ActionResult ajaxContent(IFormCollection data)
        {
            string location = Request.Query["path"].ToString();
            string path = System.IO.File.ReadAllText(location);
            //string str = string.Empty;
            //Theme _theme = this.Theme as Theme;
            //string ex = _theme.GetFileTreeAsHtml();
            return Content(path);

        }
        [HttpPost]
        [Route("admin/ajax/update-form-builder.html")]
        public JsonResult UpdateFormBuilder(IFormCollection data)
        {

            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            object response = null;
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            try
            {
                dynamic newElm = null;
                dynamic Updated = null;
                bool Created = false;
                data.TryGetValue("TabEnable", out StringValues TabEnable);
                data.TryGetValue("Type", out StringValues ElementType);
                data.TryGetValue("ElementId", out StringValues ElementId);
                data.TryGetValue("ActionFor", out StringValues ActionFor);
                data.TryGetValue("Action", out StringValues Action);
                data.TryGetValue("FormId", out StringValues FormId);
                data.TryGetValue("VisibleinGrid", out StringValues VisibleinGrid);
                data.TryGetValue("FrontendVisible", out StringValues FrontendVisible);
                data.TryGetValue("BackendVisible", out StringValues BackendVisible);
                data.TryGetValue("IsCurrency", out StringValues IsCurrency);
                data.TryGetValue("IsTelephoneNumber", out StringValues IsTelephoneNumber);
                data.TryGetValue("automationEnable", out StringValues automationEnable);
                data.TryGetValue("VisibleinFooter", out StringValues VisibleinFooter);
                data.TryGetValue("BackendIconVisible", out StringValues BackendIconVisible);
                data.TryGetValue("FrontendIconVisible", out StringValues FrontendIconVisible);
                data.TryGetValue("IsDelete", out StringValues IsDelete);
                data.TryGetValue("UseFromDatabase", out StringValues UserFormDatabase);
                data.TryGetValue("SelectFromDbIsToggleOption", out StringValues SelectFromDbIsToggleOption);

                data.TryGetValue("FrontendLabelVisibility", out StringValues FrontendLabelVisibility);
                data.TryGetValue("FrontendIconVisibility", out StringValues FrontendIconVisibility);
                data.TryGetValue("FrontendImageVisibility", out StringValues FrontendImageVisibility);
                data.TryGetValue("BackendLabelVisibility", out StringValues BackendLabelVisibility);
                data.TryGetValue("BackendIconVisibility", out StringValues BackendIconVisibility);
                data.TryGetValue("BackendImageVisibility", out StringValues BackendImageVisibility);
                data.TryGetValue("IsOnChangeEvent", out StringValues IsOnChangeEvent);
                data.TryGetValue("IsOnClickEvent", out StringValues IsOnClickEvent);
                data.TryGetValue("IsOnKeyUpEvent", out StringValues IsOnKeyUpEvent);
                data.TryGetValue("IsOnLoadEvent", out StringValues IsOnLoadEvent);
                data.TryGetValue("IsOnSaveFormEvent", out StringValues IsOnSaveFormEvent);
                data.TryGetValue("IsOnSwitchTabEvent", out StringValues IsOnSwitchTabEvent);
                data.TryGetValue("IsValidateOnTabSwitch", out StringValues IsValidateOnTabSwitch);
                data.TryGetValue("IsOnResponseTarget", out StringValues IsOnResponseTarget);
                data.TryGetValue("EnablePopup", out StringValues EnablePopup);
                data.TryGetValue("DisablePopupClose", out StringValues DisablePopupClose);

                data.TryGetValue("FrontendLabelVisible", out StringValues FrontendLabelVisible);
                data.TryGetValue("BackendLabelVisible", out StringValues BackendLabelVisible);
                data.TryGetValue("ParentElementId", out StringValues ParentElementId);
                data.TryGetValue("ElementTable", out StringValues ElementTable);
                data.TryGetValue("IsCreate", out StringValues IsCreate);

                data.TryGetValue("SetAsRepeatItem", out StringValues SetAsRepeatItem);
                data.TryGetValue("ShowInAccordion", out StringValues ShowInAccordion);

                if (ParentElementId.Count > 0)
                {
                    if (IsCreate.Count > 0 && Convert.ToBoolean(IsCreate.FirstOrDefault()))
                    {


                        data.TryGetValue("ElmId", out StringValues ElmId);
                        data.TryGetValue("TableColumnIndex", out StringValues TableColumnIndex);
                        data.TryGetValue("ElementType", out StringValues ElmType);
                        newElm = Activator.CreateInstance(Type.GetType("" + ElmType.FirstOrDefault() + ", Cicero.Service"));
                        Created = true;
                        newElm.TabIndex = data["TabIndex"].ToString();
                        newElm.RowIndex = data["RowIndex"].ToString();
                        newElm.ColumnIndex = data["ColumnIndex"].ToString();
                        newElm.TableElementId = ParentElementId.ToString();
                        newElm.TableColumnIndex = TableColumnIndex.ToString();
                        newElm.ElementId = ElmId.ToString();
                        Updated = newElm;
                    }
                }
                int valFormId = Convert.ToInt32(FormId.FirstOrDefault());
                if (valFormId == 0)
                {
                    CaseFormViewModel Newccvm = new CaseFormViewModel
                    {
                        Id = valFormId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Name = "Untitled"
                    };
                    FormBuilderViewModel NewOrders1 = new FormBuilderViewModel();
                    this.TryUpdateModelAsync(NewOrders1);
                    FormBuilderViewModel fbvm1 = new FormBuilderViewModel();
                    Newccvm.TabEnable = NewOrders1.Forms?.TabEnable;
                    if (ElementType.FirstOrDefault() == "Form")
                    {
                        fbvm1.Forms = NewOrders1.Forms;
                        Newccvm.UrlIdentifier = NewOrders1.Forms.Navigations.Identifier;
                        Newccvm.ModelName = NewOrders1.Forms.Navigations.Name;
                        Newccvm.Icon = NewOrders1.Forms.Navigations.Icon;
                        Newccvm.ModelTitle = NewOrders1.Forms.Navigations.Title;

                        Created = true;
                    }
                    else
                    {
                        Form frm = new Form();
                        Navigation nv = new Navigation();
                        frm.Navigations = nv;
                        frm.ElementId = Utils.GenerateId();
                        fbvm1.Forms = frm;
                    }

                    foreach (Cicero.Service.Models.Core.Elements.Tab tab_item in NewOrders1.Tab)
                    {
                        if (!string.IsNullOrEmpty(tab_item.ElementId))
                        {
                            Cicero.Service.Models.Core.Elements.Tab tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                            Created = true;
                            this.TryUpdateModelAsync(tab);
                            if (ElementId.FirstOrDefault() == tab.ElementId && ActionFor.FirstOrDefault() == "tab")
                            {
                                IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(tab);
                                Updated = tab;
                            }
                            tab.ElementId = tab_item.ElementId;
                            tab.ElementIndex = tab_item.ElementIndex;
                            tab.Type = tab.GetType().FullName;
                            foreach (var row_item in tab_item.Row)
                            {
                                if (!string.IsNullOrEmpty(row_item.ElementId))
                                {


                                    Cicero.Service.Models.Core.Elements.Row row = new Cicero.Service.Models.Core.Elements.Row() { };
                                    Created = true;

                                    if (ElementId.FirstOrDefault() == row.ElementId && ActionFor.FirstOrDefault() == "row")
                                    {
                                        IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(row);
                                        Updated = row;
                                    }
                                    if (row.ElementId == ElementId)
                                    {
                                        row.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                        row.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                        row.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                        row.SetAsRepeatItem = (SetAsRepeatItem.Count == 0 ? false : true);
                                        row.ShowInAccordion = (ShowInAccordion.Count == 0 ? false : true);

                                    }
                                    row.ElementId = row_item.ElementId;
                                    row.ElementIndex = row_item.ElementIndex;
                                    row.Type = row.GetType().FullName;

                                    foreach (var column_item in row_item.Column)
                                    {
                                        if (!string.IsNullOrEmpty(column_item.ElementId))
                                        {

                                            Cicero.Service.Models.Core.Elements.Column column = new Cicero.Service.Models.Core.Elements.Column() { };
                                            Created = true;

                                            if (ElementId.FirstOrDefault() == column.ElementId && ActionFor.FirstOrDefault() == "column")
                                            {
                                                IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(column);
                                                Updated = column;
                                            }
                                            column.ElementId = column_item.ElementId;
                                            column.ElementIndex = column_item.ElementIndex;
                                            column.Type = column.GetType().FullName;

                                            foreach (var element_item in column_item.Element)
                                            {
                                                if (!string.IsNullOrEmpty(element_item.ElementId))
                                                {

                                                    dynamic element = null;

                                                    element = Activator.CreateInstance(Type.GetType("" + ElementType.FirstOrDefault() + ", Cicero.Service"));
                                                    Created = true;
                                                    if (element != null && element.ElementId != null && ElementId.FirstOrDefault() == element.ElementId && ActionFor.FirstOrDefault() == "element")
                                                    {
                                                        IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(element);
                                                        Updated = element;
                                                    }
                                                    if (element != null)
                                                    {
                                                        element.ElementId = element_item.ElementId;
                                                        element.ElementIndex = element_item.ElementIndex;
                                                        element.Type = element?.GetType().FullName;
                                                        if (element.ElementId == ElementId)
                                                        {
                                                            element.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                                            element.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                                            element.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                                        }
                                                        column.Element.Add(element);
                                                    }
                                                }
                                            }
                                            row.Column.Add(column);
                                        }
                                    }
                                    tab.Row.Add(row);
                                }
                            }
                            fbvm1.Tab.Add(tab);
                        }
                    }

                    if (fbvm1.Tab.Count > 0)
                    {
                        string jsonsstr1 = JsonConvert.SerializeObject(fbvm1, settings).Trim();
                        Newccvm.Fields = jsonsstr1;

                        Newccvm = _formBuilderService.CreateOrUpdateModal(Newccvm);

                    }
                    //return this.RenderFormBuilderElements(data);

                    //For Table Creation and Core Form Builder
                    valFormId = Newccvm.Id;
                    if (data["Type"] == "Form")
                    {

                        _CoreFormService.CreateDymanicCoreForm(fbvm1, valFormId);
                    }
                    valFormId = Newccvm.Id;
                }
                else
                {

                    var ccvm = db.CaseForm.Where(x => x.Id == Convert.ToInt32(FormId.FirstOrDefault())).FirstOrDefault();
                   
                    FormBuilderViewModel OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);

                    FormBuilderViewModel NewOrders = new FormBuilderViewModel();
                    this.TryUpdateModelAsync(NewOrders);
                    FormBuilderViewModel fbvm = new FormBuilderViewModel();
                    ccvm.TabEnable = NewOrders.Forms?.TabEnable;
                    if (ElementType.FirstOrDefault() == "Form")
                    {
                        fbvm.Forms = NewOrders.Forms;
                        // OldFbvm.Forms.Tables[0].Fields
                        ccvm.UrlIdentifier = NewOrders.Forms.Navigations.Identifier;
                        ccvm.ModelName = NewOrders.Forms.Navigations.Name;
                        ccvm.Icon = NewOrders.Forms.Navigations.Icon;
                        ccvm.ModelTitle = NewOrders.Forms.Navigations.Title;
                     

                        Created = true;
                    }
                    else
                    {
                        fbvm.Forms = OldFbvm.Forms;

                        ccvm.UrlIdentifier = OldFbvm.Forms.Navigations.Identifier;
                        ccvm.ModelName = OldFbvm.Forms.Navigations.Name;
                        ccvm.Icon = OldFbvm.Forms.Navigations.Icon;
                        ccvm.ModelTitle = OldFbvm.Forms.Navigations.Title;
                    }
                    foreach (Cicero.Service.Models.Core.Elements.Tab tab_item in NewOrders.Tab)
                    {
                        if (!string.IsNullOrEmpty(tab_item.ElementId))
                        {
                            OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                            Cicero.Service.Models.Core.Elements.Tab tab = OldFbvm?.Tab?.Where(x => x.ElementId == tab_item.ElementId)?.FirstOrDefault();
                            if (tab == null)
                            {
                                tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                                Created = true;
                                //this.TryUpdateModelAsync(tab);

                            }
                            else
                            {
                                if (tab.ElementId == ElementId)
                                {
                                    tab.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                    tab.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                    tab.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                    var isit = tab.GetType().GetProperty("BackendIconVisible");
                                    if (isit != null)
                                    {
                                        tab.BackendIconVisible = (BackendIconVisible.Count == 0 ? false : true);
                                    }

                                    isit = tab.GetType().GetProperty("FrontendIconVisible");
                                    if (isit != null)
                                    {
                                        tab.FrontendIconVisible = (FrontendIconVisible.Count == 0 ? false : true);
                                    }

                                    isit = tab.GetType().GetProperty("FrontendLabelVisible");
                                    if (isit != null)
                                    {
                                        tab.FrontendLabelVisible = (FrontendLabelVisible.Count == 0 ? false : true);
                                    }

                                    isit = tab.GetType().GetProperty("BackendLabelVisible");
                                    if (isit != null)
                                    {
                                        tab.BackendLabelVisible = (BackendLabelVisible.Count == 0 ? false : true);
                                    }

                                }

                            }
                            if (ElementId.FirstOrDefault() == tab_item.ElementId && ActionFor.FirstOrDefault() == "tab")
                            {
                                IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(tab);
                                Updated = tab;
                            }
                            tab.ElementId = tab_item.ElementId;
                            tab.ElementIndex = tab_item.ElementIndex;
                            tab.Type = tab.GetType().FullName;
                            tab.Row.Clear();
                            foreach (var row_item in tab_item.Row)
                            {
                                if (!string.IsNullOrEmpty(row_item.ElementId))
                                {
                                    //OldFbvm = OldFbvm1;
                                    OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                    Cicero.Service.Models.Core.Elements.Row row = OldFbvm.Tab?.Where(t => t.ElementId == tab_item.ElementId)?.FirstOrDefault()?.Row?.Where(x => x.ElementId == row_item.ElementId)?.FirstOrDefault();
                                    if (row == null)
                                    {
                                        row = new Cicero.Service.Models.Core.Elements.Row() { };
                                        Created = true;
                                    }
                                    if (ElementId.FirstOrDefault() == row.ElementId && ActionFor.FirstOrDefault() == "row")
                                    {
                                        IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(row);
                                        Updated = row;
                                    }
                                    if (row.ElementId == ElementId)
                                    {
                                        row.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                        row.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                        row.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                        row.SetAsRepeatItem = (SetAsRepeatItem.Count == 0 ? false : true);
                                        row.ShowInAccordion = (ShowInAccordion.Count == 0 ? false : true);
                                    }
                                    row.ElementId = row_item.ElementId;
                                    row.ElementIndex = row_item.ElementIndex;
                                    row.Type = row.GetType().FullName;
                                    row.Column.Clear();
                                    foreach (var column_item in row_item.Column)
                                    {
                                        if (!string.IsNullOrEmpty(column_item.ElementId))
                                        {
                                            //OldFbvm = OldFbvm1;
                                            OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                            Cicero.Service.Models.Core.Elements.Column column = OldFbvm.Tab?.Where(t => t.ElementId == tab_item.ElementId)?.FirstOrDefault()?.Row?.Where(r => r.ElementId == row_item.ElementId)?.FirstOrDefault()?.Column?.Where(x => x.ElementId == column_item.ElementId)?.FirstOrDefault();
                                            if (column == null)
                                            {
                                                column = new Cicero.Service.Models.Core.Elements.Column() { };
                                                Created = true;
                                            }
                                            if (ElementId.FirstOrDefault() == column.ElementId && ActionFor.FirstOrDefault() == "column")
                                            {
                                                IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(column);
                                                Updated = column;
                                            }
                                            if (column.ElementId == ElementId)
                                            {
                                                column.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                                column.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                                column.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                            }
                                            column.ElementId = column_item.ElementId;
                                            column.ElementIndex = column_item.ElementIndex;
                                            column.Type = column.GetType().FullName;
                                            column.Element.Clear();
                                            foreach (var element_item in column_item.Element)
                                            {
                                                if (!string.IsNullOrEmpty(element_item.ElementId))
                                                {
                                                    //OldFbvm = OldFbvm1;
                                                    OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                                    //dynamic element = OldFbvm.Tab?.Where(t => t.ElementId == tab_item.ElementId)?.FirstOrDefault()?.Row?.Where(rx => rx.ElementId == row_item.ElementId)?.FirstOrDefault()?.Column?.Where(x2 => x2.ElementId == column_item.ElementId)?.FirstOrDefault()?.Element?.Where(x1 => x1.ElementId == element_item.ElementId)?.FirstOrDefault();
                                                    dynamic element = null;
                                                    dynamic elmCols = null;

                                                    bool found = false;
                                                    string value = "";
                                                    foreach (dynamic tabitem in OldFbvm.Tab)
                                                    {
                                                        if (found) break;
                                                        foreach (dynamic rowitem in tabitem.Row)
                                                        {
                                                            if (found) break;
                                                            foreach (dynamic colitem in rowitem.Column)
                                                            {
                                                                if (found) break;
                                                                foreach (var elmsitem in colitem.Element)
                                                                {
                                                                    if (elmsitem.ElementId == element_item.ElementId)
                                                                    {
                                                                        element = elmsitem;
                                                                        found = true;
                                                                        break;
                                                                    }
                                                                }

                                                            }

                                                        }
                                                    }
                                                    //OldFbvm.Tab?.Where(t => t.Row.ElementId == tab_item.ElementId)?.FirstOrDefault()?.Row?.Where(rx => rx.ElementId == row_item.ElementId)?.FirstOrDefault()?.Column?.Where(x2 => x2.ElementId == column_item.ElementId)?.FirstOrDefault()?.Element?.Where(x1 => x1.ElementId == element_item.ElementId)?.FirstOrDefault();

                                                    if (element == null && !string.IsNullOrEmpty(ElementType))
                                                    {
                                                        element = Activator.CreateInstance(Type.GetType("" + ElementType.FirstOrDefault() + ", Cicero.Service"));
                                                        Created = true;
                                                        element.TabIndex = data["TabIndex"].ToString();
                                                        element.RowIndex = data["RowIndex"].ToString();
                                                        element.ColumnIndex = data["ColumnIndex"].ToString();
                                                        Updated = element;

                                                    }

                                                    if (element != null && element.ElementId != null && ElementId.FirstOrDefault() == element.ElementId && ActionFor.FirstOrDefault() == "element")
                                                    {
                                                        IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(element);
                                                        var isit = element.GetType().GetProperty("Column");
                                                        if (isit != null)
                                                        {
                                                            if (element.Column != null && element.Column.Count > 0)
                                                            {
                                                                int j = 0;
                                                                found = false;
                                                                OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                                                foreach (dynamic tabitem in OldFbvm.Tab)
                                                                {
                                                                    if (found) break;
                                                                    foreach (dynamic rowitem in tabitem.Row)
                                                                    {
                                                                        if (found) break;
                                                                        foreach (dynamic colitem in rowitem.Column)
                                                                        {
                                                                            if (found) break;
                                                                            foreach (var elmsitem in colitem.Element)
                                                                            {
                                                                                if (elmsitem.ElementId == element_item.ElementId)
                                                                                {
                                                                                    elmCols = elmsitem;
                                                                                    found = true;
                                                                                    break;
                                                                                }
                                                                            }

                                                                        }

                                                                    }
                                                                }
                                                                if (elmCols != null && elmCols.Column !=null)
                                                                {
                                                                    if (element.Column != null && element.Column.Count > 0)
                                                                    {
                                                                        if (element.Column.Count < elmCols.Column.Count)
                                                                        {
                                                                            foreach (var col in element.Column)
                                                                            {
                                                                                element.Column[j].ColumnElement = elmCols.Column[j].ColumnElement;
                                                                                j++;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (var col in elmCols.Column)
                                                                            {
                                                                                element.Column[j].ColumnElement = elmCols.Column[j].ColumnElement;
                                                                                j++;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        Updated = element;
                                                    }
                                                    if (element != null)
                                                    {
                                                        element.ElementId = element_item.ElementId;
                                                        element.ElementIndex = element_item.ElementIndex;
                                                        element.Type = element?.GetType().FullName;
                                                        if (element.ElementId == ElementId)
                                                        {
                                                            element.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                                            element.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                                            element.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                                            element.VisibleinFooter = (VisibleinFooter.Count == 0 ? false : true);
                                                            var isit = element.GetType().GetProperty("automationEnable");
                                                            if (isit != null)
                                                            {
                                                                element.automationEnable = (automationEnable.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsCurrency");
                                                            if (isit != null)
                                                            {
                                                                element.IsCurrency = (IsCurrency.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsTelephoneNumber");
                                                            if (isit != null)
                                                            {
                                                                element.IsTelephoneNumber = (IsTelephoneNumber.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("UseFromDatabase");
                                                            if (isit != null)
                                                            {
                                                                element.UseFromDatabase = (UserFormDatabase.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("SelectFromDbIsToggleOption");
                                                            if (isit != null)
                                                            {
                                                                element.SelectFromDbIsToggleOption = (SelectFromDbIsToggleOption.Count == 0 ? false : true);
                                                            }

                                                            isit = element.GetType().GetProperty("FrontendLabelVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.FrontendLabelVisibility = (FrontendLabelVisibility.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("FrontendIconVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.FrontendIconVisibility = (FrontendIconVisibility.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("FrontendImageVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.FrontendImageVisibility = (FrontendImageVisibility.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("BackendLabelVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.BackendLabelVisibility = (BackendLabelVisibility.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("BackendIconVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.BackendIconVisibility = (BackendIconVisibility.Count == 0 ? false : true);
                                                            }

                                                            isit = element.GetType().GetProperty("BackendImageVisibility");
                                                            if (isit != null)
                                                            {
                                                                element.BackendImageVisibility = (BackendImageVisibility.Count == 0 ? false : true);
                                                            }

                                                            isit = element.GetType().GetProperty("IsOnChangeEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnChangeEvent = (IsOnChangeEvent.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsOnClickEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnClickEvent = (IsOnClickEvent.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsOnKeyUpEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnKeyUpEvent = (IsOnKeyUpEvent.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsOnLoadEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnLoadEvent = (IsOnLoadEvent.Count == 0 ? false : true);
                                                            }

                                                            isit = element.GetType().GetProperty("IsOnSaveFormEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnSaveFormEvent = (IsOnSaveFormEvent.Count == 0 ? false : true);
                                                            }

                                                            isit = element.GetType().GetProperty("IsOnSwitchTabEvent");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnSwitchTabEvent = (IsOnSwitchTabEvent.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsValidateOnTabSwitch");
                                                            if (isit != null)
                                                            {
                                                                element.IsValidateOnTabSwitch = (IsValidateOnTabSwitch.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("IsOnResponseTarget");
                                                            if (isit != null)
                                                            {
                                                                element.IsOnResponseTarget = (IsOnResponseTarget.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("EnablePopup");
                                                            if (isit != null)
                                                            {
                                                                element.EnablePopup = (EnablePopup.Count == 0 ? false : true);
                                                            }
                                                            isit = element.GetType().GetProperty("DisablePopupClose");
                                                            if (isit != null)
                                                            {
                                                                element.DisablePopupClose = (DisablePopupClose.Count == 0 ? false : true);
                                                            }
                                                        }
                                                        if (ParentElementId == element.ElementId)
                                                        {
                                                            Updated = element;
                                                            var isit = element.GetType().GetProperty("Column");
                                                            bool isSave = false;
                                                            if (isit != null)
                                                            {
                                                                if (element.Column != null)
                                                                {
                                                                    int colIndex = 0;
                                                                    foreach (var tblColumn in element.Column)
                                                                    {

                                                                        dynamic tblColElm = null;
                                                                        tblColElm = tblColumn.ColumnElement;
                                                                        if (tblColElm != null)
                                                                        {
                                                                            if (tblColElm.ElementId == ElementId)
                                                                            {
                                                                                IAsyncResult IsFormUpdated = this.TryUpdateModelAsync(tblColElm);

                                                                                tblColElm.BackendVisible = (BackendVisible.Count == 0 ? false : true);
                                                                                tblColElm.FrontendVisible = (FrontendVisible.Count == 0 ? false : true);
                                                                                tblColElm.VisibleinGrid = (VisibleinGrid.Count == 0 ? false : true);
                                                                                tblColElm.VisibleinFooter = (VisibleinFooter.Count == 0 ? false : true);
                                                                                isit = tblColElm.GetType().GetProperty("automationEnable");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.automationEnable = (automationEnable.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsCurrency");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsCurrency = (IsCurrency.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsTelephoneNumber");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsTelephoneNumber = (IsTelephoneNumber.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("UseFromDatabase");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.UseFromDatabase = (UserFormDatabase.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("SelectFromDbIsToggleOption");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.SelectFromDbIsToggleOption = (SelectFromDbIsToggleOption.Count == 0 ? false : true);
                                                                                }

                                                                                isit = tblColElm.GetType().GetProperty("FrontendLabelVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.FrontendLabelVisibility = (FrontendLabelVisibility.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("FrontendIconVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.FrontendIconVisibility = (FrontendIconVisibility.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("FrontendImageVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.FrontendImageVisibility = (FrontendImageVisibility.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("BackendLabelVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.BackendLabelVisibility = (BackendLabelVisibility.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("BackendIconVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.BackendIconVisibility = (BackendIconVisibility.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("BackendImageVisibility");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.BackendImageVisibility = (BackendImageVisibility.Count == 0 ? false : true);
                                                                                }

                                                                                isit = tblColElm.GetType().GetProperty("IsOnChangeEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnChangeEvent = (IsOnChangeEvent.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsOnClickEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnClickEvent = (IsOnClickEvent.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsOnKeyUpEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnKeyUpEvent = (IsOnKeyUpEvent.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsOnLoadEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnLoadEvent = (IsOnLoadEvent.Count == 0 ? false : true);
                                                                                }

                                                                                isit = tblColElm.GetType().GetProperty("IsOnSaveFormEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnSaveFormEvent = (IsOnSaveFormEvent.Count == 0 ? false : true);
                                                                                }

                                                                                isit = tblColElm.GetType().GetProperty("IsOnSwitchTabEvent");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnSwitchTabEvent = (IsOnSwitchTabEvent.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("IsOnResponseTarget");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.IsOnResponseTarget = (IsOnResponseTarget.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("EnablePopup");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.EnablePopup = (EnablePopup.Count == 0 ? false : true);
                                                                                }
                                                                                isit = tblColElm.GetType().GetProperty("DisablePopupClose");
                                                                                if (isit != null)
                                                                                {
                                                                                    tblColElm.DisablePopupClose = (DisablePopupClose.Count == 0 ? false : true);
                                                                                }
                                                                                Updated = tblColElm;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (newElm != null)
                                                                            {
                                                                                if (Convert.ToInt32(newElm.TableColumnIndex) == colIndex)
                                                                                {
                                                                                    tblColElm = newElm;
                                                                                    isSave = true;
                                                                                  
                                                                                }
                                                                            }

                                                                        }
                                                                        tblColumn.ColumnElement = tblColElm;
                                                                        colIndex++;
                                                                    }
                                                                }

                                                                if(!isSave)
                                                                {
                                                                    if (newElm != null)
                                                                    {

                                                                        Service.Models.Core.Elements.Table.TableColumn tblCol = new Cicero.Service.Models.Core.Elements.Table.TableColumn();
                                                                        tblCol.ColumnElement = newElm;
                                                                        if (element.Column == null)
                                                                        {
                                                                            element.Column = new List<Service.Models.Core.Elements.Table.TableColumn>();
                                                                            element.Column.Add(tblCol);
                                                                        }
                                                                        else
                                                                        {
                                                                            element.Column.Add(tblCol);
                                                                        }

                                                                    }
                                                                }
                                                                
                                                            }
                                                        }


                                                        column.Element.Add(element);
                                                    }
                                                }
                                            }

                                            row.Column.Add(column);
                                        }

                                    }

                                    tab.Row.Add(row);
                                }

                            }

                            fbvm.Tab.Add(tab);
                        }

                    }


                    if (fbvm.Tab.Count > 0)
                    {
                        string jsonsstr = JsonConvert.SerializeObject(fbvm, settings).Trim();

                        ccvm.Fields = jsonsstr;
                        db.CaseForm.Update(ccvm);
                        db.SaveChanges();
                        //Updated = true;
                    }
                    //return this.RenderFormBuilderElements(data);



                    //For Table Creation and Core Form Builder
                    if (data["Type"] == "Form")
                    {
                        //check here Old Form and new form changes 
                        //OldFbvm;
                        _CoreFormService.CreateDymanicCoreForm(fbvm, ccvm.Id);
                    }

                }
                if (Updated == null)
                {
                    if (IsDelete == "true")
                    {
                        response = new { Content = str, Status = "success", Message = "Changes are saved successfully.", ElementId = ElementId.FirstOrDefault(), FormId = _utils.EncryptId(valFormId).ToString(), ActionFor = ActionFor.FirstOrDefault() };
                    }
                    else
                    {
                        if (Created || ActionFor.FirstOrDefault() == "autosave")
                        {
                            response = new { Content = str, Status = "success", Message = "Changes are saved successfully.", ElementId = ElementId.FirstOrDefault(), FormId = _utils.EncryptId(valFormId).ToString(), ActionFor = ActionFor.FirstOrDefault() };
                        }
                        else
                        {
                            response = new { Content = str, Status = "error", Message = "Please try again, something went wrong.", ElementId = ElementId.FirstOrDefault(), FormId = _utils.EncryptId(valFormId).ToString(), ActionFor = ActionFor.FirstOrDefault() };
                        }
                    }
                }
                else
                {
                    ElementResponse setting = new ElementResponse() { Content = "" };
                    ElementResponse er = new ElementResponse() { Content = "" };
                    ElementResponse er1 = new ElementResponse() { Content = "" };
                    if (ActionFor.FirstOrDefault() == "element")
                    {
                        PropertyInfo pef = Updated.GetType().GetProperty("HttpContext");
                        if (pef != null)
                        {
                            pef.SetValue(Updated, this.HttpContext);
                        }
                        Updated.Data = FormId;
                        Updated.Theme = this.Theme as Theme;
                        var ccvm = db.CaseForm.Where(x => x.Id == Convert.ToInt32(FormId.FirstOrDefault())).FirstOrDefault();
                        settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                        FormBuilderViewModel OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                        Updated.ModelData = OldFbvm;
                        er = Updated.GetType().GetMethod("GetPreviewTemplate").Invoke(Updated, new object[] { Updated, null });
                        setting = Updated.GetType().GetMethod("RenderSetting").Invoke(Updated, new object[] { Updated, null });
                        er1 = Updated.GetType().GetMethod("GetSettingTemplate").Invoke(Updated, new object[] { Updated, null });
                    }
                    response = new { Content = er.Content, Setting = setting.Content, Status = "success", ContentSetting = er1.Content, Message = "Changes are saved successfully.", ElementId = Updated.ElementId, FormId = _utils.EncryptId(valFormId).ToString(), ActionFor = ActionFor.FirstOrDefault() };
                }
            }
            catch (Exception ex)
            {

            }

            return Json(response);


        }

        [HttpPost]
        [Route("admin/ajax/check-form-table.html")]
        public JsonResult CHeckFormTableAlter(IFormCollection data)
        {
            List<JObject> lsttableRowValues = new List<JObject>();

            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            data.TryGetValue("Type", out StringValues ElementType);
            data.TryGetValue("ElementId", out StringValues ElementId);
            data.TryGetValue("ActionFor", out StringValues ActionFor);
            data.TryGetValue("Action", out StringValues Action);
            data.TryGetValue("FormId", out StringValues FormId);


            var ccvm = db.CaseForm.Where(x => x.Id == Convert.ToInt32(FormId.FirstOrDefault())).FirstOrDefault();
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            FormBuilderViewModel OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            FormBuilderViewModel NewOrders = new FormBuilderViewModel();
            this.TryUpdateModelAsync(NewOrders);
            FormBuilderViewModel fbvm = new FormBuilderViewModel();

            if (ElementType.FirstOrDefault() == "Form" && OldFbvm.Forms.Tables != null)
            {
                bool isDataChange = false;
                fbvm.Forms = NewOrders.Forms;

                foreach (var table in OldFbvm.Forms.Tables)
                {
                    JObject tableRowValues = new JObject();
                    tableRowValues.Add("Table", table.Name);
                    Table a = fbvm.Forms.Tables.Where(x => x.Name == table.Name).FirstOrDefault();
                    string strArr = "";
                    if (a != null)
                    {
                        if (table.Fields != null && table.Fields.Count > 0)
                        {
                            foreach (var fields in table.Fields)
                            {
                                if (Utils.ConvertToString(fields.Name) != "")
                                {
                                    if (!a.Fields.Exists(x => x.Name == fields.Name))
                                    {
                                        strArr = strArr + fields.Name + ", ";
                                        isDataChange = true;
                                    }
                                }

                            }
                        }
                        tableRowValues.Add("Fields", strArr);
                    }
                    if (isDataChange)
                        lsttableRowValues.Add(tableRowValues);
                }

            }
            string result = JsonConvert.SerializeObject(lsttableRowValues, Formatting.None);
            return Json(result);
        }
        [HttpPost]
        [Route("admin/ajax/render-form-builder-elements.html")]
        public JsonResult RenderFormBuilderElements(IFormCollection data)
        {

            string str = string.Empty;
            Theme _theme = this.Theme as Theme;
            ViewData["theme"] = _theme;
            object response = new { };
            data.TryGetValue("Type", out StringValues ElementType);
            data.TryGetValue("ElementId", out StringValues ElementId);
            data.TryGetValue("ActionFor", out StringValues ActionFor);
            data.TryGetValue("Action", out StringValues Action);
            data.TryGetValue("FormId", out StringValues FormId);
            data.TryGetValue("ParentElementId", out StringValues ParentElementId);
            int valFormId = Convert.ToInt32(FormId.FirstOrDefault());
            try
            {
                FormBuilderViewModel OldFbvm = new FormBuilderViewModel();
                CaseForm ccvm = new CaseForm();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                if (valFormId == 0)
                {
                    CaseForm Newccvm = new CaseForm
                    {
                        Id = valFormId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Name = "Untitled"
                    };
                    FormBuilderViewModel NewOrders1 = new FormBuilderViewModel();
                    this.TryUpdateModelAsync(NewOrders1);
                    FormBuilderViewModel fbvm1 = new FormBuilderViewModel();
                    Form frm = new Form();
                    Navigation nv = new Navigation();
                    frm.Navigations = nv;
                    frm.ElementId = Utils.GenerateId();
                    fbvm1.Forms = frm;
                    string jsonsstr = JsonConvert.SerializeObject(fbvm1, settings).Trim();

                    Newccvm.Fields = jsonsstr;
                    db.CaseForm.Add(Newccvm);
                    db.SaveChanges();
                    ccvm = Newccvm;
                    OldFbvm = fbvm1;
                }
                else
                {
                    CaseForm Newccvm1 = db.CaseForm.Where(x => x.Id == Convert.ToInt32(FormId.FirstOrDefault())).FirstOrDefault();
                    OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(Newccvm1.Fields, settings);
                    ccvm = Newccvm1;
                }

                //var AppSetting=this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting

                dynamic Updated = null;

                if (ElementType.FirstOrDefault() == "Form")
                {
                    Updated = OldFbvm.Forms;
                    ElementResponse ers = new ElementResponse() { Content = "" };
                    ers = OldFbvm.RenderSetting(OldFbvm, "Form", this.HttpContext);
                    //ers = OldFbvm.GetType().GetMethod("GetSettingTemplate").Invoke(OldFbvm, new object[] { OldFbvm, null });
                    return Json(new { Content = ers.Content, Status = "success", Message = "Successfully saved.", FormId = ccvm.Id, ElementId = ElementId.FirstOrDefault(), ActionFor = ActionFor.FirstOrDefault() });
                }
                foreach (Cicero.Service.Models.Core.Elements.Tab tab_item in OldFbvm.Tab)
                {
                    if (!string.IsNullOrEmpty(tab_item.ElementId))
                    {
                        OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                        Cicero.Service.Models.Core.Elements.Tab tab = OldFbvm.Tab.Where(x => x.ElementId == tab_item.ElementId)?.FirstOrDefault();
                        if (tab == null)
                        {
                            tab = new Cicero.Service.Models.Core.Elements.Tab() { };
                        }
                        if (ElementId.FirstOrDefault() == tab.ElementId)
                        {
                            Updated = tab;
                        }
                        else
                        {
                            foreach (var row_item in tab_item.Row)
                            {
                                if (!string.IsNullOrEmpty(row_item.ElementId))
                                {
                                    OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                    Cicero.Service.Models.Core.Elements.Row row = tab_item.Row.Where(x => x.ElementId == row_item.ElementId)?.FirstOrDefault();
                                    if (row == null)
                                    {
                                        row = new Cicero.Service.Models.Core.Elements.Row() { };
                                    }
                                    if (ElementId.FirstOrDefault() == row.ElementId)
                                    {
                                        Updated = row;
                                    }
                                    else
                                    {
                                        foreach (var column_item in row_item.Column)
                                        {
                                            if (!string.IsNullOrEmpty(column_item.ElementId))
                                            {
                                                OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                                Cicero.Service.Models.Core.Elements.Column column = row_item.Column.Where(x => x.ElementId == column_item.ElementId)?.FirstOrDefault();
                                                if (ElementId.FirstOrDefault() == column.ElementId)
                                                {
                                                    Updated = column;
                                                }
                                                else
                                                {
                                                    foreach (var element_item in column_item.Element)
                                                    {
                                                        if (!string.IsNullOrEmpty(element_item.ElementId))
                                                        {
                                                            OldFbvm = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                                                            dynamic element = column_item.Element.Where(x1 => x1.ElementId == element_item.ElementId)?.FirstOrDefault();
                                                            if (ElementId.FirstOrDefault() == element.ElementId)
                                                            {
                                                                Updated = element;
                                                            }
                                                            if (ParentElementId.Count > 0)
                                                            {
                                                                if (ParentElementId.FirstOrDefault() == element.ElementId)
                                                                {
                                                                    Service.Models.Core.Elements.Table elm = (Service.Models.Core.Elements.Table)element;
                                                                    var ColumnElement = elm.Column.Where(x => x.ColumnElement.ElementId == ElementId.FirstOrDefault()).FirstOrDefault().ColumnElement;

                                                                    //Updated = Convert.ChangeType(ColumnElement, Type.GetType("" + ColumnElement.Type + ", Cicero.Service"));
                                                                    Updated = ColumnElement;
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

                ElementResponse er = new ElementResponse() { Content = "" };
                if (Updated != null)
                {
                    Updated.ModelData = OldFbvm;
                    PropertyInfo pef = Updated.GetType().GetProperty("HttpContext");
                    if (pef != null)
                    {
                        pef.SetValue(Updated, this.HttpContext);
                    }
                    Updated.Data = FormId;
                    Updated.Theme = this.Theme as Theme;
                    er = Updated.GetType().GetMethod("GetSettingTemplate").Invoke(Updated, new object[] { Updated, null });
                    response = new { Content = er.Content, Status = "success", Message = "Successfully saved.", FormId = _utils.EncryptId(valFormId).ToString(), ElementId = ElementId.FirstOrDefault(), ActionFor = ActionFor.FirstOrDefault() };
                }
                else if (data["Type"] == "Tab")
                {

                    Updated = new Cicero.Service.Models.Core.Elements.Tab();
                    Updated.ModelData = OldFbvm;
                    PropertyInfo pef = Updated.GetType().GetProperty("HttpContext");
                    if (pef != null)
                    {
                        pef.SetValue(Updated, this.HttpContext);
                    }
                    er = Updated.GetType().GetMethod("GetSettingTemplate").Invoke(Updated, new object[] { Updated, null });
                    response = new { Content = er.Content, Status = "success", Message = "Successfully saved.", FormId = _utils.EncryptId(valFormId).ToString(), ElementId = ElementId.FirstOrDefault(), ActionFor = ActionFor.FirstOrDefault() };
                }

            }
            catch (Exception ex)
            {

            }

            return Json(response);


        }

    }

}
