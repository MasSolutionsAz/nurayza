using System;
using System.Threading.Tasks;
using ILoveBaku.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ILoveBaku.API
{
    public class Program
    {   
        public static async Task Main(string[] args)
        {
            IHost webHost = CreateHostBuilder(args).Build();
            using(IServiceScope scope = webHost.Services.CreateScope())
            {
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.SeedAsync();
            }

            await webHost.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults((Action<IWebHostBuilder>)(webBuilder =>
                {
                    WebHostBuilderExtensions.UseStartup<Startup>(webBuilder);
                }));
    }
}
