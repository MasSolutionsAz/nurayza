using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Payment.Commands.Accept;
using ILoveBaku.Application.CQRS.Payment.Commands.Paid;
using ILoveBaku.Application.CQRS.ProductCashOut.Models;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Areas.Admin.Logics.Order;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> List()
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            var result = await _orderService.GetOrders(branchId);
            if (result == null)
                return RedirectToAction("Error", "Home");

            return View(result);
        }

        public async Task<IActionResult> Detail(int? productCashOutId)
        {
            if (productCashOutId == null)
                return RedirectToAction("List", "Order", new { area = "Admin" });

            var result = await _orderService.CreateVm(productCashOutId);
            if (result == null)
                return RedirectToAction("List", "Order", new { area = "Admin" });

            ViewBag.ProductCashOutId = productCashOutId;
            return View(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePacket(string Name)
        {
            var result = await _orderService.CreatePacket(Name);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePacketDetail(int? ProductCashOutId, int? packetId)
        {
            var result = await _orderService.CreatePacketDetail(ProductCashOutId, packetId);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AcceptOrder(int? ProductCashOutId)
        {
            if (ProductCashOutId == null)
                return Json(new
                {
                    status = 400,
                    error = "Xəta baş verdi."
                });

            var model = new AcceptCashOutRequestModel
            {
                ProductCashOutId = (int)ProductCashOutId
            };

            var result = await API.PostAsync<AcceptCashOutRequestModel, ApiResult<int?>>("payment/accept", model);

            if(result!=null && result.Succeeded)
            {
                return Json(new
                {
                    status = 200
                });
            }


            return Json(new
            {
                status = 400,
                error = "Xəta baş verdi."
            });
        }


        [HttpPost]
        public async Task<JsonResult> FinishOrder(int? ProductCashOutId)
        {
            if (ProductCashOutId == null)
                return Json(new
                {
                    status = 400,
                    error = "Xəta baş verdi."
                });

            var model = new PaymentRequestModel
            {
                ProductCashOutId = (int)ProductCashOutId,
                PaymentType = (int)PaymentType.WithoutCard
            };

            var result = await API.PostAsync<PaymentRequestModel, ApiResult<int?>>("payment/paid", model);

            if (result != null && result.Succeeded)
            {
                return Json(new
                {
                    status = 200
                });
            }


            return Json(new
            {
                status = 400,
                error = "Xəta baş verdi."
            });
        }
    }
}