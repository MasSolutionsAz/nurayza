using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.User.Commands.LoginUser;
using ILoveBaku.Application.CQRS.User.Commands.RegisterUser;
using ILoveBaku.Application.CQRS.User.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.Infrastructure.Extensions;
using ILoveBaku.Infrastructure.Helpers;
using ILoveBaku.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext _context;

        private readonly IToken _tokenService;

        private readonly IIPService _ipService;

        public IdentityService(ApplicationDbContext context, IToken tokenService, IIPService IPService)
        {
            _context = context;
            _tokenService = tokenService;
            _ipService = IPService;
        }

        public async Task<IQueryable<Users>> GetUsers()
        {
            return await Task.FromResult(_context.Users);
        }

        public async Task<IQueryable<Users>> GetUsers(Expression<Func<Users, bool>> expression)
        {
            return await Task.FromResult(_context.Users.Where(expression));
        }

        public async Task<Users> GetUserAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Users> GetUserAsync(Expression<Func<Users, bool>> expression)
        {
            return await _context.Users.FirstOrDefaultAsync(expression);
        }

        public async Task<UsersLogins> GetUserLoginAsync(Guid userId)
        {
            return await _context.UsersLogins.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<UsersLogins> GetUserLoginAsync(Expression<Func<UsersLogins, bool>> expression)
        {
            return await _context.UsersLogins.Include(ul => ul.User).ThenInclude(u => u.Tokens).FirstOrDefaultAsync(expression);
        }

        public async Task<string> GetUserEmailAsync(Guid userId)
        {
            return (await _context.UsersLogins.FirstOrDefaultAsync(u => u.UsersId == userId))?.Email;
        }

        public async Task<string> GetUserEmailAsync(Expression<Func<UsersLogins, bool>> expression)
        {
            return (await _context.UsersLogins.FirstOrDefaultAsync(expression))?.Email;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            return (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))?.Name;
        }

        public async Task<string> GetUserNameAsync(Expression<Func<Users, bool>> expression)
        {
            return (await _context.Users.FirstOrDefaultAsync(expression))?.Name;
        }

        public async Task<bool> HasUserAsync(Expression<Func<UsersLogins, bool>> expression)
        {
            return await _context.UsersLogins.AnyAsync(expression);
        }

        public bool CheckUserLoginPassword(UsersLogins userLogin, string password)
        {
            return Hashing.VerifyPassword(userLogin.Password, userLogin.Salt, password);
        }
        public async Task GenerateUserCard(Guid userId)
        {
            long cardNumber = Convert.ToInt64(Luhn.Generate());

            while (await _context.UsersCards.AnyAsync(uc => uc.CardNumber == cardNumber)) cardNumber = Convert.ToInt64(Luhn.Generate());

            UsersCards userCard = new UsersCards()
            {
                UsersId = userId,
                CardNumber = cardNumber,
                UserCardsTypesId = (byte)UserCardType.BonusCard,
                UserCardsStatusesId = (byte)UserCardStatus.Active,
                CreatedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddYears(1)
            };

            await _context.UsersCards.AddAsync(userCard);

            await _context.SaveChangesAsync();
        }
        public async Task<ApiResult<UserResponse>> Register(RegisterVM model, CancellationToken cancellationToken)
        {
            if (await _context.UsersLogins.AnyAsync(ul => ul.Email == model.RegisterEmail))
                return ApiResult<UserResponse>.CreateResponse(null,
                                               new Dictionary<string, string>()
                                               {
                                                   { "RegisterEmail" , "Bu Email artıq istifadə olunub." }
                                               },
                                               new ErrorDetail()
                                               {
                                                   ErrorMessage = "register failed"
                                               });

            Users user = new Users()
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Surname = model.Surname.Trim(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedIp = _ipService.GetRequestIP<int>(),
                UsersStatusesId = (byte)UserStatus.Active,
                BranchesId = 1,
                ContactEmail = model.RegisterEmail,
                Phone = model.PhoneNumber
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync(cancellationToken);

            byte[] salt = Hashing.GenerateSalt();

            UsersLogins userLogin = new UsersLogins()
            {
                Id = Guid.NewGuid(),
                UsersId = user.Id,
                Email = model.RegisterEmail,
                Password = Hashing.Hash(model.RegisterPassword, salt),
                Salt = salt,
                EmailConfirmed = false,
                CreatedIp = user.CreatedIp,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            await _context.UsersLogins.AddAsync(userLogin);

            long cardNumber = Convert.ToInt64(Luhn.Generate());

            while (await _context.UsersCards.AnyAsync(uc => uc.CardNumber == cardNumber)) cardNumber = Convert.ToInt64(Luhn.Generate());

            UsersCards userCard = new UsersCards()
            {
                UsersId = user.Id,
                CardNumber = cardNumber,
                UserCardsTypesId = (byte)UserCardType.BonusCard,
                UserCardsStatusesId = (byte)UserCardStatus.Active,
                CreatedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddYears(1)
            };

            await _context.UsersCards.AddAsync(userCard);

            await _context.SaveChangesAsync(cancellationToken);

            return await Login(new LoginVM() { Email = userLogin.Email, Password = model.RegisterPassword }, cancellationToken);
        }

        public async Task<ApiResult<UserResponse>> Login(LoginVM model, CancellationToken cancellationToken)
        {
            //find userlogin
            UsersLogins userLogin = await GetUserLoginAsync(u => u.Email == model.Email &&
                                                                 u.LockoutEnd <= DateTime.Now &&
                                                                 u.User.UsersStatusesId == (int)UserStatus.Active);

            //check exists and correct credentials
            if (userLogin.IsNull() || !CheckUserLoginPassword(userLogin, model.Password))
            {
                return ApiResult<UserResponse>.CreateResponse(null,
                                               new Dictionary<string, string>
                                               {
                                                   { "" , "Email və ya şifrə yanlışdır." }
                                               },
                                               new ErrorDetail()
                                               {
                                                   ErrorMessage = "login failed"
                                               });
            }

            //block all tokens
            userLogin.User.Tokens
                            .Where(u => u.UsersTokensStatusesId == (byte)TokenStatus.Active && u.UsersTokensTypesId == (byte)UserTokenType.Login)
                            .ForEach(u => u.UsersTokensStatusesId = (byte)TokenStatus.Blocked);

            //make token
            string token = _tokenService.MakeToken();

            //add token to database
            await _tokenService.AddToDatabaseAsync(token, new TokenSessionInfo
            {
                UserId = userLogin.UsersId,
                Claims = userLogin.User.Claims.ToList(),
                ExpireDate = DateTime.Now.AddDays(7)
            },UserTokenType.Login);

            //add token to cache
            _tokenService.Add(token, new TokenSessionInfo
            {
                UserId = userLogin.UsersId,
                Claims = userLogin.User.Claims.ToList(),
                ExpireDate = DateTime.Now.AddDays(7)
            });

            await _context.SaveChangesAsync(cancellationToken);

            List<Guid> roleIds = _context.UsersRolesRelations.Where(c => c.UsersId == userLogin.UsersId).Select(c => c.UsersRolesId).ToList();
            string roles = null;
            for (int i = 0; i < roleIds.Count; i++)
            {
                var role = await _context.UsersRoles.Where(c => c.Id == roleIds[i]).FirstOrDefaultAsync() ;
                if (role != null)
                {
                    roles += role.Name;
                }
            }
            
            return ApiResult<UserResponse>.CreateResponse(new UserResponse
            {
                Id = userLogin.UsersId,
                Token = token,
                Name = userLogin.User.Name,
                Surname = userLogin.User.Surname,
                BranchId = userLogin.User.BranchesId,
                Roles = roles
            });
        }

        public async Task ChangePasswordAsync(UsersLogins userLogin, string newPassword)
        {
            userLogin.Password = Hashing.Hash(newPassword, userLogin.Salt);
            await _context.SaveChangesAsync();
        }

        public async Task<UsersTokens> GetUserTokenByToken(string token)
        {
            return await _context.UsersTokens.FirstOrDefaultAsync(u => u.Value == token);
        }

        public async Task<ApiResult<string>> LogoutUser(string token)
        {
            UsersTokens userToken = await GetUserTokenByToken(token);
            if (userToken == null)
                return ApiResult<string>.CreateResponse(null, null, new ErrorDetail
                {
                    ErrorMessage = "Bele bir login yoxdur"
                });

            _tokenService.Remove(userToken.UsersId);

            await _tokenService.RemoveFromDatabase(token);

            return ApiResult<string>.CreateResponse("success", null, null);
        }

        public async Task<ApiResult<UserResponse>> LoginExternal(Guid userLoginId)
        {
            //find userlogin
            UsersLogins userLogin = await GetUserLoginAsync(u => u.Id == userLoginId &&
                                                                 u.LockoutEnd <= DateTime.Now &&
                                                                 u.User.UsersStatusesId == (int)UserStatus.Active);

            UsersExternalLogins userExternalLogin = await _context.UsersExternalLogins.Where(c => c.UsersId == userLogin.UsersId).FirstOrDefaultAsync();
            if (userExternalLogin == null)
            {
                return ApiResult<UserResponse>.CreateResponse(null);
            }

            //block all tokens
            userLogin.User.Tokens
                            .Where(u => u.UsersTokensStatusesId == (byte)TokenStatus.Active)
                            .ForEach(u => u.UsersTokensStatusesId = (byte)TokenStatus.Blocked);

            //make token
            string token = _tokenService.MakeToken();

            //add token to database
            await _tokenService.AddToDatabaseAsync(token, new TokenSessionInfo
            {
                UserId = userLogin.UsersId,
                Claims = userLogin.User.Claims.ToList(),
                ExpireDate = DateTime.Now.AddDays(7)
            },UserTokenType.Login);

            //add token to cache
            _tokenService.Add(token, new TokenSessionInfo
            {
                UserId = userLogin.UsersId,
                Claims = userLogin.User.Claims.ToList(),
                ExpireDate = DateTime.Now.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return ApiResult<UserResponse>.CreateResponse(new UserResponse
            {
                Id = userLogin.UsersId,
                Token = token,
                Name = userLogin.User.Name,
                Surname = userLogin.User.Surname,
                BranchId = userLogin.User.BranchesId
            });
        }

        public async Task<ApiResult<Guid?>> AddExternalUser(UserDto model)
        {
            Users user = new Users()
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Surname = model.Surname.Trim(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedIp = _ipService.GetRequestIP<int>(),
                UsersStatusesId = (byte)UserStatus.Active,
                BranchesId = 1,
                ContactEmail = model.Email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            UsersExternalLogins externalLogin = new UsersExternalLogins
            {
                Id = Guid.NewGuid(),
                UsersId = user.Id,
                UsersExternalLoginsProvidersId = (byte)Enum.Parse(typeof(UserExternalLoginProvider), model.Provider),
                ProviderKey = model.Provider
            };


            await _context.UsersExternalLogins.AddAsync(externalLogin);
            await _context.SaveChangesAsync();


            byte[] salt = Hashing.GenerateSalt();

            UsersLogins userLogin = new UsersLogins()
            {
                Id = Guid.NewGuid(),
                UsersId = user.Id,
                Email = user.ContactEmail,
                Password = Hashing.Hash(Guid.NewGuid().ToString(), salt),
                Salt = salt,
                EmailConfirmed = false,
                CreatedIp = user.CreatedIp,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            await _context.UsersLogins.AddAsync(userLogin);

            long cardNumber = Convert.ToInt64(Luhn.Generate());

            while (await _context.UsersCards.AnyAsync(uc => uc.CardNumber == cardNumber)) cardNumber = Convert.ToInt64(Luhn.Generate());

            UsersCards userCard = new UsersCards()
            {
                UsersId = user.Id,
                CardNumber = cardNumber,
                UserCardsTypesId = (byte)UserCardType.BonusCard,
                UserCardsStatusesId = (byte)UserCardStatus.Active,
                CreatedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddYears(1)
            };

            await _context.UsersCards.AddAsync(userCard);
            await _context.SaveChangesAsync();

            return ApiResult<Guid?>.CreateResponse(user.Id);
        }
    }
}
