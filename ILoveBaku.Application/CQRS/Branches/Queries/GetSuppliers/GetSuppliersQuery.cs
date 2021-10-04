using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
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

namespace ILoveBaku.Application.CQRS.Branches.Queries.GetSuppliers
{
    public class GetSuppliersQuery : BaseRequest<ApiResult<List<SupplierDto>>>
    {
        public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, ApiResult<List<SupplierDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetSuppliersQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
            {
                var suppliers = await _context.Suppliers.Where(c => c.CompanyDetails.CompanyDetailsTypesId == (byte)CompanyDetailType.Firma)
                                                           .Select(c => new SupplierDto
                                                           {
                                                               Id = c.Id,
                                                               Name = c.CompanyDetails.Name
                                                           })
                                                                .ToListAsync();

                suppliers = suppliers ??= new List<SupplierDto>();
                return ApiResult<List<SupplierDto>>.CreateResponse(suppliers);
            }
        }
    }
}
