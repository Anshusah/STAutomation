using System;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Cicero.Areas.Identity.IdentityHostingStartup))]
namespace Cicero.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                //services.AddDefaultIdentity<ApplicationUser>(config =>
                //{
                //    //email confimed for login
                //    //config.SignIn.RequireConfirmedEmail = true;
                //}).AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


                services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
                    {
                        config.SignIn.RequireConfirmedEmail = true;
                    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            });
        }
    }
}