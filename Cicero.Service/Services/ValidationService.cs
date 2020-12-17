using Cicero.Service.Helpers;

using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using AutoMapper;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Cicero.Service.Models.Core;
using Newtonsoft.Json.Linq;
using Cicero.Service.Models.Core.Elements;
using System.Text.RegularExpressions;

namespace Cicero.Service.Services
{
    public struct ElementValidation
    {
        public string ElementId { get; set; }
        public string ValidationMessage { get; set; }
        public bool IsValid { get; set; }
    }
    public class ValidationVM
    {
        public string FieldOperator { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ValidationValues { get; set; }
        public int Order { get; set; }
    }
    public struct Validations
    {
        public List<ElementValidation> ElementValidations { get; set; }
        public bool isFormValid { get; set; }
    }
    public interface IValidationService
    {
        List<ElementValidation> ValidateFormData(IFormCollection formData, string form, List<string> hiddenFormData, bool isFrontValidation);
        string ValidateElement(string type, string val, string checkOp, string opVal, string opMesg);
    }
    class ValidationService : IValidationService
    {
        private readonly IUserService _userService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly Utils _utils;
        private readonly ITenantService _tenantService;
        private readonly AppSetting _appSetting;
        private readonly ICommonService _ICommonService;
        private readonly IFormService _formService;
        private readonly IQueueService _queueService;
        private readonly ICaseService _caseService;
        public ValidationService(IFormService IFormService, ICaseService caseService, IQueueService queueService, ICommonService ics, IUserService userService, AppSetting appSetting, IFormBuilderService formBuilderService, Utils utils, ITenantService tenantService)
        {
            _userService = userService;
            _formBuilderService = formBuilderService;
            _utils = utils;
            _tenantService = tenantService;
            _appSetting = appSetting;
            _ICommonService = ics;
            _formService = IFormService;
            _queueService = queueService;
            _caseService = caseService;
        }

        public List<ElementValidation> ValidateFormData(IFormCollection formData, string form, List<string> hiddenFormData, bool isFrontValidation)
        {
            string loggeduser = _ICommonService.getLoggedInUserId();

            List<string> hiddenData = new List<string>();
            foreach (var item in hiddenFormData)
            {
                string str = item.Split("[")[0];
                hiddenData.Add(str);
            }

            string roleid = _ICommonService.GetRoleIdByUserId(loggeduser);
            var isSuperAdmin = _ICommonService.IsSuperAdmin().Result;
            dynamic elem1;
            var data = formData.Where(x => x.Key.Contains("elm") || x.Key.Contains("Media")).ToList();
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            ccvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
            List<ElementValidation> elmValidations = new List<ElementValidation>();
            try
            {
                foreach(var elmKey in data)
                {
                    bool breakloop = false;
                    foreach (Tab TabBody in a.Tab)
                    {
                        if (breakloop) break;
                        bool checkElementsOnTab = (isFrontValidation) ? TabBody.FrontendVisible : TabBody.BackendVisible;
                        var checkPermission = new Element.Permission();

                        if (checkElementsOnTab)
                        {
                            bool checkTabByPermission = false;
                            if (roleid != " ") // if superadmin
                            {
                                if (TabBody.Permissions != null && TabBody.Permissions.Count > 0)
                                {
                                    foreach (var permission in TabBody.Permissions)
                                    {
                                        if (permission.RoleId == roleid)
                                        {
                                            if (permission.Write != false)
                                            {
                                                checkTabByPermission = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (isSuperAdmin || checkTabByPermission)
                            {
                                //    if (checkElementsOnTab)//check for tab visible
                                //{
                                foreach (Row RowBody in TabBody.Row)
                                {
                                    if (breakloop) break;
                                    bool checkElementsOnRow = (isFrontValidation) ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                    if (checkElementsOnRow) //check for row visible
                                    {
                                        foreach (Column ColumnBody in RowBody.Column)
                                        {
                                            if (breakloop) break;
                                            bool checkElementsOnCol = (isFrontValidation) ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                            if (checkElementsOnCol) //check for column visible
                                            {
                                                foreach (dynamic ElementBody in ColumnBody.Element)
                                                {
                                                    if (breakloop) break;
                                                    elem1 = ElementBody;
                                                    bool checkElement = (isFrontValidation) ? ElementBody.FrontendVisible : ElementBody.BackendVisible;

                                                    if (checkElement) //check for element visible
                                                    {
                                                        bool checkByPermission = false;
                                                        if (roleid != " ") // if superadmin
                                                        {
                                                            if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                            {
                                                                foreach (var permission in ElementBody.Permissions)
                                                                {
                                                                    if (permission.RoleId == roleid)
                                                                    {
                                                                        if (permission.Write != false)
                                                                        {
                                                                            checkByPermission = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (roleid == " " || checkByPermission == true) //check for permission
                                                        {
                                                            ElementValidation elmValidation = new ElementValidation()
                                                            {
                                                                IsValid = true
                                                            };

                                                            string elmId = string.Concat("elm", ElementBody.ElementId.ToString());
                                                            if (!hiddenData.Any(elmId.Contains))
                                                            {
                                                            
                                                                if(elmKey.Key.Contains(elmId))
                                                                {
                                                                    var elm = elmKey;
                                                                    breakloop = true;
                                                                    var elmIdVal = elm.Key.Substring(9);
                                                                    elmIdVal = elmIdVal.Substring(0, elmIdVal.Length - 1);
                                                                    switch (ElementBody.GetType().Name.ToLower())
                                                                    {
                                                                        case "textbox":
                                                                            elmValidation = ValidateData(ElementBody, Convert.ToString(elm.Value.ToString().Length), elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "number":
                                                                            elmValidation = ValidateData(ElementBody, elm.Value, elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "textarea":
                                                                            elmValidation = ValidateData(ElementBody, Convert.ToString(elm.Value.ToString().Length), elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "selectbox":
                                                                            elmValidation = ValidateData(ElementBody, elm.Value, elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "multiselectbox":
                                                                            elmValidation = ValidateData(ElementBody, elm.Value.Count.ToString(), elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "checkboxgroup":
                                                                            elmValidation = ValidateData(ElementBody, elm.Value.Count.ToString(), elm.Value);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                        case "radiogroup":
                                                                            string radioValue = "";
                                                                            if (((isFrontValidation == true && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("ONOFF")) || (isFrontValidation == false && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("ONOFF"))) && ElementBody.RadioOptions.Count == 2)
                                                                            {
                                                                                radioValue = "false";
                                                                            }
                                                                            else
                                                                            {
                                                                                radioValue = elm.Value;
                                                                            }
                                                                            elmValidation = ValidateData(ElementBody, radioValue, radioValue);
                                                                            elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);

                                                                            break;
                                                                        case "media":
                                                                            elmId = "";
                                                                            if (isFrontValidation)
                                                                            {
                                                                                elmId = ElementBody.FrontendId.ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                elmId = ElementBody.BackendId.ToString();
                                                                            }
                                                                            elm = data.Where(x => x.Key.Contains(elmId)).SingleOrDefault();
                                                                            elmValidation = ValidateData(ElementBody, elm.Value.Count.ToString(), elm.Value);
                                                                            elmValidation.ElementId = elmId;
                                                                            //elmValidation.ElementId = elmIdVal;
                                                                            elmValidations.Add(elmValidation);
                                                                            break;
                                                                    }
                                                                }
                                                               
                                                            }


                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {

            }
            return elmValidations;
        }

        private List<ValidationVM> ReOrderValidations(dynamic validations)
        {
            List<ValidationVM> orderedValidations = new List<ValidationVM>();
            foreach (var item in validations)
            {
                ValidationVM valid = new ValidationVM();
                valid.ErrorMessage = item.ErrorMessage;
                valid.FieldOperator = item.FieldOperator;
                valid.ValidationValues = item.ValidationValues;
                switch (item.FieldOperator)
                {
                    case "required":
                        valid.Order = 1;
                        break;
                    case ">":
                        valid.Order = 2;
                        break;
                    case "<":
                        valid.Order = 3;
                        break;
                    case "=":
                        valid.Order = 4;
                        break;
                    case "between":
                        valid.Order = 5;
                        break;
                    case "email":
                        valid.Order = 6;
                        break;
                    case "regex":
                        valid.Order = 7;
                        break;
                }
                orderedValidations.Add(valid);
            }
            return (orderedValidations.OrderBy(x => x.Order).ToList());
        }

        public ElementValidation ValidateData(dynamic ElementBody, string value, string inputVal)
        {
            ElementValidation temp = new ElementValidation();
            bool breakloop = false;
            try
            {
                if (ElementBody.GetType().GetProperty("Validations") != null && ElementBody.Validations != null && ElementBody.Validations.Count > 0)
                {
                    List<ValidationVM> elmValidations = ReOrderValidations(ElementBody.Validations);
                    foreach (var valid in elmValidations)
                    {
                        var last = string.Empty;
                        var first = string.Empty;
                        var error = valid?.ErrorMessage;
                        if (valid?.ValidationValues?.Count > 0)
                        {
                            first = valid?.ValidationValues[0];
                        }
                        if (valid?.ValidationValues?.Count > 1)
                        {
                            last = valid?.ValidationValues[1];
                        }
                        string elementType = ElementBody.GetType().Name.ToLower();

                        switch (valid.FieldOperator)
                        {
                            case "required":
                                if (first != null && first != "")
                                {
                                    if (first == "true")
                                    {
                                        if (value == string.Empty || value == "" || inputVal == null || inputVal == string.Empty || inputVal == "")
                                        {
                                            breakloop = true;
                                        }
                                    }
                                }
                                break;
                            case ">":
                                if ((first != null && first != string.Empty && first != ""))
                                {
                                    if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                    {
                                        if ((Convert.ToDecimal(value) > Convert.ToDecimal(first)))
                                        {
                                            breakloop = true;
                                        }
                                    }
                                }
                                break;
                            case "<":
                                if ((first != null && first != string.Empty && first != ""))
                                {
                                    if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                    {
                                        if ((Convert.ToDecimal(value) < Convert.ToDecimal(first)))
                                        {
                                            breakloop = true;
                                        }
                                    }

                                }
                                break;
                            case "=":
                                if ((first != null && first != string.Empty && first != ""))
                                {
                                    if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                    {
                                        if (Convert.ToDecimal(value) == Convert.ToDecimal(first))
                                        {
                                            breakloop = true;
                                        }
                                    }
                                }
                                break;
                            case "between":
                                if ((first != null && first != string.Empty && first != "") && (last != null && last != string.Empty && last != ""))
                                {
                                    if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                    {
                                        if (((Convert.ToDecimal(value) >= Convert.ToDecimal(first)) || (!(Convert.ToDecimal(value) <= Convert.ToDecimal(last)))))
                                        {
                                            breakloop = true;
                                        }
                                    }
                                }
                                break;
                            case "email":

                                if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                {
                                    bool isEmail = Regex.IsMatch(inputVal, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (!isEmail)
                                    {
                                        breakloop = true;
                                    }
                                }
                                break;
                            case "regex":
                                if ((first != null && first != string.Empty && first != ""))
                                {
                                    if (value != string.Empty && value != "" && inputVal != null && inputVal != string.Empty && inputVal != "")
                                    {
                                        bool isEmail = Regex.IsMatch(inputVal, first, RegexOptions.IgnoreCase);
                                        if (!isEmail)
                                        {
                                            breakloop = true;
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                        if (breakloop)
                        {
                            //temp = "{elm:'elm" + ElementBody.ElementId + "',msg:'" + error + "'}";
                            temp.ElementId = string.Concat("elm", ElementBody.ElementId.ToString());
                            temp.IsValid = false;
                            temp.ValidationMessage = (error != string.Empty && error != null && error != "") ? error : GenerateValidationMessage(ElementBody.GetType().Name.ToLower(), first, last, valid.FieldOperator);

                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (!breakloop)
            {
                temp.ElementId = string.Concat("elm", ElementBody.ElementId.ToString());
                temp.IsValid = true;
                temp.ValidationMessage = "";
            }

            return temp;
        }

        public string ValidateElement(string type, string val, string checkOp, string opVal, string opMesg)
        {
            string valid = "true";
            if (opVal != null)
            {
                var op = checkOp.Split(",,");
                int length;
                var opValue = opVal.Split(",,");
                var opMeg = opMesg.Split(",,");
                try
                {
                    switch (type)
                    {
                        case "selectbox":
                        case "radiogroup":
                            for (int i = 0; i < op.Length - 1; i++)
                            {
                                switch (op[i])
                                {
                                    case "required":
                                        if (opValue[i] == "true")
                                        {
                                            if (val == null || val == string.Empty)
                                            {

                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));

                                                i = op.Length;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;

                        case "checkboxgroup":
                        case "multiselectbox":
                            for (int i = 0; i < op.Length - 1; i++)
                            {
                                switch (op[i])
                                {
                                    case "required":
                                        if (opValue[i] == "true")
                                        {
                                            if (val == null || val == string.Empty)
                                            {
                                                if (Convert.ToInt64(val) < 1)
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case ">":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if ((Convert.ToInt64(val) > Convert.ToInt64(opValue[i])))
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case "<":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if ((Convert.ToInt64(val) < Convert.ToInt64(opValue[i])))
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case "=":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if (Convert.ToInt64(val) != Convert.ToInt64(opValue[i]))
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case "between":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            try
                                            {
                                                if (((Convert.ToInt64(val) >= Convert.ToInt64(opValue[i].Split("-")[0])) && (Convert.ToInt64(val) <= Convert.ToInt64(opValue[i].Split("-")[1]))))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i].Split("-")[0], opValue[i].Split("-")[1], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                        }
                                        break;
                                }
                            }
                            break;

                        case "number":
                            for (int i = 0; i < op.Length - 1; i++)
                            {
                                switch (op[i])
                                {
                                    case "required":
                                        if (val == "" || val == string.Empty || val == null)
                                        {
                                            if (opValue[i] == "true")
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case ">":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if (val != "" && val != string.Empty && val != null)
                                            {
                                                if ((Convert.ToDecimal(val) > Convert.ToDecimal(opValue[i])))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "<":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if (val != "" && val != string.Empty && val != null)
                                            {
                                                if ((Convert.ToDecimal(val) < Convert.ToDecimal(opValue[i])))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "=":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if (val != "" && val != string.Empty && val != null)
                                            {
                                                if (Convert.ToDecimal(val) == Convert.ToDecimal(opValue[i]))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "between":
                                        if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                        {
                                            if (val != "" && val != string.Empty && val != null)
                                            {
                                                try
                                                {
                                                    if (((Convert.ToDecimal(val) >= Convert.ToDecimal(opValue[i].Split("-")[0])) && (Convert.ToDecimal(val) <= Convert.ToDecimal(opValue[i].Split("-")[1]))))
                                                    {
                                                        valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i].Split("-")[0], opValue[i].Split("-")[1], op[i]));
                                                        i = op.Length;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                            }
                                        }
                                        break;
                                    case "regex":
                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                            {
                                                bool isRegex = Regex.IsMatch(val, opValue[i], RegexOptions.IgnoreCase);
                                                if (!isRegex)
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;

                                }
                            }
                            break;

                        case "textarea":
                        case "textbox":
                            if (val == string.Empty || val == null)
                            {
                                length = 0;
                            }
                            else
                            {
                                length = val.Length;
                            }
                            for (int i = 0; i < op.Length - 1; i++)
                            {
                                switch (op[i])
                                {
                                    case "required":
                                        if (opValue[i] == "true")
                                        {
                                            if (val == "" || val == string.Empty || val == null)
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case ">":

                                        if (val != "" && val != string.Empty && val != null)
                                        {

                                            if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                            {
                                                if ((length > Convert.ToInt16(opValue[i])))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "<":

                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                            {
                                                if ((length < Convert.ToInt16(opValue[i])))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "=":

                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            if (length ==
                                                Convert.ToInt16(opValue[i]))
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case "email":
                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            bool isEmail = Regex.IsMatch(val, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (!isEmail)
                                            {
                                                valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                i = op.Length;
                                            }
                                        }
                                        break;
                                    case "regex":
                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                            {
                                                bool isRegex = Regex.IsMatch(val, opValue[i], RegexOptions.IgnoreCase);
                                                if (!isRegex)
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i], opValue[i], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                    case "between":
                                        if (val != "" && val != string.Empty && val != null)
                                        {
                                            if ((opValue[i] != null && opValue[i] != string.Empty && opValue[i] != ""))
                                            {
                                                if ((length >= Convert.ToInt16(opValue[i].Split("-")[0])) && (length <= Convert.ToInt16(opValue[i].Split("-")[1])))
                                                {
                                                    valid = "false," + ((opMeg[i] != "") ? opMeg[i] : GenerateValidationMessage(type.ToLower(), opValue[i].Split("-")[0], opValue[i].Split("-")[1], op[i]));
                                                    i = op.Length;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return valid;
        }

        private string GenerateValidationMessage(string type, string checkVal1, string checkVal2, string checkOperator)
        {
            string message = string.Empty;
            switch (type)
            {

                case "selectbox":
                case "radiogroup":
                    switch (checkOperator)
                    {
                        case "required":
                            message = "This Field is required.";
                            break;
                    }
                    break;

                case "checkboxgroup":
                case "multiselectbox":
                    switch (checkOperator)
                    {
                        case "required":
                            message = "This Field is required.";
                            break;
                        case ">":
                            message = "Please select less than " + checkVal1 + " options.";
                            break;
                        case "<":
                            message = "Please select greater than " + checkVal1 + " options.";
                            break;
                        case "=":
                            message = "Please do not select " + checkVal1 + " options.";
                            break;
                        case "between":
                            message = "Please do not select " + checkVal1 + " to " + checkVal2 + " options.";
                            break;
                    }

                    break;
                case "number":

                    switch (checkOperator)
                    {
                        case "required":
                            message = "This Field is required.";
                            break;
                        case ">":
                            message = "Please enter number less than " + checkVal1 + " .";
                            break;
                        case "<":
                            message = "Please enter number greater than " + checkVal1 + " .";
                            break;
                        case "=":
                            message = "Please enter number not equal to " + checkVal1 + " .";
                            break;
                        case "between":
                            message = "Please do not enter number between " + checkVal1 + " and " + checkVal2 + " .";
                            break;
                    }
                    break;
                case "media":
                    switch (checkOperator)
                    {
                        case "required":
                            message = "This Field is required.";
                            break;
                        case ">":
                            message = "Please select less than " + checkVal1 + " media.";
                            break;
                        case "<":
                            message = "Please select greater than " + checkVal1 + " media.";
                            break;
                        case "=":
                            message = "Please do not select " + checkVal1 + " media.";
                            break;
                        case "between":
                            message = "Please do not select " + checkVal1 + " to " + checkVal2 + " media.";
                            break;
                        case "regex":
                            message = "This Field is invalid.";
                            break;
                    }
                    break;
                case "textarea":
                case "textbox":
                    switch (checkOperator)
                    {
                        case "required":
                            message = "This Field is required.";
                            break;
                        case ">":
                            message = "Please enter less than " + checkVal1 + " characters.";
                            break;
                        case "<":
                            message = "Please enter greater than " + checkVal1 + " characters.";
                            break;
                        case "=":
                            message = "Please do not enter " + checkVal1 + " characters.";
                            break;
                        case "between":
                            message = "Please do not enter " + checkVal1 + " to " + checkVal2 + " characters.";
                            break;
                        case "email":
                            message = "Please enter valid email address";
                            break;
                        case "regex":
                            message = "This Field is invalid.";
                            break;
                        
                    }
                    break;

            }
            return message;
        }

    }
}
