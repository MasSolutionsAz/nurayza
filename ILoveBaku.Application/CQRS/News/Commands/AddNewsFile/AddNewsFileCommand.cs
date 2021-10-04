using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.News.Commands.AddNewsFile
{
    public class AddNewsFileCommand:BaseRequest<ApiResult<PhotoModel>>
    {
        public ProductFileDto Model { get; set; }
        public int NewsId { get; set; }
        public class AddNewsFileCommandHandler : IRequestHandler<AddNewsFileCommand, ApiResult<PhotoModel>>
        {
            private readonly IApplicationDbContext _context;
            public AddNewsFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<PhotoModel>> Handle(AddNewsFileCommand request, CancellationToken cancellationToken)
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

                string fileName = Guid.NewGuid().ToString() + "." + type;

                Files file = new Files
                {
                    CreatedDate = DateTime.Now,
                    FilesTypesId = type != "" ? Convert.ToInt32(Enum.Parse(typeof(FileType), type)) : 1,
                    ContentLength = request.Model.Length,
                    FilesFoldersId = Convert.ToInt32(FileFolder.Portfolios),
                    IsActive = true,
                    Name = fileName,
                    Path = request.Model.Path + "/" + fileName
                };

                await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();

                NewsFiles newsFile = new NewsFiles()
                {
                    FilesId = file.Id,
                    NewsId = request.NewsId,
                    IsMain = request.Model.IsMain,
                    Priority = 0,
                    CreatedDate = DateTime.Now
                };

                await _context.NewsFiles.AddAsync(newsFile);
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
