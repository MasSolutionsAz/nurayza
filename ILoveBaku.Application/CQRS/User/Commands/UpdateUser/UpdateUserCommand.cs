using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.UpdateUser
{
    public class UpdateUserCommand:BaseRequest<ApiResult<Guid?>>
    {
        public UserProfileInfoDto Model { get; set; }
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResult<Guid?>>
        {
            private readonly IApplicationDbContext _context;
            public UpdateUserCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<Guid?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(c => c.Id == request.UserId).FirstOrDefaultAsync();
                if(user == null)
                {
                    request.Errors.Add("", "Belə bir istifadəçi yoxdur.");
                    return ApiResult<Guid?>.CreateResponse(null, request.Errors);
                }

                user.Name = request.Model.Name;
                user.Surname = request.Model.Surname;
                user.ContactEmail = request.Model.Email;
                user.Phone = request.Model.Phone;
                user.Birthday = new DateTime(Convert.ToInt32(request.Model.Year), Convert.ToInt32(request.Model.Month), Convert.ToInt32(request.Model.Day));
                user.Gender = request.Model.Gender != null ? request.Model.Gender : null;

                await _context.SaveChangesAsync();
                return ApiResult<Guid?>.CreateResponse(user.Id);

            }
        }
    }
}
