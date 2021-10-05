using ILoveBaku.Application.Common.Extension;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Controllers
{
    public class BaseController : Controller
    {
        private IConfiguration _configuration;

        protected IConfiguration Configuration => _configuration ??= HttpContext.RequestServices.GetService<IConfiguration>();

        private CultureService _cultureService;

        private CultureService CultureService => _cultureService ??= HttpContext.GetServiceAsync<CultureService>().Result;

        protected string Culture => CultureService.CurrentCulture;

        protected string ShortCulture => CultureService.ShortCulture;

        protected ApiRequestService _api;

        protected ApiRequestService API => GetApiRequestService().Result;

        private async Task<ApiRequestService> GetApiRequestService()
        {
            if (!_api.IsNull())
                return _api;
            else
            {
                ApiRequestService instance = await HttpContext.GetServiceAsync<ApiRequestService>();
                instance.Configure(options =>
                {
                    //options.Headers.TryGetValue("token", out string token);
                    //if (token == null)
                    options.AddHeader("token", HttpContext.Request.Cookies["token"] ?? string.Empty);
                    options.AddHeader("culture", CultureService.GetCultureOriginalName(Culture) ?? string.Empty);
                });
                return instance;
            }
        }

        protected async Task<string> GetServerRootPathAsync(params string[] folders)
        {
            IEnumerable<string> _folders = folders.Prepend((await HttpContext.GetServiceAsync<IWebHostEnvironment>()).ContentRootPath);
            if (folders.Length == 0)
            {
                _folders = _folders.Append("uploads");
            }
            return Path.Combine(_folders.ToArray());
        }

        public static async Task<X509Certificate2> LoadCertificateAsync(string certificatePath, string privateKeyPath)
        {
            using X509Certificate2 x509Certificate2 = new X509Certificate2(certificatePath);
            var privateKeyBlocks = (await System.IO.File.ReadAllTextAsync(privateKeyPath)).Split('-', StringSplitOptions.RemoveEmptyEntries);
            var privateKeyBytes = Convert.FromBase64String(privateKeyBlocks[1]);
            using RSA rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            return new X509Certificate2(x509Certificate2.CopyWithPrivateKey(rsa).Export(X509ContentType.Pfx));
        }
    }
}