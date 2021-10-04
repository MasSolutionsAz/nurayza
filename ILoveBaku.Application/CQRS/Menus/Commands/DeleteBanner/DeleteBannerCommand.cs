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

namespace ILoveBaku.Application.CQRS.Menus.Commands.DeleteBanner
{
    public class DeleteBannerCommand : BaseRequest<ApiResult<string>>
    {
        public string Name { get; set; }
        public int MenuId { get; set; }
        public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, ApiResult<string>>
        {
            private readonly IApplicationDbContext _context;
            public DeleteBannerCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<string>> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
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

                MenuBannerItems menuBanner = await _context.MenuBannerItems.Where(c => c.FilesId == file.Id && c.MenuId == request.MenuId).FirstOrDefaultAsync();
                if (menuBanner == null)
                {
                    request.Errors.Add("File", "Belə bir file mövcud deyil.");
                    return ApiResult<string>.CreateResponse(null, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Error"
                    });
                }

                string fileName = file.Name;

                _context.MenuBannerItems.Remove(menuBanner);
                _context.Files.Remove(file);

                await _context.SaveChangesAsync();
                return ApiResult<string>.CreateResponse(fileName);
            }
        }
    }
}
