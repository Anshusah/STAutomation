using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{
   public class JsonWorkFlowPointsViewModel
    {
        public FirstPoint First { get; set; }

        public bool IsActive { get; set; } = true;

        public LastPoint Last { get; set; }

        public PointStyle Style { get; set; }

        public int CaseFormId { get; set; }

        public bool IsLock { get; set; } = false;

        public bool InPath { get; set; }

        public string Type { get; set; }

    }

    
    public class FirstPoint
    {

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public bool IsLock { get; set; } = true;

        public bool Aero { get; set; }

        public string LineCap { get; set; }

        public ActionObject AObject { get; set; }
    }

    public class LastPoint
    {

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public bool IsLock { get; set; } = true;

        public bool Aero { get; set; }

        public string LineCap { get; set; }

        public ActionObject AObject { get; set; }
    }

    public class PointStyle
    {
        public string Color { get; set; }

        public string HoverColor { get; set; }
    }

    public class ActionObject
    {
        public string XAxis { get; set; }

        public string YAxis { get; set; }

        public int TypeId { get; set; }

        public int Id { get; set; }

        public string Type { get; set; }

        public string Lock { get; set; }

    }
}

