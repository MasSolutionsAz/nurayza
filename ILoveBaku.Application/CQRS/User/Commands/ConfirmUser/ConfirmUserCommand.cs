using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.ConfirmUser
{
    public class ConfirmUserCommand:BaseRequest<ApiResult<int?>>
    {
        public string userId { get; set; }
        public string token { get; set; }
        public class ConfirmUserCommandHandler : IRequestHandler<ConfirmUserCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public ConfirmUserCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
            {
                var userToken = await _context.UsersTokens.FirstOrDefaultAsync(c => c.UsersId.ToString() == request.userId
                                                                    &&
                                                                    c.Value == request.token
                                                                    &&
                                                                    c.UsersTokensStatusesId == (int)TokenStatus.Active
                                                                    &&
                                                                    c.UsersTokensTypesId == (byte)UserTokenType.Confirmation
                                                                    &&
                                                                    c.ExpireDate >= DateTime.Now);

                if (userToken == null)
                {
                    request.Errors.Add("error", "Xəta baş verdi.Yenidən cəhd edin.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                var userLogin = await _context.UsersLogins.Where(c => c.UsersId == userToken.UsersId).FirstOrDefaultAsync();
                if (userLogin == null)
                {
                    request.Errors.Add("error", "Xəta baş verdi.Yenidən cəhd edin.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                userLogin.EmailConfirmed = true;
                userToken.UsersTokensStatusesId = (int)TokenStatus.Blocked;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(200);
            }
        }
    }
}
