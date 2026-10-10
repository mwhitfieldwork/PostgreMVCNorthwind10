using System;
using System.Collections.Generic;
using System.Text;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface ITokenService
    {
        string CreateToken(string userId, string email, string? name = null);
    }
}
