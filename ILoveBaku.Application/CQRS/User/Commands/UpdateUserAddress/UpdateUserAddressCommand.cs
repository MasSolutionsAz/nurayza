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

namespace ILoveBaku.Application.CQRS.User.Commands.UpdateUserAddress
{
    public class UpdateUserAddressCommand:BaseRequest<ApiResult<int?>>
    {
        public UserAddressInfoDto Model { get; set; }
        public int AddressId { get; set; }
        public class UpdateUserAddressCommandHandler : IRequestHandler<UpdateUserAddressCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateUserAddressCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateUserAddressCommand request, CancellationToken cancellationToken)
            {
                var address = await _context.UsersAddressInfo.Where(c => c.Id == request.Model.AddressId && c.UsersId == request.UserId).FirstOrDefaultAsync();
                if (address == null)
                    return ApiResult<int?>.CreateResponse(null);


                address.UsersId = request.UserId;
                address.RegionsId = request.Model.RegionId;
                address.ZipCode = request.Model.ZipCode;
                address.Address = request.Model.Address;


                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(address.Id);
            }
        }
    }
}
