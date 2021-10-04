using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Branches.Queries.GetBranches
{
    public class GetBranchesQuery : BaseRequest<ApiResult<List<BranchesDto>>>
    {
        public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, ApiResult<List<BranchesDto>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IConfiguration _configuration;
            public GetBranchesQueryHandler(IApplicationDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
            public async Task<ApiResult<List<BranchesDto>>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
            {
                var branchId = Convert.ToInt32(_configuration["OnlineBranchId"]);
                var data = await _context.Branches.Select(c => new BranchesDto
                {
                    Address = c.CompanyDetails.Address,
                    ContactEmail = c.CompanyDetails.Email,
                    IsOnlineBranch = c.Id == branchId?true:false ,
                    Phone = c.CompanyDetails.Phone,
                    Addresses =  _context.Branches.Select(a=>a.CompanyDetails.Address).ToList()
                }).ToListAsync();

                return ApiResult<List<BranchesDto>>.CreateResponse(data);
            }
        }
    }
}
