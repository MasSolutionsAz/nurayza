using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.CartOrders.Models;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Application.CQRS.Payment.Commands.Paid;
using ILoveBaku.Application.CQRS.Payment.Commands.Pay;
using ILoveBaku.Application.CQRS.Payment.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice;
using ILoveBaku.Application.CQRS.User.Commands.AddManualUser;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using ILoveBaku.MVC.Services;
using ILoveBaku.MVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace ILoveBaku.MVC.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly KapitalPaymentService Kapital;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILocService _locService;
        private readonly IIdentityService _identityService;
        private readonly IUrlHelper _urlHelper;
        public PaymentController(KapitalPaymentService kapitalPaymentService, IConfiguration configuration, IEmailService emailService, ILocService locService, IIdentityService identityService, IUrlHelper urlHelper)
        {
            Kapital = kapitalPaymentService;
            _configuration = configuration;
            _emailService = emailService;
            _locService = locService;
            _identityService = identityService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("UnRegisteredCheckout", "Payment");

            var response = await API.GetAsync<ApiResult<List<CartDetailDto>>>("carts");
            var addressesResponse = await API.GetAsync<ApiResult<List<UserAddressInfoDto>>>("account/addresses");
            var userInfoResponse = await API.GetAsync<ApiResult<UserDto>>("account/details");
            var paymentTypesResponse = await API.GetAsync<ApiResult<List<PaymentTypeDto>>>("payment/types");
            ViewBag.Shipping = Convert.ToInt32(_configuration["Web:Shipping:Today"]);
            CheckoutVM model = new CheckoutVM()
            {
                PaymentTypes = (paymentTypesResponse != null && paymentTypesResponse.Succeeded) ? paymentTypesResponse.Response : new List<PaymentTypeDto>(),
                UserInfo = (userInfoResponse != null && userInfoResponse.Succeeded) ? userInfoResponse.Response : new UserDto(),
                Carts = response.Response ?? new List<CartDetailDto>(),
                Addresses = (addressesResponse != null && addressesResponse.Succeeded) ? addressesResponse.Response : new List<UserAddressInfoDto>()
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UnRegisteredCheckout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails") ?? new List<CartDetailDto>();
            var paymentTypesResponse = await API.GetAsync<ApiResult<List<PaymentTypeDto>>>("payment/types");
            var countriesResponse = await API.GetAsync<ApiResult<List<Countries>>>("countries");
            ViewBag.Shipping = Convert.ToInt32(_configuration["Web:Shipping:Today"]);
            CheckoutVM model = new CheckoutVM()
            {
                PaymentTypes = (paymentTypesResponse != null && paymentTypesResponse.Succeeded) ? paymentTypesResponse.Response : new List<PaymentTypeDto>(),
                Carts = cartDetails,
                Countries = (countriesResponse != null && countriesResponse.Succeeded) ? countriesResponse.Response : new List<Countries>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PayUnRegistered(UnRegisteredPaymentModel model)
        {
            //take cards from session
            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails");
            if (cartDetails == null)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Səbət boşdur.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            //end take cards from session


            //validation
            if (model.Phone.Length != 10)
                return Json(new
                {
                    status = 400,
                    error = " Nümunə :0501234567",
                    title = _locService.GetLocalizedHtmlString("Nömrə 10 simvol olmalıdır").Value,
                });

            if (!ModelState.IsValid)
                return Json(new
                {
                    status = 400,
                    error = ModelState.GetErrorsWithKey().FirstOrDefault().Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });
            //end validation

            if (model.ShipmentDate == null && Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.AnyDay)
            {
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Çatdırılma tarixini qeyd edin.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });
            }

            model.ShipmentDate = Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Today ? DateTime.Now.ToString("dd/MM/yyyy") : (Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Tomorrow ? DateTime.Now.AddDays(1).ToString("dd/MM/yyyy") : model.ShipmentDate);


            model.ResponseUrl = _urlHelper.Action("Profile", "Account", null, protocol: HttpContext.Request.Scheme);
            model.ResponseUrl += "#userInfo";
            model.ConfirmationUrl = _urlHelper.Action("ConfirmEmail", "Account", null, protocol: HttpContext.Request.Scheme);
            //create user
            var userCreateResult = await API.PostAsync<UnRegisteredPaymentModel, ApiResult<UserResponse>>("users", model);
            if (userCreateResult == null)
                return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });

            if (!userCreateResult.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString(userCreateResult.ErrorList.FirstOrDefault().Value).Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });
            //end create user

            //sign in user
            //await _identityService.SignInAsync(userCreateResult.Response);
            //HttpContext.Session.Remove("cartDetails");
            //end sign in user



            //map carts with user
            var cartMappingResult = await API.PostAsync<List<CartDetailDto>, ApiResult<int?>>($"carts/{userCreateResult.Response.Id}/map", cartDetails);
            if (cartMappingResult == null)
                return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });

            if (!cartMappingResult.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString(cartMappingResult.ErrorList.FirstOrDefault().Value).Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            //end map carts with user

            //make pay request model
            var shippingPrice = Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Today ? Convert.ToInt32(_configuration["Web:Shipping:Today"]) : 0;
            model.ShippingPrice = shippingPrice;

            var payModel = new PayCommandRequestModel
            {
                Name = model.Name,
                Surname = model.Surname,
                ShipmentOptions = model.ShipmentOptions,
                PaymentType = model.PaymentType,
                IsUnRegistered = true,
                ShippingPrice = shippingPrice.ToString(),
                Phone = model.Phone,
                ShipmentDate = model.ShipmentDate,
                AddressId = userCreateResult.Response.AddressId.ToString()
            };
            //end make pay request model


            //check if with card
            if (Convert.ToInt32(model.PaymentType) == (int)PaymentType.WithCard)
            {
                decimal amount = cartDetails.Sum(c => c.Price * c.Count);
                amount += shippingPrice;

                XmlDocument xmlDocument = await Kapital.PayAsync(amount, ShortCulture);

                if (xmlDocument.GetInnerText("Status") == "00")
                {
                    string orderId = xmlDocument.GetInnerText("OrderID");
                    string sessionId = xmlDocument.GetInnerText("SessionID");

                    payModel.OrderId = orderId;
                    payModel.SessionId = sessionId;
                    var payWithCardResult = await API.PostAsync<PayCommandRequestModel, ApiResult<PayCommandResponseModel>>($"payment/pay/{userCreateResult.Response.Id}", payModel);

                    if (payWithCardResult == null || !payWithCardResult.Succeeded)
                        return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });


                    HttpContext.Session.SetString("P_userId", payWithCardResult.Response.UsersId.ToString());
                    HttpContext.Session.SetString("P_cashOutId", payWithCardResult.Response.ProductCashOutId.ToString());
                    HttpContext.Session.SetString("P_totalPrice", payWithCardResult.Response.TotalPrice.ToString());
                    HttpContext.Session.SetString("P_paymentType", payModel.PaymentType);
                    HttpContext.Session.SetString("P_isUnRegistered", "true");

                    return Json(new
                    {
                        withCard = true,
                        status = 200,
                        data = $"{xmlDocument.GetInnerText("URL")}?ORDERID={orderId}&SESSIONID={sessionId}"
                    });
                }
                return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });
            }
            //end check if with card



            //pay request
            var payWithCashResult = await API.PostAsync<PayCommandRequestModel, ApiResult<PayCommandResponseModel>>($"payment/pay/{userCreateResult.Response.Id}", payModel);
            if (payWithCashResult == null)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Server xətası baş verdi.Zəhmət olmasa daha sonra yenidən cəhd edin.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });


            if (!payWithCashResult.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString(payWithCashResult.ErrorList.FirstOrDefault().Value).Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            //pay request

            HttpContext.Session.Remove("cartDetails");
            TempData["Payment"] = "true";
            return Json(new
            {
                withCard = false,
                status = 200,
                data = "/home/index"
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Pay(PayCommandRequestModel model)
        {

            if (!ModelState.IsValid)
                return Json(new
                {
                    status = 400,
                    error = ModelState.GetErrorsWithKey().FirstOrDefault().Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            var checkEmailConfirmation = await API.GetAsync<ApiResult<bool>>("account/confirmation");
            if (checkEmailConfirmation != null && !checkEmailConfirmation.Response)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("E-poçt ünvanı təsdiq edilməyib.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            if(model.ShipmentDate == null && Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.AnyDay)
            {
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Çatdırılma tarixini qeyd edin.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });
            }

            model.ShipmentDate = Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Today ? DateTime.Now.ToString("dd/MM/yyyy") : (Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Tomorrow ? DateTime.Now.AddDays(1).ToString("dd/MM/yyyy") : model.ShipmentDate);
            var shippingPrice = Convert.ToInt32(model.ShipmentOptions) == (int)ShipmentOptions.Today ? Convert.ToInt32(_configuration["Web:Shipping:Today"]) : 0;
            ApiResult<List<CartDetailDto>> cartDetailsResponse = await API.GetAsync<ApiResult<List<CartDetailDto>>>($"carts");
            if (cartDetailsResponse.IsNull() || !cartDetailsResponse.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = cartDetailsResponse?.ErrorDetail?.ErrorMessage ?? _locService.GetLocalizedHtmlString("Səbət boşdur.")
                });

            var cartDetails = cartDetailsResponse.Response;
            var emptyUsersId = Guid.Empty;
            //check if with card
            if (Convert.ToInt32(model.PaymentType) == (int)PaymentType.WithCard)
            {
                decimal amount = cartDetails.Sum(c => c.Price * c.Count);
                amount += shippingPrice;

                XmlDocument xmlDocument = await Kapital.PayAsync(amount, ShortCulture);

                if (xmlDocument.GetInnerText("Status") == "00")
                {
                    string orderId = xmlDocument.GetInnerText("OrderID");
                    string sessionId = xmlDocument.GetInnerText("SessionID");

                    model.OrderId = orderId;
                    model.SessionId = sessionId;

                    var payWithCardResult = await API.PostAsync<PayCommandRequestModel, ApiResult<PayCommandResponseModel>>($"payment/pay/{emptyUsersId}", model);

                    if (payWithCardResult == null || !payWithCardResult.Succeeded)
                        return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });


                    HttpContext.Session.SetString("P_userId", payWithCardResult.Response.UsersId.ToString());
                    HttpContext.Session.SetString("P_cashOutId", payWithCardResult.Response.ProductCashOutId.ToString());
                    HttpContext.Session.SetString("P_totalPrice", payWithCardResult.Response.TotalPrice.ToString());
                    HttpContext.Session.SetString("P_paymentType", model.PaymentType);


                    HttpContext.Response.Cookies.Append("Key", "Value", new CookieOptions()
                    {
                        SameSite = SameSiteMode.None,
                        Secure = true,
                    });

                    return Json(new
                    {
                        status = 200,
                        withCard = true,
                        data = $"{xmlDocument.GetInnerText("URL")}?ORDERID={orderId}&SESSIONID={sessionId}"
                    });
                }
                return Json(new { status = 400, error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value, title = _locService.GetLocalizedHtmlString("Xəta").Value });
            }
            //end check if with card



            //pay request
            var payWithCashResult = await API.PostAsync<PayCommandRequestModel, ApiResult<PayCommandResponseModel>>($"payment/pay/{emptyUsersId}", model);
            if (payWithCashResult == null)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Server xətası baş verdi.Zəhmət olmasa daha sonra yenidən cəhd edin.").Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });


            if (!payWithCashResult.Succeeded)
                return Json(new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString(payWithCashResult.ErrorList.FirstOrDefault().Value).Value,
                    title = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value,
                });

            //pay request


            return Json(new
            {
                status = 200,
                withCard = false,
                data = "/account/profile"
            });
        }

        public async Task<IActionResult> PaymentResult()
        {
            var userId = HttpContext.Session.GetString("P_userId");
            ApiResult<CartOrderDto> response = await API.GetAsync<ApiResult<CartOrderDto>>($"cartOrders/{userId}/last");

            if (response.IsNull() || !response.Succeeded || response.Response.IsNull())
                return Content(response?.ErrorDetail.ErrorMessage ?? "Failed");

            XmlDocument xmlDocument = await Kapital.CheckAsync(response.Response.OrderId, response.Response.SessionId);

            if (xmlDocument.GetInnerText("OrderStatus") == Enum.GetName(typeof(KapitalOrderStatus), KapitalOrderStatus.APPROVED))
            {

                var cashOutId = HttpContext.Session.GetString("P_cashOutId");
                var totalPrice = HttpContext.Session.GetString("P_totalPrice");
                var paymentType = HttpContext.Session.GetString("P_paymentType");

                if (userId == null || cashOutId == null || totalPrice == null || paymentType == null)
                {
                    //todo
                }

                var paidRequestModel = new PaymentRequestModel
                {
                    PaymentType = Convert.ToInt32(paymentType),
                    ProductCashOutId = Convert.ToInt32(cashOutId),
                    TotalPrice = Convert.ToDecimal(totalPrice),
                    UsersId = Guid.Parse(userId)
                };

                var paidResponse = await API.PostAsync<PaymentRequestModel, ApiResult<int>>($"payment/paid", paidRequestModel);
                HttpContext.Session.Remove("cartDetails");

                TempData["paid"] = "true";
                bool isUnRegistered = HttpContext.Session.GetString("P_isUnRegistered") != null;
                if (isUnRegistered)
                {
                    TempData["Payment"] = "true";
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Profile", "Account", null, "orders");
            }

            else if (xmlDocument.GetInnerText("OrderStatus") == Enum.GetName(typeof(KapitalOrderStatus), KapitalOrderStatus.DECLINED))
                return RedirectToAction("Decline", "Payment");
            else if (xmlDocument.GetInnerText("OrderStatus") == Enum.GetName(typeof(KapitalOrderStatus), KapitalOrderStatus.CANCELED))
                return RedirectToAction("Cancel", "Payment");

            return Content("Xeta bas verdi");
        }

        [HttpPost]
        public IActionResult Paid()
        {
            return RedirectToAction("PaymentResult", "Payment");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            var userId = HttpContext.Session.GetString("P_userId");
            var cashOutId = HttpContext.Session.GetString("P_cashOutId");
            var totalPrice = HttpContext.Session.GetString("P_totalPrice");
            var paymentType = HttpContext.Session.GetString("P_paymentType");

            var paidRequestModel = new PaymentRequestModel
            {
                PaymentType = Convert.ToInt32(paymentType),
                ProductCashOutId = Convert.ToInt32(cashOutId),
                TotalPrice = Convert.ToDecimal(totalPrice),
                UsersId = Guid.Parse(userId)
            };

            _ = await API.PostAsync<PaymentRequestModel,ApiResult<int>>($"payment/cancel", paidRequestModel);
            return Content("Cancelled");
        }
        [HttpPost]
        public async Task<IActionResult> Decline()
        {
            var userId = HttpContext.Session.GetString("P_userId");
            var cashOutId = HttpContext.Session.GetString("P_cashOutId");
            var totalPrice = HttpContext.Session.GetString("P_totalPrice");
            var paymentType = HttpContext.Session.GetString("P_paymentType");

            var paidRequestModel = new PaymentRequestModel
            {
                PaymentType = Convert.ToInt32(paymentType),
                ProductCashOutId = Convert.ToInt32(cashOutId),
                TotalPrice = Convert.ToDecimal(totalPrice),
                UsersId = Guid.Parse(userId)
            };

            _ = await API.PostAsync<PaymentRequestModel,ApiResult<int>>($"payment/decline", paidRequestModel);
            return Content("Declined");
        }
    }
}