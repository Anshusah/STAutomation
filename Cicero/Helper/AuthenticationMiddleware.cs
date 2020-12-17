using Cicero.Data;
using Cicero.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Helper
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private string secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        private readonly SimpleTransferApplicationDbContext db;
        public IConfiguration Configuration { get; }


        public AuthenticationMiddleware(RequestDelegate next, SimpleTransferApplicationDbContext db, IConfiguration configuration)
        {
            _next = next;
            this.db = db;
            this.Configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["AuthToken"];
            if (authHeader != null)
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
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                try
                {
                    var claimsPrincipal = new JwtSecurityTokenHandler()
                        .ValidateToken(authHeader, validationParameters, out var rawValidatedToken);
                    if (claimsPrincipal.Identity.IsAuthenticated)
                    {
                        var user = db.ApiUserToken.Where(x => x.Token == authHeader).FirstOrDefault();
                        if (user != null)
                        {
                            if (user.TokenExpiryDatetime >= DateTime.Now)
                            {
                                await _next.Invoke(context);
                            }
                            else
                            {
                                context.Response.StatusCode = 401; //Unauthorized
                                return;
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401; //Unauthorized
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = 401; //Unauthorized
                    return;
                }
            }
            else{
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }          
           
        }
    }
}
