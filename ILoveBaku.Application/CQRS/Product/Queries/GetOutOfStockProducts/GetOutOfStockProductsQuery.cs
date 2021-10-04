using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Stored_Procedures;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetOutOfStockProducts
{
    public class GetOutOfStockProductsQuery:BaseRequest<ApiResult<List<ProductList_sp>>>
    {
        public int BranchId { get; set; }
        public class GetOutOfStockProductsQueryHandler : IRequestHandler<GetOutOfStockProductsQuery, ApiResult<List<ProductList_sp>>>
        {
            private readonly IApplicationDbContext _context;
            public GetOutOfStockProductsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductList_sp>>> Handle(GetOutOfStockProductsQuery request, CancellationToken cancellationToken)
            {
                List<ProductList_sp> products = _context.Products.Where(c => !c.ProductsStocks.Any(c => c.BranchesId == request.BranchId)).Select(c => new ProductList_sp
                {
                    ProductCreatedDate = c.CreatedDate,
                    ProductGroupCreatedDate = c.ProductGroup.CreatedDate,
                    CategoryId = c.ProductGroup.CategoriesId,
                    CategoryName = c.ProductGroup.Category.CategoriesLangs.FirstOrDefault(cc=>cc.Lang.Culture == request.Culture).Name,
                    ProductGroupId = c.ProductGroupsId,
                    ProductGroupName = c.ProductGroup.Name,
                    ProductId = c.Id,
                    ProductName = c.Name
                }).ToList();

                return ApiResult<List<ProductList_sp>>.CreateResponse(products);
            }
        }
    }
}
