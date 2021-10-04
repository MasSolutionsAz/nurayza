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

namespace ILoveBaku.Application.CQRS.ProductGroup.Commands.UpdateGroup
{
    public class UpdateGroupCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductGroupDto Model { get; set; }
        public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateGroupCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
            {
                var group = await _context.ProductGroups.Where(c => c.Id == request.Model.Id).FirstOrDefaultAsync();
                if(group == null)
                {
                    request.Errors.Add("", "Belə bir məhsul qrupu yoxdur.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }


                group.Name = request.Model.Name;
                group.IsActive = request.Model.IsActive;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(group.Id);
            }
        }
    }
}
