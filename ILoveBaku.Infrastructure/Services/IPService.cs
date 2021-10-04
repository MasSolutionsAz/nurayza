using ILoveBaku.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using ILoveBaku.Infrastructure.Extensions;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Linq;

namespace ILoveBaku.Infrastructure.Services
{
    public class IPService : IIPService
    {
        private readonly HttpContext _httpContext;

        public IPService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IPService(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public T GetRequestIP<T>(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhiteSpace() && _httpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhiteSpace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhiteSpace())
                throw new Exception("Unable to determine caller's IP.");

            if (typeof(T) == typeof(string))
                return (T)(object)ip;

            if (typeof(T) == typeof(int))
                return (T)(object)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0));

            return default;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            if (_httpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhiteSpace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default;
        }
    }
}
