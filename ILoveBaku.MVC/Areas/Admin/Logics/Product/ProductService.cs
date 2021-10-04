using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Domain.Stored_Procedures;
using ILoveBaku.Infrastructure.Helpers;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Areas.Admin.Models;
using ILoveBaku.MVC.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Product
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public async Task<object> GetProduct(int? productId)
        {
            if (productId == null)
                return new { status = 400, errors = new Dictionary<string, string>() { { "product", "Belə bir məhsul yoxdur." } } };

            var data = await API.GetAsync<ApiResult<ProductVm>>($"products/{productId}");
            if (data != null && data.Succeeded)
                return new { status = 200, data = data.Response };

            if (data != null)
                return new { status = 400, errors = data.ErrorList };

            return new { status = 400, errors = new Dictionary<string, string>() };
        }
        public async Task<ProductAdminListVm> GetProducts(int page)
        {
            var result = await API.GetAsync<ApiResult<ProductAdminListVm>>("products/?page="+page);
            if (result != null && result.Succeeded)
                return result.Response;

            return null;
        }
        public async Task<List<ProductGroupDto>> GetProductGroups()
        {
            ApiResult<List<ProductGroupDto>> result = await API.GetAsync<ApiResult<List<ProductGroupDto>>>("products/groups");
            if (result != null && result.Succeeded)
                return result.Response;

            return null;
        }

        public async Task<List<ProductGroupDto>> GetGroupsByCategoryId(int? categoryId)
        {
            var data = await API.GetAsync<ApiResult<List<ProductGroupDto>>>($"products/{categoryId}/groups/");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<string> GetLastBarcode()
        {
            var data = await API.GetAsync<ApiResult<string>>("products/barcode");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }
        public async Task<List<ProductList_sp>> GetNewProducts(int? branchId)
        {
            var data = await API.GetAsync<ApiResult<List<ProductList_sp>>>($"products/{branchId}/outOfStock");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }
        public async Task<string> MakeBarcode()
        {
            var data = await this.GetLastBarcode();
            if (data == null)
                return null;

            if (data == "")
            {
                string last = EAN13.ChecksumEan13("100000000001").ToString();
                return "100000000001" + last;
            }

            string twelve = (Convert.ToInt64(data.Substring(0, data.Length - 1)) + 1).ToString();
            string result = twelve + EAN13.ChecksumEan13(twelve);
            return result;
        }

        public async Task<object> CreateProduct(ProductVm model, ModelStateDictionary modelState)
        {

            if (model.ProductName == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "", "Məhsulun adı qeyd olunmayıb." } } };

            if (!modelState.IsValid)
                return new { status = 400, errors = modelState.GetErrorsWithKey() };

            if (!EAN13.CheckBarcode(model.Barcode))
                return new { status = 400, errors = new Dictionary<string, string> { { "barcode", "Barkod standartlara uyğun deyil." } } };

            var result = await API.PostAsync<ProductVm, ApiResult<int?>>("products", model);
            if (result != null && result.Succeeded)

                return new { status = 200, data = result.Response };

            return new
            {
                status = 400,
                errors = result.ErrorList
            };
        }

        public async Task<object> CreateGroup(ProductGroupVm model, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return new
                {
                    status = 400,
                    errors = modelState.GetErrorsWithKey()
                };

            var data = await API.PostAsync<ProductGroupVm, ApiResult<int?>>("products/groups", model);
            if (data != null && data.Succeeded)
                return new
                {
                    data = data.Response,
                    status = 200
                };

            return new
            {
                status = 400,
                errors = data.ErrorList
            };
        }

        public async Task<object> UpdateProduct(ProductVm model, int? productId, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return new { status = 400, errors = modelState.GetErrorsWithKey() };

            if (!EAN13.CheckBarcode(model.Barcode))
                return new { status = 400, errors = new Dictionary<string, string> { { "barcode", "Barkod standartlara uyğun deyil." } } };

            if (productId == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "product", "Belə bir məhsul yoxdur." } } };

            var result = await API.PutAsync<ProductVm, ApiResult<int?>>($"products/{productId}", model);

            if (result != null && result.Succeeded)
                return new { status = 200, data = result.Response };

            return new
            {
                status = 400,
                errors = result.ErrorList
            };
        }

        public async Task<object> GetPhotos(int? productId)
        {
            if (productId == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Xəta baş verdi." } } };

            var data = await API.GetAsync<ApiResult<List<ProductFileDto>>>($"products/{productId}/files");
            if (data != null && data.Succeeded)
                return new { status = 200, data = data.Response };

            return new { status = 400};
        }
    }
}
