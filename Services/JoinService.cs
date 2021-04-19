using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public JoinService(ChatContext db , ICryptoProvider cryptoProvider,
            IHttpContextAccessor contextAccessor , ILogger<JoinService> logger)
        {
            _db = db;
            _cryptoProvider = (CryptoProvider)cryptoProvider;
            _contextAcessor = contextAccessor;
            _logger = logger;
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


        public Task<Client> JoinClient(JoinModel model)
        {
            return Task.Run(() => JoinClientAsync(model));
        }


        private async Task<Guid> JoinUserAsync(JoinModel model , string type)
        {

            var localHash = _cryptoProvider.GetRandomSaltString();
            var passwordHash = _cryptoProvider.GetPasswordHash(model.PasswordString, localHash);

            User user = new User
            {
                UserGuid = Guid.NewGuid(),
                Email = model.Email,
                Phone = model.Phone,
                LocalHash = localHash,
                PasswordHash = passwordHash,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Role = _db.Roles.FirstOrDefault(x => x.Name == type.ToUpper())
            };

           var tt = user;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user.UserGuid;
        }


        private async Task<Client> JoinClientAsync(JoinModel model)
        {
            try
            {
                Guid userGuid= await JoinUserAsync(model , Roles.Client);

                Client client = new Client
                {
                    Age = model.Age,
                    UserIp = _contextAcessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserGuid = userGuid,
                    User = _db.Users.SingleOrDefault(x => x.Email == model.Email)
                };

                _db.Clients.Add(client);


                await _db.SaveChangesAsync();
                _logger.LogInformation($"Client email {client.User.Email} has been joined");

                return client;
            }
            catch(DbUpdateException e)
            {
                _logger.LogError(e.HResult, (Exception)e, e.Message);

                //returns empty client enity
                return new Client();
            }
            
        }

        public Task<Employee> JoinEmployee(JoinEmployeeModel model)
        {
            return Task.Run(() => JoinEmployeeAsync(model));
        }


        private async Task<Employee> JoinEmployeeAsync(JoinEmployeeModel model)
        {

            var localHash = _cryptoProvider.GetRandomSaltString();
            var passwordHash = _cryptoProvider.GetPasswordHash(model.PasswordString, localHash);

            try
            {
                
                Guid userGuid = await JoinUserAsync(model, Roles.Employee);

                Employee employee = new Employee
                {

                    Age = model.Age,
                    UserGuid = userGuid,
                    User = _db.Users.SingleOrDefault(x => x.Email == model.Email)
                 }; 
                
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                return employee;

            }catch(DbUpdateException e)
            {
                _logger.LogError(e.HResult, (Exception)e, e.Message);

                //returns empty employee entity
                return new Employee();
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
                await JoinUserAsync(model, Roles.Admin);

                return true;
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.HResult, (Exception)e, e.Message);
                return false;

            }
        }
    }
}
