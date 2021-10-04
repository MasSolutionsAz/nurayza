using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetResetToken
{
    public class GetResetTokenQuery:BaseRequest<ApiResult<ResetPasswordDto>>
    {
        public string Email { get; set; }
        public class GetResetrTokenQueryHandler : IRequestHandler<GetResetTokenQuery, ApiResult<ResetPasswordDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IToken _tokenService;
            public GetResetrTokenQueryHandler(IApplicationDbContext context,IToken tokenService)
            {
                _context = context;
                _tokenService = tokenService;
            }
            public async Task<ApiResult<ResetPasswordDto>> Handle(GetResetTokenQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.UsersLogins.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
                if(user == null)
                {
                    request.Errors.Add("xeta", "İstifadəçi tapılmadı.");
                    return ApiResult<ResetPasswordDto>.CreateResponse(null,request.Errors);
                }

                var token = _tokenService.MakeToken();

               var activeToken= user.User.Tokens.Where(c => c.UsersTokensStatusesId == (int)TokenStatus.Active
                                          &&
                                          c.UsersTokensTypesId == (byte)UserTokenType.Reset
                                          &&
                                          c.ExpireDate>=DateTime.Now).FirstOrDefault();
                if(activeToken != null)
                {
                    request.Errors.Add("error","Parolun yenilənməsi üçün artıq email göndərilmişdir.");
                    return ApiResult<ResetPasswordDto>.CreateResponse(null, request.Errors);
                }

                await _tokenService.AddToDatabaseAsync(token, new TokenSessionInfo
                {
                    UserId = user.UsersId,
                    ExpireDate = DateTime.Now.AddHours(1)
                }, UserTokenType.Reset);

                ResetPasswordDto dto = new ResetPasswordDto
                {
                    Token = token,
                    UserId = user.UsersId
                };

                return ApiResult<ResetPasswordDto>.CreateResponse(dto);
            }
        }
    }
}
