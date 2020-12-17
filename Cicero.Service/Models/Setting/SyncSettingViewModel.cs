using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{
    public class SyncSettingViewModel
    {
        public SyncSettingViewModel()
        {
            configs = new List<Configs>();
        }
        public List<Configs> configs { get; set; }

    }

    public class Configs
    {
        //public string enable { get; set; }

        public string typesource { get; set; }

        public string tenant { get; set; }

        public string source { get; set; }

        public string destination { get; set; }

        public List<string> sourcefield { get; set; }

        //public List<SourceField> sourcefield { get; set; }

        public List<string> destinationfield { get; set; }

        //public List<DestinationField> destinationfield { get; set; }

        public string pull { get; set; }

        public string pass { get; set; }

        public string fail { get; set; }

        public string policyfield { get; set; }

        public string process { get; set; }
    }

    public class SourceField
    {
        public string sourcefield { get; set; }
    }

    public class DestinationField
    {
        public string destinationfield { get; set; }
    }

}
