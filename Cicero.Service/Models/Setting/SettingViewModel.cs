using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{
    public class SettingViewModel
    {
        public int Id { get; set; }

        public string FieldKey { get; set; }

        public string FieldValue { get; set; }

        public string FieldDisplay { get; set; }

        public int FieldVisiblity { get; set; }

        public string FieldType { get; set; }

        public string FieldOptions { get; set; }

        public string FieldGridSize { get; set; }

        public int TenantId { get; set; }

    }
    public class UatSettingViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
    }
}
