using Cicero.Service.Models.Core;
using System.Collections.Generic; 

namespace Themes.Core.Widgets
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Social: Cicero.Service.Models.Core.Widgets
    {

        public string Title { get; set; }
        public List<string> Url { get; set; }
        public List<string> Icon { get; set; }
        public string Style { get; set; } 
        public string LayoutType { get; set; } 
        public string Target { get; set; } 
        public string BorderRadius { get; set; } 
        public string ButtonColor { get; set; } 
        public string ButtonHoverColor { get; set; } 
        public string ButtonBGColor { get; set; } 
        public string ButtonBGHoverColor { get; set; }
        public WidgetResponse OnUpdate(Social _new, Social _old)
        {
            return this.Update<Social>(_new) as WidgetResponse;
        }

    }
 
}
