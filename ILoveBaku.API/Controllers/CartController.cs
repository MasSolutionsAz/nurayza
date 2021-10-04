using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Carts.Commands.AddCartDetail;
using ILoveBaku.Application.CQRS.Carts.Commands.AddCartsToUserFromSession;
using ILoveBaku.Application.CQRS.Carts.Commands.DeleteCartDetail;
using ILoveBaku.Application.CQRS.Carts.Commands.UpdateCartDetail;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Application.CQRS.Carts.Queries.GetCartDetails;
using ILoveBaku.Application.CQRS.Carts.Queries.GetCheckedProductResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartController : BaseController
    {
        [HttpGet]
        public async Task<ApiResult<List<CartDetailDto>>> Get()
        {
            return await Mediator.Send(new GetCartDetailsQuery());
        }

        [HttpGet("{productId}/{count}/check")]
        public async Task<ApiResult<CartDetailDto>> GetCheckedProductResult(int productId,int count,int last)
        {
            return await Mediator.Send(new GetCheckedProductResultQuery() { ProductId = productId, Count = count,Last = last });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<int>>> Add(AddCartDetailVM model)
        {
            return await Mediator.Send(new AddCartDetailCommand(model));
        }

        [HttpPost("{usersId}/map")]
        public async Task<ActionResult<ApiResult<int?>>> Add(List<CartDetailDto> model,Guid usersId)
        {
            return await Mediator.Send(new AddCartsToUserFromSessionCommand() { Model = model ,RequestUsersId = usersId});
        }

        [HttpPut("{cartDetailId}")]
        public async Task<ActionResult<ApiResult<int>>> Update(int cartDetailId, UpdateCartDetailVM model)
        {
            return await Mediator.Send(new UpdateCartDetailCommand(cartDetailId, model));
        }

        [HttpDelete("{cartDetaildId}")]
        public async Task<ActionResult<ApiResult<int>>> Delete(int cartDetaildId)
        {
            return await Mediator.Send(new DeleteCartDetailCommand(cartDetaildId));
        }
    }
}