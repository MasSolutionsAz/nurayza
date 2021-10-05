using ILoveBaku.MVC.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace ILoveBaku.MVC.Services
{
    public class KapitalPaymentService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        private X509Certificate2 X509Certificate2
        {
            get
            {
                string certificatesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Certificates");
                string certificatePath = Path.Combine(certificatesFolder, "ilovebaku.crt");
                string privateKeyPath = Path.Combine(certificatesFolder, "privatekey.key");
                return LoadCertificateAsync(certificatePath, privateKeyPath).Result;
            }
        }

        public KapitalPaymentService(IWebHostEnvironment webHostEnvironment, CultureService cultureService, IHttpContextAccessor httpContextAccessors, IConfiguration configuration)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessors.HttpContext;
        }

        private async Task<XmlDocument> PostAsync(string requestParameter)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            X509Certificate2 x509Certificate2 = X509Certificate2;
            httpClientHandler.ClientCertificates.Add(x509Certificate2);

            using HttpClient httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("https://e-commerce.kapitalbank.az:5443/")
            };


            using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("Exec", new StringContent(requestParameter));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(await httpResponseMessage.Content.ReadAsStreamAsync());

            return xmlDocument;
        }

        public async Task<XmlDocument> PayAsync(decimal amount, string culture)
        {
            string amountAsString = (amount * 100).ToString("#.##").Replace(',', '.');

            string requestParameter = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                                         <TKKPG>
                                            <Request>
                                                <Operation>CreateOrder</Operation>
                                                <Language>{culture}</Language>
                                                <Order>
                                                    <OrderType>Purchase</OrderType>
                                                    <Merchant>E1000010</Merchant>
                                                    <Amount>{amountAsString}</Amount>
                                                    <Currency>944</Currency>
                                                    <Description>xxxxxxxx</Description>
                                                    <ApproveURL>{_configuration["Web:Payment:Success"]}</ApproveURL>
                                                    <CancelURL>{_configuration["Web:Payment:Cancel"]}</CancelURL>
                                                    <DeclineURL>{_configuration["Web:Payment:Decline"]}</DeclineURL>
                                                </Order>
                                            </Request>
                                         </TKKPG>";

            return await PostAsync(requestParameter);
        }

        public async Task<XmlDocument> CheckAsync(string orderID, string sessionID)
        {
            string requestParameter = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                                         <TKKPG>
                                            <Request>
                                                <Operation>GetOrderStatus</Operation>
 		                                        <Language>RU</Language>
                                                <Order>
                                                    <Merchant>E1000010</Merchant>
 			                                        <OrderID>{orderID}</OrderID>
                                                </Order>
                                                <SessionID>{sessionID}</SessionID>
                                            </Request>
                                         </TKKPG>";

            return await PostAsync(requestParameter);
        }

        private async Task<X509Certificate2> LoadCertificateAsync(string certificatePath, string privateKeyPath)
        {
            using X509Certificate2 x509Certificate2 = new X509Certificate2(certificatePath);

            string[]  privateKeyBlocks = (await File.ReadAllTextAsync(privateKeyPath)).Split('-', StringSplitOptions.RemoveEmptyEntries);
            var privateKeyBytes = Convert.FromBase64String(privateKeyBlocks[1]);
            using RSA rsa = RSA.Create();

            try
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes,out int byteReads);
            }
            catch(CryptographicException exp)
            {
                _httpContext.Response.Cookies.Append("exception", exp.Message);
            }
            return new X509Certificate2(x509Certificate2.CopyWithPrivateKey(rsa).Export(X509ContentType.Pfx));


        }

        private string GetServerRootPath(params string[] folders)
        {
            IEnumerable<string> _folders = folders.Prepend(_webHostEnvironment.WebRootPath);
            return Path.Combine(_folders.ToArray());
        }
    }
}
