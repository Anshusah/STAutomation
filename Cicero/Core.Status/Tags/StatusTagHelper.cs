using Core.Status.Extensions;
using Core.Status.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Core.Status.Tags
{
    //[HtmlTargetElement("div")]
    [HtmlTargetElement("div", Attributes = "[class^=status]")]
    public class StatusTagHelper : TagHelper
    {
        private ITempDataDictionary tempData;
        private IHttpContextAccessor iHttpContextAccessor;
        private ITempDataDictionaryFactory iTempDataDictionaryFactory;
        private const string HtmlStandardTemplate = "<div class=\"alert alert-{0}\">{1}</div>";

        private const string HtmlDismissableTemplate = 
            "<div class=\"alert alert-{0} alert-dismissible\" role=\"alert\">" +
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
                    "<span aria-hidden=\"true\">&times;</span>" +
                "</button>{1}" +
            "</div>";

        public StatusTagHelper(ITempDataDictionaryFactory factory, IHttpContextAccessor contextAccessor)
        {
            //.GetTempData(.HttpContext);
            iTempDataDictionaryFactory = factory;
            iHttpContextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            tempData = iTempDataDictionaryFactory.GetTempData(iHttpContextAccessor.HttpContext);
            var messages = tempData.Get<Queue<Message>>(Constants.Key) ?? new Queue<Message>();

            while (messages.Count > 0)
            {
                var message = messages.Dequeue();

                var html = message.Dismissable
                    ? string.Format(HtmlDismissableTemplate, message.Type, message.Text)
                    : string.Format(HtmlStandardTemplate, message.Type, message.Text);

                output.Content.AppendHtml(html);
                //var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value;
                //output.Attributes.SetAttribute("class", $"{classes} show ");
                output.Attributes.SetAttribute("class", "status show");
                //output.Attributes.Add("class", "status show");
            }
        }
    }
}
