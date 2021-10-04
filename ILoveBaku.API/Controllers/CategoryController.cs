using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategory;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategoryFile;
using ILoveBaku.Application.CQRS.Category.Commands.AddCategorySpecificationRelation;
using ILoveBaku.Application.CQRS.Category.Commands.DeleteCategoryFile;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategory;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategoryLanguage;
using ILoveBaku.Application.CQRS.Category.Commands.UpdateCategorySpecification;
using ILoveBaku.Application.CQRS.Category.Models;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoriesForSearch;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategory;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryChildrenList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryLanguagesById;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryParentsList;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategoryPhoto;
using ILoveBaku.Application.CQRS.Category.Queries.GetCategorySpecifications;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using CategoryDto = ILoveBaku.Application.CQRS.Category.Models.CategoryDto;
using CategoryDto2 = ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById.CategoryDto;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : BaseController
    {
        #region GET
        ///// <summary>
        ///// Filter meqsedi ile istifade ederken CategoriesId,ParentId,Name,IsActive,Priority data-lardan istenilen sekilde qeyd etmek mumkundur.
        ///// </summary>
        //[HttpGet]
        //public async Task<ActionResult<ApiResult<List<CategoryFullDto>>>> Get([FromQuery]CategoryFullDto dto)
        //{
        //    return await Mediator.Send(new GetCategoryListQuery() { Model = dto });
        //}

        /// <summary>
        /// CategoryId qəbul edir.Geriyə CategoryDto (ParentId,Priority,IsActive,Title) qaytarır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<ApiResult<CategoryDto2>>> GetCategoryById(int id)
        {
            return await Mediator.Send(new GetCategoryByIdQuery() { CategoryId = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<ApiResult<CategoryDto>>> GetCategory(string name)
        {
            return await Mediator.Send(new GetCategoryQuery(name));
        }

        /// <summary>
        ///CategoryId qəbul edir.Geriyə CategoryLanguageDto (CategoriesId,Name,Description,LangsId) qaytarır.
        /// </summary>
        [HttpGet("{id}/langs")]
        public async Task<ActionResult<ApiResult<List<CategoryLanguageVm>>>> GetCategoryLanguagesById(int id)
        {
            return await Mediator.Send(new GetCategoryLanguagesByIdQuery() { CategoryId = id });
        }
        [HttpGet("{categoryId}/files")]
        public async Task<ActionResult<ApiResult<List<ProductFileDto>>>> GetCategoryPhoto(int categoryId)
        {
            return await Mediator.Send(new GetCategoryPhotoQuery { CategoryId = categoryId });
        }

        /// <summary>
        ///CategoryId qəbul edir.Geriyə CategoryChildrenDto (Name,Id,CategoryChildren) qaytarır.
        /// </summary>
        [HttpGet("{id}/children")]
        public async Task<ActionResult<ApiResult<CategoryChildrenListVm>>> GetCategoryChildren(int id, int page)
        {
            return await Mediator.Send(new GetCategoryChildrenListQuery() { CategoryId = id, Page = page });
        }

        /// <summary>
        ///Verilmiş kateqoriya İD-ə sahib elementin bütün valideynlərini qaytarır.(Paltarlar->T-shirt)
        /// </summary>
        [HttpGet("{id}/parents")]
        public async Task<ActionResult<ApiResult<List<object>>>> GetCategoryParentsList(int id)
        {
            return await Mediator.Send(new GetCategoryParentsListQuery() { CategoryId = id });
        }


        /// <summary>
        /// Verilmiş kateqoriya İD-ə sahib elementin bütün valideynlerini (parentId) geri qaytarır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/specifications")]
        public async Task<ActionResult<ApiResult<List<CategorySpecificationDto>>>> GetCategorySpecifications(int id)
        {
            return await Mediator.Send(new GetCategorySpecificationsQuery() { CategoryId = id });
        }

        /// <summary>
        /// Saytin navbar hissesinde cixan search-de istifade olunan kateqoriyalar
        /// </summary>
        [HttpGet("forSearch")]
        public async Task<ActionResult<ApiResult<List<SearchCategoryDto>>>> ForSearch()
        {
            return await Mediator.Send(new GetCategoriesForSearchQuery());
        }
        #endregion

        #region Post
        /// <summary>
        /// Category [Title,Priority,IsActive,ParentId] əlavə olunması.Eyni zamanda Title dəyərinə uyğun
        /// bütün dillərdə CategoryLang yaranır.
        /// </summary>
        [HttpPost()]
        public async Task<ActionResult<ApiResult<int?>>> AddCategory(CategoryVm model)
        {
            return await Mediator.Send(new AddCategoryCommand { Model = model });
        }


        /// <summary>
        ///Category ve CategorySpecification arasında əlaqə yaratdır.Categoryİd və Specificationİd qəbul edir.
        /// </summary>
        [HttpPost("specifications/relations")]
        public async Task<ActionResult<ApiResult<int?>>> AddCategorySpecificationRelation(CategorySpecificationVm model)
        {
            return await Mediator.Send(new AddCategorySpecificationRelationCommand { Model = model });
        }


        [HttpPost("{categoryId}/files")]
        public async Task<ActionResult<ApiResult<PhotoModel>>> AddCategoryFile(ProductFileDto model, int categoryId)
        {
            return await Mediator.Send(new AddCategoryFileCommand { CategoryId = categoryId, Model = model });
        }
        #endregion

        #region Put
        /// <summary>
        ///Category - i update olunması.Geriye update olunmuş category-nin İD-ni qaytarır.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategory(CategoryVm model, int id)
        {
            return await Mediator.Send(new UpdateCategoryCommand { Model = model, Id = id });
        }

        /// <summary>
        /// CategoryLang update olunması.Geriye update olunmuş categoryLang-nin İD-ni qaytarır.
        /// </summary>
        [HttpPut("lang/{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategoryLanguage(CategoryLanguageVm model, int id)
        {
            model.CategoriesId = id;
            return await Mediator.Send(new UpdateCategoryLanguageCommand { Model = model });
        }

        [HttpPut("{id}/specifications/{specificationId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationRelation(int id, int specificationId)
        {
            return await Mediator.Send(new UpdateCategorySpecificationCommand { CategoryId = id, CategorySpecificationId = specificationId });
        }
        #endregion

        [HttpDelete("{categoryId}/files")]
        public async Task<ActionResult<ApiResult<string>>> AddCategoryFile(string name, int categoryId)
        {
            return await Mediator.Send(new DeleteCategoryFileCommand { CategoryId = categoryId, Name = name });
        }
    }
}