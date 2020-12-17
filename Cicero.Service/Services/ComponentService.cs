using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Cicero.Service.Helpers;

namespace Cicero.Service.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ComponentService
    {
        private ApplicationDbContext db;
        private readonly Utils _utils;
        private readonly ICommonService _commonService;

        public ComponentService(ApplicationDbContext adb, Utils utils, ICommonService commonService)
        {
            db = adb;
            _utils = utils;
            _commonService = commonService;
        }

        public string Get(string _field_key, string _default = "")
        {

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            //static tenancy set logic must be changed
            if (tenantid != 0)
            {
                var _component = db.Component.FirstOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid);
                var value = _default;
                if (_component != null)
                {
                    value = _component.FieldValue;
                }
                return value;
            }

            return null;

        }
        public string Get(string _field_key, int _formId, string _elementId, int _eventId, string _default ="")
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            //static tenancy set logic must be changed
            if (tenantid != 0)
            {
                var _component = db.ElementComponent.FirstOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid && x.FormId == _formId && x.ElementId == _elementId && x.EventType == _eventId);
                var value = _default;
                if (_component != null)
                {
                    value = _component.FieldValue;
                }
                return value;
            }

            return null;
        }

        public string Update(string _field_key,string _field_value, int _formId, string _elementId, int _eventType)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            ElementComponent component = db.ElementComponent.SingleOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid && x.FormId == _formId && x.ElementId == _elementId && x.EventType == _eventType);

            if (component != null)
            {
                component.FieldValue = _field_value;
                db.SaveChanges();
            }
            else
            {
                var comp = new ElementComponent();
                comp.FieldKey = _field_key;
                comp.FieldValue = _field_value;
                comp.TenantId = tenantid;
                comp.EventType = _eventType;
                comp.FormId = _formId;
                comp.ElementId = _elementId;
                db.ElementComponent.Add(comp);
                db.SaveChanges();
            }
            return _field_value;
        }

        public string Update(string _field_key, string _field_value)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            Component component = db.Component.SingleOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid);

            if (component != null)
            {
                component.FieldValue = _field_value;
                db.SaveChanges();
            }
            else
            {
                var comp = new Component();
                comp.FieldKey = _field_key;
                comp.FieldValue = _field_value;
                comp.FieldDisplay = "Component" + _field_key;
                comp.ComponentType = "";
                comp.FieldVisiblity = 0;
                comp.TenantId = tenantid;
                db.Component.Add(comp);
                db.SaveChanges();
            }
            return _field_value;
        }

       
    }
}
