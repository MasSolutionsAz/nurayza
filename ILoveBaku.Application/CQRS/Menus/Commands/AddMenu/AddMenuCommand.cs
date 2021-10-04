using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Menus.Models;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
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

namespace ILoveBaku.Application.CQRS.Menus.Commands.AddMenu
{
    public class AddMenuCommand:BaseRequest<ApiResult<int?>>
    {
        public MenuItemDto Model { get; set; }
        public class AddMenuCommandHandler : IRequestHandler<AddMenuCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddMenuCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddMenuCommand request, CancellationToken cancellationToken)
            {
                Menu menu = new Menu
                {
                    CreatedDate = DateTime.Now,
                    Link = request.Model.Link,
                    MenuTypesId = (byte)request.Model.MenuTypeId,
                    ParentId = request.Model.ParentId,
                    Priority = request.Model.Priority,
                    Title = request.Model.Name,
                    IsActive = request.Model.IsActive
                };
               
                await _context.Menu.AddAsync(menu);
                await _context.SaveChangesAsync();

                if (!request.Model.IsManualLink)
                {
                    MenuCategoriesItems menuCategoryItem = new MenuCategoriesItems
                    {
                        CategoriesId = request.Model.CategoryId == 0 ? request.Model.CategoryParentId : request.Model.CategoryId,
                        CategoriesParentId = request.Model.CategoryParentId,
                        MenuId = menu.Id
                    };

                    await _context.MenuCategoriesItems.AddAsync(menuCategoryItem);
                    await _context.SaveChangesAsync();
                }
               


                var langs = await _context.Langs.ToListAsync();
                for (int i = 0; i < langs.Count; i++)
                {
                    MenuLangs menuLangs = new MenuLangs
                    {
                        LangsId = langs[i].Id,
                        MenuId = menu.Id,
                        Name = request.Model.Name,
                        CreatedDate = DateTime.Now
                    };

                    await _context.MenuLangs.AddAsync(menuLangs);
                    await _context.SaveChangesAsync();

                }

                return ApiResult<int?>.CreateResponse(menu.Id);
            }
        }
    }
}
