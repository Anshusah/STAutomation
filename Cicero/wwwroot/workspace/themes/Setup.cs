using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Themes.Blue.Widgets;
namespace Themes.Blue
{
    public class Setup:Theme{
        public Setup(HttpContext e)
        {
            this.HttpContext = e;
            this.This = this;

        }

        public void Init()
        {

            // Theme Name
            this.Name = "Blue";
            // Theme Version
            this.Version = "0.1";
            // Register Required CSS
            this.RegisterCss("main-frontend", "/Themes/Blue/css/main-frontend.css");
            this.RegisterCss("site", "/Themes/Blue/css/site.css");
            // Register Required JS
            this.RegisterJs("jquery", "https://code.jquery.com/jquery-3.3.1.min.js", "sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=", "anonymous");
            //this.RegisterJs("jquery", "https://code.jquery.com/jquery-2.2.4.min.js", "sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=", "anonymous");
            this.RegisterJs("popper", "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js", "sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49", "anonymous");
            this.RegisterJs("bootstrapjs", "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js", "sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy", "anonymous");
            this.RegisterJs("validate", "/lib/jquery-validation/dist/jquery.validate.js");
            this.RegisterJs("validateunobtrusive", "/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
            this.RegisterJs("site", "/Themes/Blue/js/site.js");
            this.RegisterJs("selectize", "/js/selectize.min.js");

            // Register Required Menu Locations for this Theme
            this.RegisterNav("Primary");
            this.RegisterNav("Bottom");
            //this.RegisterNav("Left");
            //this.RegisterNav("Right");

            this.RegisterWidget(
                new {
                    Title="Text Widget",
                    Description = "This Widget helps you to Place text anywhere",
                    Class = new Text(),
                    Status="Active",
                    Icon= "far fa-list-alt"
                }
            );
           
            this.RegisterWidget(
                new
                {
                    Title = "Social Icon",
                    Description = "This Widget Helps you to place social icons anywhere",
                    Class = new Social(),
                    Status = "Active",
                    Icon = "fa-share-alt"
                }
            );
            this.RegisterWidget(
                new
                {
                    Title = "Feature Article",
                    Description = "This Widget Helps you to list article anywhere in the theme",
                    Class = new FeatureArticle(),
                    Status = "Active",
                    Icon = "fa fa-deaf"
                }
            );
            this.RegisterWidget(
                new
                {
                    Title = "Custom Article",
                    Description = "This Widget Helps you to list/banner custom articles anywhere in the theme",
                    Class = new CustomArticle(),
                    Status = "Active",
                    Icon = "far fa-list-ul"
                }
            );
            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "aboutus",
                Status = "active",
                Title = "This is for Footer About us",
                TitleWrap = "<h1>{0}</h1>",
                ContentWrap = "<p>{0}</p>"
            });
            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "link",
                Status = "active",
                Title = "This is for Link List",
                TitleWrap = "<h2>{0}</h2>",
                ContentWrap = "<p>{0}</p>"
            });

            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "social",
                Status = "active",
                Title = "This is for Blue 2 location",
                TitleWrap = "<h1>{0}</h1>",
                ContentWrap = "<p>{0}</p>"
            });

            this.AddAction("blue", "Blue", 0);
            //this.AddFilter("nav_li_args", "Filter", 0);
            this.AddShortCode("gallery", "sortcode");
            this.AddAction("hook", "AjaxHook", 0);

        }
        public string AjaxHook(Dictionary<string,string> e)
        {
            e.TryGetValue("action", out string action);
            return "{\"action\":\""+action+"\"}";
        }
        public string Blue()
        {
            return "blue string";
        }
        public object Filter(object e)
        {
            //var dd = "";
            return e;
        }
        public string sortcode(object e)
        {
            var i = e;
            return "<y>";
        }


    }
}
