using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUserAddress
{
    public class GetUserAddressQuery : BaseRequest<ApiResult<UserAddressInfoDto>>
    {
        public int AddressId { get; set; }
        public class GetUserAddressQueryHandler : IRequestHandler<GetUserAddressQuery, ApiResult<UserAddressInfoDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetUserAddressQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<UserAddressInfoDto>> Handle(GetUserAddressQuery request, CancellationToken cancellationToken)
            {
                var address = await _context
                                        .UsersAddressInfo
                                            .Where(c => c.UsersId == request.UserId && c.IsActive && c.Id == request.AddressId)
                                            .Select(c => new UserAddressInfoDto
                                            {
                                                Name = c.Users.Name,
                                                Surname = c.Users.Surname,
                                                Address = c.Address,
                                                AddressId = c.Id,
                                                CountryId = c.Regions.CountryId,
                                                RegionId = c.RegionsId,
                                                ZipCode = c.ZipCode
                                            })
                                            .FirstOrDefaultAsync();

                return ApiResult<UserAddressInfoDto>.CreateResponse(address);

            }
        }
    }
}
