using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IFileUploaderService
    {
        Task<bool> SaveBankData(List<SupplierBank> data);
        Task<bool> SaveBankBranchData(List<SupplierBankBranch> data);
    }

    public class FileUploaderService : IFileUploaderService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IFileUploaderService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public FileUploaderService(SimpleTransferApplicationDbContext _db, ILogger<IFileUploaderService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public async Task<bool> SaveBankData(List<SupplierBank> data)
        {
            try
            {
                db.SupplierBank.AddRange(data);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveBankBranchData(List<SupplierBankBranch> data)
        {
            try
            {
                db.SupplierBankBranch.AddRange(data);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
