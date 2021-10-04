using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.CartOrders.Models;
using ILoveBaku.Application.CQRS.Payment.Commands.Pay;
using ILoveBaku.Application.CQRS.Payment.Commands.Paid;
using ILoveBaku.Application.CQRS.Payment.Commands.Cancel;
using ILoveBaku.Application.CQRS.Payment.Commands.Decline;
using ILoveBaku.Application.CQRS.Payment.Models;
using ILoveBaku.Application.CQRS.Payment.Queries;
using System.Collections.Generic;
using ILoveBaku.Application.CQRS.User.Models;
using System;
using ILoveBaku.Application.CQRS.Payment.Commands.Accept;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : BaseController
    {
        [HttpGet("types")]
        public async Task<ActionResult<ApiResult<List<PaymentTypeDto>>>> GetPaymentTypes()
        {
            return await Mediator.Send(new GetPaymentTypesQuery());
        }

        [HttpPost("pay/{usersId}")]
        public async Task<ActionResult<ApiResult<PayCommandResponseModel>>> Pay(PayCommandRequestModel model,Guid usersId)
        {
            return await Mediator.Send(new PayCommand() { Model = model,RequestUsersId  = usersId});
        }
        [HttpPost("paid")]
        public async Task<ActionResult<ApiResult<int>>> Paid(PaymentRequestModel model)
        {
            return await Mediator.Send(new PaidCommand(){Model = model});
        }

        [HttpPost("accept")]
        public async Task<ActionResult<ApiResult<int?>>> Accept(AcceptCashOutRequestModel model)
        {
            return await Mediator.Send(new AcceptCommand() { Model = model });
        }

        [HttpPost("cancel")]
        public async Task<ActionResult<ApiResult<int>>> Cancel(PaymentRequestModel model)
        {
            return await Mediator.Send(new CancelCommand() { Model = model});
        }

        [HttpPost("decline")]
        public async Task<ActionResult<ApiResult<int>>> Decline(PaymentRequestModel model)
        {
            return await Mediator.Send(new DeclineCommand() { Model = model});
        }
    }
}