using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.LogoutUser
{
    public class LogoutUserCommand : BaseRequest<ApiResult<string>>
    {
        public string Token { get; set; }
        public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, ApiResult<string>>
        {
            private readonly IIdentityService _identityService;
            public LogoutUserCommandHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<ApiResult<string>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Logout zamani xeta bas verdi"
                    });
                }

                return await _identityService.LogoutUser(request.Token);
            }
        }
    }
}
