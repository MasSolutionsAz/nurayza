using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Category.Commands.DeleteCategoryFile
{
    public class DeleteCategoryFileCommand : BaseRequest<ApiResult<string>>
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public class DeleteProductFileCommandHandler : IRequestHandler<DeleteCategoryFileCommand, ApiResult<string>>
        {
            private readonly IApplicationDbContext _context;
            public DeleteProductFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<string>> Handle(DeleteCategoryFileCommand request, CancellationToken cancellationToken)
            {
                Files file = await _context.Files.Where(c => c.IsActive && c.Name == request.Name).FirstOrDefaultAsync();
                if (file == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                CategoriesFiles categoriesFiles = await _context.CategoriesFiles.Where(c => c.FilesId == file.Id && c.CategoriesId == request.CategoryId).FirstOrDefaultAsync();
                if (categoriesFiles == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                string fileName = file.Name;

                _context.CategoriesFiles.Remove(categoriesFiles);
                _context.Files.Remove(file);

                await _context.SaveChangesAsync();
                return ApiResult<string>.CreateResponse(fileName);
            }
        }
    }
}
