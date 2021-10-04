using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecification;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationGroup;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.AddCategorySpecificationProperty;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategoryLanguageSpecification;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecification;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationGroup;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationGroupLanguage;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationProperty;
using ILoveBaku.Application.CQRS.CategorySpecification.Commands.UpdateCategorySpecificationPropertyLanguage;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecification;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationProperties;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationsGroups;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetCategorySpecificationsTypes;
using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/specifications")]
    [ApiController]
    public class CategorySpecificationController : BaseController
    {
        #region Get
        /// <summary>
        /// Ya xüsusi olaraq verilmiş olan,ya da bütün spesifikasiyaları geri qaytarır
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<CategoriesSpecificationsLangs>>>> GetCategorySpecificaitonsList([FromQuery] GetCategorySpecificationDto model)
        {
            return await Mediator.Send(new GetCategoriesSpecificationsQuery { Model = model });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<List<CategorySpecificationGroupDto>>>> GetSpecificationsByCategoryId(int id)
        {
            return await Mediator.Send(new GetSpecificationsByCategoryIdQuery { CategoryId = id });
        }

        /// <summary>
        /// Ya xüsusi olaraq verilmiş olan, ya da bütün spesifikasiya qrupların siyahısını qaytarır.
        /// </summary>
        [HttpGet("groups")]
        public async Task<ActionResult<ApiResult<List<CategoriesSpecificationsGroupsLangs>>>> GetCategorySpecificationsGroupsList([FromQuery] bool isActive)
        {
            return await Mediator.Send(new GetCategorySpecificationsGroupsQuery { IsActive = isActive });
        }

        /// <summary>
        /// Ya xüsusi olaraq verilmiş olan,ya da bütün spesifikasiya növləri geri qaytarır
        /// </summary>
        [HttpGet("types")]
        public async Task<ActionResult<ApiResult<List<CategoriesSpecificationsTypes>>>> GetCategorySpecificationsTypesList([FromQuery] int? controllerId, [FromQuery] string name)
        {
            return await Mediator.Send(new GetCategorySpecificationsTypesQuery { ControllerId = controllerId, Name = name });
        }

        /// <summary>
        ///Category specification property-ler əlavə olunması.Arqument olaraq Specificationİd qəbul edir.
        ///Geriye verilmiş spesifikasiyanın propertilerini qaytarır (Müvafiq dildə).
        /// </summary>
        [HttpGet("{id}/properties")]
        public async Task<ActionResult<ApiResult<List<CategoriesSpecificationsPropertiesLangs>>>> GetCategorySpecificationsPropertiesList(int? id)
        {
            return await Mediator.Send(new GetCategorySpecificationPropertiesQuery { CategorySpecificationId = id });
        }
        #endregion

        #region Post
        /// <summary>
        ///Category specification əlavə olunması.Arqument olaraq qrup,type,title qəbul edir.
        ///Eyni zamanda dillərə uyğun olaraq CategorySpecificationLangs yaranır.
        /// </summary>
        [HttpPost()]
        public async Task<ActionResult<ApiResult<int?>>> AddCategorySpecification(CategorySpecificationCommandDto model)
        {
            return await Mediator.Send(new AddCategorySpecificationCommand { Model = model });
        }

        /// <summary>
        ///Category specification qrup əlavə olunması.Arqument olaraq priority,isactive,title qəbul edir.
        ///Eyni zamanda dillərə uyğun olaraq CategorySpecificationGroupsLangs yaranır.
        /// </summary>
        [HttpPost("groups")]
        public async Task<ActionResult<ApiResult<int?>>> AddCategorySpecificationGroups(CategorySpecificationGroupCommandDto model)
        {
            return await Mediator.Send(new AddCategorySpecificationGroupCommand { Model = model });
        }

        /// <summary>
        ///Category specification property əlavə olunması.Arqument olaraq title,parentId,specificationId qəbul edir.
        ///Eyni zamanda dillərə uyğun olaraq CategorySpecificationPropertiesLangs yaranır.
        /// </summary>
        [HttpPost("{id}/properties")]
        public async Task<ActionResult<ApiResult<int?>>> AddCategorySpecificationProperty(CategorySpecificationPropertyCommandDto model, int id)
        {
            return await Mediator.Send(new AddCategorySpecificationPropertyCommand { Model = model, CategorySpecificationId = id });
        }

        #endregion

        #region Put
        /// <summary>
        ///Category specification update olunması.Arqument olaraq qrup,type,title qəbul edir.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecification(CategorySpecificationCommandDto model, int id)
        {
            return await Mediator.Send(new UpdateCategorySpecificationCommand { Model = model, Id = id });
        }

        /// <summary>
        ///Category specification property update olunması.Arqument olaraq ParentId,Title,SpecificationId qəbul edir.
        /// </summary>
        [HttpPut("{id}/properties")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationProperty(CategorySpecificationPropertyCommandDto model, int id)
        {
            return await Mediator.Send(new UpdateCategorySpecificationPropertyCommand { Model = model, CategorySpecificationId = id });
        }


        /// <summary>
        ///Category specification property lang update olunması.Arqument olaraq Name,SpecificationId qəbul edir.
        /// </summary>
        [HttpPut("{id}/properties/lang")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationPropertyLanguage(string name, int id)
        {
            return await Mediator.Send(new UpdateCategorySpecificationPropertyLanguageCommand { Name = name, CategorySpecificationId = id });
        }


        /// <summary>
        ///Category specification language update olunması.Arqument olaraq CategoryId,Name qəbul edir.
        /// </summary>
        [HttpPut("lang/{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationLanguage(string name, int id)
        {
            return await Mediator.Send(new UpdateCategorySpecificationLanguageCommand { Name = name, Id = id });
        }

        /// <summary>
        ///Category specification group update olunması.Arqument olaraq priority,isactive,title qəbul edir.
        /// </summary>
        [HttpPut("groups/{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationGroup(CategorySpecificationGroupCommandDto model, int id)
        {
            return await Mediator.Send(new UpdateCategorySpecificationGroupCommand { Model = model, Id = id });
        }

        /// <summary>
        ///Category specification group lang update olunması.Arqument olaraq CategoryId,Name qəbul edir.
        /// </summary>
        [HttpPut("groups/lang/{id}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateCategorySpecificationGroupLanguage(int id, string name)
        {
            return await Mediator.Send(new UpdateCategorySpecificationGroupLanguageCommand { CategorySpecificationGroupId = id, Name = name });
        }
        #endregion
    }
}