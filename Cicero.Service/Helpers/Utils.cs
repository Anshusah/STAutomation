using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cicero.Service.Models;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Cicero.Service.Models.Component;
using Cicero.Service.Library;
using Cicero.Service.Library.Toastr;
using Newtonsoft.Json.Linq;

namespace Cicero.Service.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Utils
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor = null;
        private readonly IActionContextAccessor IActionContext = null;
        private static readonly Assembly ThisAssembly = typeof(NToastNotifyViewComponent).Assembly;

        public Utils(ApplicationDbContext adb, IHttpContextAccessor _httpContextAccessor, IActionContextAccessor _IActionContext)
        {
            db = adb;
            httpContextAccessor = _httpContextAccessor;
            IActionContext = _IActionContext;
        }

        public string Ucfirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public void addModelError(ModelStateDictionary _state)
        {
            var errors = _state.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    if (error != null)
                    {
                        _state.AddModelError(string.Empty, error.Message);
                    }

                }
            }
        }

        public static string CreateOrEdit(dynamic e)
        {
            if (  e?.Id == null && e?.Id?.ToString() == "0")
            {
                return "Create";
            }
            return "Edit";

        }

        public List<string> getTemplates()
        {
            List<string> list = new List<string>();
            /*foreach (string file in Directory.EnumerateFiles(System.IO.Directory.GetCurrentDirectory()+"/Views/Article", "*tmpl.cshtml"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(file).Replace(".tmpl","");
                list.Add(name);
            }*/
            list.Add("Default");
            list.Add("Contact");
            return list;
        }
        public static string GenerateId()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        }
        public static string GetDefaultDateFormat(DateTime? datetime)
        {
            if (datetime == null)
            {
                return "";
            }

            return datetime.Value.ToString("dd MMMM yyyy");
        }

        public static string GetDefaultDateFormatToDetail(DateTime? datetime)
        {
            if (datetime == null)
            {
                return "";
            }

            return datetime.Value.ToString("dd MMM yyyy");
        }

        public static string ConvertToString(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";
            return obj.ToString().Trim();
        }
        public int DecryptId(string e)
        {
            byte[] b;
            int decrypted;
            try
            {
                b = Convert.FromBase64String(e);
                decrypted = Convert.ToInt32(System.Text.Encoding.BigEndianUnicode.GetString(b));
            }
            catch (Exception)
            {
                decrypted = 0;
            }
            return decrypted;
        }

        public string EncryptId(int e)
        {

            try
            {
                byte[] b = System.Text.Encoding.BigEndianUnicode.GetBytes(e.ToString());
                string encrypted = Convert.ToBase64String(b);
                return encrypted;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Serialize(dynamic e)
        {
            var temp = JsonConvert.SerializeObject(e);

            return temp;
        }

        public static string GetDateForCase()
        {
            return DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString("00");
        }

        public static ArrayList getMenuLocations()
        {
            ArrayList ml = new ArrayList();
            ml.Add("Primary");
            ml.Add("Bottom");
            return ml;
        }

        public static string Truncate(string value, int maxLength = 250)
        {
            if (string.IsNullOrEmpty(value)) return value;
            //remove html tags
            value = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public string GetTenantFromSession()
        {
            string result = httpContextAccessor.HttpContext.Session.GetString("tenant_identifier");
            if (result != null)
            {
                return result;
            }
            else
            {
                return "";
            }
        }

        public string GetTenantForUrl(bool ask = true)
        {
            string result = GetTenantFromSession();
            if (!string.IsNullOrEmpty(result.Trim()))
            {
                if (ask == true)
                {
                    return result + "/";
                }
                else
                {
                    return "/" + result;
                }

            }
            else
            {
                return "/transfer";
            }
        }

        public string GetParams(string e)
        {
            try
            {
                var name = IActionContext.ActionContext.RouteData.Values[e];
                if (name != null)
                {
                    return name.ToString().Replace(".html", "");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
        public enum MediaType
        {
            Group = 1,
            Image = 2,
            Icon = 3
        }
        //public List<SelectListItem> GetFieldType()
        //{
        //    var names = Array.ConvertAll((SqlDbType[])Enum.GetValues(typeof(SqlDbType)),
        //                     type => type.ToString());
        //    List<SelectListItem> SelectListItemsLists = new List<SelectListItem>();
        //    foreach (var datatypes in names)
        //    {
        //        SelectListItem SelectListItems = new SelectListItem();
        //        SelectListItems.Text = datatypes;
        //        SelectListItems.Value = datatypes;
        //        SelectListItemsLists.Add(SelectListItems);
        //    }


        //    return SelectListItemsLists;
        //}
        public List<SelectListItem> DataTypeList()
        {
            var names = new List<SelectListItem>()
            {
                 new SelectListItem(){ Text="Numeric Value",Value = "integer"},
                 new SelectListItem(){ Text="Decimal Value",Value = "decimal"},
                 new SelectListItem(){ Text="Date",Value = "date"},
                 new SelectListItem(){ Text="Text",Value = "string"}
            }; 
            return names;
        }
        public static EmbeddedFileProvider GetEmbeddedFileProvider()
        {
            return new EmbeddedFileProvider(ThisAssembly, "NToastNotify");
        }

        public static ILibrary GetLibraryDetails<T>(NToastNotifyOption nToastNotifyOptions, ILibraryOptions defaultOptions) where T : class, ILibrary, new()
        {
            var library = new T();
            if (nToastNotifyOptions != null)
            {
                if (!string.IsNullOrWhiteSpace(nToastNotifyOptions.ScriptSrc))
                {
                    library.ScriptSrc = nToastNotifyOptions.ScriptSrc;
                }
                if (!string.IsNullOrWhiteSpace(nToastNotifyOptions.StyleHref))
                {
                    library.StyleHref = nToastNotifyOptions.StyleHref;
                }
            }

            if (defaultOptions != null)
            {
                library.Options = defaultOptions;
            }
            return library;
        }

        public JObject ReturnResult(string status, string message, string error = "")
        {
            JObject result = new JObject();
            result.Add("status", status);
            result.Add("message", message);
            if (error != "")
            {
                result.Add("error", error);
            }
            return result;
        }

    }
}
