using System;
using System.Collections.Generic; 
using Cicero.Service.Models.Core;
using Microsoft.AspNetCore.Mvc;

namespace Themes.Core.Components
{
    public class PolicySystem:  Component
    {
        public PolicySystem()
        {
            configs = new List<PolicyConfigs>();
        }
        public List<PolicyConfigs> configs { get; set; }
        public string Title { get; set; }
        public ComponentResponse OnUpdate(PolicySystem _new, PolicySystem _old)
        {
            return Update<PolicySystem>(_new) as ComponentResponse;
        }
        public ComponentResponse OnUpdateElm(PolicySystem _new, PolicySystem _old, int formId, string elementId, int eventType)
        {
            return Update<PolicySystem>(_new,eventType,elementId,formId) as ComponentResponse;
        }
    }
    public class PolicyConfigs
    {
        //public string enable { get; set; }

        public string typesource { get; set; }

        public string tenant { get; set; }

        public string source { get; set; }

        public string destination { get; set; }

        public List<string> sourcefield { get; set; }

        public List<string> sourcetable { get; set; }

        public List<string> destinationfield { get; set; }

        public List<string> destinationtable { get; set; }

        public string pull { get; set; }

        public string pass { get; set; }

        public string fail { get; set; }

        public List<string> sourcepolicyfieldtable { get; set; }
        public List<string> sourcepolicyfield { get; set; }
        public List<string> destpolicyfieldtable { get; set; }
        public List<string> destpolicyfield { get; set; }
        public string policyConCheck { get; set; }
        public List<string> destFormElm { get; set; }
        public List<string> destFormElmValue { get; set; }
        //check unique
        public List<string> sourceuniquetable { get; set; }
        public List<string> sourceuniquefield { get; set; }
        public List<string> destuniquetable { get; set; }
        public List<string> destuniquefield { get; set; }

        //mapping
        public List<string> sourcemappingtable { get; set; }
        public List<string> sourcemappingfield { get; set; }
        public List<string> destmappingtable { get; set; }
        public List<string> destmappingfield { get; set; }


        public List<string> synccondition { get; set; }
        

        public string ApiUrl { get; set; }
        public bool isGetMethod { get; set; }
        public List<string> ApiKeyList { get; set; }
        public List<string> ApiValueList { get; set; }
        public List<string> ApiParameterList { get; set; }
        public List<string> ApiParameterDefaultValue { get; set; }
        public List<string> ApiParameterFormatList { get; set; }
        public List<string> ApiParameterSourceTable { get; set; }
        public List<string> ApiParameterSourceField { get; set; }
        public List<string> ApiResponseList { get; set; }
        public List<string> ApiResponseFormatList { get; set; }
        public List<string> ApiResponseDestTable { get; set; }
        public List<string> ApiResponseDestField { get; set; }
        public List<string> ApiParameterElementName { get; set; }
        public List<string> ApiParameterElement { get; set; }
        public List<string> ApiResponseElementName { get; set; }
        public List<string> ApiResponseElement { get; set; }
        public List<string> ApiParameterElementDefaultValue { get; set; }
        public bool IsApiParResFromElement { get; set; }
        public bool RequireAuthentication { get; set; }
        public string GetChecked(bool? checkValue)
        {
            if (checkValue == true)
                return " checked=" + checkValue;
            return "";
        }
    }


}
