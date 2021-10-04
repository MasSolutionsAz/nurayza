using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.AddExternalUser
{
    public class AddExternalUserCommand:BaseRequest<ApiResult<Guid?>>
    {
        public UserDto Model { get; set; }
        public class AddExternalUserCommandHandler : IRequestHandler<AddExternalUserCommand, ApiResult<Guid?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityService _identityService;
            public AddExternalUserCommandHandler(IApplicationDbContext context, IIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }
            public async Task<ApiResult<Guid?>> Handle(AddExternalUserCommand request, CancellationToken cancellationToken)
            {
                return await _identityService.AddExternalUser(request.Model);
            }
        }
    }
}
