using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Contact.Commands.SendContact;
using ILoveBaku.Application.CQRS.Contact.Models;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : BaseController
    {
        [HttpPost("send")]
        public async Task<ActionResult<ApiResult<int>>> Register(ContactVM model)
        {
            return await Mediator.Send(new SendContactCommand(model));
        }
    }
}
