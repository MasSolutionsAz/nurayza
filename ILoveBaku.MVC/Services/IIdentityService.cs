using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public interface IIdentityService
    {
        Task SignInAsync(UserResponse user);
        Task SignOutAsync();
    }
}
