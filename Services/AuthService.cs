using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace TechnicalSupport.Services
{

    public class AuthService : IAuthService
    {
        private ChatContext _db;
        private readonly CryptoProvider _cryProvider;
        private readonly IHttpContextAccessor _contextAcessor;
        private AuthStatusResult _authResult;
        public AuthService(ChatContext dbContext , ICryptoProvider cryptoProvider , IHttpContextAccessor contextAccessor)
        {
            _db = dbContext;
            _cryProvider = (CryptoProvider)cryptoProvider;
            _contextAcessor = contextAccessor;
            _authResult = new AuthStatusResult();

        }

        public Task<AuthStatusResult> AuthenticateUserAsync(AuthModel model)
        {
            return Task.Run(() => AuthenticateUser(model));
        }

        private async Task<AuthStatusResult> AuthenticateUser(AuthModel model)
        {
            if(model.UserString == "test")
            {
                var id = new ClaimsIdentity(new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, "Name") }, "ApplicationCookie");
                var cp = new ClaimsPrincipal(id);
                await AuthenticationHttpContextExtensions.SignInAsync(_contextAcessor.HttpContext, cp);
                return _authResult;
            }
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Phone == model.UserString || x.Email == model.UserString);

            if(user == null)
            {
                _authResult.ErrorMessage = "User not found";
                _authResult.isSuccessful = false;

                return _authResult;
            }

            List<Claim> userClaims = await VerifyUserAsync(user);

            if(userClaims == null)
            {
                _authResult.ErrorMessage = "Wrong Credentials";
                _authResult.isSuccessful = false;

                return _authResult;
            }

            var u_id = new ClaimsIdentity(userClaims, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(u_id);

            await AuthenticationHttpContextExtensions.SignInAsync(_contextAcessor.HttpContext, claimsPrincipal);

            return _authResult;
        }


        private Task< List<Claim> > VerifyUserAsync(User user)
        {
            return Task.Run(() => VerifyUser(user));
        }


        private async Task< List<Claim>> VerifyUser(User user)
        {
            switch (1)
            {
                case 1:
                    return await CreateClientClaims(user);
                    break;
                case 2:
                    return await CreateEmployeeClaims(user);
                    break;
                default:
                    return null;
                    break;
            }
        }


        private async Task<List<Claim>> CreateClientClaims(User user)
        {
            var client = await _db.Clients.SingleOrDefaultAsync(x => x.ClientId == user.RoleId);

            if(client == null)
            {
                ///logic 
                ///
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , client.FirstName + client.LastName),
                new Claim(ClaimTypes.Role , nameof(client).ToUpper())
            };

            return claims;
        }
        private async Task<List<Claim>> CreateEmployeeClaims(User user)
        {
            var employee = await _db.Employees.SingleOrDefaultAsync(x => x.EmployeeId == user.RoleId);

            if(employee == null)
            {
                //logic 

                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , employee.FirstName + employee.LastName),
                new Claim(ClaimTypes.Role , nameof(employee).ToUpper())
            };

            return claims;
        }
        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
