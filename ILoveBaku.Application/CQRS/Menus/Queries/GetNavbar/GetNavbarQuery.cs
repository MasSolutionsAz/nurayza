using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetNavbar
{
    public class GetNavbarQuery : BaseRequest<ApiResult<MenuDto>>
    {
        public int MenuTypeId { get; set; }
        public class GetNavbarQueryHandler : IRequestHandler<GetNavbarQuery, ApiResult<MenuDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetNavbarQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResult<MenuDto>> Handle(GetNavbarQuery request, CancellationToken cancellationToken)
            {
                //var menuLangs = _context.MenuLangs.Where(m => m.Lang.Culture == request.Culture &&
                //                                              m.Menu.ParentId.GetValueOrDefault(0) == 0 &&
                //                                              m.Menu.MenuTypesId == request.MenuTypeId);

                //var menuCategoriesItems = await _context.MenuCategoriesItems.ToListAsync();

                //var menuBannerItems = await _context.MenuBannerItems.ToListAsync();

                //var parentMenuCategoriesItems = menuCategoriesItems.Where(mc => mc.CategoriesParentId.IsNullOrZore());

                //var childMenuCategoriesItems = menuCategoriesItems.Where(mc => !mc.CategoriesParentId.IsNullOrZore());

                //List<MenuItem> menuItems = new List<MenuItem>();

                //foreach (var m in menuLangs)
                //{
                //    List<MenuCategory> parentMenuCategories = new List<MenuCategory>();

                //    foreach (var pmc in parentMenuCategoriesItems?.Where(pmc => pmc.MenuId == m.MenuId))
                //    {
                //        List<MenuCategory> childMenuCategories = new List<MenuCategory>();

                //        foreach (var cmc in childMenuCategoriesItems.Where(cmc => cmc.CategoriesParentId == pmc.Id))
                //        {
                //            childMenuCategories.Add(new MenuCategory()
                //            {
                //                Title = cmc.Category?.CategoriesLangs?.FirstOrDefault(cl => cl.Lang.Culture == request.Culture)?.Name,
                //                Link = cmc.Link
                //            });
                //        }

                //        parentMenuCategories.Add(new MenuCategory()
                //        {
                //            Title = pmc.Category?.CategoriesLangs?.FirstOrDefault(cl => cl.Lang.Culture == request.Culture)?.Name,
                //            Link = pmc.Link,
                //            Children = childMenuCategories
                //        });
                //    }

                //    List<MenuBanner> menuBanners = new List<MenuBanner>();

                //    foreach (var mb in menuBannerItems.Where(mb => mb.MenuId == m.MenuId))
                //    {
                //        menuBanners.Add(new MenuBanner()
                //        {
                //            Image = mb.File?.Path,
                //            Link = mb.Link
                //        });
                //    }

                //    menuItems.Add(new MenuItem()
                //    {
                //        Title = m.Name,
                //        Link = m.Menu.Link,
                //        MenuCategories = parentMenuCategories,
                //        MenuBanners = menuBanners
                //    });
                //}

                //return ApiResult<MenuDto>.CreateResponse(new MenuDto()
                //{
                //    MenuItems = menuItems
                //});

                throw new NullReferenceException();
            }
        }
    }
}
