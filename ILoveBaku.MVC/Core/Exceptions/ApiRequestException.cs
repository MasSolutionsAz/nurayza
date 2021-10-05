using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Core.Exceptions
{
    public class ApiRequestException : Exception
    {
        public ApiRequestException() { }

        public ApiRequestException(string message) : base($"Api request failed: {message}") { }
    }
}
