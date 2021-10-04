using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Behaviours
{
    public class RequestClientBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IApplicationDbContext _context;

        private readonly HttpContext _httpContext;

        public RequestClientBehaviour(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_httpContext.Request.Headers.TryGetValue("token", out StringValues token))
            {
                Guid userId = (await _context.Users.FirstOrDefaultAsync(u => u.Tokens.Any(ut => ut.Value == token.ToString() &&
                                                                                                ut.ExpireDate >= DateTime.Now)))?.Id ?? 
                                                                                                                                  default;

                typeof(TRequest).GetProperty("UserId", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(request, userId);
            }
            return await next();

        }
    }
}
