using ILoveBaku.Application.Common.Models;
using ILoveBaku.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Interfaces
{
    public interface IToken
    {
        Dictionary<string, TokenSessionInfo> Tokens { get;  set; }
        Task<bool> CheckTokenByClaim(string token, Claims? claim);
        void Add(string key, TokenSessionInfo tokenSessionInfo);
        Task AddToDatabaseAsync(string key, TokenSessionInfo tokenSessionInfo, UserTokenType tokenType);
        void Remove(Guid userId);
        Task RemoveFromDatabase(string token);
        string MakeToken();
        bool HasValue(Guid userId);
    }
}
