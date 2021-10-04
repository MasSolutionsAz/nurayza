using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.ProductGroup.Models;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductGroup.Queries.GetProductGroups
{
    public class GetProductGroupsQuery:BaseRequest<ApiResult<ProductGroupListVm>>
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public class GetProductGroupsQueryHandler : IRequestHandler<GetProductGroupsQuery, ApiResult<ProductGroupListVm>>
        {
            public IApplicationDbContext _context { get; set; }
            public GetProductGroupsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductGroupListVm>> Handle(GetProductGroupsQuery request, CancellationToken cancellationToken)
            {
                int count = await _context.ProductGroups.CountAsync();
                int total = (int)Math.Ceiling(count / (decimal)request.Take);
                var productGroups = await _context.ProductGroups.Select(c => new ProductGroupDto
                {
                    CategoriesId = c.CategoriesId,
                    CategoryName = _context.CategoriesLangs.Where(a=>a.LangsId == Convert.ToInt32(Lang.Az)&&a.CategoriesId == c.CategoriesId).Select(c=>c.Name).FirstOrDefault(),
                    IsActive = c.IsActive ?? false,
                    Name = c.Name,
                    Id = c.Id
                }).Skip((request.Page-1)*request.Take).Take(request.Take).OrderByDescending(c=>c.Id).ToListAsync();

                ProductGroupListVm vm = new ProductGroupListVm
                {
                    Groups = productGroups,
                    Total = total,
                    Page = request.Page
                };
                return ApiResult<ProductGroupListVm>.CreateResponse(vm);
            }
        }
    }
}
