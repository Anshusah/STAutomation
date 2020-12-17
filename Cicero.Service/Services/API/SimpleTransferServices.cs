using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Services.API
{
    public interface ISimpleTransferService
    {
        LoginResponse GetUserToken(LoginRequest request);
        void CreateOrUpdate(ExchangeRatesHistory exchangeRatesHistory);
        void CreateOrUpdate(ExchangeRates exchangeRates);
        bool ValidateUser(SetSafkhanRateRequest request);
        bool ValidateToken(string token);

    }
    public class SimpleTransferService : ISimpleTransferService
    {
        private readonly SimpleTransferApplicationDbContext db;
        DateTime utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private string secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public IConfiguration Configuration { get; }
        private double expirationMinute = 60;

        public SimpleTransferService(SimpleTransferApplicationDbContext db,IConfiguration configuration)
        {
            this.db = db;
            this.Configuration = configuration;
        }

        public void CreateOrUpdate(ExchangeRates exchangeRates)
        {
            var exchangeRate = db.ExchangeRates.Where(x => x.Source == exchangeRates.Source && x.FromCurrencyCode == exchangeRates.FromCurrencyCode && x.ToCurrencyCode == exchangeRates.ToCurrencyCode).FirstOrDefault();
            if (exchangeRate != null)
            {
                exchangeRate.ExchangeRate = exchangeRates.ExchangeRate;
                exchangeRate.UpdatedOn = DateTime.Now;
                db.Update(exchangeRate);
                db.SaveChangesAsync();
            }
            db.ExchangeRates.Add(exchangeRates);
            db.SaveChangesAsync();
            
        }

        public void CreateOrUpdate(ExchangeRatesHistory exchangeRatesHistory)
        {
            db.ExchangeRatesHistory.Add(exchangeRatesHistory);
            db.SaveChangesAsync();
        }

        public LoginResponse GetUserToken(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            var user = db.PaymentApiPartner.Where(x => x.Username == request.Username &&
              x.Password == request.Password && x.SystemId == request.SystemId).FirstOrDefault();
            if (user != null) {
                response = GenerateToken(request.Username, "",request);
                var apiUser = new ApiUserToken()
                {
                    UserTokenId=0,
                    Token = response.AccessToken,
                    UserId = request.SystemId,
                    Status = true,
                    TokenCreatedDate = Convert.ToDateTime(response.IssueDate),
                    TokenModifiedDate = Convert.ToDateTime(response.IssueDate),
                    TokenExpiryDatetime = Convert.ToDateTime(response.ExpiryDate)
                };
                var previousTokens = db.ApiUserToken.Where(x => x.UserId == request.SystemId).ToList();

                db.ApiUserToken.Add(apiUser);
                previousTokens.ForEach(x => x.Status = false);
                previousTokens.ForEach(x => x.TokenModifiedDate = Convert.ToDateTime(response.IssueDate));
                db.SaveChangesAsync();
            }
            else {
                response.ErrorCode = (int)System.Net.HttpStatusCode.Unauthorized;
                response.ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_CREDENTIAL.ToDescription();
            }
            return response;
        }
        private LoginResponse GenerateToken(string username, string securityStamp, LoginRequest request)
        {
            LoginResponse response = new LoginResponse();

            DateTime issueTime = DateTime.UtcNow;

            var iat = (string)issueTime.Subtract(utc0).TotalSeconds.ToString();
            //Add Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, issueTime.AddMinutes(expirationMinute).ToUnixTime().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, issueTime.ToUnixTime().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); //Secret
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["SimpleTransfer:Issuer"],
                Configuration["SimpleTransfer:Audience"],
                claims,
                expires: issueTime.AddMinutes(expirationMinute),
                signingCredentials: creds);

            response.AccessToken= new JwtSecurityTokenHandler().WriteToken(token);
            response.ExpiryDate = DateTime.Now.AddMinutes(60).ToString();
            response.IssueDate = DateTime.Now.ToString();
            response.UserName = username;
            return response;
        }
   
        public bool ValidateUser(SetSafkhanRateRequest request)
        {
            return true;
        }
       
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public bool ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); //Secret
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidAudience = Configuration["SimpleTransfer:Audience"],
                ValidIssuer = Configuration["SimpleTransfer:Issuer"],
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireSignedTokens = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validationParameters, out var rawValidatedToken);
                if (claimsPrincipal.Identity.IsAuthenticated)
                {
                    var user = db.ApiUserToken.Where(x => x.Token == token && x.Status).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.TokenExpiryDatetime >= DateTime.Now)
                        {
                            return true;
                        }
                        else
                        {
                            throw new Exception(Enumerations.EnumValidationFailureError.TOKEN_EXPIRED.ToDescription());
                        }
                    }
                    throw new Exception(Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription());
                }
                else
                {
                    throw new Exception(Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription());
                    return false;
                }
            }
            catch
            {
                throw new Exception(Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription());
            }
        }
    } 
    
}
