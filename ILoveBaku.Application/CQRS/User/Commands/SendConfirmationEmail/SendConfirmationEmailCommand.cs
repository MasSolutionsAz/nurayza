using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.SendConfirmationEmail
{
    public class SendConfirmEmailModel
    {
        public string ResponseUrl { get; set; }
        public Guid UserId { get; set; }
    }
    public class ConfirmationVm
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class SendConfirmationEmailCommand : BaseRequest<ApiResult<int?>>
    {
        public SendConfirmEmailModel Model { get; set; }
        public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand, ApiResult<int?>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IToken _tokenService;
            private readonly IRazorViewToStringRenderer _renderer;
            private readonly IConfiguration _configuration;

            public SendConfirmationEmailCommandHandler(IApplicationDbContext context, IToken tokenService, IRazorViewToStringRenderer renderer, IConfiguration configuration)
            {
                _context = context;
                _renderer = renderer;
                _tokenService = tokenService;
                _configuration = configuration;
            }
            public async Task<ApiResult<int?>> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
            {

                var user = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == request.Model.UserId);
                if (user == null)
                {
                    request.Errors.Add("xeta", "İstifadəçi tapılmadı.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                var token = _tokenService.MakeToken();

                var activeToken = user.User.Tokens.Where(c => c.UsersTokensStatusesId == (int)TokenStatus.Active
                                            &&
                                            c.UsersTokensTypesId == (byte)UserTokenType.Confirmation
                                            &&
                                            c.ExpireDate >= DateTime.Now).FirstOrDefault();

                if (activeToken != null)
                {
                    request.Errors.Add("error", "Təsdiq emaili artıq göndərilmişdir.");
                    return ApiResult<int?>.CreateResponse(null, request.Errors);
                }

                await _tokenService.AddToDatabaseAsync(token, new TokenSessionInfo
                {
                    UserId = user.UsersId,
                    ExpireDate = DateTime.Now.AddDays(30)
                }, UserTokenType.Confirmation);


                try
                {
                    var from = new MailAddress(_configuration["MVC:EmailCredentials:Email"], _configuration["MVC:EmailCredentials:DisplayName"]);
                    var to = new MailAddress(user.Email);

                    var model = new ConfirmationVm
                    {
                        Name = user.User.Name,
                        Url = request.Model.ResponseUrl + "&token=" + token
                    };

                    //string view = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,"ILoveBaku.Infrastructure","Templates","User","Confirmation");
                    string view = "/Views/User/Confirmation";
                    var htmlBody = await _renderer.RenderViewToStringAsync($"{view}.cshtml", model);

                    var message = new MailMessage(from, to)
                    {
                        Subject = "Confirmation",
                        Body = htmlBody
                    };

                    message.AlternateViews.Add(
                      AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html));

                    using (var smtp = new SmtpClient(_configuration["MVC:EmailCredentials:MailServer"], Convert.ToInt32(_configuration["MVC:EmailCredentials:Port"])))
                    {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(_configuration["MVC:EmailCredentials:Email"], _configuration["MVC:EmailCredentials:Password"]);
                        await smtp.SendMailAsync(message);
                    }

                    return ApiResult<int?>.CreateResponse(200);
                }
                catch (Exception e)
                {
                    throw new NullReferenceException();
                    //return StatusCode(500, $"Failed to send email: {e.Message}");
                }
            }
        }
    }
}
