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

namespace Cicero.Service.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AppSetting
    {
        private ApplicationDbContext db;
        private readonly Utils _utils;
        private readonly ICommonService _commonService;

        public AppSetting(ApplicationDbContext adb, Utils utils, ICommonService commonService)
        {
            db = adb;
            _utils = utils;
            _commonService = commonService;
        }

        public string Get(string _field_key, string _default = "")
        {
            int tenantid = 0;
            try
            {
                tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            }
            catch (Exception)
            {
                tenantid = 14;
            }

            //static tenancy set logic must be changed
            if (tenantid != 0)
            {
                var _setting = db.Setting.FirstOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid);
                var value = _default;
                if (_setting != null)
                {
                    value = _setting.FieldValue;
                }
                return value;
            }

            return null;

        }
        public string Update(string _field_key, string _field_value)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            Setting _setting = db.Setting.SingleOrDefault(x => x.FieldKey == _field_key && x.TenantId == tenantid);

            if (_setting != null)
            {
                _setting.FieldValue = _field_value;
                db.Update(_setting);
                db.SaveChanges();
            }
            else
            {
                var sett = new Setting();
                sett.FieldKey = _field_key;
                sett.FieldValue = _field_value;
                sett.FieldDisplay = "Navigation - " + _field_key;
                sett.FieldType = "TEXTBOX";
                sett.FieldVisiblity = 0;
                sett.TenantId = tenantid;
                db.Setting.Add(sett);
                db.SaveChanges();
            }
            return _field_value;
        }
        public List<NavigationJsonItems> getMenuByLocation(string e)
        {
            var str = Get(e);
            if(string.IsNullOrEmpty(str)){
                str = "[]"; 
            }
            return JsonConvert.DeserializeObject<List<NavigationJsonItems>>(str);

        }
		
    }
}
