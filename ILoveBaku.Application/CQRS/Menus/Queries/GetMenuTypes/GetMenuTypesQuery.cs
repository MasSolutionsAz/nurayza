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

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenuTypes
{
    public class GetMenuTypesQuery:BaseRequest<ApiResult<List<MenuTypes>>>
    {
        public class GetMenuTypesQueryHandler : IRequestHandler<GetMenuTypesQuery, ApiResult<List<MenuTypes>>>
        {
            private readonly IApplicationDbContext _context;
            public GetMenuTypesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<MenuTypes>>> Handle(GetMenuTypesQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.MenuTypes.ToListAsync();
                return ApiResult<List<MenuTypes>>.CreateResponse(data);
            }
        }
    }
}
