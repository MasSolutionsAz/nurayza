using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.ResetAndUpdatePassword
{
    public class ResetAndUpdatePasswordCommand : BaseRequest<ApiResult<Guid?>>
    {
        public string userId { get; set; }
        public string token { get; set; }
        public ResetPasswordModel Model { get; set; }
        public class ResetAndUpdatePasswordCommandHandler : IRequestHandler<ResetAndUpdatePasswordCommand, ApiResult<Guid?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityService _identityService;
            public ResetAndUpdatePasswordCommandHandler(IApplicationDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }
            public async Task<ApiResult<Guid?>> Handle(ResetAndUpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var userToken = await _context.UsersTokens.Where(c => c.UsersId.ToString() == request.userId
                                                                    &&
                                                                    c.Value == request.token
                                                                    &&
                                                                    c.UsersTokensStatusesId == (int)TokenStatus.Active
                                                                    &&
                                                                    c.UsersTokensTypesId == (byte)UserTokenType.Reset
                                                                    &&
                                                                    c.ExpireDate >= DateTime.Now).FirstOrDefaultAsync();

                if (userToken == null)
                {
                    request.Errors.Add("error", "Xəta baş verdi.Yenidən cəhd edin.");
                    return ApiResult<Guid?>.CreateResponse(null,request.Errors);
                }

                var userLogin = await _context.UsersLogins.Where(c => c.UsersId == userToken.UsersId).FirstOrDefaultAsync();
                if(userLogin  == null)
                {
                    request.Errors.Add("error", "Xəta baş verdi.Yenidən cəhd edin.");
                    return ApiResult<Guid?>.CreateResponse(null,request.Errors);
                }

                await _identityService.ChangePasswordAsync(userLogin, request.Model.Password);
                userToken.UsersTokensStatusesId = (int)TokenStatus.Blocked;

                await _context.SaveChangesAsync();
                return ApiResult<Guid?>.CreateResponse(userLogin.UsersId);
            }
        }
    }
}
