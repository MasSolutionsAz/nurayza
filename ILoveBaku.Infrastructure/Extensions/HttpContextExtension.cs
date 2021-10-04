using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ILoveBaku.Infrastructure.Extensions
{
    public static class HttpContextExtension
    {
        public static async Task<string> GetHeader(this HttpContext httpContext, string key)
        {
            httpContext.Request.Headers.TryGetValue(key, out StringValues value);
            return await Task.FromResult(value);
        }

        public static async Task<bool> HasHeader(this HttpContext httpContext, string key)
        {
            return await Task.FromResult(httpContext.Request.Headers.ContainsKey(key));
        }
    }
}
