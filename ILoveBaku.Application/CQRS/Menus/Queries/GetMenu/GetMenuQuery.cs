using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenu
{
    public class GetMenuQuery : BaseRequest<ApiResult<MenuItemDto>>
    {
        public int MenuId { get; set; }
        public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, ApiResult<MenuItemDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetMenuQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<MenuItemDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
            {
                MenuItemDto menuItem = await _context
                                                    .Menu
                                                            .Where(c => c.Id == request.MenuId)
                                                                .Select(c => new MenuItemDto
                                                                {
                                                                    Id = c.Id,
                                                                    MenuTypeId = c.MenuTypesId,
                                                                    CategoryId = c.MenuCategoriesItems!=null?c.MenuCategoriesItems.CategoriesId:0,
                                                                    CategoryParentId = c.MenuCategoriesItems!=null?(c.MenuCategoriesItems.CategoriesParentId==null?0:(int)c.MenuCategoriesItems.CategoriesParentId):0,
                                                                    Link = c.Link,
                                                                    Name = c.Title,
                                                                    ParentId = c.ParentId==null?0:(int)c.ParentId,
                                                                    Priority = c.Priority==null?0:(int)c.Priority,
                                                                    IsActive = c.IsActive,
                                                                    IsManualLink = c.MenuCategoriesItems==null
                                                                }).FirstOrDefaultAsync();

                return ApiResult<MenuItemDto>.CreateResponse(menuItem);
            }
        }
    }
}
