using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ILoveBaku.Application.CQRS.User.Commands.LoginExternalUser
{
    public class LoginExternalUserLoginCommand : BaseRequest<ApiResult<UserResponse>>
    {
        public Guid ExternalUserId { get; set; }
        public class AddExternalUserLoginCommandHandler : IRequestHandler<LoginExternalUserLoginCommand, ApiResult<UserResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityService _identityService;
            public AddExternalUserLoginCommandHandler(IApplicationDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }
            public async Task<ApiResult<UserResponse>> Handle(LoginExternalUserLoginCommand request, CancellationToken cancellationToken)
            {
                var userLogin = await _context.UsersLogins.Where(c => c.UsersId == request.ExternalUserId).FirstOrDefaultAsync();
                if (userLogin == null)
                    return ApiResult<UserResponse>.CreateResponse(null);

                return await _identityService.LoginExternal(userLogin.Id);
            }
        }
    }
}
