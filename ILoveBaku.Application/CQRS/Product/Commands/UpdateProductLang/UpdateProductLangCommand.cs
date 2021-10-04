using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductLangs;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.UpdateProductLang
{
    public class UpdateProductLangCommand:BaseRequest<ApiResult<int?>>
    {
        public ProductLangDto Model { get; set; }
        public class UpdateProductLangCommandHandler : IRequestHandler<UpdateProductLangCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProductLangCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateProductLangCommand request, CancellationToken cancellationToken)
            {
                var productLang = await _context.ProductsLangs.Where(c => c.Id == request.Model.Id).FirstOrDefaultAsync();
                if(productLang == null)
                {
                    request.Errors.Add("", "Xeta bas verdi!");
                }

                productLang.Description = request.Model.Description;
                productLang.Name = request.Model.Name;

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(productLang.Id);
            }
        }
    }
}
