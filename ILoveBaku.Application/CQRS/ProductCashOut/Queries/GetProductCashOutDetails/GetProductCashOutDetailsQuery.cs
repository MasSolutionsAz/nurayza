using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails
{
    public class GetProductCashOutDetailsQuery:BaseRequest<ApiResult<List<ProductCashOutDetailDto>>>
    {
        public int ProductCashOutId { get; set; }
        public class GetProductCashOutDetailsQueryHandler : IRequestHandler<GetProductCashOutDetailsQuery, ApiResult<List<ProductCashOutDetailDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductCashOutDetailsQueryHandler(IApplicationDbContext context,
                                                        IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductCashOutDetailDto>>> Handle(GetProductCashOutDetailsQuery request, CancellationToken cancellationToken)
            {

                var productCashOutDetails = await _context.Exec<ProductCashOutDetailDto>("GetProductCashOutDetails", request.ProductCashOutId.ToString(), request.Culture);


                productCashOutDetails.ForEach( c =>
                {
                    var barcode =  _context.ProductSpecificationValuesBarcodes
                                                            .Where(a => a.ProductsId == c.ProductsId && !a.IsDeleted)
                                                            .Select(a=>a.Value)
                                                            .FirstOrDefault();
                    c.Barcode = barcode;
                });

                return ApiResult<List<ProductCashOutDetailDto>>.CreateResponse(productCashOutDetails);
            }
        }
    }
}
