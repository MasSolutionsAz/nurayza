using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Infrastructure.Services;
using ILoveBaku.MVC.Core.Middleware;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IIdentityService = ILoveBaku.MVC.Services.IIdentityService;
using RequestCultureProvider = ILoveBaku.MVC.Core.Localization.RequestCultureProvider;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;
using ILoveBaku.MVC.Extensions;
using FluffySpoon.AspNet.EncryptWeMust;
using FluffySpoon.AspNet.EncryptWeMust.Certes;
using Certes;

namespace ILoveBaku.MVC
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            services.AddApiRequestService(options =>
            {
                options.BaseAddress = new Uri(_configuration["BaseAddress"]);
                options.AddHeader("culture", _configuration["Default:Culture"]);
            });

            services.AddLogics();

            services.AddScoped<CultureService>();

            services.AddTransient<IIPService, IPService>();

            services.AddLocalization(x => x.ResourcesPath = "Resources");

            services.AddTransient<ILocService, LocService>();

            services.AddMvc()
              .AddViewLocalization().AddDataAnnotationsLocalization(options =>
              {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                  {
                      var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                      return factory.Create(nameof(SharedResource), assemblyName.Name);
                  };
              }).AddXmlSerializerFormatters().AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<EmailServiceOption>((option) =>
            {
                option.DisplayName = "I Love Baku";
                option.Email = "ilovebaku.official@gmail.com";
                option.Password = "ilovebaku123@";
                option.Host = "smtp.gmail.com";
                option.Port = 587;
                option.EnableSSL = true;
            });

            services.AddSingleton<IEmailService, EmailService>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("az-AZ"),
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU"),
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
                options.RequestCultureProviders.Insert(0, new RequestCultureProvider());
            });

            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddCookie("TempCookie")
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.SignInScheme = "TempCookie";
                facebookOptions.AppId = "260444925377080";
                facebookOptions.AppSecret = "43b54d3df165ed66a6f69ff24872709b";

                facebookOptions.ClaimActions.MapJsonKey("birthday", "user_birthday");
                facebookOptions.ClaimActions.MapJsonKey("gender", "user_gender");
                facebookOptions.SaveTokens = true;
            })
            .AddGoogle(option =>
            {
                option.SignInScheme = "TempCookie";
                option.ClientSecret = "avkG1Yu32Tgi3MOiuzjD-M6p";
                option.ClientId = "181112774598-bf3khk14nbcp88emqqsdvq4g4r6uk2od.apps.googleusercontent.com";
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowMyApplication", builder => builder.WithOrigins("http://ilovebaku.az/"));
            //});

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddScoped<KapitalPaymentService>();

            services.AddSession(options =>
            {
                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromHours(1);
                //options.Cookie.Name = "MySessionCookie";
            });
            services.AddDistributedMemoryCache();

            services.Configure<CookieOptions>(options =>
            {
                //options.SameSite = SameSiteMode.None;
                options.IsEssential = true;
                options.HttpOnly = true;
                options.Secure = true;
                options.Expires = DateTimeOffset.UtcNow.AddHours(1);
            });


            //email service options
            services.Configure<EmailSenderOptions>((option) =>
            {
                option.DisplayName = _configuration["Web:Email:DisplayName"];
                option.Email = _configuration["Web:Email:Email"];
                option.Password = _configuration["Web:Email:Password"];
                option.Host = _configuration["Web:Email:MailServer"];
                option.Port = Convert.ToInt32(_configuration["Web:Email:Port"]);
                option.EnableSSL = true;
            });
            //email service
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            #region Localization
            app.UseRequestLocalization();
            //options =>
            //{
            //    using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            //    CultureService cultureService = serviceScope.ServiceProvider.GetService<CultureService>();
            //    ApiRequestService apiRequestService = serviceScope.ServiceProvider.GetService<ApiRequestService>();

            //    List<CultureInfo> cultures = cultureService.GetCultures(apiRequestService).Result.Select(c => new CultureInfo(c.Name)).ToList();

            //    options.SupportedCultures = cultures;
            //    options.SupportedUICultures = cultures;
            //    options.DefaultRequestCulture = new RequestCulture(cultureService.DefaultCulture, cultureService.DefaultCulture);
            //    options.RequestCultureProviders.Insert(0, new RequestCultureProvider());
            //}
            #endregion

            app.UseRouting();
            //app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                          name: "admin",
                          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                          name: "culture",
                          pattern: "{culture}/{controller}/{action}",
                          defaults: new { culture = "az-AZ", controller = "Home", action = "Index" },
                          constraints: new { culture = new RegexRouteConstraint("^[a-z]{2}(?:-[A-Z]{2})?$") });

                endpoints.MapControllerRoute(
                          name: "default",
                          pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}
