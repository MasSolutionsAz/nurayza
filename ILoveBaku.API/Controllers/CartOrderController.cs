using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.CartOrders.Models;
using ILoveBaku.Application.CQRS.CartOrders.Queries.GetLastCartOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/cartOrders")]
    public class CartOrderController : BaseController
    {
        [HttpGet("{requestUsersId}/last")]
        public async Task<ActionResult<ApiResult<CartOrderDto>>> Paid(Guid requestUsersId)
        {
            return await Mediator.Send(new GetLastCartOrderQuery() { RequestUsersId = requestUsersId });
        }
    }
}
