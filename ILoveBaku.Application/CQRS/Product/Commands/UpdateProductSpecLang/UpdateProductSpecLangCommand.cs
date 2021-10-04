using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Product.Commands.UpdateProductSpecLang
{
    public class UpdateProductSpecLangCommand:BaseRequest<ApiResult<int?>>
    {
        public string LangName { get; set; }
        public List<ProductSpecLangValueDto> Model { get; set; }
        public class UpdateProductSpecLangCommandHandler : IRequestHandler<UpdateProductSpecLangCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateProductSpecLangCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<int?>> Handle(UpdateProductSpecLangCommand request, CancellationToken cancellationToken)
            {
                foreach (var item in request.Model)
                {
                   var oldValue = await _context.ProductsSpecificationsValuesStringsLangs.Where(c => !c.IsDeleted
                                                                                &&
                                                                                c.ProductsStockSpecificationsValuesStringsId == item.Id
                                                                                &&
                                                                                c.Langs.DisplayName == request.LangName).FirstOrDefaultAsync();

                    if(oldValue != null)
                    {
                        oldValue.IsDeleted = true;
                    }

                    var newValue = new Domain.Entities.ProductsSpecificationsValuesStringsLangs
                    {
                        ProductsStockSpecificationsValuesStringsId = item.Id,
                        IsDeleted = false,
                        LangsId = _context.Langs.Where(c => c.DisplayName == request.LangName).Select(c => c.Id).FirstOrDefault(),
                        Value = item.Value
                    };

                   await _context.ProductsSpecificationsValuesStringsLangs.AddAsync(newValue);

                }

                await _context.SaveChangesAsync();
                return ApiResult<int?>.CreateResponse(100);
            }
        }
    }
}
