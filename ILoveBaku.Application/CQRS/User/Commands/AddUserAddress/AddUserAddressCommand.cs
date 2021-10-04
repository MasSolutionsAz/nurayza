using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.AddUserAddress
{
    public class AddUserAddressCommand:BaseRequest<ApiResult<int?>>
    {
        public UserAddressInfoDto Model { get; set; }
        public class AddUserAddressCommandHandler : IRequestHandler<AddUserAddressCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddUserAddressCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddUserAddressCommand request, CancellationToken cancellationToken)
            {
                UsersAddressInfo address = new UsersAddressInfo
                {
                    Address = request.Model.Address,
                    RegionsId = request.Model.RegionId,
                    UsersId = request.UserId,
                    ZipCode = request.Model.ZipCode,
                    IsActive = true
                };


                await _context.UsersAddressInfo.AddAsync(address);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(address.Id);
            }
        }
    }
}
