using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
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

namespace ILoveBaku.Application.CQRS.Menus.Commands.UpdateMenuLang
{
    public class UpdateMenuLangCommand:BaseRequest<ApiResult<int?>>
    {
        public MenuLangDto Model { get; set; }
        public int MenuLangId { get; set; }
        public class UpdateMenuLangCommandHandler : IRequestHandler<UpdateMenuLangCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateMenuLangCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateMenuLangCommand request, CancellationToken cancellationToken)
            {
                MenuLangs menuLang = await _context.MenuLangs.Where(c => c.Id == request.MenuLangId).FirstOrDefaultAsync();
                if (menuLang == null)
                    return ApiResult<int?>.CreateResponse(null);

                menuLang.Name = request.Model.Name;
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(menuLang.Id);
            }
        }
    }
}
