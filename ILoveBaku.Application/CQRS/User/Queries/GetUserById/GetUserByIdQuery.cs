using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserById
{
    public class GetUserByIdQuery : BaseRequest<ApiResult<UserDto>>
    {
        public Guid GetUserId { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResult<UserDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _context.UsersLogins
                                            .Where(c => c.UsersId == request.GetUserId && c.User.BranchesId == 1)
                                                .Select(c => new UserDto
                                                {
                                                    Surname = c.User.Surname,
                                                    Name = c.User.Name,
                                                    FatherName = c.User.Patronymic,
                                                    Addresses = _context.UsersAddressInfo.Where(a=>a.UsersId == c.UsersId).Select(c=>c.Address).ToList(),
                                                    Gender = c.User.Gender,
                                                    UserId = c.UsersId,
                                                    RegisterDate = c.CreatedDate
                                                })
                                                 .FirstOrDefaultAsync();

                return ApiResult<UserDto>.CreateResponse(user);
            }
        }
    }
}
