using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;

namespace ILoveBaku.Application.CQRS.Payment.Commands.Paid
{
    public class PaymentInvoiceModel
    {
        public string Name { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public int PaymentType { get; set; }
        public string OrderAddress { get; set; }
        public List<CartDetailDto> Details { get; set; }
    }
    public class PaymentRequestModel
    {
        public Guid UsersId { get; set; }
        public int PaymentType { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductCashOutId { get; set; }
    }
    public class PaidCommand : BaseRequest<ApiResult<int>>
    {

        public PaymentRequestModel Model { get; set; }

        public class PaidCommandHandler : IRequestHandler<PaidCommand, ApiResult<int>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IRazorViewToStringRenderer _renderer;

            public PaidCommandHandler(IApplicationDbContext context, IRazorViewToStringRenderer renderer)
            {
                _context = context;
                _renderer = renderer;
            }

            public async Task<ApiResult<int>> Handle(PaidCommand request, CancellationToken cancellationToken)
            {
                if (request.Model.PaymentType == (int)PaymentType.WithCard)
                {
                    List<Cart> carts = await _context.Carts.Where(c => c.UserId == request.Model.UsersId && !c.IsDeleted &&
                                                         c.CartStatusId == (byte)CartStatus.OnPayment).ToListAsync();

                    var cart = carts.LastOrDefault();
                    if (cart.IsNull())
                        return ApiResult<int>.CreateResponse(400, null, new ErrorDetail()
                        { ErrorMessage = "Səbət boşdur." });

                    cart.CartStatusId = (byte)CartStatus.Paid;
                    cart.IsDeleted = true;

                    CartOrder cartOrder = await _context.CartOrders.OrderByDescending(co => co.CreatedDate)
                                                                         .FirstOrDefaultAsync(co => co.CartId == cart.Id);

                    if (cartOrder != null)
                        cartOrder.CartOrderStatusId = (byte)KapitalOrderStatus.APPROVED;
                }

                var productCashOut = await _context.ProductsCashOut.FirstOrDefaultAsync(c => c.Id == request.Model.ProductCashOutId);
                if (productCashOut == null)
                {
                    //todo
                }
                productCashOut.ProductsCashOutStatusesId = (int)ProductCashOutStatus.Paid;


                if (request.Model.TotalPrice == 0)
                {
                    request.Model.TotalPrice = productCashOut.ProductsCashOutDetails.Sum(c => c.Count * c.PayAmount);
                }

                ProductsCashOutPayments productCashOutPayment = new ProductsCashOutPayments()
                {
                    ProductsCashOutId = productCashOut.Id,
                    Amount = request.Model.TotalPrice,
                    CreatedDate = DateTime.Now,
                    ProductsCashOutPaymentsTypesId = request.Model.PaymentType == (int)PaymentType.WithCard ? (int)ProductCashOutPaymentType.PaymentByCard : (int)ProductCashOutPaymentType.CashPayment
                };

                await _context.ProductsCashOutPayments.AddAsync(productCashOutPayment);
                await _context.SaveChangesAsync();

                //send email
                var usersId = productCashOut.ProductsCashOutCards.UsersCards.UsersId;
                var userInfo = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == usersId);
                var from = new MailAddress("ilovebaku.official@gmail.com", "ILoveBaku Admin");
                var to = new MailAddress(userInfo.Email); //new MailAddress("info@ilovebaku.az")


                var shippingPacketDetail = await _context.ProductsCashOutShippingsPacketsDetails.FirstOrDefaultAsync(c => c.ProductsCashOutsId == request.Model.ProductCashOutId);
                var model = new PaymentInvoiceModel
                {
                    Name = userInfo.User.Name,
                    PaidDate = DateTime.Now,
                    Total = request.Model.TotalPrice,
                    PaymentType = request.Model.PaymentType,
                    ShippingPrice = shippingPacketDetail.Price,
                    Subtotal = request.Model.TotalPrice - shippingPacketDetail.Price,
                    Discount = 0,
                    TransactionId = productCashOut.TransactionId?.ToString(),
                    OrderAddress = await _context.ProductsCashOutAddresses
                                                    .Where(c => c.ProductsCashOutId == request.Model.ProductCashOutId)
                                                        .Select(c => c.UsersAddressInfo.Users.Name + " " + c.UsersAddressInfo.Users.Surname + ", " + c.UsersAddressInfo.Regions.Name + ", " + c.UsersAddressInfo.Users.Phone + ", " + c.UsersAddressInfo.Address + ", " + c.UsersAddressInfo.ZipCode)
                                                         .FirstOrDefaultAsync(),

                };

                var products = productCashOut.ProductsCashOutDetails.Select(c => c.ProductsId).Distinct();

                List<CartDetailDto> details = new List<CartDetailDto>();
                foreach (var item in products)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == item);
                    if (product != null)
                    {
                        details.Add(new CartDetailDto
                        {
                            Name = product.ProductsLangs.FirstOrDefault(c=>c.Langs.Culture == request.Culture).Name,
                            Count = (int)productCashOut.ProductsCashOutDetails.Where(c=>c.ProductsId == item).Sum(c=>c.Count),
                            Price = productCashOut.ProductsCashOutDetails.Where(c=>c.ProductsId == item).FirstOrDefault().PayAmount
                        });
                    }

                }

                model.Details = details;

                string view = "/Views/Payment/PaymentInfoToAdmin";
                var htmlBody = await _renderer.RenderViewToStringAsync($"{view}.cshtml", model);

                var message = new MailMessage(from, to)
                {
                    Subject = "Confirmation",
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

                return ApiResult<int>.CreateResponse(productCashOut.Id);
            }

            private async Task AddToPacket(int productCashOutId, int shippingPrice)
            {
                var packets = await _context.ProductsCashOutShippingsPackets.Where(c => c.Name.ToLower().Contains("online")).FirstOrDefaultAsync();

                if (packets != null)
                {
                    var packetDetail = new ProductsCashOutShippingsPacketsDetails
                    {
                        ProductsCashOutShippingsPacketsDetailsStatuses = (int)ProductCashOutShippingPacketDetailStatus.Aktiv,
                        ProductsCashOutShippingsPacketsId = packets.Id,
                        Price = shippingPrice,
                        ProductsCashOutsId = productCashOutId,
                        TrackingNumber = Guid.NewGuid()
                    };

                    await _context.ProductsCashOutShippingsPacketsDetails.AddAsync(packetDetail);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
