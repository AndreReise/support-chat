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

namespace TechnicalSupport.Services
{
    public class AuthService : IAuthService
    {
        private SupportContext _db;
        private readonly CryptoProvider _cryProvider;
        private readonly IHttpContextAccessor _contextAcessor;
        public AuthService(SupportContext dbContext , ICryptoProvider cryptoProvider , IHttpContextAccessor contextAccessor)
        {
            _db = dbContext;
            _cryProvider = (CryptoProvider)cryptoProvider;
            _contextAcessor = contextAccessor;

        }


        public Task AuthenticateUserAsync(AuthModel model)
        {

            return Task.Run(() => AuthenticateUser(model));

        }
        public Task SignOutAsync()
        {
            return Task.Run(() => SignOut());
        }

        private async Task AuthenticateUser(AuthModel model)
        {
            var user = _db.Users.SingleOrDefault(x => x.Email == model.UserString || x.Phone == model.UserString);

            List<Claim> userClaims;

            if (user != null)
            {
                userClaims = user.RoleName == 1
                    ? userClaims = CreateClientClaims( await VerifyClient(user, model.Password))
                    : userClaims = CreateEmployeeClaims (await VerifyEmployee(user , model.Password));

                var id = new ClaimsIdentity(userClaims, "ApplicaionCookie");
                var claimsPrincipal = new ClaimsPrincipal(id);

                await AuthenticationHttpContextExtensions.SignInAsync(_contextAcessor.HttpContext, claimsPrincipal);
                
            }
            else
            {
                //change !!!
                
            }

        }

        private async Task SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(_contextAcessor.HttpContext);
        }
        private async Task<object> VerifyClient(User user , string password)
        {
            var client = await _db.Clients.SingleOrDefaultAsync(
                x => x.ClientId == user.RoleId);

            if (isCorrectPassword(password, client.PasswordHash, client.LocalHash))
            {
                return client;
            }
            else
            {
                return new 
                {
                    ErrorMessage = "Wrong Credentials"
                };
            }
        }

        private async Task<object> VerifyEmployee(User user , string password)
        {
            var employee = await _db.Employees.SingleOrDefaultAsync(
                x => x.EmployeeId == user.RoleId
                );

            if(isCorrectPassword(password , employee.PasswordHash , employee.LocalHash)){

                return employee;

            }
            else
            {
                return new
                {
                    ErrorMessage = "Wrong Credentials"
                };
            }
        }

        private List<Claim> CreateClientClaims(object clientObject)
        {
            Client client = (Client)clientObject;

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType , client.FirstName),
                new Claim(ClaimTypes.Role, nameof(Client).ToUpper() ),
            };

            return claims;
        }
        private List<Claim> CreateEmployeeClaims(object employeeObject)
        {
            Employee employee = (Employee)employeeObject;

            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType , employee.FirstName),
                new Claim(ClaimTypes.Role, nameof(Employee).ToUpper())
            };

            return claims;
        }
        private bool  isCorrectPassword(string password , byte[] dbPassword, byte[] l_hash)
        {
            byte[] passwordHash = _cryProvider.GetPasswordHash(password, l_hash);

            return passwordHash.SequenceEqual(dbPassword);
        }
        
    }
}
