using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.ProductGroup.Commands.AddProductGroup
{
    public class AddProductGroupCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductGroupDto Model { get; set; }
        public class AddProductGroupCommandHandler : IRequestHandler<AddProductGroupCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddProductGroupCommand request, CancellationToken cancellationToken)
            {

                var group = new ProductGroups
                {
                    CategoriesId = (int)request.Model.CategoriesId,
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    IsActive = request.Model.IsActive,
                    Name = request.Model.Name,
                    UpdatedDate = DateTime.Now
                };

                await _context.ProductGroups.AddAsync(group);
                await _context.SaveChangesAsync();

                return ApiResult<int?>.CreateResponse(group.Id);
            }
        }
    }
}
