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

namespace ILoveBaku.Application.CQRS.News.Commands.DeleteNewsFile
{
    public class DeletNewsFileCommand:BaseRequest<ApiResult<string>>
    {
        public string Name { get; set; }
        public int NewsId { get; set; }
        public class DeletNewsFileCommandHandler : IRequestHandler<DeletNewsFileCommand, ApiResult<string>>
        {
            private readonly IApplicationDbContext _context;
            public DeletNewsFileCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<string>> Handle(DeletNewsFileCommand request, CancellationToken cancellationToken)
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

                NewsFiles newsFiles = await _context.NewsFiles.Where(c => c.FilesId == file.Id && c.NewsId  == request.NewsId).FirstOrDefaultAsync();
                if (newsFiles == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                string fileName = file.Name;

                _context.NewsFiles.Remove(newsFiles);
                _context.Files.Remove(file);

                await _context.SaveChangesAsync();
                return ApiResult<string>.CreateResponse(fileName);
            }
        }
    }
}
