using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups;
using ILoveBaku.Application.CQRS.ProductGroup.Commands.AddProductGroup;
using ILoveBaku.Application.CQRS.ProductGroup.Commands.UpdateGroup;
using ILoveBaku.Application.CQRS.ProductGroup.Models;
using ILoveBaku.Application.CQRS.ProductGroup.Queries.GetProductGroup;
using ILoveBaku.Application.CQRS.ProductGroup.Queries.GetProductGroups;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class ProductGroupController : BaseController
    {
        [HttpGet("{page}/{take}")]
        public async Task<ActionResult<ApiResult<ProductGroupListVm>>> GetGroups(int page,int take)
        {
            return await Mediator.Send(new Application.CQRS.ProductGroup.Queries.GetProductGroups.GetProductGroupsQuery { Page = page, Take = take });
        }

        [HttpGet("{groupId}")]
        public async Task<ActionResult<ApiResult<ProductGroupDto>>> GetProductGroup(int groupId)
        {
            return await Mediator.Send(new GetProductGroupQuery { GroupId = groupId });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<int?>>> AddProductGroup(ProductGroupDto model)
        {
            return await Mediator.Send(new AddProductGroupCommand() { Model = model });
        }
        [HttpPut]
        public async Task<ActionResult<ApiResult<int?>>> UpdateProductGroup(ProductGroupDto model)
        {
            return await Mediator.Send(new UpdateGroupCommand { Model = model });
        }
    }
}
