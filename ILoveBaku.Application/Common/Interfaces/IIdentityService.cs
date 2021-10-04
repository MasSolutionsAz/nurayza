using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Commands.RegisterUser;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IQueryable<Users>> GetUsers();
        Task GenerateUserCard(Guid userId);
        Task<IQueryable<Users>> GetUsers(Expression<Func<Users, bool>> expression);
        Task<Users> GetUserAsync(Guid userId);
        Task<Users> GetUserAsync(Expression<Func<Users,bool>> expression);
        Task<UsersLogins> GetUserLoginAsync(Guid userId);
        Task<UsersLogins> GetUserLoginAsync(Expression<Func<UsersLogins, bool>> expression);
        Task<UsersTokens> GetUserTokenByToken(string token);
        Task<string> GetUserNameAsync(Guid userId);
        Task<string> GetUserNameAsync(Expression<Func<Users, bool>> expression);
        Task<string> GetUserEmailAsync(Guid userId);
        Task<string> GetUserEmailAsync(Expression<Func<UsersLogins, bool>> expression);
        Task<bool> HasUserAsync(Expression<Func<UsersLogins, bool>> expression);
        bool CheckUserLoginPassword(UsersLogins userLogin, string password);
        Task<ApiResult<UserResponse>> Register(RegisterVM model, CancellationToken cancellationToken);
        Task<ApiResult<UserResponse>> Login(LoginVM model, CancellationToken cancellationToken);
        Task<ApiResult<UserResponse>> LoginExternal(Guid userLoginId);
        Task ChangePasswordAsync(UsersLogins userLogin, string newPassword);
        Task<ApiResult<string>> LogoutUser(string token);
        Task<ApiResult<Guid?>> AddExternalUser(UserDto model);
    }
}
