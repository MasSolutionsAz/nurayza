using System.IO;
using System.Collections.Generic;
using ILoveBaku.API.Swagger;
using ILoveBaku.Application;
using ILoveBaku.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using ILoveBaku.Application.Common.Interfaces;
using System.Linq;

namespace ILoveBaku.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructure(configuration);

            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddHttpContextAccessor();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyApplication", builder => builder.WithOrigins("http://api.ilovebaku.az/"));
            });

            //services.AddControllersWithViews();
            services.AddRazorPages();

            //swagger
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1",
            //        new OpenApiInfo
            //        {
            //            Title = "ILoveBaku API",
            //            Version = "v1",
            //            Contact = new OpenApiContact
            //            {
            //                Name = "Nəqliyyat, Rabitə və Yüksək Texnologiyalar Nazirliyi",
            //                Email = "test"
            //            }
            //        });

            //    //authorize header
            //    c.AddSecurityDefinition("Token", new OpenApiSecurityScheme
            //    {
            //        Description = @"API-dan istifadə üçün login \r\n\r\n 
            //          Token-i daxil edin.",
            //        Name = "token",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Token"
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //           {
            //              {
            //                new OpenApiSecurityScheme
            //                {
            //                  Reference = new OpenApiReference
            //                    {
            //                      Type = ReferenceType.SecurityScheme,
            //                      Id = "Token"
            //                    },
            //                    Scheme = "oauth2",
            //                    Name = "Token",
            //                    In = Par//});ameterLocation.Header,

            //                  },
            //                  new List<string>()
            //                }
            //              });

            //    //culture header
            //    c.OperationFilter<AddRequiredHeaderParameter>();


            //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            //    c.CustomSchemaIds(a => a.FullName);
            //    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ILoveBaku.API.xml");
            //    //c.IncludeXmlComments(filePath);

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            app.UseCors("AllowMyApplication");

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //// specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
