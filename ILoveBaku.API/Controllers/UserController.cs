using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.User.Commands.AddManualUser;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserById;
using ILoveBaku.Application.CQRS.User.Queries.GetUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpGet("{page}/{take}")]
        public async Task<ActionResult<ApiResult<UserListVm>>> GetUsers(int page,int take)
        {
            return await Mediator.Send(new GetUsersQuery { Page = page, Take = take });
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApiResult<UserDto>>> GetUserById(Guid userId)
        {
            return await Mediator.Send(new GetUserByIdQuery { GetUserId = userId });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<UserResponse>>> AddManualUser(UnRegisteredPaymentModel model)
        {
            return await Mediator.Send(new AddManualUserCommand { Model = model });
        }
    }
}
