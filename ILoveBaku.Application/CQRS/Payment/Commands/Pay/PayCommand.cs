using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.CartOrders.Models;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CartStatus = ILoveBaku.Domain.Enums.CartStatus;
using KapitalOrderStatus = ILoveBaku.Domain.Enums.KapitalOrderStatus;

namespace ILoveBaku.Application.CQRS.Payment.Commands.Pay
{
    public class PayCommandRequestModel
    {
        [Required(ErrorMessage = "Zəhmət olmasa address qeyd edin.")]
        public string AddressId { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa ödəniş növünü qeyd edin.")]
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa çatdırılma növünü qeyd edin.")]
        public string ShipmentOptions { get; set; }
        public string ShippingPrice { get; set; }
        public string ShipmentDate { get; set; }

        public bool IsUnRegistered { get; set; }

        public string OrderId { get; set; }
        public string SessionId { get; set; }


        [Required(ErrorMessage = "Nömrə daxil edilməlidir.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Telefon nömrəsi düzgün deyil.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Ad boş qala bilməz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad boş qala bilməz.")]
        public string Surname { get; set; }

    }
    public class PayCommandResponseModel
    {
        public int ProductCashOutId { get; set; }
        public Guid UsersId { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class PayCommand : BaseRequest<ApiResult<PayCommandResponseModel>>
    {
        public PayCommandRequestModel Model { get; set; }
        public Guid RequestUsersId { get; set; }

        public class PayCommandHandler : IRequestHandler<PayCommand, ApiResult<PayCommandResponseModel>>
        {
            private readonly IApplicationDbContext _context;

            public PayCommandHandler(IApplicationDbContext context) => _context = context;

            public async Task<ApiResult<PayCommandResponseModel>> Handle(PayCommand request, CancellationToken cancellationToken)
            {
                var branchId = 1;
                var cashDescId = 1;

                Guid userId = request.RequestUsersId == Guid.Empty ? request.UserId : request.RequestUsersId;

                Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted &&
                                                          c.CartStatusId == (byte)CartStatus.OnHold);

                if (!request.Model.IsUnRegistered)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

                    if (user != null)
                    {
                        user.Name = request.Model.Name;
                        user.Surname = request.Model.Surname;
                        user.Phone = request.Model.Phone;
                    }
                }


                if (cart.IsNull())
                    return ApiResult<PayCommandResponseModel>.CreateResponse(null, null, new ErrorDetail()
                    { ErrorMessage = "Səbət boşdur." });

                if(Convert.ToInt32(request.Model.PaymentType) == (int)PaymentType.WithCard)
                {
                    CartOrder cartOrder = new CartOrder()
                    {
                        OrderId = request.Model.OrderId,
                        SessionId = request.Model.SessionId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CartOrderStatusId = (byte)KapitalOrderStatus.CREATED,
                        CartId = cart.Id,
                    };

                    await _context.CartOrders.AddAsync(cartOrder);
                    await _context.SaveChangesAsync();
                }


                ProductsCashOut productCashOut = new ProductsCashOut()
                {
                    TransactionId = Guid.NewGuid(),
                    CashDeskSeanceId = (await _context.CashDeskSeance
                                                         .FirstOrDefaultAsync(cds => cds.CashDeskId == cashDescId)).Id,
                    Description = "Online alis-veris.",
                    CreatedDate = DateTime.Now,
                    ProductsCashOutStatusesId = (byte)ProductCashOutStatus.OnHold,
                    PaymentType = Convert.ToInt32(request.Model.PaymentType)
                };

                await _context.ProductsCashOut.AddAsync(productCashOut);
                await _context.SaveChangesAsync();

                List<CartDetail> cartDetails = cart.CartDetails.ToList();

                decimal totalPrice = 0;
                foreach (var cartDetail in cartDetails)
                {

                    ProductsStock productStock = cartDetail.Product.ProductsStocks.FirstOrDefault(ps => ps.BranchesId == branchId);



                    decimal price = productStock.GetPrice();

                    decimal discountedPrice = price.PercentReductionOf(productStock.ProductsStockDiscountsDetails?.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                                      .Sum(ps => ps.ProductsStockDiscounts.DiscountValue) ?? default).Round(2);

                    totalPrice += (discountedPrice * cartDetail.Count);

                    for (int i = 0; i < cartDetail.Count; i++)
                    {
                        ProductsCashOutDetails productCashOutDetail = new ProductsCashOutDetails()
                        {
                            ProductsCashOutId = productCashOut.Id,
                            ProductsId = cartDetail.ProductId,
                            Count = 1,
                            SaleAmount = price,
                            TaxPercent = cartDetail.Product.TaxPercent ?? 0,
                            DiscountPercent = price - discountedPrice,
                            PayAmount = discountedPrice,
                            CreatedDate = DateTime.Now
                        };

                        await _context.ProductsCashOutDetails.AddAsync(productCashOutDetail);
                    }
                }

                if (Convert.ToInt32(request.Model.PaymentType) != (int)ILoveBaku.Domain.Enums.PaymentType.WithCard)
                {
                    cart.CartStatusId = (byte)CartStatus.Paid;
                    cart.IsDeleted = true;
                }
                else
                {
                    cart.CartStatusId = (byte)CartStatus.OnPayment;
                }


                ProductsCashOutAddresses productCashOutAddress = new ProductsCashOutAddresses()
                {
                    ProductsCashOutId = productCashOut.Id,
                    UsersAddressInfoId = Convert.ToInt32(request.Model.AddressId)
                };

                await _context.ProductsCashOutAddresses.AddAsync(productCashOutAddress);

                ProductsCashOutCards productCashOutCard = new ProductsCashOutCards()
                {
                    ProductsCashOutId = productCashOut.Id,
                    UsersCardsId = (await _context.UsersCards.FirstOrDefaultAsync(uc => uc.UsersId == userId)).Id
                };

                await _context.ProductsCashOutCards.AddAsync(productCashOutCard);

                await _context.SaveChangesAsync();
                await AddToPacket(productCashOut.Id, Convert.ToInt32(request.Model.ShippingPrice),request.Model.ShipmentDate);

                return ApiResult<PayCommandResponseModel>.CreateResponse(new PayCommandResponseModel
                {
                    ProductCashOutId = productCashOut.Id,
                    TotalPrice = totalPrice,
                    UsersId = userId
                });
            }

            private async Task AddToPacket(int productCashOutId, int shippingPrice,string shipmentDate)
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
                        TrackingNumber = Guid.NewGuid(),
                        WantedShipmentDate = shipmentDate
                    };

                    await _context.ProductsCashOutShippingsPacketsDetails.AddAsync(packetDetail);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
