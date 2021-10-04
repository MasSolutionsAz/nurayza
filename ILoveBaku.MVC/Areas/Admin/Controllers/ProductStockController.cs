using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using ILoveBaku.Application.CQRS.Branches.Queries.GetSuppliers;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Domain.Enums;
using ILoveBaku.MVC.Areas.Admin.Logics.ProductStock;
using ILoveBaku.MVC.Areas.Admin.Models;
using ILoveBaku.MVC.Controllers;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class ProductStockController : BaseController
    {
        private readonly IProductStockService _productStockService;
        private readonly IConfiguration _configuration;
        private readonly int take = 20;
        public ProductStockController(IProductStockService productStockService, IConfiguration configuration)
        {
            _productStockService = productStockService;
            _configuration = configuration;
        }
        public async Task<IActionResult> List(int page = 1)
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            if (branchId == 0 || branchId == null)
                return RedirectToAction("Login", "Account");

            var result = await _productStockService.GetProductStocksByBranchId(branchId, page, take);
            if (result != null)
                return View(result);

            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> Process(int? productStockId)
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            if (branchId == null || branchId == 0)
                return RedirectToAction("List", "ProductStock", new { area = "Admin" });

            if (productStockId == null)
                return RedirectToAction("List", "ProductStock", new { area = "Admin" });

            var result = await _productStockService.GetProductStock(productStockId, branchId);
            if (result == null)
                return RedirectToAction("List", "ProductStock", new { area = "Admin" });

            return View(result);
        }

        [HttpPost]
        public async Task<JsonResult> Process(ProductStockDetailDto model)
        {
            var result = await _productStockService.UpdateProductStock(model);
            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> Add(List<int> products)
        {
            var result = await _productStockService.Add(products);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Transaction()
        {
            ProductTransactionFilter model = new ProductTransactionFilter
            {
                ProductTransactionType = (byte)ProductTransactionType.StockEntry
            };
            var transactionsResult = await API.GetAsync<ProductTransactionFilter, ApiResult<List<ProductTransactionDto>>>("transactions", model);
            if (transactionsResult != null && transactionsResult.Succeeded)
                return View(transactionsResult.Response);

            return View(new List<ProductTransactionDto>());
        }

        [HttpGet]
        public IActionResult CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateTransaction(ProductTransactionCreateDto model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = ModelState.GetErrorsWithKey()
                });

            model.ProductTransactionStatus = (byte)ProductTransactionStatus.OnHold;
            model.ProductTransactionType = (byte)ProductTransactionType.StockEntry;

            var createTransactionResult = await API.PostAsync<ProductTransactionCreateDto, ApiResult<int?>>("transactions", model);
            if (createTransactionResult != null && createTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = createTransactionResult.Response
                });

            if (createTransactionResult != null && !createTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = createTransactionResult.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTransaction(int? transactionId)
        {
            if (transactionId == null)
                return RedirectToAction("Transaction", "ProductStock");

            var transactionGetResult = await API.GetAsync<ApiResult<ProductTransactionDto>>($"transactions/{transactionId}/?transactionType="+(byte)ProductTransactionType.StockEntry);
            var suppliersResult = await API.GetAsync<ApiResult<List<SupplierDto>>>("branches/suppliers");
            var transactionDetailsResult = await API.GetAsync<ApiResult<List<ProductTransactionDetailsDto>>>($"transactions/{transactionId}/details");
            ProductTransationVm vm = new ProductTransationVm();
            if (suppliersResult!=null && transactionGetResult != null && transactionDetailsResult!=null)
            {
                ViewBag.TransactionFinished = transactionGetResult.Succeeded && transactionGetResult.Response.TransactionStatus == (byte)ProductTransactionStatus.Finished ?true:false;

                    vm = new ProductTransationVm
                {
                    Suppliers = suppliersResult.Succeeded?suppliersResult.Response:new List<SupplierDto>(),
                    Transaction = transactionGetResult.Succeeded?transactionGetResult.Response:new ProductTransactionDto(),
                    TransactionDetails = transactionDetailsResult.Succeeded? transactionDetailsResult.Response:new List<ProductTransactionDetailsDto>()
                };
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTransaction(ProductTransactionDto model)
        {
            var updateTransactionResult = await API.PutAsync<ProductTransactionDto, ApiResult<int?>>($"transactions/{model.Id}",model);
            if (updateTransactionResult != null && updateTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = updateTransactionResult.Response
                });

            if (updateTransactionResult != null && !updateTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = updateTransactionResult.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetSuppliers()
        {
            var suppliersResult = await API.GetAsync<ApiResult<List<SupplierDto>>>("branches/suppliers");
            if (suppliersResult != null && suppliersResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = suppliersResult.Response
                });

            return Json(new
            {
                status = 400
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetProductByBarcode(string barcode)
        {
            if (barcode == null)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = new Dictionary<string, string>() { {"error","Barkod daxil edilməyib." } }
                });

            int branchId = Convert.ToInt32(_configuration["OnlineBranchId"]); 
            var productStock = await API.GetAsync<ApiResult<ProductStockDetailDto>>($"products/{branchId}/products/{barcode}");
            if (productStock != null && productStock.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = new
                    {
                       buyAmount = productStock.Response.BuyAmount==null?0:productStock.Response.BuyAmount,
                       costAmount = productStock.Response.CostAmount==null?0:productStock.Response.CostAmount
                    }
                });

            if (productStock != null && !productStock.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = productStock.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddTransactionDetails(int? transactionId,ProductTransactionDetailsModel model)
        {
            if (transactionId == null || !ModelState.IsValid)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = ModelState.GetErrorsWithKey()
                });

            var addTransactionDetailsResult = await API.PostAsync<ProductTransactionDetailsModel,ApiResult<int?>>($"transactions/{transactionId}/details", model);
            if (addTransactionDetailsResult != null && addTransactionDetailsResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = addTransactionDetailsResult.Response
                });

            if (addTransactionDetailsResult != null && !addTransactionDetailsResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = addTransactionDetailsResult.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }


        [HttpGet]
        public async Task<JsonResult> DeleteTransactionDetail(int? transactionId ,int? transactionDetailId)
        {
            if (transactionId == null || transactionDetailId == null)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
                });


            var deleteResult = await API.DeleteAsync<ApiResult<int?>>($"transactions/{transactionId}/details/{transactionDetailId}");
            if (deleteResult != null && deleteResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = deleteResult.Response
                });

            if (deleteResult != null && !deleteResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = deleteResult.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }

        [HttpGet]
        public async Task<JsonResult> FinishTransaction(int? transactionId)
        {
            if(transactionId == null)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
                });


            ProductTransactionDto model = new ProductTransactionDto
            {
                Id = (int)transactionId
            };
            var finsihTransactionResult = await API.PutAsync<ProductTransactionDto, ApiResult<int?>>($"transactions/{transactionId}/?transactionStatus=" + (byte)ProductTransactionStatus.Finished,model);
            if (finsihTransactionResult != null && finsihTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.OK,
                    data = finsihTransactionResult.Response
                });

            if (finsihTransactionResult != null && !finsihTransactionResult.Succeeded)
                return Json(new
                {
                    status = HttpStatusCode.BadRequest,
                    errors = finsihTransactionResult.ErrorList
                });


            return Json(new
            {
                statis = 400,
                errors = new Dictionary<string, string>() { { "error", "Server xətası" } }
            });
        }
    }
}