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

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs
{
    public class GetMenuLangsQuery : BaseRequest<ApiResult<List<MenuLangDto>>>
    {
        public int MenuId { get; set; }
        public class GetMenuLangsQueryHandler : IRequestHandler<GetMenuLangsQuery, ApiResult<List<MenuLangDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetMenuLangsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<MenuLangDto>>> Handle(GetMenuLangsQuery request, CancellationToken cancellationToken)
            {
                var data = await _context.MenuLangs.Where(c => c.MenuId == request.MenuId).Select(c => new MenuLangDto
                {
                    Id = c.Id,
                    MenuId = c.MenuId,
                    LangId = c.LangsId,
                    LangName = c.Lang.DisplayName,
                    Name = c.Name
                }).ToListAsync();

                return ApiResult<List<MenuLangDto>>.CreateResponse(data);
            }
        }
    }
}
