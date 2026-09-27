using Microsoft.AspNetCore.Mvc;
using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NWCodeFirstMVC.Domain.GoogleAuthModels;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface ILoginService : IGenericRepository<User>
    {
        public Task<IActionResult> Authenticate(User userModel);
        Task<IActionResult> AuthenticateWithGoogle(GoogleUserInfo googleUser);

    }
}
