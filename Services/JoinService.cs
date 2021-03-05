using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public class JoinService : IJoinService
    {

        private readonly ChatContext _db;
        private readonly CryptoProvider _cryptoProvider;
        private readonly IHttpContextAccessor _contextAcessor;

        public JoinService(ChatContext db , ICryptoProvider cryptoProvider , IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _cryptoProvider = (CryptoProvider)cryptoProvider;
            _contextAcessor = contextAccessor;
        }


        public Task<bool> canJoin(JoinModel model)
        {
            return Task.Run(() => canJoinAsync(model));
        }


        private async Task<bool> canJoinAsync(JoinModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email || x.Phone == model.Phone);

            return user == null;
        }


        public Task JoinClient(JoinModel model)
        {
            return Task.Run(() => JoinClientAsync(model));
        }


        private async Task JoinUserAsync(JoinModel model , string type)
        {

            var localHash = _cryptoProvider.GetRandomSaltString();
            var passwordHash = _cryptoProvider.GetPasswordHash(model.PasswordString, localHash);

            User user = new User
            {
                Email = model.Email,
                Phone = model.Phone,
                LocalHash = localHash,
                PasswordHash = passwordHash,
                Role = _db.Roles.First(x => x.Name == type.ToUpper())
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }


        private async Task JoinClientAsync(JoinModel model)
        {

            Client client = new Client
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Phone = model.Phone,
                Email = model.Email,
                //Sex = _db.Sexes.First(x => x.SexName == model.SexName) //NEED SEX TABLE CHANGES 
                UserIp = _contextAcessor.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            try
            {
                await JoinUserAsync(model , nameof(Client));
                _db.Clients.Add(client);

                await _db.SaveChangesAsync();

            }catch(DbUpdateException e)
            {

            }
            
        }

        public Task<bool> JoinEmployee(JoinEmployeeModel model)
        {
            return Task.Run(() => JoinEmployeeAsync(model));
        }


        private async Task<bool> JoinEmployeeAsync(JoinEmployeeModel model)
        {

            var localHash = _cryptoProvider.GetRandomSaltString();
            var passwordHash = _cryptoProvider.GetPasswordHash(model.PasswordString, localHash);

            Employee employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Email = model.Email
            };

            try
            {
                await JoinUserAsync(model, nameof(Employee));
                _db.Employees.Add(employee);

                await _db.SaveChangesAsync();

                return true;

            }catch(Exception e)
            {
                return false;
            }

        }


        public Task<bool> JoinAdmin(JoinModel model)
        {

            return Task.Run(() => JoinAdminAsync(model));
        }


        private async Task<bool> JoinAdminAsync(JoinModel model)
        {

            try
            {
                await JoinUserAsync(model, "ADMIN");

                return true;
            }
            catch (DbUpdateException e)
            {

                return false;

            }
        }
    }
}
