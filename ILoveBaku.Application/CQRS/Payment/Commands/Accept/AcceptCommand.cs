using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.Payment.Commands.Accept
{
    public class AcceptedMailModel
    {
        public string TransactionId { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
    public class AcceptCashOutRequestModel
    {
        public int ProductCashOutId { get; set; }
        public string ImageUrl { get; set; }
    }
    public class AcceptCommand:BaseRequest<ApiResult<int?>>
    {
     
        public AcceptCashOutRequestModel Model { get; set; }
        public class AcceptCommandHandler : IRequestHandler<AcceptCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IRazorViewToStringRenderer _renderer;

            public AcceptCommandHandler(IApplicationDbContext context, IRazorViewToStringRenderer renderer)
            {
                _context = context;
                _renderer = renderer;
            }
            public async Task<ApiResult<int?>> Handle(AcceptCommand request, CancellationToken cancellationToken)
            {
                var productCashOut = await _context.ProductsCashOut.FirstOrDefaultAsync(c => c.Id == request.Model.ProductCashOutId && c.ProductsCashOutStatusesId == (byte)ProductCashOutStatus.OnHold);
                if (productCashOut == null)
                    return ApiResult<int?>.CreateResponse(null, request.Errors);

                productCashOut.ProductsCashOutStatusesId = (byte)ProductCashOutStatus.Accepted;
                await _context.SaveChangesAsync();
                var model = new AcceptedMailModel();
                model.TransactionId = productCashOut.TransactionId.ToString();
                model.AcceptedDate = DateTime.Now;

                var userId = productCashOut.ProductsCashOutCards.UsersCards.UsersId;
                var userLogin = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == userId);
                //send email
                var from = new MailAddress("ilovebaku.official@gmail.com", "ILoveBaku Admin");
                var to = new MailAddress(userLogin.Email); //new MailAddress("info@ilovebaku.az")


                string view = "/Views/Payment/AcceptOrder";
                var htmlBody = await _renderer.RenderViewToStringAsync($"{view}.cshtml", model);

                var message = new MailMessage(from, to)
                {
                    Subject = "Order Info",
                    Body = htmlBody
                };

                message.AlternateViews.Add(
                  AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html));

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("ilovebaku.official@gmail.com", "ilovebaku123@");
                    await smtp.SendMailAsync(message);
                }

                //end send email

                return ApiResult<int?>.CreateResponse(200);

            }
        }
    }
}
