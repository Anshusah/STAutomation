using System; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cicero.Data;
using Cicero.Service.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Cicero.Data.Entities;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure; 
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Configuration;
using Cicero.Service.Services.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cicero.Service.Services.SimpleTransfer;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Linq;
using AutoMapper;

namespace Cicero
{
    public class Startup
    {
        private ILexisNexisService lexisNexisService;
        private ICaseService caseService;
        private IFormService formService;
        private Utils utils;
        private IUserService userService;
        private IWorkflowService workflowService;
        private IMapperService mapperService;
        private IExchangeRateServices exchangeRateServices;
        private IBankBranchServcie branchServcie;
        private ICityService cityService;
        private ITransactionMgmtService transactionMgmtService;
        private ICountryService countryService;
        private ICustomerService customerService;
        private IMapper mapper;
        private SimpleTransferApplicationDbContext db;
        private ISourceOfFundSetupService sourceOfFundSetupService;
        private IPaymentPurposeSetupService paymentPurposeSetupService;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            //services.ConfigureNonBreakingSameSiteCookies();

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax; // By default this is set to 'Strict'.
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(30);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<SimpleTransferApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthorization(options =>
            {

                options.AddPolicy("FrontOffice", policy => policy.RequireClaim("Side", "frontend"));
                options.AddPolicy("BackOffice", policy => policy.RequireAssertion(x => x.User.HasClaim(y => (y.Type == "access" && y.Value == "sa") || (y.Type == "Side" && y.Value == "backend"))));

            });


            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10000);
                options.Cookie.HttpOnly = true;
            });

            //services.AddDefaultIdentity<ApplicationUser>(config =>
            //{
            //    //email confimed for login
            //    //config.SignIn.RequireConfirmedEmail = true;
            //}).AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //services.AddIdentity<ApplicationUser,ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<EmailSender, EmailSender>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddAutoMapper();
            
            services.AddHelpers().AddStatus().AddServices().AddThemeSupport();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IActionsService, ActionsService>();
            services.AddScoped<ITenantConfig, TenantConfig>();
            services.AddScoped<ISwimLineService, SwimLineService>();

            var provider = services.BuildServiceProvider();

            mapperService = provider.GetService<IMapperService>();
            exchangeRateServices = provider.GetService<IExchangeRateServices>();
            branchServcie = provider.GetService<IBankBranchServcie>();
            cityService = provider.GetService<ICityService>();
            transactionMgmtService = provider.GetService<ITransactionMgmtService>();
            countryService = provider.GetService<ICountryService>();
            customerService = provider.GetService<ICustomerService>();
            mapper = provider.GetService<IMapper>();
            workflowService = provider.GetService<IWorkflowService>();
            userService = provider.GetService<IUserService>();
            utils = provider.GetService<Utils>();
            formService = provider.GetService<IFormService>();
            caseService = provider.GetService<ICaseService>();
            lexisNexisService = provider.GetService<ILexisNexisService>();
            sourceOfFundSetupService = provider.GetService<ISourceOfFundSetupService>();
            //services.Configure<TwilioAccountDetails>(Configuration.GetSection("TwilioAccountDetails"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/user/login.html";
                options.AccessDeniedPath = new PathString("/404.html");
            });
            /*
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.Configure<MvcOptions>(options => {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAnyOrigin"));
            });
            */

            //services for user
            //services.AddScoped<ICiceroUserService,CiceroUserService>();
            //services.AddScoped<ICiceroCaseService,CiceroCaseService>();
            ////services.AddScoped<ICiceroRazorViewToStringRenderer, CiceroRazorViewToStringRenderer>();
            //services.AddScoped<ICiceroActivityLogService, CiceroActivityLogService>();
            //services.AddScoped<ICiceroMessageService, CiceroMessageService>();
            //services.AddScoped<ICiceroTemplateService, CiceroTemplateService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddNToastNotifyToastr();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleTransfer API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            var autoScheduler = new AutoScheduler(userService, mapperService, db, exchangeRateServices, branchServcie, cityService, transactionMgmtService, countryService, 
                customerService,sourceOfFundSetupService,paymentPurposeSetupService, mapper, workflowService, utils, formService, caseService, lexisNexisService);
            /*app.Use(async (context, next) =>
            {
                if (!YourWayOfCheckingIfAppIsConfigured())
                {
                    //redirect to another location if not ready
                    context.Response.Redirect("/Home/NotReady");
                    return;
                }

                //app is ready, invoke next component in the pipeline (MVC)
                await next.Invoke(context);
            });

*/
            //app.Use(async (context,next) =>
            //{
            //    var c = context;
            //    if(context.Request.IsHttps==false || context.Request.Host.Value=="5345.545.545"){
            //        context.Response.Redirect("https://www.google.com");
            //    }else{
            //        await next.Invoke();
            //    }
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/admin/error.html");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleTransfer V1");
            });
            app.UseMvc(routes =>
            {
                //routes.MapRoute(name: "areaRouteSimpleTransfer",template: "{area:exists}/simpletransfer/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "areaRoute",template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                        name: "default_route",
                        template: "{url}",
                        defaults: new { Areas = "admin", controller = "Article", action = "FrontIndex" });
            });
            app.UseNToastNotify();

            autoScheduler.IntervalInSeconds(13, 11, 3600, () =>
            {
              //  autoScheduler.TransactionStatus(Configuration);
                autoScheduler.CheckBTTrxNew(Configuration);
            });

        }

    }
}
