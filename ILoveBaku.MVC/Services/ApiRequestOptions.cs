using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public class ApiRequestOptions
    {
        public Uri BaseAddress { get; set; }

        public Dictionary<string, string> Headers { get; }

        public ApiRequestOptions()
        {
            Headers = new Dictionary<string, string>();
        }

        public void AddHeader(string key, string value)
        {
            if (!Headers.TryAdd(key, value)) Headers[key] = value;
        }
    }
}
