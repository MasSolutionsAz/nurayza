using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Category.Models;
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

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategory
{
    public class GetCategoryQuery : BaseRequest<ApiResult<CategoryDto>>
    {
        public string Title { get; set; }

        public GetCategoryQuery(string title) => Title = title;

        public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, ApiResult<CategoryDto>>
        {
            private readonly IApplicationDbContext _context;

            public GetCategoryQueryHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                string title = request.Title;

                CategoriesLangs category = await _context.CategoriesLangs.FirstOrDefaultAsync(cl => cl.Category.IsActive && cl.Lang.Culture == request.Culture &&
                                                                                                    cl.Category.Title == title);

                if (category.IsNull()) return ApiResult<CategoryDto>.CreateResponse();

                CategoryDto categoryDto = new CategoryDto()
                {
                    Id = category.CategoriesId,
                    Name = category.Name,
                    RouteName = category.Category.Title
                };

                return ApiResult<CategoryDto>.CreateResponse(categoryDto);
            }
        }
    }
}
