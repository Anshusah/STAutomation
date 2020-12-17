using Cicero.Service.Models.Core;
using Cicero.Service.Services.Core.Themes.Services;
using Microsoft.AspNetCore.Mvc.Razor; 

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ThemeCollection
    {
        public static IServiceCollection AddThemeSupport(this IServiceCollection services)
        {


            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.Configure<RazorViewEngineOptions>(options => {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
            services.AddTransient<Theme>();
            return services;
        }
    }
}