using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
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

namespace ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts
{
    public class GetProductCashOutsQuery : BaseRequest<ApiResult<List<ProductCashOutDto>>>
    {
        public int BranchId { get; set; }
        public bool Profile { get; set; }
        public class GetProductCashOutsQueryHandler : IRequestHandler<GetProductCashOutsQuery, ApiResult<List<ProductCashOutDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public GetProductCashOutsQueryHandler(IApplicationDbContext context,
                                                  IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ApiResult<List<ProductCashOutDto>>> Handle(GetProductCashOutsQuery request, CancellationToken cancellationToken)
            {
                var cashDeskSeance = await _context.CashDeskSeance.Include(c => c.CashDesk)
                                                                    .ThenInclude(c => c.BranchesFloorsRelations)
                                                                    .ThenInclude(c => c.BranchesPlaces)
                                                                    .Where(c => c.CashDesk.BranchesFloorsRelations.BranchesPlaces.BranchesId == request.BranchId)
                                                                    .FirstOrDefaultAsync();
                if (cashDeskSeance == null)
                {
                    request.Errors.Add("seance", "Kassa tapılmadı.");
                    return ApiResult<List<ProductCashOutDto>>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                var productCashOuts = await (from ProductCashOut in _context.ProductsCashOut
                                             where ProductCashOut.CashDeskSeanceId == cashDeskSeance.Id &&
                                                   request.Profile ? (ProductCashOut.ProductsCashOutCards.UsersCards.UsersId == request.UserId)
                                                   : true
                                             select new ProductCashOutDto
                                             {
                                                 Name = ProductCashOut.ProductsCashOutCards.UsersCards.Users.Name,
                                                 Surname = ProductCashOut.ProductsCashOutCards.UsersCards.Users.Surname,
                                                 Status = ProductCashOut.ProductsCashOutStatuses.Name,
                                                 CashDeskSeanceId = ProductCashOut.CashDeskSeanceId,
                                                 Id = ProductCashOut.Id,
                                                 CreatedDate = ProductCashOut.CreatedDate,
                                                 Details =  _context.Exec<ProductCashOutDetailDto>("GetProductCashOutDetails", ProductCashOut.Id.ToString(),request.Culture).Result,
                                                 PaymentType = ProductCashOut.PaymentType
                                             }).OrderByDescending(c=>c.CreatedDate).ToListAsync();


                if (productCashOuts == null)
                {
                    request.Errors.Add("cash", "Sifariş tapılmadı.");
                    return ApiResult<List<ProductCashOutDto>>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                return ApiResult<List<ProductCashOutDto>>.CreateResponse(productCashOuts);
            }
        }
    }
}
