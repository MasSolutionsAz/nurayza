using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Commands.AddCategoryFile
{
    public class AddCategoryFileCommand:BaseRequest<ApiResult<PhotoModel>>
    {
        public ProductFileDto Model { get; set; }
        public int CategoryId { get; set; }
        public class AddCategoryCommandHandler : IRequestHandler<AddCategoryFileCommand, ApiResult<PhotoModel>>
        {
            private readonly IApplicationDbContext _context;
            public AddCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<PhotoModel>> Handle(AddCategoryFileCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<PhotoModel>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Validation error"
                    });

                var fileType = request.Model.ContentType;
                string[] types = Enum.GetNames(typeof(FileType));
                string type = "";
                foreach (var t in types)
                {
                    if (fileType.Contains(t))
                    {
                        type = t;
                        break;
                    }
                }

                string fileName = Guid.NewGuid() +"."+type;

                Files file = new Files
                {
                    CreatedDate = DateTime.Now,
                    FilesTypesId = type != "" ? Convert.ToInt32(Enum.Parse(typeof(FileType), type)) : 1,
                    ContentLength = request.Model.Length,
                    FilesFoldersId = Convert.ToInt32(FileFolder.Categories),
                    IsActive = true,
                    Name = fileName,
                    Path = request.Model.Path + "/" + fileName
                };

                await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();

                CategoriesFiles categoriesFiles = new CategoriesFiles()
                {
                    FilesId = file.Id,
                    CategoriesId = request.CategoryId
                };

                await _context.CategoriesFiles.AddAsync(categoriesFiles);
                await _context.SaveChangesAsync();
                PhotoModel photoModel = new PhotoModel
                {
                    IsMain = request.Model.IsMain,
                    Name = file.Name
                };

                return ApiResult<PhotoModel>.CreateResponse(photoModel);
            }
        }
    }
}
