using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserConfirmation
{
    public class GetUserConfirmationQuery:BaseRequest<ApiResult<bool>>
    {
        public class GetUserConfirmationQueryHandler : IRequestHandler<GetUserConfirmationQuery, ApiResult<bool>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserConfirmationQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<bool>> Handle(GetUserConfirmationQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == request.UserId);
                if (user != null)
                    return ApiResult<bool>.CreateResponse(user.EmailConfirmed);

                return ApiResult<bool>.CreateResponse(false);
            }
        }
    }
}
