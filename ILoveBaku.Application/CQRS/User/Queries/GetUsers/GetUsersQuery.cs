using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUsers
{
    public class GetUsersQuery:BaseRequest<ApiResult<UserListVm>>
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResult<UserListVm>>
        {
            private readonly IApplicationDbContext _context;
            public GetUsersQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<UserListVm>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                int total = (int)Math.Ceiling((await _context.Users.CountAsync()) / (decimal)request.Take);
                var users = await _context.UsersLogins.OrderByDescending(c=>c.User.CreatedDate).Skip((request.Page-1)*request.Take).Take(request.Take).Select(c => new UserDto
                {
                    Surname = c.User.Surname,
                    Name = c.User.Name,
                    Birthday = c.User.Birthday!=null?c.User.Birthday.Value.ToShortDateString():"",
                    Gender = c.User.Gender,
                    Phone = c.User.Phone,
                    UserId = c.User.Id,
                    Email = c.User.ContactEmail,
                    UserStatusId = c.User.UsersStatusesId
                }).ToListAsync();

                var userListVm = new UserListVm
                {
                    Page = request.Page,
                    Users = users,
                    Total = total
                };
                return ApiResult<UserListVm>.CreateResponse(userListVm);
            }
        }
    }
}
