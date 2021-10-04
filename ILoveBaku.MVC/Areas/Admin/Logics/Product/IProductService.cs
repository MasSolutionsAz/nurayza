using ILoveBaku.Application.CQRS.Product.Commands.AddProduct;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Domain.Stored_Procedures;
using ILoveBaku.MVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Product
{
    public interface IProductService
    {
        Task<ProductAdminListVm> GetProducts(int page);
        Task<object> GetProduct(int? productId);
        Task<List<ProductGroupDto>> GetProductGroups();
        Task<List<ProductGroupDto>> GetGroupsByCategoryId(int? categoryId);
        Task<List<ProductList_sp>> GetNewProducts(int? branchId);
        Task<object> GetPhotos(int? productId);
        Task<object> CreateGroup(ProductGroupVm model,ModelStateDictionary modelState);
        Task<object> CreateProduct(ProductVm model,ModelStateDictionary modelState);
        Task<object> UpdateProduct(ProductVm model, int? productId, ModelStateDictionary modelState);
        Task<string> GetLastBarcode();
        Task<string> MakeBarcode();


    }
}
