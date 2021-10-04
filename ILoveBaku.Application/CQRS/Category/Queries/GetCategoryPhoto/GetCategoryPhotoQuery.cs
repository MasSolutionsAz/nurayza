using AutoMapper;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
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

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryPhoto
{
    public class GetCategoryPhotoQuery : BaseRequest<ApiResult<List<ProductFileDto>>>
    {
        public int CategoryId { get; set; }

        public class GetCategoryParentsListQueryHandler : IRequestHandler<GetCategoryPhotoQuery, ApiResult<List<ProductFileDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetCategoryParentsListQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<ProductFileDto>>> Handle(GetCategoryPhotoQuery request, CancellationToken cancellationToken)
            {
                var photos = await _context.CategoriesFiles.Where(c => c.CategoriesId == request.CategoryId).Select(c => new ProductFileDto
                {
                    Name = c.Files.Name,
                    IsMain = c.Files.FileTypes.FilesTypesGroupsId == (byte)FileTypeGroup.Picture ? true : false,
                    Path = c.Files.Path
                }).ToListAsync();


                return ApiResult<List<ProductFileDto>>.CreateResponse(photos);
            }
        }
    }
}
