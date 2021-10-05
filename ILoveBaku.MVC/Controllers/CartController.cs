using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class CartController : BaseController
    {
        private readonly ILocService _locService;
        public CartController(ILocService locService)
        {
            _locService = locService;
        }
        [HttpPost]
        public async Task<JsonResult> Add(AddCartDetailVM model)
        {
            bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                var result = await AddToSession(model);
                return Json(result);
            }

            ApiResult<int> response = await API.PostAsync<AddCartDetailVM, ApiResult<int>>("carts", model);

            if (!response.Succeeded)
            {
                return Json(new
                {
                    status = response.Response,
                    error = _locService.GetLocalizedHtmlString(response.ErrorDetail.ErrorMessage).Value
                });
            }

            return Json(new
            {
                status = 200,
                cartId = response.Response
            });
        }
        private async Task<object> AddToSession(AddCartDetailVM model)
        {
            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails") ?? new List<CartDetailDto>();
            var lastId = cartDetails.Count != 0 ? cartDetails.LastOrDefault().Id : 0;
            var cartDetailResponse = await API.GetAsync<ApiResult<CartDetailDto>>($"carts/{model.ProductId}/{model.Count}/check/?last={lastId}");
            if (cartDetailResponse == null)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Xəta baş verdi").Value
                };


            if (!cartDetailResponse.Succeeded)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString(cartDetailResponse.ErrorList.FirstOrDefault().Value).Value
                };

            var cartDetail = cartDetailResponse.Response;
            if (cartDetails.Any(c => c.ProductId == cartDetail.ProductId))
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Məhsul artıq səbətdədir.").Value
                };
            else
                cartDetails.Add(cartDetail);

            HttpContext.Session.SetObject("cartDetails", cartDetails);
            return new
            {
                status = 200,
                cartId = cartDetail.Id
            };
        }

        [HttpPut]
        public async Task<JsonResult> Update(UpdateCartDetailVM model)
        {

            bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                var result = UpdateToSession(model);
                return Json(result);
            }
            ApiResult<int> response = await API.PutAsync<UpdateCartDetailVM, ApiResult<int>>($"carts/{model.CartDetailId}", model);

            if (!response.Succeeded)
            {
                return Json(new
                {
                    status = response.Response,
                    error = _locService.GetLocalizedHtmlString(response.ErrorDetail.ErrorMessage).Value
                });
            }

            return Json(new
            {
                status = 200,
                cartId = response.Response
            });
        }

        private object UpdateToSession(UpdateCartDetailVM model)
        {
            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails");
            if (cartDetails == null)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Sizin üçün ayrılımış sessiya müddəti bitmişdir.").Value
                };

            var cartDetail = cartDetails.FirstOrDefault(c => c.Id == model.CartDetailId);
            if(cartDetail == null)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Səbətdə belə bir məhsul tapılmadı.").Value
                };

            cartDetail.Count = model.Count;

            HttpContext.Session.SetObject("cartDetails", cartDetails);

            return new
            {
                status = 200,
                cartId = cartDetail.Id
            };
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(int cartDetaildId)
        {

            bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if(!isAuthenticated)
            {
                var result = DeleteToSession(cartDetaildId);
                return Json(result);
            }

            ApiResult<int> response = await API.DeleteAsync<ApiResult<int>>($"carts/{cartDetaildId}");

            if (!response.Succeeded)
            {
                return Json(new
                {
                    status = response.Response,
                    error = _locService.GetLocalizedHtmlString(response.ErrorDetail.ErrorMessage).Value
                });
            }

            return Json(new
            {
                status = 200,
                cartId = response.Response
            });
        }

        private object DeleteToSession(int cartDetaildId)
        {
            var cartDetails = HttpContext.Session.GetObject<List<CartDetailDto>>("cartDetails");
            if (cartDetails == null)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Sizin üçün ayrılımış sessiya müddəti bitmişdir.").Value
                };

            var cartDetail = cartDetails.FirstOrDefault(c => c.Id == cartDetaildId);
            if (cartDetail == null)
                return new
                {
                    status = 400,
                    error = _locService.GetLocalizedHtmlString("Səbətdə belə bir məhsul tapılmadı.").Value
                };

            cartDetails.Remove(cartDetail);
            HttpContext.Session.SetObject("cartDetails", cartDetails);

            return new
            {
                status = 200,
                cartId = cartDetaildId
            };
        }
    }
}