using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Models.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using static Cicero.Service.Models.Core.FormBuilderViewModel.Form;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore.rel

namespace Cicero.Service.Services
{

    /// <summary>
    /// This Service is used to work on the table creation and datasave using SQL for dynamic Form and table builder
    /// Prakash Bhatta
    /// 2019/05/21
    /// </summary>
    /// 

    public interface ICiceroCoreFormService
    {
        bool CreateDymanicCoreForm(FormBuilderViewModel fbvm, int caseformId);
        bool DeleteDymanicCoreForm(  int caseformId);
        CoreCaseTable GetCoreCaseTableByName(string Name, int? TenantIds);
    }

    public class CiceroCoreFormService : ICiceroCoreFormService
    {
        private readonly Utils Utils;
        public ApplicationDbContext db = null;
        private readonly ICommonService commonService;
        private readonly ILogger<CommonService> _log;
        public CiceroCoreFormService(ICommonService ICs, Utils _utils, ILogger<CommonService> log, ApplicationDbContext _db)
        {

            commonService = ICs;
            db = _db;
            Utils = _utils;
            _log = log;
        }
        private string CreateTable(Table t)
        {
            String CaseCreateTable = string.Format(@"CREATE TABLE [dbo].[{0}](
	[Id] [int] IDENTITY(1,1) NOT NULL,	
	[CreatedAt] [datetime2](3) NOT NULL,
	[UpdatedAt] [datetime2](3) NOT NULL,	
	[Status] [bit] NOT NULL,
	[Extras] [nvarchar](max) NULL,	
	[UserId] [varchar](256) NULL,
	[TenantId] [int] NOT NULL,
	[CoreCaseTableId] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[CaseId] [nvarchar](max) NULL,
	[JsonExtras] [nvarchar](max) NULL,
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", t.Name);

            return CaseCreateTable;
        }
        private string DeleteTable(string t)
        {
            String DeleteTable = string.Format(@"Drop TABLE [dbo].[{0}]", t );

            return DeleteTable;
        }
        private string CreateJson()
        {
            return string.Empty;
        }
        public bool CreateDymanicCoreForm(FormBuilderViewModel fbvm,int caseformId)
        {
            try
            {
                if (fbvm.Forms.Tables != null)
                {
                    List<Table> TableList = fbvm.Forms.Tables;
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

                    foreach (Table t in TableList)
                    {

                        string jsonsstr = JsonConvert.SerializeObject(t, settings).Trim();
                        CoreCaseTable ObjCoreCase = new CoreCaseTable();
                        ObjCoreCase.Name = t.Name;
                        ObjCoreCase.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                        ObjCoreCase.UserId = commonService.getLoggedInUserId();
                        ObjCoreCase.Fields = jsonsstr;
                        ObjCoreCase.CaseFormId = caseformId;
                        CoreCaseTable CoreCaseTables = GetCoreCaseTableByCaseName(ObjCoreCase.Name, caseformId);
                        if (CoreCaseTables != null)
                        {
                            db.Update(CoreCaseTables);
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Add(ObjCoreCase);
                            string sqlQuery = CreateTable(t);
                            db.Database.ExecuteSqlCommand(sqlQuery);
                            db.SaveChanges();

                        }
                        // db.re
                        //db.Database.re
                    }
                }
                

            }

            catch (Exception ex)
            {

            }


            return true;
        }
        public bool DeleteDymanicCoreForm(  int caseformId)
        {
            try
            { 
                        List<CoreCaseTable> CoreCaseTables = db.CoreCaseTable
                              .Where(x => x.CaseFormId == caseformId).ToList();//GetCoreCaseTableByCaseName(ObjCoreCase.Name, caseformId);
                        if (CoreCaseTables != null)
                        {
                            foreach(CoreCaseTable cct in CoreCaseTables)
                            {
                                string sqlQuery = DeleteTable(cct.Name);
                                db.Database.ExecuteSqlCommand(sqlQuery);
                                db.SaveChanges();
                                db.Remove(cct);
                                db.SaveChanges();
                            }
                            
                        }
                        
            } 
            catch (Exception ex)
            {

            }


            return true;
        }
        public CoreCaseTable GetCoreCaseTableByName(string Name, int? TenantIds)
        {
            CoreCaseTable result = null;
            try
            {
                  result = db.CoreCaseTable
                                .Where(x => x.Name == Name && x.TenantId == TenantIds)
                                .FirstOrDefault();

                 
                
            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - GetTenantIdByIdentifier - " + ex);
                
            }
            return result;
        }
        public CoreCaseTable GetCoreCaseTableByCaseName(string Name, int  caseFormId)
        {
            CoreCaseTable result = null;
            try
            {
                result = db.CoreCaseTable
                              .Where(x => x.Name == Name && x.CaseFormId == caseFormId)
                              .FirstOrDefault();



            }
            catch (Exception ex)
            {
                _log.LogError("CommonServices - GetTenantIdByIdentifier - " + ex);

            }
            return result;
        }
    }
}
