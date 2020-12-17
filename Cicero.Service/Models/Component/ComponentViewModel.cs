using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Component
{
   public class ComponentViewModel
    {
        public int Id { get; set; }

        public string FieldKey { get; set; }

        public string FieldValue { get; set; }

        public string FieldDisplay { get; set; }

        public int FieldVisiblity { get; set; }

        public string ComponentType { get; set; }

        public string FieldOptions { get; set; }

        public string FieldGridSize { get; set; }

        public int TenantId { get; set; }
    }
}
