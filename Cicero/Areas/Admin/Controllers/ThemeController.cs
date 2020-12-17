using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.AspNetCore.Mvc; 
using Cicero.Service.Services;
using Newtonsoft.Json;
using Cicero.Service.Helpers;
using Cicero.Data; 
using Cicero.Service.Models.Core;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ThemeController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly AppSetting Setting = null;

        public ThemeController(Utils _utils, ApplicationDbContext _ApplicationDbContext, AppSetting _Setting, IUserService _UserService):base(_UserService)
        {
            db = _ApplicationDbContext;
            utils = _utils;
            Setting = _Setting;
            userService = _UserService;
        }

        [Route("admin/themes.html")]
        [Route("admin/{tenant_identifier}/themes.html")]
        public ActionResult Index()
        {

           

            Theme theme = this.Theme as Theme;
            var WidgetNameAsString = "app_themes";

            var Widget = db.Setting.Where(d=>d.FieldKey=="app_themes").FirstOrDefault();
            string ef = string.Empty;
            if (Widget == null || Widget.FieldValue == "")
            {
                ef = "[]";
            }
            else
            {
                ef = Widget.FieldValue;
            }

            var obj = JsonConvert.DeserializeObject<List<Theme>>(ef);



            return View(obj);


        }
		[Route("admin/edit.html")]
        [Route("admin/{tenant_identifier}/edit.html")]
        public ActionResult Edit()
        {

            /*
            var a = db.Article.ToList();

            Theme theme = this.Theme as Theme;
            var WidgetNameAsString = "app_theme_widgets_" + theme.GetName();

            var Widget = Setting.Get(WidgetNameAsString, "");

            if (Widget == null || Widget == "")
            {
                Widget = "[]";
            }

            var obj = JsonConvert.DeserializeObject<List<Widget>>(Widget);


    */
            return View();


        }
        [HttpPost]
        [Route("admin/theme/activate.html")]
        [Route("admin/{tenant_identifier}/theme/activate.html")]
        public JsonResult Activate(string theme)
        {

            Setting.Update("app_theme", theme);

            return Json(new { status="success"});
        }

    }
}