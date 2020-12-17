using Cicero.Service.Helpers;
using Cicero.Service.Models;
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

namespace Cicero.Service.Services
{
    public interface ISwimLineService
    {
        List<CaseFormListViewModel> GetCaseFromList(bool isActiveOnly);
        List<CaseViewModel> GetCasesByFormId(string formId);
        dynamic GetCasesByCaseFormId(string formId, string searchText);
        bool UpdatePosition(int newPosition, int oldPosition, int sourceId, int targetId, int claimId, string formId);
        bool CheckPermission(string claimId);
    }
    public class SwimLineService : ISwimLineService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TenantConfig> _log;
        private readonly ICommonService _commonService;
        private readonly Utils _utils;
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;
        private readonly IRoleService _roleService;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IQueueService _queueService;
        private readonly IMediaService _mediaService;
        private readonly ITenantConfig _tenantConfigService;
        private readonly ICaseService _caseService;
        private readonly IMapper _mapper;

        public SwimLineService(ApplicationDbContext db, IMapper mapper, ICaseService caseService, ITenantConfig tenantConfigService, IMediaService mediaService, IQueueService queueService, ILogger<TenantConfig> log, ICommonService commonService, Utils utils, IUserService userService, ITenantService tenantService, IRoleService roleService, IFormBuilderService formBuilderService)
        {
            _db = db;
            _log = log;
            _utils = utils;
            _userService = userService;
            _tenantService = tenantService;
            _caseService = caseService;
            _roleService = roleService;
            _formBuilderService = formBuilderService;
            _commonService = commonService;
            _queueService = queueService;
            _mediaService = mediaService;
            _tenantConfigService = tenantConfigService;
            _mapper = mapper;
        }

        public List<CaseFormListViewModel> GetCaseFromList(bool isActiveOnly)
        {
            return _tenantConfigService.GetTenantForm(isActiveOnly);
        }

        public List<CaseViewModel> GetCasesByFormId(string formId)
        {
            return _caseService.GetListofCasesByFormId(_utils.DecryptId(formId));
        }

        public dynamic GetCasesByCaseFormId(string formId, string searchText)
        {
            return _caseService.GetListOfCasesByStateId(_utils.DecryptId(formId), searchText);
        }

        private int GetPosition(int newPos, int oldPos)
        {

            if (newPos > oldPos)
            {
                int min = newPos * 10;
                int max = (newPos + 1) * 10;
                return ((max + min) / 2);
            }
            else
            {
                int min = newPos * 10;
                int max = (newPos - 1) * 10;
                return ((max + min) / 2);

            }

        }
        public bool UpdatePosition(int newPosition, int oldPosition, int sourceId, int targetId, int claimId, string formId)
        {
            bool success = false;
            if (targetId == sourceId)
            {
                List<CaseViewModel> caseList = _caseService.GetListByStateId(targetId, _utils.DecryptId(formId));
                //caseList.ForEach(c => c.Order = c.Order * 10);
                //caseList.Where(x => x.Id == claimId).SingleOrDefault().Order = GetPosition(newPosition, oldPosition);
                //success = UpdateClaims(ChangeOrder(caseList.OrderBy(x => x.Order).ToList()));
                //_caseService.CreateOrUpdate(caseList.Where(x => x.Id == claimId).SingleOrDefault());
            }
            else
            {
                try
                {
                    CaseViewModel cs = _caseService.GetCaseById(claimId);
                    //if (cs.CaseTasks.Exists(x => x.Id == targetId))
                    //{
                    //    List<CaseViewModel> caseListSource = _caseService.GetListByStateId(sourceId, _utils.DecryptId(formId));
                    //    CaseViewModel caseView = new CaseViewModel();
                    //    List<CaseViewModel> caseListTarget = _caseService.GetListByStateId(targetId, _utils.DecryptId(formId));
                    //    caseView = caseListSource.Where(x => x.Id == claimId).FirstOrDefault();
                    //    caseListSource.Remove(caseView);
                    //    caseListTarget.ForEach(c => c.Order = c.Order * 10);
                    //    caseView.Order = GetPosition(newPosition, oldPosition);
                    //    caseView.StateId = targetId;
                    //    caseListTarget.Add(caseView);
                    //    caseListTarget = ChangeOrder(caseListTarget.OrderBy(x => x.Order).ToList());
                    //    caseListSource = ChangeOrder(caseListSource.OrderBy(x => x.Order).ToList());
                    //    caseListSource = caseListSource.Union(caseListTarget).ToList();
                    //    success = UpdateClaims(caseListSource);
                    //    //_caseService.CreateOrUpdate(caseView);
                    //}
                    //else
                    //{
                    //    success = false;
                    //}
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }
            return success;
        }


        private bool UpdateClaims(List<CaseViewModel> caseList)
        {
            try
            {
                return _caseService.UpdateCase(caseList);
            }
            catch
            {
                return false;
            }

        }
        private List<CaseViewModel> ChangeOrder(List<CaseViewModel> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                //list[i].Order = i + 1;
            }
            return list;
        }
        public bool CheckPermission(string claimId)
        {
            bool hasPermission = false;
            try
            {
                CaseViewModel cs = _caseService.GetCaseById(_utils.DecryptId(claimId));
                if (cs.Id != 0)
                {
                    hasPermission = true;
                }
            } catch(Exception ex) {}
            return hasPermission;
        }
    }
}
