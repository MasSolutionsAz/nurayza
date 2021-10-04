using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Infrastructure.Identity;
using ILoveBaku.Infrastructure.Persistence;
using ILoveBaku.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ILoveBaku.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                //local
                options.UseSqlServer(configuration["Database:Connection"]);
            });

            //email service
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            //dbcontextservice
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            //tokenservice
            services.AddTransient<IToken, Token>();

            //identityservice
            services.AddTransient<IIdentityService, IdentityService>();

            //ipservice
            services.AddTransient<IIPService, IPService>();

            //add razor pages
            //services.AddRazorPages();

            return services;
        }
    }
}
