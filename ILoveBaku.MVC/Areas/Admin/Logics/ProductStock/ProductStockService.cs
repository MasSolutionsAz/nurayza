using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.ProductStock
{
    public class ProductStockService : BaseService, IProductStockService
    {
        public ProductStockService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public async Task<object> Add(List<int> products)
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            if (branchId == null || branchId == 0)
                return new { status = 400, errors = new Dictionary<string, string>() { { "xeta", "Gözlənilməz xəta baş verdi.Sistemdən çıxıb yenidən daxil olun." } } };

            foreach (var product in products)
            {
                var data = await API.PostAsync<int,ApiResult<int?>>($"products/{branchId}/stocks/?productId={product}",product);
                if(data==null)
                    return new { status = 400, errors = new Dictionary<string, string>() { { "xeta", "Gözlənilməz xəta baş verdi.Sistemdən çıxıb yenidən daxil olun." } } };
           
                 if(data!=null && !data.Succeeded)
                    return new { status = 400, errors = data.ErrorList };

            }

            return new
            {
                status = 200
            };
        }

        public async Task<ProductStockVM> GetProductStock(int? productStockId, int? branchId)
        {
            var data = await API.GetAsync<ApiResult<ProductStockVM>>($"products/{branchId}/stocks/{productStockId}");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<ProductStocksVM> GetProductStocksByBranchId(int? branchId, int page, int take)
        {
            var data = await API.GetAsync<ApiResult<ProductStocksVM>>($"products/{branchId}/stocks/?take={take}&page={page}");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<object> UpdateProductStock(ProductStockDetailDto model)
        {
            int? branchId = Convert.ToInt32(HttpContext.Session.GetString("branchId"));
            if (branchId == null || branchId == 0)
                return new { status = 400, errors = new Dictionary<string, string>() { { "xeta", "Gözlənilməz xəta baş verdi.Sistemdən çıxıb yenidən daxil olun." } } };
            var data = await API.PutAsync<ProductStockDetailDto, ApiResult<int?>>($"products/{branchId}/stocks/{model.ProductId}", model);

            if (data == null)
                return new { status = 400, errors = new Dictionary<string, string>() { { "xeta", "Gözlənilməz xəta baş verdi.Sistemdən çıxıb yenidən daxil olun." } } };

            if (data != null && !data.Succeeded)
                return new { status = 400, errors = data.ErrorList };

            return new { status = 200 };
        }
    }
}
