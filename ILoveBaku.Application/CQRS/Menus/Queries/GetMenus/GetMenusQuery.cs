using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenus
{
    public class GetMenusQuery : BaseRequest<ApiResult<List<MenuItemDto>>>
    {
        public int ParentId { get; set; }
        public int Page { get; set; }
        public int MenuTypeId { get; set; }
        public bool? IsActive { get; set; }
        public class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, ApiResult<List<MenuItemDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IConfiguration _configuration;
            public GetMenusQueryHandler(IApplicationDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
            public async Task<ApiResult<List<MenuItemDto>>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
            {
                var take = Convert.ToInt32(_configuration["Default:MenuList"]);
                var data = await Recursive(request.Culture, request.MenuTypeId, request.IsActive, request.ParentId);
                data = request.Page == 0 ? data.OrderBy(c => c.Priority).Skip((request.Page - 1) * take).Take(take).ToList() : data.OrderBy(c => c.Priority).ToList();
                return ApiResult<List<MenuItemDto>>.CreateResponse(data);
            }
            public async Task<List<MenuItemDto>> Recursive(string Culture, int menuTypeId, bool? isActive, int menuId = 0)
            {
                var result = new List<MenuItemDto>();

                var menuLangs = await _context.MenuLangs
                                                        .Where(c =>c.Menu.ParentId == menuId 
                                                                   &&
                                                                   (menuTypeId != 0? c.Menu.MenuTypesId == menuTypeId:true)
                                                                   &&
                                                                   c.Lang.Culture == Culture
                                                                   && (isActive != null ? c.Menu.IsActive == isActive : true)
                                                                                  )
                                                        .OrderBy(c => c.Menu.Priority)
                                                        .ToListAsync();  

                foreach (var item in menuLangs)
                {
                    result.Add(new MenuItemDto
                    {
                        Id = item.MenuId,
                        Priority = item.Menu.Priority != null ? (int)item.Menu.Priority : 0,
                        Name = item.Name,
                        CategoryRootName = item.Name.ToParameterizingRoute(),
                        Link = item.Menu.Link,
                        MenuTypeId = item.Menu.MenuTypesId,
                        ParentId = item.Menu.ParentId != null ? (int)item.Menu.ParentId : 0,
                        Children = await Recursive(Culture, menuTypeId, isActive, item.MenuId),
                        IsActive = item.Menu.IsActive,
                        Banners = item.Menu.MenuBannerItems.Select(c=>new MenuBannerItemDto { 
                             Link = c.Link,
                             Path = c.File.Path
                        }).ToList()
                    });
                }

                return result;
            }
        }
    }
}
