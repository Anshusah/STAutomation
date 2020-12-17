 
using Cicero.Service.Models.Core; 

namespace Themes.Core.Widgets
{
    public class Text: Cicero.Service.Models.Core.Widgets
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FormatContent { get; set; }
        public WidgetResponse OnUpdate(Text _new,Text _old)
        { 
             return Update<Text>(_new) as WidgetResponse; 
        }
    }
 
}
