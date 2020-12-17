using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.AspNetCore.Mvc; 
using Cicero.Service.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Cicero.Data;
using Cicero.Service.Helpers; 
using System.Reflection;
using Cicero.Service.Models.Core;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseController : Controller
    {
        public readonly string errorPageUrl="~/admin/error.html";
        private readonly IUserService UserService;
        private ApplicationDbContext db = null;
        private AppSetting appSetting = null;
        public Theme Theme;
        public int TenantId = 0;
        private ICommonService commonService = null;
        private Utils utils = null;
        protected BaseController(IUserService _UserService)
        {
            UserService = _UserService;
        }
        [HttpGet]
        public string GetTenantIdentifier()
        {
            try
            {
               
                var routeDate = RouteData.Values.Where(x => x.Key == "tenant_identifier").FirstOrDefault();
                string result = "";
                if (routeDate.Value != null && HttpContext.Session.GetString("tenant_identifier") != null)
                {
                    result = routeDate.Value.ToString();
                }
                else
                {
                    result = UserService.GetTenantIdentifierByUserId();

                }
                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.Trim()) && UserService.CheckUserInTenant(result))
                    {
                        result = result.ToLower().Replace(".html", "");

                        HttpContext.Session.SetString("tenant_identifier", result);
                    }
                    else
                    {
                        //override session if user types tenant identifier in the url
                        result = UserService.GetTenantIdentifierByUserId();
                        HttpContext.Session.SetString("tenant_identifier", UserService.GetTenantIdentifierByUserId());
                        //result = HttpContext.Session.GetString("tenant_identifier");
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string result = GetTenantIdentifier();

            
            try
            {
                if (db == null) db = HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
                if (appSetting == null) appSetting = HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
                if (commonService == null) commonService = HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                if (utils == null) utils = HttpContext.RequestServices.GetService(typeof(Utils)) as Utils;
                this.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var _setting = db.Setting.Where(_x => _x.FieldKey == "app_themes").FirstOrDefault();//.SingleOrDefault(x => x.FieldKey == "themes");
                if (_setting != null)
                {
                    string str_json = _setting.FieldValue;
                    var t = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Theme>>(str_json);

                }

                string controller = filterContext.RouteData.Values["Controller"].ToString();
                string action = filterContext.RouteData.Values["Action"].ToString();
                //string tenant_theme = (appSetting.Get("app_theme") == "" ? "Test" : appSetting.Get("app_theme"));

                string tenant_theme =  appSetting.Get("app_theme","JazzCash");
                Type _type = Type.GetType("Themes." + tenant_theme + ".Setup");
                var o = Activator.CreateInstance(_type, HttpContext);
                object p = Convert.ChangeType(o, Type.GetType("Themes." + tenant_theme + ".Setup"));
                MethodInfo mi = _type.GetMethod("Init");
                mi.Invoke(p, null);

                _type.GetProperty("ControllerAction").SetValue(p, action);
                _type.GetProperty("ControllerName").SetValue(p, controller);
                this.Theme = p as Theme;
                ViewData["theme"] = p;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            base.OnActionExecuting(filterContext);

        }

    }
}
