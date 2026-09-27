using BCrypt.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.PocoModels;
using NWCodeFirstMVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NWCodeFirstMVC.Domain.GoogleAuthModels;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class LoginService : GenericService<User>, ILoginService
    {
        private readonly PgNwContext _dc;
        public LoginService(PgNwContext dc) : base(dc)
        {
            this._dc = dc;
        }
        [HttpPost]
        public async Task<IActionResult> Authenticate(User userModel)
        {
            if (userModel == null || string.IsNullOrEmpty(userModel.UserName) || string.IsNullOrEmpty(userModel.Passowrd))
            {
                return new BadRequestObjectResult("Invalid input data.");
            }
            //userModel.Passowrd = BCrypt.Net.BCrypt.HashPassword(userModel.Passrd);
            var userDetails = await _dc.Users
            .FirstOrDefaultAsync(x => x.Username == userModel.UserName && x.Password == userModel.Passowrd);



            if (userDetails == null)
            {
                return new UnauthorizedObjectResult("Invalid username or password.");
            }

            return new OkObjectResult(new
            {
                Message = "Authentication successful.",
                User = userDetails
                // Token = token // Uncomment if a token is generated
            });

        }
        public async Task<IActionResult> AuthenticateWithGoogle(GoogleUserInfo googleUser)
        {
            var userDetails = await _dc.Users
                .FirstOrDefaultAsync(x => x.Username == googleUser.Email);

            if (userDetails == null)
            {
                userDetails = new NWCodeFirstMVC.Infrastructure.PgModels.User
                {
                    Username = googleUser.Email,
                    Password = Guid.NewGuid().ToString(),
                    Firstname = googleUser.Name,
                    Admin = false, // granted later via the Admin Users screen
                    Occupation = string.Empty,
                    Picture = googleUser.Picture
                };

                _dc.Users.Add(userDetails);
            }
            else
            {
                userDetails.Picture = googleUser.Picture;
                // Admin is no longer touched here — managed via the checkbox in Admin Users
            }

            await _dc.SaveChangesAsync();

            return new OkObjectResult(new
            {
                Message = "Authentication successful.",
                User = userDetails
            });
        }

    }
}
