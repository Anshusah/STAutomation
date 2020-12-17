using Cicero.Service.Models.Core;
using System;
using System.Collections.Generic;

namespace Themes.Core.Widgets
{

    public class CustomArticle: Cicero.Service.Models.Core.Widgets
    {

        public string Title { get; set; }
        public List<string> ArticleId { get; set; }
        public List<string> ArticleTitle { get; set; }
        public List<string> ShortDescription { get; set; }
        public List<string> AnchorLink { get; set; }
        public List<string> ArticleImage { get; set; }
        public List<string> ArticleImageUrl { get; set; }

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

        public WidgetResponse OnUpdate(CustomArticle _new, CustomArticle _old)
        {
            return this.Update<CustomArticle>(_new) as WidgetResponse;
        }
    }

}
