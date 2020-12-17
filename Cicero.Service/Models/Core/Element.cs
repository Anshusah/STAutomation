using Cicero.Service.Helpers;
using Cicero.Service.Services.Core.Themes;
using Cicero.Service.Services.Core.Themes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cicero.Service.Models.Core
{
    public class Element
    {

        // Required Service Instance
        [JsonIgnore]
        protected AppSetting appSetting;
        [JsonIgnore]
        public HttpContext HttpContext { get; set; }
        [JsonIgnore]
        public Theme Theme { get; set; }
        [JsonIgnore]
        protected IViewRenderService viewRenderService;
        // Required base field
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public virtual string Id { get; set; }
        public virtual string ElementId { get; set; }
        public virtual string Type { get; set; }
        public virtual string ElementType { get; set; }
        public virtual string Data { get; set; }
        public virtual FormBuilderViewModel ModelData { get; set; }
        public virtual string TabIndex { get; set; }
        public virtual string RowIndex { get; set; }
        public virtual string ColumnIndex { get; set; }
        public virtual string GroupIndex { get; set; }
        public virtual string ElementIndex { get; set; }
        public virtual string TableElementId { get; set; }
        public virtual string TableColumnIndex { get; set; }
        public virtual string Template { get; set; }
        public virtual string SetValueFrom { get; set; }
        public virtual bool LabelVisible { get; set; } = true;// required on render
        public virtual string WrapperTemplate { get; set; } = "";// required on render

        public virtual List<Permission> Permissions { get; set; }
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public ElementResponse GetSettingTemplate(dynamic _new, string _path = null)
        {
            Type ValueType = _new.GetType();
            if (_path == null)
            {
                _path = ValueType.Name;
            }
            this.HttpContext = _new.HttpContext as HttpContext;
            if (this.viewRenderService == null) this.viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;
            if (this.appSetting == null) this.appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            //string template_path = appSetting.Get("app_theme") + "/Elements/" + _path + "/Setting";
            string template_path = "Core/Elements/" + _path + "/Setting";

            var rendered = this.viewRenderService.RenderToStringAsync(template_path, _new);
            ElementResponse res = new ElementResponse();
            try
            {
                res.Success = true;
                res.Content = rendered.Result;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Content = ex.ToString();
                res.Message = ex.ToString();
            }
            res.Element = _new;
            return res;
        }
        public ElementResponse GetBackendPreviewTemplate(dynamic _new, string _path = null)
        {
            Type ValueType = _new.GetType();
            if (_path == null)
            {
                _path = ValueType.Name;
            }
            this.HttpContext = _new.HttpContext as HttpContext;
            if (this.viewRenderService == null) this.viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;
            if (this.appSetting == null) this.appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            string template_path = appSetting.Get("app_theme") + "/Elements/" + _path + "/Preview";
            var rendered = this.viewRenderService.RenderToStringAsync(template_path, _new);
            ElementResponse res = new ElementResponse();
            try
            {
                res.Success = true;
                res.Content = rendered.Result;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Content = ex.ToString();
                res.Message = ex.ToString();
            }
            res.Element = _new;
            return res;
        }
        public ElementResponse RenderSetting(dynamic _new, string _path = null)
        {
            Type ValueType = _new.GetType();
            if (_path == null)
            {
                _path = ValueType.Name;
            }
            this.HttpContext = _new.HttpContext as HttpContext;
            if (this.viewRenderService == null) this.viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;
            if (this.appSetting == null) this.appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            //string template_path = appSetting.Get("app_theme") + "/Elements/" + _path + "/Setting";
            string template_path = "Core/Elements/" + _path + "/Setting";
            var rendered = this.viewRenderService.RenderToStringAsync(template_path, _new);
            ElementResponse res = new ElementResponse();
            try
            {
                res.Success = true;
                res.Content = rendered.Result;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Content = ex.ToString();
                res.Message = ex.ToString();
            }
            res.Element = _new;
            return res;
        }
        public ElementResponse GetPreviewTemplate(dynamic _new, string _path = null)
        {

            Type ValueType = _new.GetType();
            if (_path == null)
            {
                _path = ValueType.Name;
            }
            if (_path == "Row" || _path == "Column" || _path == "row" || _path == "column")
            {
                return new ElementResponse();
            }
            this.HttpContext = _new.HttpContext as HttpContext;
            if (this.viewRenderService == null) this.viewRenderService = this.HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;
            if (this.appSetting == null) this.appSetting = this.HttpContext.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            //string template_path = appSetting.Get("app_theme") + "/Elements/" + _path + "/Preview";
            string template_path = "Core/Elements/" + _path + "/Preview";
            var rendered = this.viewRenderService.RenderToStringAsync(template_path, _new);
            ElementResponse res = new ElementResponse();
            try
            {
                res.Success = true;
                res.Content = rendered.Result;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Content = ex.ToString();
                res.Message = ex.ToString();
            }
            res.Element = _new;
            return res;
        }
        public class Permission
        {
            public string RoleId { get; set; }
            public bool Write { get; set; } = false;
            public bool Read { get; set; } = false;
        }
        public List<SelectListItem> TargetSelectOptions()
        {
            List<SelectListItem> targetOption = new List<SelectListItem>()
            {
                 new SelectListItem()
                 {
                    Text="Show",
                     Value="true"
                 },
                new SelectListItem()
                {
                    Text="Hide",
                    Value="false"
                },
                new SelectListItem()
                {
                    Text = "Disable",
                    Value = "disable"
                },
                new SelectListItem()
                {
                    Text="Enable",
                    Value="enable"
                }
            };
            return targetOption as List<SelectListItem>;

        }

    }
    public class ElementResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public string Content { set; get; }
        public string Setting { set; get; }
        public Element Element { set; get; }
        public string Title { set; get; }
        public string SubTitle { set; get; }
        public string Icon { get; set; }
    }

    public class FormBuilderViewModel
    {
        public FormBuilderViewModel()
        {
            Tab = new List<Elements.Tab>();
        }
        public List<Elements.Tab> Tab { get; set; }
        public Form Forms { get; set; }

        public FormBuilderViewModel DeepCopy()
        {
            FormBuilderViewModel other = (FormBuilderViewModel)this.MemberwiseClone();
            return other;
        }

        public IViewRenderService viewRenderService;
        public AppSetting appSetting;
        public ElementResponse RenderSetting(object ef, string _path, HttpContext ivr)
        {


            //this.appSetting = appSetting;
            if (this.viewRenderService == null) this.viewRenderService = ivr.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;
            if (appSetting == null) appSetting = ivr.RequestServices.GetService(typeof(AppSetting)) as AppSetting;
            //string template_path = appSetting.Get("app_theme") + "/Elements/" + _path + "/Setting";
            string template_path = "Core/Elements/" + _path + "/Setting";
            var rendered = this.viewRenderService.RenderToStringAsync(template_path, ef);
            ElementResponse res = new ElementResponse();
            try
            {
                res.Success = true;
                res.Content = rendered.Result;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Content = ex.ToString();
                res.Message = ex.ToString();
            }
            return res;
        }
        public class Form
        {
            public string TabEnable { get; set; }
            public string DisplayAs { get; set; } = "tab";
            public string ElementId { get; set; }
            public string Type { get; set; }
            public Navigation Navigations { get; set; }
            public List<Table> Tables { get; set; }
            public class Table
            {
                private string _tableName;
                public string Name
                {
                    get { return _tableName; }
                    set { _tableName = (value == null ? "" : value).Trim().Replace(" ", "_"); }
                }
                public List<Field> Fields { get; set; }

            }
            public class Field
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Size { get; set; }
                public string Default { get; set; }
                public bool Nullable { get; set; } = false;
                public bool PrimaryKey { get; set; } = false;
            }
            public class Navigation
            {
                public string Name { get; set; }
                public string Title { get; set; }
                public string Order { get; set; }
                public string Identifier { get; set; }
                public string Icon { get; set; }
            }
        }
    }

    public class ElementFieldValue
    {
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public List<string> Values { get; set; }
        public string Type { get; set; }
    }

}
