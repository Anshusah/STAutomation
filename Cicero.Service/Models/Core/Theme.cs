using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Service.Services.Core.Themes.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cicero.Service.Models.Core
{
    public class Theme
    {

        public List<Path> Css = new List<Path>();
        public List<Path> Js = new List<Path>();
        public List<object> Widgets = new List<object>();
        public List<object> Components = new List<object>();
        public List<object> Elements = new List<object>();
        public List<WidgetLocation> WidgetLocations = new List<WidgetLocation>();
        public ArrayList Navigation = new ArrayList();
        public HttpContext HttpContext = null;
        protected List<Action> Actions = new List<Action>();
        protected List<Filter> Filters = new List<Filter>();
        protected List<ShortCode> ShortCodes = new List<ShortCode>();

        protected object This = null;
        protected AppSetting appSetting = null;
        protected ComponentService compSetting = null;
        protected IArticleService articleService = null;
        protected IViewRenderService viewRenderService;
        protected Utils utils = null;
        public Theme()
        {
            ShortCodes.Add(new ShortCode() { Code = "widget", Method = "WidgetShortCodeRender", Order = 1 });
        }
        public void RegisterCss(string id, string url, string integrity = null, string crossorigin = null)
        {
            this.Css.Add(new Path() { Id = id, Url = url, Integrity = integrity, CrossOrigin = crossorigin });
        }
        public void RegisterJs(string id, string url, string integrity = null, string crossorigin = null)
        {
            this.Js.Add(new Path() { Id = id, Url = url, Integrity = integrity, CrossOrigin = crossorigin });
        }
        public void RegisterNav(string navs = null)
        {
            this.Navigation.Add(navs);
        }
        public void RegisterWidget(object _block)
        {
            this.Widgets.Add(_block);
        }
        public void RegisterComponent(object _block)
        {
            this.Components.Add(_block);
        }
        public void RegisterElement(object _block)
        {
            this.Elements.Add(_block);
        }
        public void RegisterWidgetLocation(WidgetLocation e)
        {
            this.WidgetLocations.Add(e);
        }
        public string GetCssUrl()
        {
            string cu = "";
            foreach (var item in this.Css)
            {
                cu = cu + "<link href='" + item.Url + "' rel='stylesheet'>";
            }
            return cu;
        }
        public string GetJsUrl()
        {
            string ju = "";
            foreach (var item in this.Js)
            {
                if (!string.IsNullOrWhiteSpace(item.CrossOrigin) && !string.IsNullOrWhiteSpace(item.Integrity))
                {
                    ju = ju + "<script src='" + item.Url + "' integrity='" + item.Integrity + "' crossorigin = '" + item.CrossOrigin + "'></script>";
                }
                else
                {
                    ju = ju + "<script src='" + item.Url + "'></script>";
                }

            }
            return ju;
        }
        public string Name
        {
            get;
            set;
        }
        public string Version
        {
            get;
            set;
        }
        public string ControllerName
        {
            get;
            set;
        }
        public string ControllerAction
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Screenshot
        {
            get;
            set;
        }
        public string ShortDescription
        {
            get;
            set;
        }
        
        public void AddAction(string action, string method, int order = 0)
        {
            this.Actions.Add(new Action { Name = action, Method = method, Order = order });

        }
        public string DoAction(string action = "", object[] arg = null)
        {
            string data = "";
            foreach (Action item in this.Actions)
            {
                if (item.Name == action)
                {

                    Type module = this.This.GetType();
                    MethodInfo mi = module.GetMethod(item.Method);
                  
                    data = (string)mi.Invoke(this.This, arg);

                }
            }
            return data;
        }
        public void AddFilter(string action, string method, int order = 0)
        {
            this.Filters.Add(new Filter { Name = action, Method = method, Order = order });
        }
        public List<Menu> GetThemeNavigations(ICommonService commonService)
        {
            utils = this.HttpContext.RequestServices.GetService(typeof(Utils)) as Utils;
            List<Menu> MenuList = new List<Menu>();
            try
            {
                string[] icons = { "fa-inbox", "fa-road", "fa-quote-left", "fa-sliders", "fa-tty", "fa-clone", "fa-calendar-times-o" };
                List<CaseFormViewModel> list = commonService.GetCaseFormListForActiveTenantId();
                foreach (CaseFormViewModel item in list)
                {
                    if (!string.IsNullOrWhiteSpace(item.UrlIdentifier))
                    {
                        var stricon = item.Icon;
                        if (stricon == null || stricon == "")
                        {
                            stricon = icons[new Random().Next(0, icons.Length)];
                        }
                        //identifier and name should be in table currently is in json we need to change it
                        MenuList.Add(new Menu(
                            item.UrlIdentifier,
                            item.ModelName,
                            "~/admin/form/" + utils.GetTenantForUrl(true) + item.UrlIdentifier + ".html",
                            "0",
                            //stricon + " fa-fw mr-3",
                            stricon + "",
                            0
                        ));

                        foreach (var qitem in commonService.GetQueueListByFormId(item.Id))
                        {
                            MenuList.Add(new Menu(
                                qitem.Key,
                                qitem.Value,
                                "~/admin/form/" + utils.GetTenantForUrl(true) + item.UrlIdentifier + "/" + qitem.Key + ".html",
                                item.UrlIdentifier,
                                "fas fa-bezier-curve",
                                0
                            ));
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }


            return MenuList;
        }
        public List<object> GetElements()
        {
            List<object> _components = new List<object>();
            string theme = "app_elements";
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

            var all_components = appSetting.Get(theme, "[]");
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {

                    dynamic RealModel = Activator.CreateInstance(Type.GetType(_Co.ComponentType));

                    string dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as string;
                    if (!string.IsNullOrEmpty(dt))
                    {
                        dt = _Co.GetJson();
                    }

                    Type mt = RealModel.GetType();
                    var evs = JsonConvert.DeserializeObject(dt, mt);
                    Type tp = evs.GetType();
                    tp?.GetProperty("Data")?.SetValue(evs, dt);
                    if (evs != null)
                    {
                        tp.GetProperty("appSetting").SetValue(evs, appSetting);
                        tp.GetProperty("HttpContext").SetValue(evs, this.HttpContext);
                        tp.GetProperty("Theme").SetValue(evs, this);
                        tp.GetProperty("viewRenderService").SetValue(evs, viewRenderService);
                        _NewComponent.Add(evs);
                    }

                }
                return _NewComponent;

            }

            return _components;
        }
        public dynamic GetElement(string element_id = "")
        {
            Element _components = new Element();
            string theme = "app_elements";
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

            var all_components = appSetting.Get(theme, "[]");
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Element> _Component = JsonConvert.DeserializeObject<List<Element>>(all_components);
                foreach (var __item in _Component)
                {
                    if (__item.Id == element_id)
                    {
                        dynamic RealModel = Activator.CreateInstance(Type.GetType(__item.Type));

                        string dt = __item.GetType().GetProperty("Data").GetValue(__item) as string;
                        if (dt == null)
                        {
                            dt = __item.GetJson();
                        }

                        Type mt = RealModel.GetType();
                        var evs = JsonConvert.DeserializeObject(dt, mt);
                        //var evsjob = JsonConvert.DeserializeObject<JObject>(SeT);
                        evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                        return evs;
                    }
                }

            }

            return _components;
        }

        public List<object> GetComponentsByType(string component_type = null)
        {
            List<object> _components = new List<object>();
            string theme = "app_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            var all_components = compSetting.Get(theme);
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {
                    if (_Co.ComponentType == component_type)
                    {
                        string type = _Co.ComponentType + ",Cicero";
                        dynamic RealModel = Activator.CreateInstance(Type.GetType(type));

                        JObject dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as JObject;
                        if (dt == null)
                        {
                            dt = JObject.Parse(_Co.GetJson());
                        }
                        string SeT = JsonConvert.SerializeObject(dt);


                        Type mt = RealModel.GetType();
                        dynamic evs = JsonConvert.DeserializeObject(SeT, mt);

                        evs.Data = dt;
                        evs.HttpContext = this.HttpContext;
                        evs.Theme = this;
                        if (evs != null)
                        {
                            _NewComponent.Add(evs);
                        }
                    }

                }
                return _NewComponent;

            }
            return _components;
        }
        public List<object> GetComponents()
        {
            List<object> _components = new List<object>();
            string theme = "app_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            var all_components = compSetting.Get(theme);
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {

                    string type = _Co.ComponentType + ",Cicero";
                    dynamic RealModel = Activator.CreateInstance(Type.GetType(type));
                    //  dynamic RealModel = Activator.CreateInstance(_Co.ComponentType.Split('.')[3]);
                    JObject dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as JObject;
                    if (dt == null)
                    {
                        dt = JObject.Parse(_Co.GetJson());
                    }
                    string SeT = JsonConvert.SerializeObject(dt);


                    Type mt = RealModel.GetType();
                    var evs = JsonConvert.DeserializeObject(SeT, mt);
                    evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                    if (evs != null)
                    {
                        _NewComponent.Add(evs);
                    }

                }
                return _NewComponent;

            }

            return _components;
        }
        
        public List<object> GetComponentsByFormId(string FormId)
        {
            List<object> _components = new List<object>();
            string theme = "app_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            var all_components = compSetting.Get(theme);
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                _Component = _Component.Where(x => x.FormId == FormId).ToList();
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {

                    string type = _Co.ComponentType + ",Cicero";
                    dynamic RealModel = Activator.CreateInstance(Type.GetType(type));
                    //  dynamic RealModel = Activator.CreateInstance(_Co.ComponentType.Split('.')[3]);
                    JObject dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as JObject;
                    if (dt == null)
                    {
                        dt = JObject.Parse(_Co.GetJson());
                    }
                    string SeT = JsonConvert.SerializeObject(dt);


                    Type mt = RealModel.GetType();
                    var evs = JsonConvert.DeserializeObject(SeT, mt);
                    evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                    if (evs != null)
                    {
                        _NewComponent.Add(evs);
                    }

                }
                return _NewComponent;

            }

            return _components;
        }
        public dynamic GetComponent(string component_id = "")
        {
            Component _components = new Component();
            string theme = "app_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;
            var all_components = compSetting.Get(theme, "[]");
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                foreach (var __item in _Component)
                {
                    if (__item.ComponentId == component_id)
                    {
                        string type = __item.ComponentType + ",Cicero";
                        dynamic RealModel = Activator.CreateInstance(Type.GetType(type));

                        JObject dt = __item.GetType().GetProperty("Data").GetValue(__item) as JObject;
                        if (dt == null)
                        {
                            dt = JObject.Parse(__item.GetJson());
                        }
                        string SeT = JsonConvert.SerializeObject(dt);


                        Type mt = RealModel.GetType();
                        var evs = JsonConvert.DeserializeObject(SeT, mt);
                        //var evsjob = JsonConvert.DeserializeObject<JObject>(SeT);
                        evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                        return evs;
                    }
                }

            }

            return _components;
        }

        #region Element Componet Func
        public List<object> GetElementComponents(int formId, string elementId, int eventType)
        {
            List<object> _components = new List<object>();
            string theme = "elm_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            var all_components = compSetting.Get(theme, formId, elementId, eventType);
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {

                    string type = _Co.ComponentType + ",Cicero";
                    dynamic RealModel = Activator.CreateInstance(Type.GetType(type));
                    //  dynamic RealModel = Activator.CreateInstance(_Co.ComponentType.Split('.')[3]);
                    JObject dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as JObject;
                    if (dt == null)
                    {
                        dt = JObject.Parse(_Co.GetJson());
                    }
                    string SeT = JsonConvert.SerializeObject(dt);


                    Type mt = RealModel.GetType();
                    var evs = JsonConvert.DeserializeObject(SeT, mt);
                    evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                    if (evs != null)
                    {
                        _NewComponent.Add(evs);
                    }



                }
                return _NewComponent;

            }

            return _components;
        }
        public dynamic GetElementComponentById(string componentId, int formId, string elementId, int eventType)
        {
            Component _components = new Component();
            string theme = "elm_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;
            var all_components = compSetting.Get(theme, formId, elementId, eventType, "[]");
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                foreach (var __item in _Component)
                {
                    if (__item.ComponentId == componentId)
                    {
                        string type = __item.ComponentType + ",Cicero";
                        dynamic RealModel = Activator.CreateInstance(Type.GetType(type));

                        JObject dt = __item.GetType().GetProperty("Data").GetValue(__item) as JObject;
                        if (dt == null)
                        {
                            dt = JObject.Parse(__item.GetJson());
                        }
                        string SeT = JsonConvert.SerializeObject(dt);


                        Type mt = RealModel.GetType();
                        var evs = JsonConvert.DeserializeObject(SeT, mt);
                        //var evsjob = JsonConvert.DeserializeObject<JObject>(SeT);
                        evs?.GetType()?.GetProperty("Data")?.SetValue(evs, dt);
                        return evs;
                    }
                }

            }
            return _components;

        }
        public List<object> GetElementComponentsByType(int formId, string elementId, int eventType, string component_type = null)
        {
            List<object> _components = new List<object>();
            string theme = "elm_components";
            if (compSetting == null) compSetting = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            var all_components = compSetting.Get(theme, formId, elementId, eventType);
            if (!string.IsNullOrEmpty(all_components))
            {
                List<Component> _Component = JsonConvert.DeserializeObject<List<Component>>(all_components);
                List<object> _NewComponent = new List<object>();
                foreach (var _Co in _Component)
                {
                    if (_Co.ComponentType == component_type)
                    {
                        string type = _Co.ComponentType + ",Cicero";
                        dynamic RealModel = Activator.CreateInstance(Type.GetType(type));

                        JObject dt = _Co.GetType().GetProperty("Data").GetValue(_Co) as JObject;
                        if (dt == null)
                        {
                            dt = JObject.Parse(_Co.GetJson());
                        }
                        string SeT = JsonConvert.SerializeObject(dt);


                        Type mt = RealModel.GetType();
                        dynamic evs = JsonConvert.DeserializeObject(SeT, mt);

                        evs.Data = dt;
                        evs.HttpContext = this.HttpContext;
                        evs.Theme = this;
                        if (evs != null)
                        {
                            _NewComponent.Add(evs);
                        }
                    }

                }
                return _NewComponent;

            }
            return _components;
        }
        #endregion

        public Widgets GetWidget(string widget_location, string widget_id)
        {
            Widgets w = null;
            if (widget_location != null)
            {
                List<Widgets> _widgets = GetWidgets(widget_location);
                Widgets wg = null;
                if (_widgets != null)
                {
                    foreach (var ___item in _widgets)
                    {
                        if (___item.WidgetId == widget_id)
                        {
                            return ___item as Widgets;
                        }
                    }
                }
                return wg;
            }
            else
            {
                List<Widgets> _widgets = new List<Widgets>();
                string theme = "app_theme_widgets_" + this.This.GetType().Namespace.ToLower().Replace("themes.", "");
                if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;

                var all_widgets = appSetting.Get(theme);
                if (!string.IsNullOrEmpty(all_widgets))
                {
                    List<Widget> _Widget = JsonConvert.DeserializeObject<List<Widget>>(all_widgets);
                    foreach (var __item in _Widget)
                    {
                        foreach (var _____item in __item.Widgets)
                        {
                            if (_____item.WidgetId == widget_id)
                            {
                                w = _____item;
                            }
                        }
                    }
                }

            }
            return w;
        }

        public string GetName(bool ToLower = true)
        {
            if (ToLower)
            {
                return this.This.GetType().Namespace.ToLower().Replace("themes.", "");
            }
            else
            {
                return this.This.GetType().Namespace.Replace("Themes.", "");
            }
        }
        public List<Widgets> GetWidgets(string widget_location)
        {
            List<Widgets> _widgets = new List<Widgets>();
            string theme = "app_theme_widgets_" + this.This.GetType().Namespace.ToLower().Replace("themes.", "");
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;

            var all_widgets = appSetting.Get(theme);
            if (!string.IsNullOrEmpty(all_widgets))
            {
                List<Widget> _Widget = JsonConvert.DeserializeObject<List<Widget>>(all_widgets);
                foreach (var __item in _Widget)
                {
                    if (__item.Location == widget_location)
                    {
                        return __item.Widgets as List<Widgets>;
                    }
                }
            }

            return _widgets;
        }
       

        public string GetWidgetsAsHtml(string widget_location)
        {
            string data = string.Empty;
            List<Cicero.Service.Models.Core.Widgets> t = GetWidgets(widget_location) as List<Cicero.Service.Models.Core.Widgets>;
            foreach (var ___item in t)
            {
                Type _type = Type.GetType(___item.WidgetType);
                var o = Activator.CreateInstance(_type);

                MethodInfo mi = _type.GetMethod("RenderWidgets");
                foreach (var _jval in ___item.Data as JObject)
                {
                    var _name = _jval.Key.ToString();
                    PropertyInfo pef = o.GetType().GetProperty(_jval.Key.ToString());
                    try
                    {
                        if (pef != null) pef.SetValue(o, _jval.Value.ToString());
                    }
                    catch (Exception ex)
                    {


                    }
                }

                PropertyInfo pefh = o.GetType().GetProperty("HttpContext");
                if (pefh != null) pefh.SetValue(o, this.HttpContext);


                pefh = o.GetType().GetProperty("Theme");
                if (pefh != null) pefh.SetValue(o, this.This);
                pefh = o.GetType().GetProperty("WidgetId");
                if (pefh != null) pefh.SetValue(o, ___item.WidgetId);
                pefh = o.GetType().GetProperty("WidgetLocation");
                if (pefh != null) pefh.SetValue(o, ___item.WidgetLocation);
                pefh = o.GetType().GetProperty("Data");
                if (pefh != null) pefh.SetValue(o, ___item.Data);
                var exes = "";
                if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

                var rendered = viewRenderService.RenderToStringAsync(appSetting.Get("app_theme") + "/Widgets/" + _type.Name + "/View", o);
                try
                {
                    exes = rendered.Result;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                data = data + exes;
            }

            return data;
        }
        public string GetFileTreeAsHtml(string root = "./wwwroot/workspace/themes/", string str_collection = "")
        {
            string[] dir_list = Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);
            string[] file_list = Directory.GetFiles(root, "*", SearchOption.TopDirectoryOnly);
            string[] dirfile = dir_list.Concat(file_list).ToArray();
            string str = str_collection;
            if (dirfile.Count() > 0)
            {
                str += "<ul class=\"jqueryFileTree\" style=\"display: none;\">";
                int dirindex = 0;
                int fileindex = 0;
                foreach (string file in dirfile)
                {
                    if (Directory.Exists(file))
                    {
                        dirindex++;
                        str += "<li class=\"directory collapsed\">" +
                        "<a href=\"javascript:void(0)\" date-file-id='" + GetFileId(fileindex) + "' rel='" + file + "'>" +
                            new DirectoryInfo(file).Name +
                        "</a>" +
                        //GetFileTreeAsHtml(file, str) +
                        "</li>";
                    }
                    if (File.Exists(file))
                    {
                        fileindex++;
                        var fi = new FileInfo(file);
                        if (fi.Name != ".DS_Store")
                        {
                            string exc = fi.Extension.Replace(".", "");
                            str += "<li class=\"file collapsed ext_" + exc + "\">" +
                                         "<a href=\"javascript:void(0)\" date-file-id='" + GetFileId(fileindex) + "' rel='" + file + "'>" +
                                           fi.Name +
                                       "</a>" +
                                   "</li>";
                        }


                    }
                }
                str += "</ul>";
            }
            return str;
            /*
             $_POST['dir'] =urldecode($_POST['dir']);
            $root='';
            if( file_exists($root . $_POST['dir']) ) {
                $files = scandir($root . $_POST['dir']);
                natcasesort($files);
                $dirindex=0;
                $fileindex=0;
                if( count($files) > 2 ) { // The 2 accounts for . and .. 
                        echo "<ul class=\"jqueryFileTree\" style=\"display: none;\">";
                        // All dirs
                        foreach ( $files as $file ) {
                        $dirindex++;
                            if (file_exists($root. $_POST['dir']. $file) && $file != '.' && $file != '..' && is_dir($root. $_POST['dir']. $file) ) {
                                echo "<li class=\"directory collapsed\"><a href=\"#\" date-file-id='".ids($dirindex)."' rel=\"".htmlentities($_POST['dir']. $file). "/\">".htmlentities($file). "</a></li>";
                            }
                        }
                        // All files
                        foreach ( $files as $file ) {
                        $fileindex++;
                            if (file_exists($root. $_POST['dir']. $file) && $file != '.' && $file != '..' && !is_dir($root. $_POST['dir']. $file) ) {
                            $ext = preg_replace('/^.*\./', '', $file);
                                echo "<li class=\"file ext_$ext\"><a href=\"#\" date-file-id='".ids($fileindex)."' rel=\"".htmlentities($_POST['dir']. $file). "\">".htmlentities($file). "</a></li>";
                            }
                        }
                        echo "</ul>";
                    }
                }




             */

        }
        public string GetFileId(int index = 0)
        {
            string date = DateTime.Now.ToString("yyyyMMddhhmmss");
            //st e=date.ToString("yyMMdd ");
            string stamp = date;
            string ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            string orderid = stamp + "-" + ip;
            orderid = orderid.Replace(".", ""); //str_replace(".", "", "$orderid");
            orderid = orderid.Replace(":", "");
            orderid = orderid.Replace("-", "");// str_replace(":", "", "$orderid");
            return orderid + "" + index.ToString();
        }
        public string GetNav(string nav_location)
        {
            //this.Httpcontext.RequestServices.GetService(typeof(AppSetting));
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            if (articleService == null) articleService = this.HttpContext.RequestServices.GetService(typeof(IArticleService)) as IArticleService;
            var navs = appSetting.getMenuByLocation(nav_location);
            int index = 0;
            string navstr = "";
            foreach (var _menu in navs)
            {
                index++;
                string _url = _menu.url;
                var _target = string.Empty;
                string _slug = _url;
                if (_menu.type == "article")
                {
                    _slug = articleService.GetArticleById(Convert.ToInt32(_menu.url)).Slug;
                    string cc = this.HttpContext.Request.Scheme + "://" + this.HttpContext.Request.Host.Value;
                    _url = cc + "/" + _slug + ".html";
                }
                if (_menu.target == "on")
                {
                    _target = " target='_blank' ";
                }
                string _li_class = "nav-item";
                string _li_a_class = "nav-link";
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("Location", nav_location);
                args.Add("Index", index.ToString());
                args.Add("Target", _target);
                args.Add("Slug", _slug);
                args.Add("Li_Class", _li_class);
                args.Add("Li_A_Class", _li_a_class);

                Dictionary<string, string> _args = this.ApplyFilter("nav_li_args", args) as Dictionary<string, string>;
                foreach (KeyValuePair<string, string> kvp in _args)
                {
                    if (kvp.Key == "Slug") _slug = kvp.Value;
                    if (kvp.Key == "Target") _target = kvp.Value;
                    if (kvp.Key == "Li_Class") _li_class = kvp.Value;
                    if (kvp.Key == "Li_A_Class") _li_a_class = kvp.Value;
                }

                navstr = navstr + "<li class=" + _li_class + "><a class=" + _li_a_class + " href=" + _url + " " + _target + ">" + _menu.menu + "</a></li>";
            }
            return navstr;
        }
        public object ApplyFilter(string action, dynamic ob = null)
        {
            var data = ob;
            foreach (Filter item in this.Filters)
            {
                if (item.Name == action)
                {

                    Type module = this.This.GetType();
                    MethodInfo mi = module.GetMethod(item.Method);
                    data = mi.Invoke(this.This, new object[] { ob });

                }
            }
            return data;
        }
        public void AddShortCode(string code, string method, int order = 0)
        {
            this.ShortCodes.Add(new ShortCode { Code = code, Method = method, Order = order });
        }
        public string DoShortCode(string str)
        {
            Regex regex = new Regex(@"\[(.*?)\]+");
            return regex.Replace(str, this.ShortCodeParse);
        }
        public string WidgetShortCodeRender(Dictionary<string, string> arg2)
        {
            string WidgetId = null;
            string data = null;
            arg2.TryGetValue("id", out WidgetId);
            if (!string.IsNullOrEmpty(WidgetId))
            {
                Widgets ___item = this.GetWidget(null, WidgetId);
                if (___item != null)
                {
                    Type _type = Type.GetType(___item.WidgetType);
                    var o = Activator.CreateInstance(_type);
                    foreach (var _jval in ___item.Data as JObject)
                    {
                        var _name = _jval.Key.ToString();
                        PropertyInfo pef = o.GetType().GetProperty(_jval.Key.ToString());
                        try
                        {
                            Type a1 = _jval.Value.GetType();
                            Type a2 = typeof(JValue);
                            if (pef != null && a1 == typeof(JValue))
                            {
                                pef.SetValue(o, _jval.Value.ToString());
                            }
                            if (pef != null && a1 == typeof(JArray))
                            {
                                pef.SetValue(o, _jval.Value.ToObject<List<string>>());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    PropertyInfo pefh = o.GetType().GetProperty("HttpContext");
                    if (pefh != null) pefh.SetValue(o, this.HttpContext);
                    pefh = o.GetType().GetProperty("Theme");
                    if (pefh != null) pefh.SetValue(o, this.This);
                    pefh = o.GetType().GetProperty("WidgetId");
                    if (pefh != null) pefh.SetValue(o, ___item.WidgetId);
                    pefh = o.GetType().GetProperty("WidgetLocation");
                    if (pefh != null) pefh.SetValue(o, "shortcode");
                    pefh = o.GetType().GetProperty("Data");
                    if (pefh != null) pefh.SetValue(o, ___item.Data);
                    var exes = "";
                    if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

                    var rendered = viewRenderService.RenderToStringAsync(appSetting.Get("app_theme") + "/Widgets/" + _type.Name + "/View", o);
                    try
                    {
                        exes = rendered.Result;
                    }
                    catch (Exception ex)
                    {
                        exes = ex.ToString();

                    }
                    data = data + exes;
                }
            }
            return data;
        }
        public string ShortCodeParse(Match match)
        {
            Parse parse = null;
            string data = match.Groups[0].Value;

            if (!string.IsNullOrEmpty(match.Groups[1].Value))
            {

                var e = match.Groups[1].Value;
                e = e.Replace("'", "").Replace("\"", "");
                string[] codes = e.Split(' ');
                Dictionary<string, string> list = new Dictionary<string, string>();
                bool IsCode = true;
                parse = new Parse { Code = "", Args = null };
                foreach (string item in codes)
                {
                    if (IsCode)
                    {
                        IsCode = false;
                        parse.Code = item;
                    }
                    else
                    {
                        string[] x = item.Split("=");
                        list.Add(x[0], x[1]);
                    }

                }
                parse.Args = list;
                if (parse != null)
                {
                    foreach (ShortCode item in this.ShortCodes)
                    {
                        if (item.Code == parse.Code)
                        {
                            if (parse.Code == "widget")
                            {
                                Type module = this.This.GetType();
                                MethodInfo mi = module.GetMethod(item.Method);
                                object[] arg = new object[] { parse.Args };
                                data = (string)mi.Invoke(this, arg);
                            }
                            else
                            {
                                Type module = this.This.GetType();
                                MethodInfo mi = module.GetMethod(item.Method);
                                object[] arg = new object[] { parse.Args };
                                data = (string)mi.Invoke(this.This, arg);

                            }

                        }
                    }
                }

            }
            return data;

        }

    }
    public class Path
    {

        public string Id
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public bool IsLive
        {
            get; set;
        }
        public string CrossOrigin
        {
            get; set;
        }
        public string Integrity
        {
            get; set;
        }

    }
    public class Parse
    {
        public string Code
        {
            get;
            set;
        }
        public Dictionary<string, string> Args
        {
            get;
            set;
        }
    }
    public class Action
    {

        public string Name
        {
            get;
            set;
        }
        public string Method
        {
            get;
            set;
        }
        public int Order
        {
            get; set;
        }

    }
    public class Filter
    {

        public string Name
        {
            get;
            set;
        }
        public string Method
        {
            get;
            set;
        }
        public int Order
        {
            get; set;
        }

    }
    public class ShortCode
    {

        public string Code
        {
            get;
            set;
        }
        public string Method
        {
            get;
            set;
        }
        public int Order
        {
            get; set;
        }

    }
    public class WidgetLocation
    {
        public string Id = null;
        public string TitleWrap = "<h1>%</h1>";
        public string ContentWrap = "<p>%</p>";
        public string Status = "active";
        public string Title = "";



    }

    public class Widget
    {

        public string Location { get; set; }

        public List<Widgets> Widgets { get; set; }



    }
    public class Widgets
    {
        protected AppSetting appSetting;
        [JsonIgnore]
        public HttpContext HttpContext { get; set; }
        [JsonIgnore]
        public Theme Theme { get; set; }
        [JsonIgnore]
        protected IViewRenderService viewRenderService;
        public string GetJson()
        {

            return JsonConvert.SerializeObject(this);

        }
        public WidgetResponse Update<T>(T value) where T : class
        {
            //if(value)

            Type t = value.GetType();

            //PropertyInfo prop = 

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string widget_id = t.GetProperty("WidgetId").GetValue(value).ToString();
            string widget_location = t.GetProperty("WidgetLocation").GetValue(value).ToString();
            string widget_type = t.GetProperty("WidgetType").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;
            Widgets widgets = new Widgets { WidgetId = widget_id, WidgetLocation = widget_location, Data = new JObject(), Action = action, WidgetType = widget_type };
            JObject jo = t.GetProperty("Data").GetValue(value) as JObject;
            //===
            Type type = Type.GetType(WidgetType);
            var real_instance = Activator.CreateInstance(type);
            // real_instance.
            PropertyInfo[] properties = t.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "HttpContext" || property.Name == "Theme" || property.Name == "Name" || property.Name == "Data") continue;
                PropertyInfo p = real_instance.GetType().GetProperty(property.Name);
                if (p != null)
                {

                    var vl = t.GetProperty(property.Name).GetValue(value);
                    ///var jsonstr="[]";
                    //var xx = vl.GetType();
                    if (vl is IList && vl.GetType().IsGenericType)
                    {

                        // jsonstr = vl;// as string[];//.JsonConvert.SerializeObject(vl);
                        p.SetValue(real_instance, vl);
                    }
                    else
                    {
                        //jsonstr = vl.ToString();
                        p.SetValue(real_instance, vl);
                    }



                    var val = property.GetValue(value, null);
                    if (jo.ContainsKey(property.Name))
                    {
                        if (val is IList && val.GetType().IsGenericType)
                        {


                            var val1 = JsonConvert.SerializeObject(val).ToString();

                            var val2 = Regex.Unescape(val1.ToString());

                            val = val2;
                        }
                        jo["" + property.Name + ""] = new JValue(val);
                    }
                    else
                    {
                        jo.Add(property.Name, new JValue(val));
                    }


                }

            }
            if (action == "render" || action == "update")
            {


                JObject e = t.GetProperty("Data").GetValue(value) as JObject;

                foreach (var _jval_ in e)
                {
                    if (_jval_.Key == "HttpContext" || _jval_.Key == "Theme" || _jval_.Key == "Data" || _jval_.Key == "Action" || _jval_.Key == "WidgetId" || _jval_.Key == "WidgetLocation" || _jval_.Key == "Name") continue;

                    PropertyInfo pi = real_instance.GetType().GetProperty(_jval_.Key.ToString());
                    if (pi != null)
                    {
                        Type type__ = _jval_.Value.GetType();

                        try
                        {
                            var c = JsonConvert.DeserializeObject<List<string>>(_jval_.Value.ToString());
                            if (c != null && c.Count() > 0)
                            {
                                pi.SetValue(real_instance, c);
                            }
                            else
                            {
                                pi.SetValue(real_instance, _jval_.Value.ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                pi.SetValue(real_instance, _jval_.Value);
                            }
                            catch (Exception ex1)
                            {
                                try
                                {

                                    pi.SetValue(real_instance, _jval_.Value.ToString());
                                }
                                catch
                                {
                                    pi.SetValue(real_instance, null);
                                }

                            }

                        }

                    }



                }

            }
            //

            PropertyInfo hcpi = real_instance.GetType().GetProperty("HttpContext");
            if (hcpi != null) hcpi.SetValue(real_instance, this.HttpContext);

            PropertyInfo wipi = real_instance.GetType().GetProperty("WidgetId");
            if (wipi != null) wipi.SetValue(real_instance, widget_id);

            PropertyInfo wlpi = real_instance.GetType().GetProperty("WidgetLocation");
            if (wlpi != null) wlpi.SetValue(real_instance, widget_location);

            PropertyInfo wtpi = real_instance.GetType().GetProperty("WidgetType");
            if (wtpi != null) wtpi.SetValue(real_instance, widget_type);

            PropertyInfo acpi = real_instance.GetType().GetProperty("Action");
            if (acpi != null) acpi.SetValue(real_instance, action);
            PropertyInfo tmpi = real_instance.GetType().GetProperty("Theme");
            if (tmpi != null) tmpi.SetValue(real_instance, Theme);

            PropertyInfo dapi = real_instance.GetType().GetProperty("Data");
            if (dapi != null) dapi.SetValue(real_instance, jo);



            //
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            string json = appSetting.Get("app_theme_widgets_" + Theme.GetName());
            string subtitle = (string)jo.SelectToken("Title");
            bool IsUpdated = false;
            widgets.Data = jo;
            if (!string.IsNullOrEmpty(json))
            {
                json = Regex.Unescape(json);
                List<Widget> _Widget = JsonConvert.DeserializeObject<List<Widget>>(json);
                List<Widget> __Widget = new List<Widget>();

                foreach (var __item in _Widget)
                {
                    if (__item.Location == widget_location)
                    {
                        IsUpdated = true;
                        if (action == "update")
                        {
                            foreach (var ___item in __item.Widgets)
                            {
                                if (___item.WidgetId == widget_id)
                                {

                                    ___item.Data = jo;
                                    widgets = ___item;


                                }

                            }
                        }
                        else if (action == "create")
                        {


                            __item.Widgets.Add(widgets);
                        }
                        subtitle = (string)jo.SelectToken("Title");
                    }
                    __Widget.Add(__item);
                }
                if (!IsUpdated)
                {
                    List<Widgets> wob = new List<Widgets>();
                    wob.Add(widgets);
                    __Widget.Add(new Widget() { Location = widget_location, Widgets = wob });
                }

                string w = JsonConvert.SerializeObject(__Widget);
                string stres = Regex.Unescape(w.Replace("\"[", "[").Replace("]\"", "]"));// escape array as string
                stres = stres.Replace(@"\", string.Empty);
                appSetting.Update("app_theme_widgets_" + Theme.GetName(), stres);




            }
            string toReturn = string.Empty;


            if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

            Type _type = real_instance.GetType();

            var rendered = viewRenderService.RenderToStringAsync(appSetting.Get("app_theme") + "/Widgets/" + _type.Name + "/Form", real_instance);
            try
            {
                toReturn = rendered.Result;
            }
            catch (Exception ex)
            {
                toReturn = ex.ToString();
            }

            //=========
            string Name = "";
            string Icon = "fa-buromobelexperte";
            var Field = real_instance.GetType().GetField("Name", BindingFlags.Public | BindingFlags.Instance);


            foreach (var itm in Theme.Widgets)
            {
                PropertyInfo jobs = itm.GetType().GetProperty("Class");
                if (jobs != null && jobs.GetValue(itm, null).ToString() == real_instance.GetType().ToString())
                {
                    PropertyInfo pi = itm.GetType().GetProperty("Icon");
                    Icon = pi != null ? pi.GetValue(itm, null).ToString() : "fa-buromobelexperte";
                }
            }


            if (Field != null)
            {
                Name = Field.GetValue(real_instance).ToString();
            }
            return new WidgetResponse() { Icon = Icon, Success = true, SubTitle = subtitle, Content = toReturn + "<div class=\"widget-foot mt-3 text-right\"><button class=\"btn btn-primary save-widgets\" type=\"submit\">Update</button></div>", Title = Name, Widgets = real_instance as Widgets };

        }

        public string WidgetId { get; set; }
        public string WidgetType { get; set; }
        public string WidgetLocation { get; set; }
        public JObject Data { get; set; }
        public string Action { get; set; } = "create";
        public WidgetResponse Delete<T>(T value) where T : class
        {
            //if(value)

            Type t = value.GetType();

            //PropertyInfo prop = 

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string widget_id = t.GetProperty("WidgetId").GetValue(value).ToString();
            string widget_location = t.GetProperty("WidgetLocation").GetValue(value).ToString();
            string widget_type = t.GetProperty("WidgetType").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;
            Widgets widgets = null;
            if (appSetting == null) appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            string json = appSetting.Get("app_theme_widgets_" + Theme.GetName());
            if (!string.IsNullOrEmpty(json))
            {
                json = Regex.Unescape(json);
                List<Widget> _Widget = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Widget>>(json);
                List<Widget> __Widget = new List<Widget>();
                foreach (var __item in _Widget)
                {
                    if (__item.Location == widget_location)
                    {
                        List<Widgets> wl = new List<Widgets>();
                        foreach (var ___item in __item.Widgets)
                        {
                            if (___item.WidgetId != widget_id)
                            {
                                wl.Add(___item);
                            }
                            else
                            {
                                widgets = ___item;
                            }

                        }
                        __item.Widgets = wl;
                        __Widget.Add(__item);

                    }
                    else
                    {
                        __Widget.Add(__item);
                    }

                }


                string w = Newtonsoft.Json.JsonConvert.SerializeObject(__Widget);
                appSetting.Update("app_theme_widgets_" + Theme.GetName(), w);




            }



            return new WidgetResponse() { Success = true, Content = "Successfully Deleted", Widgets = widgets, Message = "Widget is successfully deleted." };

        }

    }
    public class Component
    {
        protected AppSetting appSetting;
        protected ComponentService ComponentService;
        [JsonIgnore]
        public HttpContext HttpContext { get; set; }
        [JsonIgnore]
        public Theme Theme { get; set; }
        public virtual string Title { get; set; }
        [JsonIgnore]
        protected IViewRenderService viewRenderService;
        public string GetJson()
        {

            return JsonConvert.SerializeObject(this);

        }
        public ComponentResponse Update<T>(T value) where T : class
        {

            Type t = value.GetType();

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string component_id = t.GetProperty("ComponentId").GetValue(value).ToString();
            string component_type = t.GetProperty("ComponentType").GetValue(value).ToString();
            string form_id = t.GetProperty("FormId").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;

            JObject jo = t.GetProperty("Data").GetValue(value) as JObject;
            var components = new Component { ComponentId = component_id, Data = jo, Action = action, ComponentType = component_type, FormId = form_id };

            //===
            Type type = Type.GetType(component_type);
            var real_instance = value;
            // real_instance.
            PropertyInfo[] properties = t.GetProperties();

            PropertyInfo hcpi = real_instance.GetType().GetProperty("HttpContext");
            if (hcpi != null) hcpi.SetValue(real_instance, this.HttpContext);





            if (ComponentService == null) ComponentService = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            string subtitle = real_instance?.GetType().GetProperty("Title")?.GetValue(real_instance)?.ToString();
            bool IsUpdated = false;
            if (subtitle == null && jo?.Property("Title") != null)
            {
                subtitle = jo.Property("Title")?.Value?.FirstOrDefault()?.ToString(); //["subtitle"].ToString();//.GetType().GetProperty("Title").GetValue(jo).ToString();

            }
            List<dynamic> _Component = Theme.GetComponents();
            List<object> __Component = new List<object>();
            if (_Component.Count() > 0)
            {
                foreach (var __item in _Component)
                {
                    if (action == "update")
                    {
                        if (__item.ComponentId == component_id)
                        {
                            IsUpdated = true;
                            real_instance.GetType().GetProperty("Data").SetValue(real_instance, JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(real_instance)) as JObject);
                            __Component.Add(real_instance);
                        }
                        else
                        {
                            __Component.Add(__item);
                        }
                    }

                }
            }
            if (!IsUpdated)
            {
                __Component.Add(real_instance);
            }
            if (action == "update")
            {
                string w = JsonConvert.SerializeObject(__Component);
                string stres = Regex.Unescape(w.Replace("\"[", "[").Replace("]\"", "]"));// escape array as string
                stres = stres.Replace(@"\", string.Empty);
                ComponentService.Update("app_components", stres);
            }



            //}
            string toReturn = string.Empty;


            if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

            Type _type = real_instance.GetType();
            string typ = _type.GetProperty("ComponentType").GetValue(real_instance).ToString();
            Type __type = Type.GetType(typ);
            //string datavalue=
            if (action == "render")
            {
                real_instance = value;
            }

            var rendered = viewRenderService.RenderToStringAsync("Core/Components/" + _type.Name + "/Form", real_instance);
            try
            {
                toReturn = rendered.Result;
            }
            catch (Exception ex)
            {
                toReturn = ex.ToString();
            }

            string Name = subtitle;
            string Icon = "fa-buromobelexperte";
            var Field = real_instance.GetType().GetField("Name", BindingFlags.Public | BindingFlags.Instance);


            foreach (var itm in Theme.Components)
            {
                PropertyInfo jobs = itm.GetType().GetProperty("Class");
                if (jobs != null && jobs.GetValue(itm, null).ToString() == real_instance.GetType().ToString())
                {
                    PropertyInfo pi = itm.GetType().GetProperty("Icon");
                    Icon = pi != null ? pi.GetValue(itm, null).ToString() : "fa-buromobelexperte";
                }
            }


            if (Field != null)
            {
                Name = Field.GetValue(real_instance).ToString();
            }
            return new ComponentResponse() { Icon = Icon, Success = true, SubTitle = subtitle, Content = toReturn + "<div class=\"widget-foot mt-3 text-right\"><button class=\"btn btn-primary save-widgets\" type=\"submit\">Update</button></div>", Title = Name, Component = real_instance as Component };

        }
        /// <summary>
        /// Update for Element Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="eventType">For Specific Element Type</param>
        /// <param name="elementId">For Specific Element</param>
        /// <param name="formId">Form</param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public ComponentResponse Update<T>(T value, int eventType, string elementId, int formId) where T:class
        {
            Type t = value.GetType();

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string component_id = t.GetProperty("ComponentId").GetValue(value).ToString();
            string component_type = t.GetProperty("ComponentType").GetValue(value).ToString();
            string form_id = t.GetProperty("FormId").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;

            JObject jo = t.GetProperty("Data").GetValue(value) as JObject;
            var components = new Component { ComponentId = component_id, Data = jo, Action = action, ComponentType = component_type, FormId = form_id };

            //===
            Type type = Type.GetType(component_type);
            var real_instance = value;
            // real_instance.
            PropertyInfo[] properties = t.GetProperties();

            PropertyInfo hcpi = real_instance.GetType().GetProperty("HttpContext");
            if (hcpi != null) hcpi.SetValue(real_instance, this.HttpContext);





            if (ComponentService == null) ComponentService = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;

            string subtitle = real_instance?.GetType().GetProperty("Title")?.GetValue(real_instance)?.ToString();
            bool IsUpdated = false;
            if (subtitle == null && jo?.Property("Title") != null)
            {
                subtitle = jo.Property("Title")?.Value?.FirstOrDefault()?.ToString(); //["subtitle"].ToString();//.GetType().GetProperty("Title").GetValue(jo).ToString();

            }
            List<dynamic> _Component = Theme.GetElementComponents(formId,elementId,eventType);
            List<object> __Component = new List<object>();
            if (_Component.Count() > 0)
            {
                foreach (var __item in _Component)
                {
                    if (action == "update")
                    {
                        if (__item.ComponentId == component_id)
                        {
                            IsUpdated = true;
                            real_instance.GetType().GetProperty("Data").SetValue(real_instance, JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(real_instance)) as JObject);
                            __Component.Add(real_instance);
                        }
                        else
                        {
                            __Component.Add(__item);
                        }
                    }

                }
            }
            if (!IsUpdated)
            {
                __Component.Add(real_instance);
            }
            if (action == "update")
            {
                string w = JsonConvert.SerializeObject(__Component);
                string stres = Regex.Unescape(w.Replace("\"[", "[").Replace("]\"", "]"));// escape array as string
                stres = stres.Replace(@"\", string.Empty);
                ComponentService.Update("elm_components", stres,formId,elementId,eventType);
            }



            //}
            string toReturn = string.Empty;


            if (viewRenderService == null) viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

            Type _type = real_instance.GetType();
            string typ = _type.GetProperty("ComponentType").GetValue(real_instance).ToString();
            Type __type = Type.GetType(typ);
            //string datavalue=
            if (action == "render")
            {
                real_instance = value;
            }

            var rendered = viewRenderService.RenderToStringAsync("Core/Components/" + _type.Name + "/Form", real_instance);
            try
            {
                toReturn = rendered.Result;
            }
            catch (Exception ex)
            {
                toReturn = ex.ToString();
            }

            string Name = subtitle;
            string Icon = "fa-buromobelexperte";
            var Field = real_instance.GetType().GetField("Name", BindingFlags.Public | BindingFlags.Instance);


            foreach (var itm in Theme.Components)
            {
                PropertyInfo jobs = itm.GetType().GetProperty("Class");
                if (jobs != null && jobs.GetValue(itm, null).ToString() == real_instance.GetType().ToString())
                {
                    PropertyInfo pi = itm.GetType().GetProperty("Icon");
                    Icon = pi != null ? pi.GetValue(itm, null).ToString() : "fa-buromobelexperte";
                }
            }


            if (Field != null)
            {
                Name = Field.GetValue(real_instance).ToString();
            }
            return new ComponentResponse() { Icon = Icon, Success = true, SubTitle = subtitle, Content = toReturn + "<div class=\"widget-foot mt-3 text-right\"><button class=\"btn btn-primary save-widgets\" type=\"submit\">Update</button></div>", Title = Name, Component = real_instance as Component };

        }

        public string ComponentId { get; set; }
        public string ComponentType { get; set; }
        public JObject Data { get; set; }
        //public JArray configs { get; set; }
        public string Action { get; set; } = "create";
        public ComponentResponse Delete<T>(T value) where T : class
        {
            //if(value)

            Type t = value.GetType();

            //PropertyInfo prop = 

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string component_id = t.GetProperty("ComponentId").GetValue(value).ToString();
            string component_type = t.GetProperty("ComponentType").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;
            Component components = null;
            if (ComponentService == null) ComponentService = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;
            string json = ComponentService.Get("app_components");
            if (!string.IsNullOrEmpty(json))
            {
                json = Regex.Unescape(json);
                List<Component> _Component = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Component>>(json);
                List<Component> __Component = new List<Component>();
                foreach (var __item in _Component)
                {
                    if (__item.ComponentId == component_id)
                    {
                        components = __item;
                    }
                    else
                    {
                        __Component.Add(__item);
                    }

                }


                string w = Newtonsoft.Json.JsonConvert.SerializeObject(__Component);
                ComponentService.Update("app_components", w);




            }



            return new ComponentResponse() { Success = true, Content = "Successfully Deleted", Component = components, Message = "Component is successfully deleted." };

        }
        public ComponentResponse Delete<T>(T value, int eventType, string elementId, int formId) where T : class
        {
            //if(value)

            Type t = value.GetType();

            //PropertyInfo prop = 

            string action = t.GetProperty("Action").GetValue(value).ToString();
            string component_id = t.GetProperty("ComponentId").GetValue(value).ToString();
            string component_type = t.GetProperty("ComponentType").GetValue(value).ToString();
            this.HttpContext = t.GetProperty("HttpContext").GetValue(value) as HttpContext;
            Component components = null;
            if (ComponentService == null) ComponentService = this.HttpContext.RequestServices.GetService(typeof(ComponentService)) as ComponentService;
            string json = ComponentService.Get("elm_components",formId,elementId,eventType);
            if (!string.IsNullOrEmpty(json))
            {
                json = Regex.Unescape(json);
                List<Component> _Component = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Component>>(json);
                List<Component> __Component = new List<Component>();
                foreach (var __item in _Component)
                {
                    if (__item.ComponentId == component_id)
                    {
                        components = __item;
                    }
                    else
                    {
                        __Component.Add(__item);
                    }

                }


                string w = Newtonsoft.Json.JsonConvert.SerializeObject(__Component);
                ComponentService.Update("elm_components", w,formId,elementId,eventType);




            }



            return new ComponentResponse() { Success = true, Content = "Successfully Deleted", Component = components, Message = "Component is successfully deleted." };

        }
        public virtual string FormId { get; set; }

    }

    public class WidgetResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public string Content { set; get; }
        public Widgets Widgets { set; get; }
        public string Title { set; get; }
        public string SubTitle { set; get; }
        public string Icon { get; set; }
    }
    public class ComponentResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public string Content { set; get; }
        public Component Component { set; get; }
        public string Title { set; get; }
        public string SubTitle { set; get; }
        public string Icon { get; set; }
    }
}
