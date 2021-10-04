using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserLogin
{
    public class GetUserLoginQuery:BaseRequest<ApiResult<Guid?>>
    {
        public string Email { get; set; }
        public bool IsExternal { get; set; }
        public string Provider { get; set; }
        public class GetUserLoginQueryHandler : IRequestHandler<GetUserLoginQuery, ApiResult<Guid?>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserLoginQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            } 
            public async Task<ApiResult<Guid?>> Handle(GetUserLoginQuery request, CancellationToken cancellationToken)
            {
                var userLogin = await _context.UsersLogins.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
               
                if (userLogin == null)
                    return ApiResult<Guid?>.CreateResponse(null);

                if (request.IsExternal)
                {
                    var userExternalLogin = await _context.UsersExternalLogins.Where(c => c.UsersId == userLogin.UsersId && c.ProviderKey == request.Provider).FirstOrDefaultAsync();
                    if(userExternalLogin == null)
                        return ApiResult<Guid?>.CreateResponse(null);

                }
                return ApiResult<Guid?>.CreateResponse(userLogin.UsersId);
            }
        }
    }
}
