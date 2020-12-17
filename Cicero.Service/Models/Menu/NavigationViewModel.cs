using System.Collections;
using System;
using System.Collections.Generic;
using Cicero.Data.Entities;

namespace Cicero.Service.Models
{
    public class NavigationViewModel
    {
        public IEnumerable<Article> Article { get; set; }
        public ArrayList Locations { get; set; }
        public string SelectedLocation { get; set; }
        public string ExistingMenusEncoded { get; set; }
        public List<NavigationJsonItems> ExistingMenusDecoded { get; set; }
    }
    public class NavigationJson
    {
        public List<NavigationJsonItems> JsonItem;
    }

    public class NavigationJsonItems
    {
        public string index;
        public string menu;
        public string desc;
        public string type;
        public string url;
        public string css_class;
        public string url_title;
        public string target;
        public List<NavigationJsonItems> childrens;


    }
    public class NavigationJsonItemsChildrens
    {
        public string index;
        public string menu;
        public string desc;
        public string type;
        public string url;
        public string css_class;
        public string url_title;
        public string target;
        public List<NavigationJsonItems> childrens;


    }
}
 