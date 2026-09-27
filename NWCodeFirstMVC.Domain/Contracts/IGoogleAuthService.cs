using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NWCodeFirstMVC.Domain.GoogleAuthModels;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IGoogleAuthService
    {
        Task<GoogleTokenResponse> ExchangeCodeAsync(string code);
        Task<GoogleUserInfo> GetUserInfoAsync(string accessToken);
    }
}
