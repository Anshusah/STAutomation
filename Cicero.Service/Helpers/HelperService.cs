using AutoMapper;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Cicero.Service.Backend;
using Cicero.Service.Services;
using Cicero.Service;
//using Cicero.Service.Frontend;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HelperService
    {
        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<AppSetting>();
            services.AddScoped<ComponentService>();
            services.AddScoped<Utils>();
            services.AddScoped<Permission>();
            services.AddScoped<Globals>();
            services.AddScoped<IRazorToStringRender, RazorToStringRender>();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelProfile());
                mc.AddProfile(new SimpleTransferModelProfile());
                mc.AddProfile(new JazzCashModelProfile());
                //mc.AddProfile(new CiceroModelProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
