using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryLanguagesById;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Areas.Admin.Logics.Photo;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Category
{
    public interface ICategoryService<TResponse>
    {
        Task<CategoryDto> GetCatgoryById(int? categoryId);
        Task<List<CategoryLanguageVm>> GetCategoryLanguagesOfCategoryById(int? categoryId);
        Task<CategoryChildrenListVm> GetCategoryChildrenByParentId(int parentId, int? page);
        Task<List<CategorySpecificationDto>> GetCategorySpecifications(int? categoryId);
        Task<ApiResult<int?>> AddCategory(CategoryVm model);
        Task<Dictionary<string, string>> UpdateCategoryLanguage(CategoryLanguageVm model, int? categoryLangId);
        Task<Dictionary<string, string>> UpdateCategory(CategoryVm model,int? categoryId);
        Task<Dictionary<string, string>> UpdateCategorySpecification(int? categoryId,int? specificationId);
        Task<CategoryProcessVm> CreateProcessVm(int? categoryId,int? parentId);
        Task<CategoryListVm> CreateListVm(int categoryId, int? page);
        Task<ProcessResult<TResponse>> Process(CategoryProcessVm model);
        Task<object> Update(CategoryUpdateVm model, int? id, ModelStateDictionary modelState);
        Task<object> UploadPhoto(string path, List<PhotoModel> upload, List<string> delete, int productId);
        Task<object> GetPhotos(int? categoryId);

    }
}
