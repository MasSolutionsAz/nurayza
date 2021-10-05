using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Infrastructure.Services;
using ILoveBaku.MVC.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public class ApiRequestService
    {
        private readonly ApiRequestOptions _options;

        public ApiRequestService(ApiRequestOptions options) => _options = options ?? new ApiRequestOptions();

        public void Configure(Action<ApiRequestOptions> apiOptions)
        {
            apiOptions(_options);
        }

        private readonly IApplicationDbContext _context;

        public ApiRequestService(IApplicationDbContext context) => _context = context;

        public async Task<TResponse> GetAsync<TResponse>(string url, Action<ApiRequestOptions> apiOptions = default)
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(url))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException($"{_options.BaseAddress}{url}");
            }
        }

        public async Task<TResponse> GetAsync<TRequestParameter, TResponse>(string url, TRequestParameter parameter = default, Action<ApiRequestOptions> apiOptions = default) where TRequestParameter : new()
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync($"{url}{parameter.ToQueryString()}"))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        public async Task<TResponse> PostAsync<TResponse>(string url, Action<ApiRequestOptions> apiOptions = default)
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.PostAsync(url, null))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                        {
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                        });
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestOptions> apiOptions = default)
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.PostAsync(url,new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                        {
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                        });
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        public async Task<TResponse> PutAsync<TResponse>(string url, Action<ApiRequestOptions> apiOptions = default)
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.PutAsync(url, null))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                        {
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        });
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestOptions> apiOptions = default)
        {
            var reg = request;
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")))
                    {
                       //var id= _context.ProductsStockSaleAmounts.Where(a => a.Id == 320).FirstOrDefault();
                       // if (id !=null)
                       // {
                       //     //id.Amount = reg
                       // }

                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                        {

                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,


                        });
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url, Action<ApiRequestOptions> apiOptions = default)
        {
            try
            {
                return await ApiRequest(async (httpClient) =>
                {
                    using (HttpResponseMessage response = await httpClient.DeleteAsync(url))
                    {
                        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                        {
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        });
                    }
                }, apiOptions);
            }
            catch
            {
                throw new ApiRequestException(url);
            }
        }

        protected async Task<TResponse> ApiRequest<TResponse>(Func<HttpClient, Task<TResponse>> func, Action<ApiRequestOptions> apiOptions = default)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                apiOptions = apiOptions ?? ((_options) => { });

                apiOptions(_options);

                httpClient.BaseAddress = _options.BaseAddress;

                foreach (var header in _options.Headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                return await func(httpClient);
            }
        }
    }
}
