using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Commands.RegisterUser;
using ILoveBaku.Application.CQRS.User.Commands.SendConfirmationEmail;
using ILoveBaku.Application.CQRS.User.Commands.UpdateUser;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Domain.Entities;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using ILoveBaku.MVC.Services;
using ILoveBaku.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IIdentityService = ILoveBaku.MVC.Services.IIdentityService;

namespace ILoveBaku.MVC.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IUrlHelper _urlHelper;
        private readonly IIdentityService _identityService;
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(IIdentityService identityService,
                                IConfiguration configuration,
                                IEmailSender emailSender,
                                IUrlHelper urlHelper,
                                ICaptchaValidator captchaValidator)
        {
            _captchaValidator = captchaValidator;
            _identityService = identityService;
            _configuration = configuration;
            _emailSender = emailSender;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [@Authorize]
        public async Task<IActionResult> Profile()
        {
            var branchId = Convert.ToInt32(_configuration["OnlineBranchId"]);

            UserProfileVm vm = new UserProfileVm();

            var userDtoResponse = await API.GetAsync<ApiResult<UserDto>>($"account/details");
            var ordersResponse = await API.GetAsync<ApiResult<List<ProductCashOutDto>>>($"products/{branchId}/cashOuts/?profile=" + true);
            var addressesResponse = await API.GetAsync<ApiResult<List<UserAddressInfoDto>>>("account/addresses");
            var countriesResponse = await API.GetAsync<ApiResult<List<Countries>>>("countries");

            if (userDtoResponse != null && ordersResponse != null && addressesResponse != null && countriesResponse != null)
            {
                vm.UserDto = userDtoResponse.Succeeded ? userDtoResponse.Response : new UserDto();
                vm.ProductCashOutDto = ordersResponse.Succeeded ? ordersResponse.Response : new List<ProductCashOutDto>();
                vm.Addresses = addressesResponse.Succeeded ? addressesResponse.Response : new List<UserAddressInfoDto>();
                vm.Countries = countriesResponse.Succeeded ? countriesResponse.Response : new List<Countries>();
                return View(vm);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginVM model, string captcha)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={Configuration["Web:ReCaptcha:SiteKey"]}&response={captcha}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                ModelState.AddModelError("", "Sorğu dayandırıldı.");
                return Json(new
                {
                    status = 400,
                    errors = ModelState.GetErrorsWithKey()
                });
            }

            if (!ModelState.IsValid)
                return Json(new
                {
                    status = 400,
                    errors = ModelState.GetErrorsWithKey()
                });

            ApiResult<UserResponse> response = await API.PostAsync<LoginVM, ApiResult<UserResponse>>("account/login", model);

            if (response.IsNull()) return Json(new { status = 400, error = "Xəta baş verdi." });

            if (!response.Succeeded)
                return Json(new
                {
                    status = 400,
                    errors = response?.ErrorList
                });

            await _identityService.SignInAsync(response.Response);

            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails");
            if (cartDetails != null)
            {
                var cartMappingResult = await API.PostAsync<List<CartDetailDto>, ApiResult<int?>>($"carts/{response.Response.Id}/map", cartDetails);
            }

            return Json(new
            {
                status = 200
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public Task ExternalLogin(string provider)
        {
            return HttpContext.ChallengeAsync(provider, new AuthenticationProperties() { RedirectUri = "/Account/ExternalLoginCallback/?provider=" + provider });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string provider)
        {
            var authResult = await HttpContext.AuthenticateAsync("TempCookie");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
            var name = authResult.Principal.FindFirstValue(ClaimTypes.GivenName);
            var surname = authResult.Principal.FindFirstValue(ClaimTypes.Surname);

            await HttpContext.SignOutAsync("TempCookie");
            UserDto userDto = new UserDto
            {
                Name = name,
                Surname = surname,
                Email = email,
                Provider = provider
            };

            var userLogin = await API.GetAsync<ApiResult<Guid?>>("account/login/?email=" + userDto.Email + "&isExternal=true" + "&provider=" + provider);
            if (userLogin != null && userLogin.Response != null)
            {
                var tryToSignIn = await API.PutAsync<ApiResult<UserResponse>>("account/externalUsers/?externalUserId=" + userLogin.Response);
                await _identityService.SignInAsync(tryToSignIn.Response);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var createUserResponse = await API.PostAsync<UserDto, ApiResult<Guid?>>("account/externalUsers", userDto);
                if (createUserResponse != null && createUserResponse.Response != null)
                {
                    var tryToSignIn = await API.PutAsync<ApiResult<UserResponse>>("account/externalUsers/?externalUserId=" + createUserResponse.Response);
                    if (tryToSignIn != null && tryToSignIn.Response != null)
                    {
                        await _identityService.SignInAsync(tryToSignIn.Response);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterVM model, string captcha)
        {

            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={Configuration["Web:ReCaptcha:SiteKey"]}&response={captcha}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                ModelState.AddModelError("", "Sorğu dayandırıldı.");
                return Json(new
                {
                    status = 400,
                    errors = ModelState.GetErrorsWithKey()
                });
            }

            if (!ModelState.IsValid)
                return Json(new
                {
                    status = 400,
                    errors = ModelState.GetErrorsWithKey()
                });

            ApiResult<UserResponse> response = await API.PostAsync<RegisterVM, ApiResult<UserResponse>>("account/register", model);

            if (response.IsNull() || !response.Succeeded)
            {
                return Json(new
                {
                    status = 400,
                    errors = response?.ErrorList
                });
            }

            //await _identityService.SignInAsync(response.Response);

            var emailSendModel = new SendConfirmEmailModel
            {
                ResponseUrl = _urlHelper.Action("ConfirmEmail", "Account", new
                {
                    userId = response.Response.Id.ToString()
                }, protocol: HttpContext.Request.Scheme),
                UserId = response.Response.Id
            };

            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails");
            if (cartDetails != null)
            {
                var cartMappingResult = await API.PostAsync<List<CartDetailDto>, ApiResult<int?>>($"carts/{response.Response.Id}/map", cartDetails);
            }

            var sendConfirmationEmail = await API.PostAsync<SendConfirmEmailModel, ApiResult<int?>>($"account/confirmation", emailSendModel);

            if (sendConfirmationEmail.Succeeded)
                TempData["EmailSended"] = "true";

            return Json(new
            {
                status = 200
            });
        }

        [HttpPost]
        [@Authorize(isAjax: true)]
        public async Task<JsonResult> UpdateProfileInfo(UserProfileInfoDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400, errors = ModelState.GetErrorsWithKey() });

            var response = await API.PutAsync<UserProfileInfoDto, ApiResult<Guid?>>("account/details", model);
            if (response == null)
                return Json(new { status = 400, errors = new Dictionary<string, string>() { { "", "Xəta baş verdi." } } });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400, errors = response.ErrorList });

            return Json(new { status = 200 });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            ApiResult<string> response = await API.PutAsync<ApiResult<string>>("account/logout");

            if (!response.IsNull() && response.Succeeded)
            {
                await _identityService.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPut]
        [@Authorize(isAjax: true)]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            ApiResult<int> response = await API.PutAsync<ChangePasswordVM, ApiResult<int>>("account/changePassword", model);

            if (response.IsNull() || !response.Succeeded)
            {
                return Json(new
                {
                    status = response?.Response,
                    error = response?.ErrorDetail?.ErrorMessage
                });
            }

            return Json(new
            {
                status = 200
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddAddress(UserAddressInfoDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400, errors = ModelState.GetErrorsWithKey() });

            var response = await API.PostAsync<UserAddressInfoDto, ApiResult<int?>>("account/addresses", model);
            if (response == null)
                return Json(new { status = 400 });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400, errors = response.ErrorList });

            return Json(new { status = 200, data = response.Response });
        }

        public async Task<JsonResult> GetCities(int? countryId)
        {
            if (countryId == null)
                return Json(new { status = 400 });

            var response = await API.GetAsync<ApiResult<List<Regions>>>($"countries/{countryId}/cities");
            if (response == null)
                return Json(new { status = 400 });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = response.Response });
        }

        public async Task<JsonResult> GetAddress(int? addressId)
        {
            if (addressId == null)
                return Json(new { status = 400 });

            var response = await API.GetAsync<ApiResult<UserAddressInfoDto>>($"account/addresses/{addressId}");
            if (response == null)
                return Json(new { status = 400 });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = response.Response });
        }

        [HttpPost]
        public async Task<JsonResult> EditAddress(UserAddressInfoDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = 400 });

            var response = await API.PutAsync<UserAddressInfoDto, ApiResult<int?>>($"account/addresses/{model.AddressId}", model);
            if (response == null)
                return Json(new { status = 400 });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = response.Response });

        }

        public async Task<JsonResult> DeleteAddress(int? addressId)
        {
            if (addressId == null)
                return Json(new { status = 400 });

            var response = await API.DeleteAsync<ApiResult<int?>>($"account/addresses/{addressId}");
            if (response == null)
                return Json(new { status = 400 });

            if (response != null && !response.Succeeded)
                return Json(new { status = 400 });

            return Json(new { status = 200, data = response.Response });

        }

        public async Task<JsonResult> SendResetPasswordEmail(string email)
        {
            if (email == null)
                return Json(new { status = 400, errors = new Dictionary<string, string> { { "error", "Düzgün email daxil edin." } } });

            var getRestDto = await API.GetAsync<ApiResult<ResetPasswordDto>>($"account/reset/{email}");
            if (getRestDto != null && getRestDto.Succeeded)
            {
                await SendResetPasswordEmailAsync(getRestDto.Response, email);
                return Json(new
                {
                    status = 200
                });
            }

            if (getRestDto != null && !getRestDto.Succeeded)
                return Json(new
                {
                    status = 400,
                    errors = getRestDto.ErrorList
                });


            return Json(new
            {
                status = 400,
                errors = new Dictionary<string, string> { { "error", "Server xətası." } }
            });

        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var checkTokenResult = await API.GetAsync<ApiResult<bool?>>($"account/tokens/{userId}/{token}");
            if (checkTokenResult != null && checkTokenResult.Succeeded)
            {
                HttpContext.Session.SetString("tempToken", token);
                HttpContext.Session.SetString("forgetPassword", "true");
                HttpContext.Session.SetString("userId", userId);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = 400,
                    errors = ModelState.GetErrorsWithKey()
                });

            var userId = HttpContext.Session.GetString("userId");
            var token = HttpContext.Session.GetString("tempToken");
            if (userId == null || token == null)
                return Json(new
                {
                    status = 400,
                    errors = new Dictionary<string, string> { { "error", "Əməliyyat təxirə salındı.Sesiya sona çatdı." } }
                });


            var updatePasswordResult = await API.PostAsync<ResetPasswordModel, ApiResult<Guid?>>($"account/reset/{userId}/{token}", model);
            if (updatePasswordResult != null && updatePasswordResult.Succeeded)
            {
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("tempToken");
                HttpContext.Session.Remove("forgetPassword");
                return Json(new
                {
                    status = 200
                });
            }

            if (updatePasswordResult != null && !updatePasswordResult.Succeeded)
            {
                return Json(new
                {
                    status = 400,
                    errors = updatePasswordResult.ErrorList
                });
            }


            return Json(new
            {
                status = 400
            });
        }

        private async Task SendResetPasswordEmailAsync(ResetPasswordDto model, string email)
        {
            var url = _urlHelper.Action("ResetPassword", "Account", new
            {
                userId = model.UserId.ToString(),
                token = model.Token
            }, protocol: HttpContext.Request.Scheme);

            var message = $"<a href='{url}'>Click to reset</a>";

            await _emailSender.SendEmailAsync(email, "Reset Password", url);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var confirmResult = await API.PutAsync<ApiResult<int?>>($"account/confirmation/{userId}/{token}");

            if (confirmResult != null && confirmResult.Succeeded)
            {
                TempData["EmailConfirmed"] = "true";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}