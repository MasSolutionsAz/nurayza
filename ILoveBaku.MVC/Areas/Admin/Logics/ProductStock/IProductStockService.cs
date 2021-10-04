using ILoveBaku.Application.CQRS.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.ProductStock
{
    public interface IProductStockService
    {
        Task<ProductStocksVM> GetProductStocksByBranchId(int? branchId,int page,int take);
        Task<ProductStockVM> GetProductStock(int? productStockId, int? branchId);
        Task<object> UpdateProductStock(ProductStockDetailDto model);
        Task<object> Add(List<int> products);
    }
}
