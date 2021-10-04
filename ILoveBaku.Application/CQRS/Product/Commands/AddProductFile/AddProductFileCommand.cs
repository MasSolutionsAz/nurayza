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

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductFile
{
    public class AddProductFileCommand:BaseRequest<ApiResult<PhotoModel>>
    {
        public ProductFileDto Model { get; set; }
        public int ProfuctId { get; set; }
        public class AddProductFileCommandHandler:IRequestHandler<AddProductFileCommand, ApiResult<PhotoModel>>
        {
            private readonly IApplicationDbContext _context;
            public AddProductFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResult<PhotoModel>> Handle(AddProductFileCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<PhotoModel>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Validation error"
                    });

                var fileType = request.Model.ContentType;
                string[] types = Enum.GetNames(typeof(FileType));
                string type = "";
                foreach(var t in types)
                {
                    if (fileType.Contains(t))
                    {
                        type = t;
                        break;
                    }
                }

                string fileName = Guid.NewGuid().ToString()+"."+ type;

                Files file = new Files
                {
                    CreatedDate = DateTime.Now,
                    FilesTypesId =type!=""?Convert.ToInt32(Enum.Parse(typeof(FileType), type)):1,
                    ContentLength = request.Model.Length,
                    FilesFoldersId =Convert.ToInt32(FileFolder.Products),
                    IsActive = true,
                    Name = fileName,
                    Path = request.Model.Path+"/"+fileName
                };

                await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();

                ProductsFiles productFile = new ProductsFiles()
                {
                    FilesId = file.Id,
                    ProductsId = request.ProfuctId,
                    IsMain = request.Model.IsMain,
                };

                await _context.ProductsFiles.AddAsync(productFile);
                await _context.SaveChangesAsync();

                PhotoModel response = new PhotoModel
                {
                    IsMain = request.Model.IsMain,
                    Name = file.Name
                };

                return ApiResult<PhotoModel>.CreateResponse(response);
            }
        }
    }
}
