using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.User
{
    class UserDataModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string image { get; set; }
        public string updated_at { get; set; }
        public string role { get; set; }
        public string side { get; set; }
        public string action { get; set; }
        public string roleId { get; set; }
    }
}
