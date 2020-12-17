using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Component
{
    class SyncComponentViewModel
    {
        public SyncComponentViewModel()
        {
            Configs = new List<Configs>();
        }
        public List<Configs> Configs { get; set; }

    }

    public class Configs
    {
        //public string enable { get; set; }

        public string TypeSource { get; set; }

        public string Tenant { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public List<string> SourceField { get; set; }

        public List<SourceData> SourceData { get; set; }

        public List<string> DestinationField { get; set; }

        public List<DestinationData> DestinationData { get; set; }

        public string Pull { get; set; }

        public string Pass { get; set; }

        public string Fail { get; set; }

        public List<string> PolicyField { get; set; }

        public string Process { get; set; }
    }

    public class SourceData
    {
        public string STable { get; set; }
        public string SField { get; set; }
    }

    public class DestinationData
    {
        public string DTable { get; set; }
        public string DField { get; set; }
    }

}


