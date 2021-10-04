using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.LoginUser
{
    public class LoginUserCommand : BaseRequest<ApiResult<UserResponse>>
    {
        public LoginVM Model { get; set; }

        public LoginUserCommand(LoginVM model) => Model = model;

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResult<UserResponse>>
        {
            private readonly IIdentityService _identityService;
            private readonly IApplicationDbContext _applicationDbContext;

            public LoginUserCommandHandler(IIdentityService identityService, IApplicationDbContext applicationDbContext)
            {
                _identityService = identityService;
                _applicationDbContext = applicationDbContext;
            }

            public async Task<ApiResult<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                {
                    return ApiResult<UserResponse>.CreateResponse(null, request.Errors,
                        new ErrorDetail()
                        {
                            ErrorMessage = "Validasiya erroru"
                        });
                }

                //var userLogin = await _applicationDbContext.UsersLogins.FirstOrDefaultAsync(c => c.Email == request.Model.Email);
                //if (userLogin != null)
                //{
                //    var isEmailConfirmed = userLogin.EmailConfirmed;
                //    if (!isEmailConfirmed)
                //    {
                //        request.Errors.Add("", "E-poçt ünvanı təsdiq edilməyib.");
                //        return ApiResult<UserResponse>.CreateResponse(null, request.Errors);
                //    }
                //}

                return await _identityService.Login(request.Model, cancellationToken);
            }
        }
    }
}
