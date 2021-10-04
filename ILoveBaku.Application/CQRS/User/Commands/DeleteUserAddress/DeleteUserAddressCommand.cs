using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.DeleteUserAddress
{
    public class DeleteUserAddressCommand:BaseRequest<ApiResult<int?>>
    {
        public int AddressId { get; set; }
        public class DeleteUserAddressCommandHandler : IRequestHandler<DeleteUserAddressCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public DeleteUserAddressCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(DeleteUserAddressCommand request, CancellationToken cancellationToken)
            {
                var address = await _context.UsersAddressInfo.Where(c => c.UsersId == request.UserId && c.Id == request.AddressId).FirstOrDefaultAsync();

                if(address!=null)
                {
                    address.IsActive = false;
                    await _context.SaveChangesAsync();
                    return ApiResult<int?>.CreateResponse(0);

                }

                return ApiResult<int?>.CreateResponse(null);

            }
        }
    }
}
