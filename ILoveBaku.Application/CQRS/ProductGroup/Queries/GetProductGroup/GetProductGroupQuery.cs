using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductGroup.Queries.GetProductGroup
{
    public class GetProductGroupQuery:BaseRequest<ApiResult<ProductGroupDto>>
    {
        public int GroupId { get; set; }
        public class GetProductGroupQueryHandler : IRequestHandler<GetProductGroupQuery, ApiResult<ProductGroupDto>>
        {
            private readonly IApplicationDbContext _context;
            public GetProductGroupQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<ProductGroupDto>> Handle(GetProductGroupQuery request, CancellationToken cancellationToken)
            {
                var group = await _context.ProductGroups.Where(c => c.Id == request.GroupId).Select(c => new ProductGroupDto
                {
                    Id = c.Id,
                    CategoriesId = c.CategoriesId,
                    IsActive = c.IsActive ?? false,
                    Name = c.Name
                }).FirstOrDefaultAsync();


                if(group == null)
                {
                    request.Errors.Add("", "Xeta");
                    return ApiResult<ProductGroupDto>.CreateResponse(null, request.Errors);
                }
                return ApiResult<ProductGroupDto>.CreateResponse(group);



            }
        }
    }
}
