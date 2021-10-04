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

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserTokenExsistence
{
    public class GetUserTokenExsistenceQuery:BaseRequest<ApiResult<bool?>>
    {
        public string userId { get; set; }
        public string token { get; set; }
        public class GetUserTokenExsistenceQueryHandler : IRequestHandler<GetUserTokenExsistenceQuery, ApiResult<bool?>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserTokenExsistenceQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<bool?>> Handle(GetUserTokenExsistenceQuery request, CancellationToken cancellationToken)
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

                if(userToken == null)
                {
                    request.Errors.Add("error", "Token istifadəyə yararsızdır.");
                    return ApiResult<bool?>.CreateResponse(null, request.Errors);
                }


                return ApiResult<bool?>.CreateResponse(true);
            }
        }
    }
}
