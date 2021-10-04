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

namespace ILoveBaku.Application.CQRS.Product.Commands.DeleteProductFile
{
    public class DeleteProductFileCommand:BaseRequest<ApiResult<string>>
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public class DeleteProductFileCommandHandler : IRequestHandler<DeleteProductFileCommand, ApiResult<string>>
        {
            private readonly IApplicationDbContext _context;
            public DeleteProductFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<string>> Handle(DeleteProductFileCommand request, CancellationToken cancellationToken)
            {
                Files file = await _context.Files.Where(c => c.IsActive && c.Name == request.Name).FirstOrDefaultAsync();
                if(file == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                ProductsFiles productsFile = await _context.ProductsFiles.Where(c => c.FilesId == file.Id && c.ProductsId == request.ProductId).FirstOrDefaultAsync();
                if (productsFile == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                string fileName = file.Name;

                _context.ProductsFiles.Remove(productsFile);
                _context.Files.Remove(file);

                await _context.SaveChangesAsync();
                return ApiResult<string>.CreateResponse(fileName);
            }
        }
    }
}
