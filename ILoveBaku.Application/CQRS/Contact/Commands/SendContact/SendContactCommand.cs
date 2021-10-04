using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Contact.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Contact.Commands.SendContact
{
    public class SendContactCommand : BaseRequest<ApiResult<int>>
    {
        public ContactVM Model { get; set; }

        public SendContactCommand(ContactVM model) => Model = model;

        public class SendContactCommandHandler : IRequestHandler<SendContactCommand, ApiResult<int>>
        {
            public IApplicationDbContext _context { get; set; }

            public SendContactCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<int>> Handle(SendContactCommand request, CancellationToken cancellationToken)
            {
                if (request.Errors.Count > 0)
                    return ApiResult<int>.CreateResponse(400, request.Errors, new ErrorDetail
                    {
                        ErrorMessage = "Contact Send error."
                    });

                ContactVM model = request.Model;

                Contacts contact = new Contacts()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Message = model.Message,
                    CreatedDate = DateTime.Now,
                    ContactsStatusesId = (byte)ContactStatus.OnHold
                };

                await _context.Contacts.AddAsync(contact);

                await _context.SaveChangesAsync();

                return ApiResult<int>.CreateResponse(200);
            }
        }
    }
}
