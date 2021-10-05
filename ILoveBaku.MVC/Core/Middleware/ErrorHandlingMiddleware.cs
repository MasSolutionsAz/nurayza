using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.MVC.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Core.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IIPService IPService /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception, IPService);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IIPService IPService)
        {
            return context.Response.WriteAsync(exception.Message);
        }
    }
}
