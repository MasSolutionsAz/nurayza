using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Application.CQRS.User.Commands.RegisterUser;
using ILoveBaku.Application.CQRS.User.Commands.SendConfirmationEmail;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Commands.AddManualUser
{
    public class PasswordInfoModel
    {
        public string Link { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
    public class UnRegisteredPaymentModel
    {
        [Required(ErrorMessage = "Ad boş qala bilməz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad boş qala bilməz.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Email boş qala bilməz.")]
        public string Email { get; set; }



        [Required(ErrorMessage  = "Telefon boş qala bilməz.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Telefon nömrəsi düzgün deyil.")]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Telefon nömrəsi düzgün deyil.")]


        public string Phone { get; set; }

 

        [Required(ErrorMessage = "Ölkə/Şəhər boş qala bilməz.")]
        public string RegionId { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa address qeyd edin.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa ödəniş növünü qeyd edin.")]
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa çatdırılma növünü qeyd edin.")]
        public string ShipmentOptions { get; set; }
        public string ZipCode { get; set; }
        public int ShippingPrice { get; set; }
        public string ShipmentDate { get; set; }


        public string ResponseUrl { get; set; }
        public string ConfirmationUrl { get; set; }

        public List<CartDetailDto> CartDetails { get; set; }
    }
    public class AddManualUserCommand:BaseRequest<ApiResult<UserResponse>>
    {
        public UnRegisteredPaymentModel Model { get; set; }
        public class AddManualUserCommandHandler : IRequestHandler<AddManualUserCommand, ApiResult<UserResponse>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityService _identityService;
            private readonly IRazorViewToStringRenderer _renderer;
            private readonly IConfiguration _configuration;
            private readonly IToken _tokenService;

            public AddManualUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IRazorViewToStringRenderer renderer, IConfiguration configuration,IToken  tokenService)
            {
                _context = context;
                _identityService = identityService;
                _renderer = renderer;
                _configuration = configuration;
                _tokenService = tokenService;
            }
            public async Task<ApiResult<UserResponse>> Handle(AddManualUserCommand request, CancellationToken cancellationToken)
            {
                //Create User
                var password = RandomPasswordGenerator.InvokeAsync();
                var registerModel = new RegisterVM
                {
                    Name = request.Model.Name,
                    Surname = request.Model.Surname,
                    RegisterEmail = request.Model.Email,
                    RegisterPassword = password,
                    ConfirmPassword = password,
                    PhoneNumber = request.Model.Phone
                };

                var userLoginInfo = await _identityService.Register(registerModel, cancellationToken);
                if (!userLoginInfo.Succeeded)
                {
                    request.Errors.Add("", "Email artıq istifadə olunub.");
                    return ApiResult<UserResponse>.CreateResponse(null,request.Errors);
                }
                //end create user
                var userId = userLoginInfo.Response.Id;

                //create address to the user
                UsersAddressInfo address = new UsersAddressInfo
                {
                    Address = request.Model.Address,
                    RegionsId = Convert.ToInt32(request.Model.RegionId),
                    UsersId = userId,
                    ZipCode = request.Model.ZipCode,
                    IsActive = true
                };
                await _context.UsersAddressInfo.AddAsync(address);
                await _context.SaveChangesAsync();

                userLoginInfo.Response.AddressId = address.Id;


                //send password email
                await SendPasswordEmailAsync(_context,password, userLoginInfo, request.Model.ResponseUrl);
                //end send password email


                //send confirmation email
                var user = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == userLoginInfo.Response.Id);
                var token = _tokenService.MakeToken();

                var activeToken = user.User.Tokens.Where(c => c.UsersTokensStatusesId == (int)TokenStatus.Active
                                            &&
                                            c.UsersTokensTypesId == (byte)UserTokenType.Confirmation
                                            &&
                                            c.ExpireDate >= DateTime.Now).FirstOrDefault();

                if (activeToken != null)
                {
                    request.Errors.Add("error", "Təsdiq emaili artıq göndərilmişdir.");
                    return ApiResult<UserResponse>.CreateResponse(null, request.Errors);
                }

                await _tokenService.AddToDatabaseAsync(token, new TokenSessionInfo
                {
                    UserId = user.UsersId,
                    ExpireDate = DateTime.Now.AddDays(30)
                }, UserTokenType.Confirmation);

                await SendConfirmationEmailAsync(_configuration, userLoginInfo.Response.Name, user.Email, request.Model.ConfirmationUrl+="/?userId="+user.UsersId, token);;
                //end send confirmation email

                return ApiResult<UserResponse>.CreateResponse(userLoginInfo.Response);
                //end create address to the user
            }

            private async Task SendPasswordEmailAsync(IApplicationDbContext _context,string password,ApiResult<UserResponse> userLoginInfo,string responseUrl)
            {
                var userInfo = await _context.UsersLogins.FirstOrDefaultAsync(c => c.UsersId == userLoginInfo.Response.Id);
                var from = new MailAddress("ilovebaku.official@gmail.com", "ILoveBaku Admin");
                var to = new MailAddress(userInfo.Email); //new MailAddress("info@ilovebaku.az")
                var model = new PasswordInfoModel()
                {
                    Password = password,
                    Link = responseUrl,
                    Name = userLoginInfo.Response.Name
                };
                string view = "/Views/User/PasswordInfo";
                var htmlBody = await _renderer.RenderViewToStringAsync($"{view}.cshtml", model);

                var message = new MailMessage(from, to)
                {
                    Subject = "Password Info",
                    Body = htmlBody
                };

                message.AlternateViews.Add(
                  AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html));

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("ilovebaku.official@gmail.com", "ilovebaku123@");
                    await smtp.SendMailAsync(message);
                }
            }

            private async Task SendConfirmationEmailAsync(IConfiguration _configuration,string name,string email,string responseUrl,string token)
            {
                var from = new MailAddress(_configuration["MVC:EmailCredentials:Email"], _configuration["MVC:EmailCredentials:DisplayName"]);
                var to = new MailAddress(email);

                var model = new ConfirmationVm
                {
                    Name = name,
                    Url = responseUrl + "&token=" + token
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
            }
        }
    }
}
