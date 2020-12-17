using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Cicero.Service.Models;
using Cicero.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Cicero.Data.Entities;
using Cicero.Data;
using Cicero.Service.Helpers;
using System.Security.Policy;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Cicero.Service.Models.SimpleTransfer;
using System.Runtime.Serialization;
using Cicero.Data.Extensions;
using Cicero.Data.Entities.SimpleTransfer;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ISecureTradingService
    {
        Task<SecureTradingPaymentDetail> GetData(string requestReference);
        string GenerateJSONWebToken(string secret, string username,Payload payload);
        void SavePaymentResponseDetail(SecureTradingPaymentDetail secureTradingPaymentDetail);
        void UpdatePaymentDetailToRefund(string txnRef);

    }
    public class SecureTradingService : ISecureTradingService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<ISecureTradingService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public SecureTradingService(SimpleTransferApplicationDbContext _db, Utils _Utils, ILogger<ISecureTradingService> _Log, IMapper _mapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _Utils;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public string GenerateJSONWebToken(string secret,string username,Payload payload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;
            var jwtPayload = CreateJWTPayload(payload, username);
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(credentials);
            var secToken = new JwtSecurityToken(header, jwtPayload);
            var tokenString = tokenHandler.WriteToken(secToken);
            return tokenString;
        }
        public JwtPayload CreateJWTPayload(Payload payload, string username)
        {
            JwtPayload jwtPayload = new JwtPayload {
                {"payload",new Dictionary<string, string> {
                    { "accounttypedescription", payload.accounttypedescription },
                    { "baseamount", payload.baseamount },
                    {"currencyiso3a",payload.currencyiso3a },
                    {"sitereference",payload.sitereference }
                }
                },
                {"iat",(long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds},
                {"iss",username }
            };
            return jwtPayload;
        }

        public void SavePaymentResponseDetail(SecureTradingPaymentDetail secureTradingPaymentDetail)
        {
            db.SecureTradingPaymentDetail.Add(secureTradingPaymentDetail);
            db.SaveChanges();
        }
        public void UpdatePaymentDetailToRefund(string txnRef)
        {
            var item=db.SecureTradingPaymentDetail.Where(x=>x.transactionreference==txnRef).FirstOrDefault();
            item.isRefund = true;
            db.Update(item);
            db.SaveChanges();
        }

        public async Task<SecureTradingPaymentDetail> GetData(string requestReference)
        {
            var data = await db.SecureTradingPaymentDetail.Where(x => x.requestreference == requestReference).FirstOrDefaultAsync();
            return Mapper.Map<SecureTradingPaymentDetail>(data);
        }
    }

}
