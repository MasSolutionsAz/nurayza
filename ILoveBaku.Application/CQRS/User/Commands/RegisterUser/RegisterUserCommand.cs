using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.RegisterUser
{
    public class RegisterUserCommand : BaseRequest<ApiResult<UserResponse>>
    {
        public RegisterVM Model { get; set; }

        public RegisterUserCommand(RegisterVM model) => Model = model;

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResult<UserResponse>>
        {
            private readonly IIdentityService _identityService;

            public RegisterUserCommandHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<ApiResult<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<UserResponse>.CreateResponse(null, request.Errors,
                        new ErrorDetail()
                        {
                            ErrorMessage = "Validasiya erroru"
                        });
                }

                return await _identityService.Register(request.Model, cancellationToken);
            }
        }
    }
}
