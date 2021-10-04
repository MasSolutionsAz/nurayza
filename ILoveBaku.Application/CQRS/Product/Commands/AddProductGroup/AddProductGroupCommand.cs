using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup
{
    public class AddProductGroupCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductGroupVm Model { get; set; }
        public class AddProductGroupCommandHandler : IRequestHandler<AddProductGroupCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(AddProductGroupCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int?>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Validation error"
                    });

                ProductGroups group = new ProductGroups
                {
                    CategoriesId = request.Model.CategoryId,
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    IsActive = true,
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
