using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications;
using ILoveBaku.Application.CQRS.Language.Queries.GetLanguage;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Areas.Admin.Logics.Language;
using ILoveBaku.MVC.Areas.Admin.Logics.Photo;
using ILoveBaku.MVC.Extensions;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Category
{
    public class CategoryService : BaseService, ICategoryService<int?>
    {
        private readonly ILanguageService _languageService;
        public CategoryService(ILanguageService languageService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) => _languageService = languageService;


        public async Task<List<CategorySpecificationDto>> GetCategorySpecifications(int? categoryId)
        {
            ApiResult<List<CategorySpecificationDto>> data = await API.GetAsync<ApiResult<List<CategorySpecificationDto>>>($"categories/{categoryId}/specifications");
            if (data == null || !data.Succeeded)
                return null;

            return data.Response;
        }
        public async Task<List<CategoryLanguageVm>> GetCategoryLanguagesOfCategoryById(int? categoryId)
        {
            ApiResult<List<CategoryLanguageVm>> categoryLangs = await API.GetAsync<ApiResult<List<CategoryLanguageVm>>>($"categories/{categoryId}/langs");
            if (categoryLangs != null && categoryLangs.Succeeded)
                return categoryLangs.Response;
            else
                return null;
        }

        public async Task<CategoryDto> GetCatgoryById(int? categoryId)
        {
            ApiResult<CategoryDto> category = await API.GetAsync<ApiResult<CategoryDto>>($"categories/id/{categoryId}");
            if (category != null && category.Succeeded)
                return category.Response;
            else
                return null;
        }

        public async Task<CategoryProcessVm> CreateProcessVm(int? parentId, int? categoryId = null)
        {
            CategoryProcessVm vm = new CategoryProcessVm();

            if (categoryId == null)
            {
                vm.ParentId = parentId;
                return vm;
            }

            List<CategoryLanguageVm> categoryLangs = await this.GetCategoryLanguagesOfCategoryById(categoryId);
            List<LanguageDto> langs = await _languageService.GetLanguages();
            CategoryDto category = await this.GetCatgoryById(categoryId);
            List<CategorySpecificationDto> specifications = await this.GetCategorySpecifications(categoryId);

            if (categoryLangs != null && langs != null && category != null && specifications != null)
            {
                if (categoryLangs.Count < langs.Count)
                {
                    for (int i = 0; i < langs.Count - categoryLangs.Count; i++)
                    {
                        categoryLangs.Add(new CategoryLanguageVm());
                    }
                }

                vm.IsUpdate = true;
                vm.Languages = langs;
                vm.CategoryVm = new CategoryVm
                {
                    IsActive = category.IsActive,
                    ParentId = category.ParentId,
                    Priority = category.Priority,
                    Title = category.Title
                };
                vm.CategoryLanguageVm = categoryLangs;
                vm.CategoryId = categoryId;
                vm.ParentId = category.ParentId;
                vm.Specifications = specifications;
                return vm;
            }

            return null;
        }
        public async Task<CategoryChildrenListVm> GetCategoryChildrenByParentId(int parentId, int? page)
        {
            ApiResult<CategoryChildrenListVm> categories = await API.GetAsync<ApiResult<CategoryChildrenListVm>>($"categories/{parentId}/children/?page={page}");
            
            if (categories != null && categories.Succeeded)
            {
                 categories.Response.Children = categories.Response.Children.ToList();
                 return categories.Response;
            }
            else
                return null;
        }
        public async Task<CategoryListVm> CreateListVm(int categoryId, int? page)
        {
            CategoryChildrenListVm categories = await this.GetCategoryChildrenByParentId(categoryId, page);

            if (categories == null)
                return null;

            CategoryListVm vm = new CategoryListVm
            {
                Categories = categories.Children,
                Total = categories.Total,
                ParentId = categoryId,
                Current = page!=null?(int)page:0
            };
            return vm;
        }

        public async Task<ApiResult<int?>> AddCategory(CategoryVm model)
        {
            ApiResult<int?> categoryCreateResult = await API.PostAsync<CategoryVm, ApiResult<int?>>("categories", model);
            return categoryCreateResult;
        }
        public async Task<Dictionary<string, string>> UpdateCategoryLanguage(CategoryLanguageVm model, int? categoryLangId)
        {
            ApiResult<int?> categoryLangUpdate = await API.PutAsync<CategoryLanguageVm, ApiResult<int?>>($"categories/lang/{categoryLangId}", model);
            if (categoryLangUpdate == null || !categoryLangUpdate.Succeeded)
                return categoryLangUpdate.ErrorList;

            return null;
        }
        public async Task<Dictionary<string, string>> UpdateCategory(CategoryVm model, int? categoryId)
        {
            ApiResult<int?> categoryUpdate = await API.PutAsync<CategoryVm, ApiResult<int?>>($"categories/{categoryId}", model);
            if (categoryUpdate == null || !categoryUpdate.Succeeded)
                return categoryUpdate.ErrorList;
            return null;
        }
        public async Task<Dictionary<string, string>> UpdateCategorySpecification(int? categoryId, int? specificationId)
        {
            ApiResult<int?> result = await API.PutAsync<int?, ApiResult<int?>>($"categories/{categoryId}/specifications/{specificationId}", specificationId);
            if (result != null && !result.Succeeded)
                return result.ErrorList;

            return null;
        }
        public async Task<ProcessResult<int?>> Process(CategoryProcessVm model)
        {
            string[] prohibitedLetters = { "ö", "ü", "ə", "ı", "ğ", "ş", "ç" };
            foreach (var item in prohibitedLetters)
            {
                if (model.CategoryVm.Title.Contains(item))
                {
                    return ProcessResult<int?>.CreateResult(
                                   Response: null,
                                   IsUpdate: false,
                                   errors: new Dictionary<string, string>() { { "", "Ingilis hərflərindən istifadə edin." } },
                                   Succeeded: false
                                   );
                }
            }

            if (!model.IsUpdate)
            {
                    ApiResult<int?> categoryAddResult = await this.AddCategory(model.CategoryVm);
                if (categoryAddResult == null || !categoryAddResult.Succeeded)
                    return ProcessResult<int?>.CreateResult(
                                        Response: null,
                                        IsUpdate: false,
                                        errors: categoryAddResult.ErrorList,
                                        Succeeded: false
                                        );
                else
                    return ProcessResult<int?>.CreateResult(
                                       Response: categoryAddResult.Response,
                                       IsUpdate: false,
                                       errors: null,
                                       Succeeded: true
                                       );
            }

            List<LanguageDto> langs = await _languageService.GetLanguages();
            if (langs == null)
                return ProcessResult<int?>.CreateResult(
                                          Response: null,
                                          IsUpdate: true,
                                          errors: new Dictionary<string, string>() { { "", "Xəta baş verdi" } },
                                          Succeeded: false
                                          );

            model.Languages = langs;

            var categoryUpdateResult = await this.UpdateCategory(model.CategoryVm, model.CategoryId);
            if (categoryUpdateResult != null)
                return ProcessResult<int?>.CreateResult(
                                          Response: null,
                                          IsUpdate: true,
                                          errors: categoryUpdateResult,
                                          Succeeded: false
                                          );

            for (var i = 0; i < model.Languages.Count; i++)
            {
                model.CategoryLanguageVm[i].LangsId = model.Languages[i].Id;
                var categoryLangUpdateResult = await this.UpdateCategoryLanguage(model.CategoryLanguageVm[i], model.CategoryLanguageVm[i].Id);
                if (categoryLangUpdateResult != null)
                    return ProcessResult<int?>.CreateResult(
                                              Response: null,
                                              IsUpdate: true,
                                              errors: categoryLangUpdateResult,
                                              Succeeded: false
                                              );

            }

            return ProcessResult<int?>.CreateResult(
                                       Response: model.CategoryId,
                                       IsUpdate: true,
                                       errors: null,
                                       Succeeded: true
                                       );
        }

        public async Task<object> Update(CategoryUpdateVm model, int? id, ModelStateDictionary modelState)
        {
            //update category
            if (model.CategoryVm != null)
            {
                if (modelState.GetModelValidationState("CategoryVm") == ModelValidationState.Invalid)
                    return new
                    {
                        status = 400,
                        errors = modelState.GetErrors()
                    };

                var errors = await this.UpdateCategory(model.CategoryVm, id);
                if (errors == null)
                    return new
                    {
                        status = 200
                    };
                else
                    return new
                    {
                        status = 400,
                        errors = errors
                    };
            }
            //update category lang
            if (model.CategoryLanguageVm != null)
                foreach (var item in model.CategoryLanguageVm)
                {
                    if (item != null)
                    {
                        if (modelState.GetModelValidationState("CategoryLanguageVm") == ModelValidationState.Invalid)
                            return new
                            {
                                status = 400,
                                errors = modelState.GetErrors()
                            };

                        var errors = await this.UpdateCategoryLanguage(item, id);
                        if (errors == null)
                            return new
                            {
                                status = 200
                            };
                        else
                            return new
                            {
                                status = 400,
                                errors = errors
                            };
                    }
                }

            //update category specification
            if (model.Specification != null)
            {
                if (modelState.GetModelValidationState("Specification") == ModelValidationState.Invalid)
                    return new
                    {
                        status = 400,
                        errors = modelState.GetErrors()
                    };

                var errors = await this.UpdateCategorySpecification(model.Specification.CategoryId, model.Specification.CategorySpecificationId);
                if (errors == null)
                    return new
                    {
                        status = 200
                    };
                else
                    return new
                    {
                        status = 400,
                        errors = errors
                    };
            }

            return new
            {
                status = 400
            };
        }

        public async Task<object> UploadPhoto(string path, List<PhotoModel> upload, List<string> delete, int categoryId)
        {
            var response = new List<PhotoModel>();

            foreach (var file in upload)
            {
                ProductFileDto fileDto = new ProductFileDto
                {
                    ContentType = file.File.ContentType,
                    IsMain = file.IsMain,
                    Length = file.File.Length,
                    Name = file.File.FileName,
                    Path = "/uploads/categories"
                };

                var data = await API.PostAsync<ProductFileDto, ApiResult<PhotoModel>>($"categories/{categoryId}/files", fileDto);
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data != null && data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response.Name;
                    using (FileStream stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await file.File.CopyToAsync(stream);
                    }
                    response.Add(new PhotoModel { IsMain = data.Response.IsMain, Name = data.Response.Name });

                }
            }

            foreach (var file in delete)
            {
                var data = await API.DeleteAsync<ApiResult<string>>($"categories/{categoryId}/files/?name={file}");
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response;
                    if (System.IO.File.Exists(photoPath))
                    {
                        File.Delete(photoPath);
                    }
                }
            }
            return new
            {
                status = 200,
                data=response
            };
        }

        public async Task<object> GetPhotos(int? categoryId)
        {
            if (categoryId == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Xəta baş verdi." } } };

            var data = await API.GetAsync<ApiResult<List<ProductFileDto>>>($"categories/{categoryId}/files");
            if (data != null && data.Succeeded)
                return new { status = 200, data = data.Response };
            return new { status = 400 };
        }
    }
}
