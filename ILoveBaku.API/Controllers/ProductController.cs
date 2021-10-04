using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductToStock;
using ILoveBaku.Application.CQRS.Product.Commands.DeleteProductFile;
using ILoveBaku.Application.CQRS.Product.Commands.UpdateProduct;
using ILoveBaku.Application.CQRS.Product.Commands.UpdateProductStock;
using ILoveBaku.Application.CQRS.Product.Queries.GetLastProductBarcode;
using ILoveBaku.Application.CQRS.Product.Queries.GetOutOfStockProducts;
using ILoveBaku.Application.CQRS.Product.Queries.GetOutOfStockProductsCount;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductById;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroupsByCategoryId;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductPhotos;
using ILoveBaku.Application.CQRS.Product.Queries.GetProducts;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductStock;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductStocks;
using ILoveBaku.Application.CQRS.ProductCashOut.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetUserCardByProductCashOutId;
using ILoveBaku.Domain.Enums;
using ILoveBaku.Domain.Stored_Procedures;
using Microsoft.AspNetCore.Mvc;
using ILoveBaku.Application.CQRS.Product.Queries.GetSearchedProductStocks;
using ILoveBaku.Application.CQRS.Product.Queries.GetCoomingSoonProducts;
using ILoveBaku.Application.CQRS.Product.Queries.GetSearchFilterProductStocks;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductFilters;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductReviews;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductReview;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecifications;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductByBarcode;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductLangs;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductSpecLangs;
using ILoveBaku.Application.CQRS.Product.Commands.UpdateProductLang;
using ILoveBaku.Application.CQRS.Product.Commands.UpdateProductSpecLang;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<ProductAdminListVm>>> GetProducts(int page)
        {
            return await Mediator.Send(new GetProductsQuery() { Page = page});
        }

        [HttpGet("soon")]
        public async Task<ActionResult<ApiResult<List<ProductStockDto>>>> GetCoomingSoonProducts()
        {
            return await Mediator.Send(new GetCommingSoonProductQuery());
        }

        [HttpGet("{productId}/langs")]
        public async Task<ActionResult<ApiResult<List<ProductLangDto>>>> GetProductLangs(int productId)
        {
            return await Mediator.Send(new GetProductLangsQuery() { ProductId = productId });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<ProductVm>>> GetProductById(int id)
        {
            return await Mediator.Send(new GetProuctByIdQuery { ProductId = id });
        }
        [HttpGet("{id}/specifications")]
        public async Task<ActionResult<ApiResult<ProductSpecificationModel>>> GetProductSpecifications(int id)
        {
            return await Mediator.Send(new GetProductSpecificationsQuery { ProductId = id });
        }
        [HttpGet("{id}/specifications/langs")]
        public async Task<ActionResult<ApiResult<List<ProductSpecLangsDto>>>> GetProductSpecLangs(int id)
        {
            return await Mediator.Send(new GetProductSpecLangsQuery() { ProductId = id });
        }

        [HttpGet("{branchId}/stocks/{productName}/reviews")]
        public async Task<ActionResult<ApiResult<List<ProductReviewDto>>>> GetReviews(int branchId,string productName)
        {
            return await Mediator.Send(new GetProductReviewsQuery { ProductName = productName, BranchId = branchId });
        }

        [HttpGet("{branchId}/stocks")]
        public async Task<ActionResult<ApiResult<ProductStocksVM>>> GetProductStocks(int branchId, int categoryId, ProductStockSaleAmountType ProductStockSaleAmountType, ProductStockStatus productStockStatus, int take = 10, int page = 1)
        {
            return await Mediator.Send(new GetProductStocksQuery()
            {
                BranchId = branchId,
                CategoryId = categoryId,
                ProductStockSaleAmountType = ProductStockSaleAmountType,
                ProductStockStatus = productStockStatus,
                Take = take,
                Page = page
            });
        }

        [HttpGet("invoice/{cashOutId}")]
        public async Task<ActionResult<ApiResult<ProductInvoiceVm>>> GetProductInvoice(int cashOutId)
        {
            return await Mediator.Send(new GetProductInvoiceQuery { CashOutId = cashOutId });
        }

        #region Search and SearchFilter
        [HttpGet("search")]
        public async Task<ActionResult<ApiResult<List<ProductStockDto>>>> Search(string key, [FromQuery] string categories)
        {
            return await Mediator.Send(new GetSearchedProductStocksQuery(key, categories));
        }

        [HttpGet("searchFilter")]
        [HttpGet("searchFilter/{categoryName}")]
        public async Task<ActionResult<ApiResult<ProductListDto>>> SearchFilter(string categoryName, [FromQuery] Dictionary<string, string> filters)
        {
            return await Mediator.Send(new GetSearchFilterProductStocksQuery(categoryName, filters));
        }

        [HttpGet("filters")]
        public async Task<ActionResult<ApiResult<ProductFiltersDto>>> GetProductFilters(int categoryId)
        {
            return await Mediator.Send(new GetProductFiltersQuery(categoryId));
        }

        //[HttpGet("filters")]
        //[HttpGet("filters/{categoryName}")]
        //public async Task<ActionResult<ApiResult<ProductFiltersDto>>> GetProductFilters(string categoryName, [FromQuery] Dictionary<string, string> filters)
        //{
        //    return await Mediator.Send(new GetProductFiltersQuery(categoryName, filters));
        //}
        #endregion

        [HttpGet("{branchId}/outOfStock")]
        public async Task<ActionResult<ApiResult<List<ProductList_sp>>>> GetOutOfStockProducts(int branchId)
        {
            return await Mediator.Send(new GetOutOfStockProductsQuery { BranchId = branchId });
        }

        [HttpGet("{branchId}/stocks/{productStockId}")]
        public async Task<ActionResult<ApiResult<ProductStockVM>>> GetProductStock(int branchId, int productStockId, string productName, ProductStockStatus productStockStatus,string barcode)
        {
            return await Mediator.Send(new GetProductStockQuery(branchId, productStockId, productName, productStockStatus));
        }

        [HttpGet("{branchId}/products/{barcode}")]
        public async Task<ActionResult<ApiResult<ProductStockDetailDto>>> GetProductStockByBarcode(string barcode)
        {
            return await Mediator.Send(new GetProductStockByBarcodeQuery { Barcode = barcode });
        }

        [HttpGet("{branchId}/outOfStockCount")]
        public async Task<ActionResult<ApiResult<int?>>> GetOutOfStockProductCount(int branchId)
        {
            return await Mediator.Send(new GetOutOfStockProductsCountQuery { BranchId = branchId });
        }

        [HttpGet("groups")]
        public async Task<ActionResult<ApiResult<List<ProductGroupDto>>>> GetProductGroups()
        {
            return await Mediator.Send(new GetProductGroupsQuery());
        }

        [HttpGet("{categoryId}/groups")]
        public async Task<ActionResult<ApiResult<List<ProductGroupDto>>>> GetProductGroupsByCategoryId(int categoryId)
        {
            return await Mediator.Send(new GetProductGroupsByCategoryIdQuery { CategoryId = categoryId });
        }

        [HttpGet("barcode")]
        public async Task<ActionResult<ApiResult<string>>> GetLastProductBarcode()
        {
            return await Mediator.Send(new GetLastProductBarcodeQuery());
        }

        [HttpGet("{branchId}/cashOuts")]
        public async Task<ActionResult<ApiResult<List<ProductCashOutDto>>>> GetProductCashOuts(int branchId, bool profile)
        {
            return await Mediator.Send(new GetProductCashOutsQuery { BranchId = branchId, Profile = profile });
        }

        [HttpGet("cashOuts/{productCashOutId}/details")]
        public async Task<ActionResult<ApiResult<List<ProductCashOutDetailDto>>>> GetProductCashOutDetails(int productCashOutId)
        {
            return await Mediator.Send(new GetProductCashOutDetailsQuery { ProductCashOutId = productCashOutId });
        }

        [HttpGet("{productId}/files")]
        public async Task<ActionResult<ApiResult<List<ProductFileDto>>>> GetProductPhotos(int productId)
        {
            return await Mediator.Send(new GetProductPhotosQuery { ProductId = productId });
        }

        [HttpGet("cashOuts/{productCashOutId}/card")]
        public async Task<ActionResult<ApiResult<ProductCashOutCardDto>>> GetUserCardByCashOutId(int? productCashOutId)
        {
            return await Mediator.Send(new GetUserCardByProductCashOutId { ProductCashOutId = productCashOutId });

        }

        [HttpPost("{productId}/files")]
        public async Task<ActionResult<ApiResult<PhotoModel>>> AddProductFile(int productId, ProductFileDto model)
        {
            return await Mediator.Send(new AddProductFileCommand { ProfuctId = productId, Model = model });
        }

        [HttpPost("groups")]
        public async Task<ActionResult<ApiResult<int?>>> AddProductGroup(ProductGroupVm model)
        {
            return await Mediator.Send(new AddProductGroupCommand { Model = model });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<int?>>> AddProduct(ProductVm model)
        {
            return await Mediator.Send(new AddProductCommand { Model = model });
        }

        [HttpPost("{branchId}/stocks")]
        public async Task<ActionResult<ApiResult<int?>>> AddProductToStock(int productId, int branchId)
        {
            return await Mediator.Send(new AddProductToStockCommand { ProductId = productId, BranchId = branchId });
        }

        [HttpPost("{branchId}/stocks/{productName}/reviews")]
        public async Task<ActionResult<ApiResult<ProductReviewDto>>> AddReview(int branchId,string productName,ProductReviewDto model)
        {
            return await Mediator.Send(new AddProductReviewCommand { Model = model,BranchId = branchId,ProductName = productName });
        }
        [HttpPut("langs/{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateProductLang(int id,ProductLangDto model)
        {
            model.Id = id;
            return await Mediator.Send(new UpdateProductLangCommand { Model = model });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateProduct(ProductVm model, int productId)
        {
            return await Mediator.Send(new UpdateProductCommand { ProductId = productId, Model = model });
        }

        [HttpPut("{branchId}/stocks/{productId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateProductStock(ProductStockDetailDto model, int branchId, int productId)
        {
            return await Mediator.Send(new UpdateProductStockCommand { BranchId = branchId, ProductId = productId, Model = model });
        }
        [HttpPut("specifications/langs/{langName}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateSpecLang(string langName,List<ProductSpecLangValueDto> model)
        {
            return await Mediator.Send(new UpdateProductSpecLangCommand { LangName = langName, Model = model });
        }

        [HttpDelete("{productId}/files")]
        public async Task<ActionResult<ApiResult<string>>> DeleteProductFile(string name, int productId)
        {
            return await Mediator.Send(new DeleteProductFileCommand { ProductId = productId, Name = name });
        }
    }
}