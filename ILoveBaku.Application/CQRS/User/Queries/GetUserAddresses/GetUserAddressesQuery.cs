using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses
{
    public class GetUserAddressesQuery : BaseRequest<ApiResult<List<UserAddressInfoDto>>>
    {
        public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, ApiResult<List<UserAddressInfoDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserAddressesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<UserAddressInfoDto>>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.UsersAddressInfo.Where(c => c.IsActive && c.UsersId == request.UserId).Select(c => new UserAddressInfoDto()
                {
                    Name = c.Users.Name,
                    Surname = c.Users.Surname,
                    Country = c.Regions.Country.Name,
                    Region = c.Regions.Name,
                    Address = c.Address,
                    ZipCode = c.ZipCode,
                    Phone = c.Users.Phone,
                    AddressId = c.Id
                }).ToListAsync();

                data = data == null ? new List<UserAddressInfoDto>() : data;

                return ApiResult<List<UserAddressInfoDto>>.CreateResponse(data);
            }
        }
    }
}
