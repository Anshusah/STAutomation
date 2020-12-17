
using Cicero.Service.Models.Core;
using System.Collections.Generic; 

namespace Themes.Core.Widgets
{

    public class FeatureArticle: Cicero.Service.Models.Core.Widgets
    {

        public string Title { get; set; }
        public List<string> Article { get; set; }
        public string Style { get; set; }
        public string LayoutType { get; set; }
        public string DisplayAs { get; set; }
        public string CustomStyleArticle { get; set; }
        public string CustomStyleBanner { get; set; }
        public string Navigation { get; set; }
        public string Size { get; set; }
        public string Align { get; set; }
        public string Infinite { get; set; }
        public string SlideToShow { get; set; }
        public string SlideToScroll { get; set; }
        public string Speed { get; set; }
        public WidgetResponse OnUpdate(FeatureArticle _new, FeatureArticle _old)
        {
            return this.Update<FeatureArticle>(_new) as WidgetResponse;
        }
    }

}
