using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenus;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Menus.Commands.UpdateMenu
{
    public class UpdateMenuCommand:BaseRequest<ApiResult<int?>>
    {
        public MenuItemDto Model { get; set; }
        public int MenuId { get; set; }
        public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateMenuCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
            {
                Menu menu = await _context.Menu.Where(c => c.Id == request.MenuId).FirstOrDefaultAsync();
                if (menu == null)
                    return ApiResult<int?>.CreateResponse(null);

                menu.Title = request.Model.Name;
                menu.MenuTypesId = (byte)request.Model.MenuTypeId;
                menu.ParentId = request.Model.ParentId;
                menu.Priority = request.Model.Priority;
                menu.Link = request.Model.Link;
                menu.IsActive = request.Model.IsActive;

                if (!request.Model.IsManualLink)
                {
                    MenuCategoriesItems menuCategoriesItems = menu.MenuCategoriesItems;
                    _context.MenuCategoriesItems.Remove(menuCategoriesItems);

                    MenuCategoriesItems menuCategoryItem = new MenuCategoriesItems
                    {
                        CategoriesId = request.Model.CategoryId == 0 ? request.Model.CategoryParentId : request.Model.CategoryId,
                        CategoriesParentId = request.Model.CategoryParentId,
                        MenuId = menu.Id
                    };

                    await _context.MenuCategoriesItems.AddAsync(menuCategoryItem);
                }
               

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(menu.Id);
            }
        }
    }
}
