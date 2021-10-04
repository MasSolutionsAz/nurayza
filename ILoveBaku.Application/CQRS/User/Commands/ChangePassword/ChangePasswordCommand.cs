using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.ChangePassword
{
    public class ChangePasswordCommand : BaseRequest<ApiResult<int>>
    {
        public ChangePasswordVM Model { get; set; }

        public ChangePasswordCommand(ChangePasswordVM model) => Model = model;

        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;

            private readonly IIdentityService _identityService;

            public ChangePasswordCommandHandler(IApplicationDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<ApiResult<int>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int>.CreateResponse(400, request.Errors,
                        new ErrorDetail()
                        {
                            ErrorMessage = "Password change error."
                        });

                UsersLogins userLogin = await _context.UsersLogins.FirstOrDefaultAsync(ul => ul.UsersId == request.UserId);

                if (userLogin.IsNull())
                    return ApiResult<int>.CreateResponse(404, null,
                        new ErrorDetail()
                        {
                            ErrorMessage = "User login not found."
                        });

                if (!_identityService.CheckUserLoginPassword(userLogin, request.Model.Password))
                    return ApiResult<int>.CreateResponse(401, null,
                        new ErrorDetail()
                        {
                            ErrorMessage = "The password is incorrect."
                        });

                await _identityService.ChangePasswordAsync(userLogin, request.Model.NewPassword);
                await _context.SaveChangesAsync();
                return ApiResult<int>.CreateResponse(200);
            }
        }
    }
}
