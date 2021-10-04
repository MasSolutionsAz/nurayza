using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Commands.AddExternalUser;
using ILoveBaku.Application.CQRS.User.Commands.AddUserAddress;
using ILoveBaku.Application.CQRS.User.Commands.ChangePassword;
using ILoveBaku.Application.CQRS.User.Commands.ConfirmUser;
using ILoveBaku.Application.CQRS.User.Commands.DeleteUserAddress;
using ILoveBaku.Application.CQRS.User.Commands.LoginExternalUser;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Commands.LogoutUser;
using ILoveBaku.Application.CQRS.User.Commands.RegisterUser;
using ILoveBaku.Application.CQRS.User.Commands.ResetAndUpdatePassword;
using ILoveBaku.Application.CQRS.User.Commands.SendConfirmationEmail;
using ILoveBaku.Application.CQRS.User.Commands.UpdateUser;
using ILoveBaku.Application.CQRS.User.Commands.UpdateUserAddress;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetResetToken;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddress;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Application.CQRS.User.Queries.GetUserConfirmation;
using ILoveBaku.Application.CQRS.User.Queries.GetUserLogin;
using ILoveBaku.Application.CQRS.User.Queries.GetUserTokenExsistence;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseController
    {
       
        [HttpGet("confirmation")]
        public async Task<ActionResult<ApiResult<bool>>> GetConfirmation()
        {
            return await Mediator.Send(new GetUserConfirmationQuery());
        }

        [HttpGet("reset/{email}")]
        public async Task<ActionResult<ApiResult<ResetPasswordDto>>> GetResetToken(string email)
        {
            return await Mediator.Send(new GetResetTokenQuery() { Email = email });
        }
        [HttpGet("details")]
        public async Task<ActionResult<ApiResult<UserDto>>> GetUser(string email)
        {
            
            return await Mediator.Send(new GetUserQuery() { Email = email});
        }
        [HttpGet("addresses/{addressId}")]
        public async Task<ActionResult<ApiResult<UserAddressInfoDto>>> GetAddress(int addressId)
        {
            return await Mediator.Send(new GetUserAddressQuery { AddressId = addressId });
        }
        [HttpGet("addresses")]
        public async Task<ActionResult<ApiResult<List<UserAddressInfoDto>>>> GetUserAddresses()
        {
            return await Mediator.Send(new GetUserAddressesQuery());
        }
        [HttpGet("login")]
        public async Task<ActionResult<ApiResult<Guid?>>> GetUserLogin(string email,bool isExternal,string provider)
        {
            return await Mediator.Send(new GetUserLoginQuery() { Email = email,IsExternal = isExternal,Provider = provider });
        }
        [HttpGet("tokens/{userId}/{token}")]
        public async Task<ActionResult<ApiResult<bool?>>> CheckToken(string userId,string token)
        {
            return await Mediator.Send(new GetUserTokenExsistenceQuery() { userId = userId, token = token });
        }

        [HttpPost("reset/{userId}/{token}")]
        public async Task<ActionResult<ApiResult<Guid?>>> ResetAndUpdatePassword(string userId,string token,ResetPasswordModel model)
        {
            return await Mediator.Send(new ResetAndUpdatePasswordCommand { token = token, userId = userId, Model = model });
        }
        [HttpPost("confirmation")]
        public async Task<ActionResult<ApiResult<int?>>> SendConfirmationEmail(SendConfirmEmailModel model)
        {
            return await Mediator.Send(new SendConfirmationEmailCommand { Model = model});
        }
        /// <summary>
        /// Api-a login olmaq üçün email və şifrə daxil edilir
        /// credential-lar okeydirsə geriyə token qayıdır
        /// (Daxil olmuş user-in digər bütün tokenləri block olunur)
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResult<UserResponse>>> Login(LoginVM model)
        {
            return await Mediator.Send(new LoginUserCommand(model));
        }
        [HttpPost("externalUsers")]
        public async Task<ActionResult<ApiResult<Guid?>>> AddExternalUser(UserDto model)
        {
            return await Mediator.Send(new AddExternalUserCommand { Model = model });
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResult<UserResponse>>> Register(RegisterVM model)
        {
            return await Mediator.Send(new RegisterUserCommand(model));
        }


        [HttpPut("confirmation/{userId}/{token}")]
        public async Task<ActionResult<ApiResult<int?>>> ConfirmEmail(string userId,string token)
        {
            return await Mediator.Send(new ConfirmUserCommand() { userId = userId, token = token });
        }
        /// <summary>
        /// Api-dan logout olmaq üçün header-da token qəbul edilir
        /// User api-dan logout olur və bütün tokenlər block olunur.
        /// </summary>
        [HttpPut("logout")]
        public async Task<ActionResult<ApiResult<string>>> Logout()
        {
            HttpContext.Request.Headers.TryGetValue("token", out StringValues value);
            return await Mediator.Send(new LogoutUserCommand() { Token = value });
        }
        [HttpPut("addresses/{addressId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateAddress(UserAddressInfoDto model,int addressId)
        {
            return await Mediator.Send(new UpdateUserAddressCommand { Model = model ,AddressId = addressId});
        }
        [HttpPut("externalUsers")]
        public async Task<ActionResult<ApiResult<UserResponse>>> LoginExternalUser(Guid externalUserId)
        {
            return await Mediator.Send(new LoginExternalUserLoginCommand { ExternalUserId = externalUserId });
        }
        [HttpPut("changePassword")]
        // Authorize
        public async Task<ActionResult<ApiResult<int>>> ChangePassword(ChangePasswordVM model)
        {
            return await Mediator.Send(new ChangePasswordCommand(model));
        }
        [HttpPut("details")]
        public async Task<ActionResult<ApiResult<Guid?>>> UpdateUserInfo(UserProfileInfoDto model)
        {
            return await Mediator.Send(new UpdateUserCommand { Model = model });
        }

        [HttpPost("addresses")]
        public async Task<ActionResult<ApiResult<int?>>> AddAddress(UserAddressInfoDto model)
        {
            return await Mediator.Send(new AddUserAddressCommand { Model = model });
        }
        [HttpDelete("addresses/{addressId}")]
        public async Task<ActionResult<ApiResult<int?>>> DeleteAddress(int addressId)
        {
            return await Mediator.Send(new DeleteUserAddressCommand() { AddressId = addressId });
        }
    }
}