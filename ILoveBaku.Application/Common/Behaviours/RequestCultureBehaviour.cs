using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestCultureBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IApplicationDbContext _context;

        private readonly HttpContext _httpContext;

        public RequestCultureBehaviour(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_httpContext.Request.Headers.TryGetValue("culture", out StringValues culture))
            {
                if ((await _context.GetLanguage(culture)).IsNull())
                    return await Task.FromResult((TResponse)typeof(TResponse).GetMethod("CultureResponse")?
                                                                                .Invoke(Activator.CreateInstance(typeof(TResponse), true),
                                                                                        new object[] { new ErrorDetail() { ErrorMessage = "Api bu dili dəstəkləmir." } }));
                else
                    typeof(TRequest).GetProperty("Culture", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(request, culture.ToString());

            }
            else
                return await Task.FromResult((TResponse)typeof(TResponse).GetMethod("CultureResponse")?
                                                                            .Invoke(Activator.CreateInstance(typeof(TResponse), true),
                                                                                    new object[] { null }));

            return await next();
        }
    }
}
