using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Domain.Enums;
using System.Text;
using System.Threading.Tasks;
using ILoveBaku.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using ILoveBaku.Application.Common.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ILoveBaku.Infrastructure.Attributes
{
    public class Auth : ActionFilterAttribute
    {
        private readonly Claims _claim;

        private readonly IToken _tokenService;

        public Auth(IToken tokenService, Claims claim)
        {
            _claim = claim;
            _tokenService = tokenService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (await context.HttpContext.HasHeader("token") &&
                await _tokenService.CheckTokenByClaim(await context.HttpContext.GetHeader("token"), _claim))
                await next();
            else
                await UnAuthorizeResponseWriteAsync(context.HttpContext.Response);
        }

        private async Task UnAuthorizeResponseWriteAsync(HttpResponse response)
        {
            byte[] buffer = GetAuthResponse();
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.Body.WriteAsync(buffer, 0, buffer.Length);
        }

        private byte[] GetAuthResponse()
        {
            var result = ApiResult<object>.CreateResponse(null, null, new ErrorDetail
            {
                ErrorMessage = "auth error"
            });

            string data = JsonConvert.SerializeObject(result);
            return Encoding.ASCII.GetBytes(data);
        }
    }
}
