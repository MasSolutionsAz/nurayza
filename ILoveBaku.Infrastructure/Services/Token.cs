using ILoveBaku.Application.Common.Extension;
using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Infrastructure.Services
{
    public class Token : IToken
    {
        private readonly IApplicationDbContext _context;

        private static readonly object _lock = new object();

        public Dictionary<string, TokenSessionInfo> Tokens { get; set; }

        public Token(IApplicationDbContext context)
        {
            _context = context;
            Tokens = new Dictionary<string, TokenSessionInfo>();
        }

        public void Add(string key, TokenSessionInfo tokenSessionInfo)
        {
            lock (_lock)
            {
                if (HasValue(tokenSessionInfo.UserId))
                    Remove(tokenSessionInfo.UserId);
                else
                    Tokens.Add(key, tokenSessionInfo);
            }
        }

        public async Task AddToDatabaseAsync(string key, TokenSessionInfo tokenSessionInfo, UserTokenType tokenType)
        {
            UsersTokens token = new UsersTokens
            {
                Id = Guid.NewGuid(),
                UsersId = tokenSessionInfo.UserId,
                UsersTokensStatusesId = (byte)TokenStatus.Active,
                CreatedDate = DateTime.Now,
                ExpireDate = tokenSessionInfo.ExpireDate,
                UsersTokensTypesId = (byte)tokenType,
                Value = key
            };

            await _context.UsersTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckTokenByClaim(string token, Claims? claim)
        {
            if (claim.IsNull())
                return await Task.FromResult(true);

            return await _context.UsersTokens.AnyAsync(u => u.Value == token && u.ExpireDate >= DateTime.Now &&
                                                       u.User.Claims.Any(c => c.UserClaim.Name == Enum.GetName(typeof(Claims), claim)));
        }

        public void Remove(Guid userId)
        {
            KeyValuePair<string, TokenSessionInfo> token = Tokens.FirstOrDefault(x => x.Value.UserId == userId);
            if (!token.Equals(new KeyValuePair<string, TokenSessionInfo>()))
                Tokens.Remove(token.Key);
        }

        public async Task RemoveFromDatabase(string token)
        {
            if (token != null)
            {
                var userToken = await _context.UsersTokens.FirstOrDefaultAsync(u => u.Value == token);
                userToken.UsersTokensStatusesId = (int)TokenStatus.Blocked;
                await _context.SaveChangesAsync();
            }
        }

        public bool HasValue(Guid userId)
        {
            return Tokens?.Values?.Any(t => t.UserId == userId) ?? false;
        }

        public string MakeToken()
        {
            lock (_lock)
            {
                using (SHA512 algorithm = SHA512.Create())
                {
                    byte[] hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")));

                    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
            }
        }
    }
}
