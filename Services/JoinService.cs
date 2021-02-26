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


        public Task JoinUser(JoinModel model)
        {
            return Task.Run(() => JoinUserAsync(model));
        }


        private async Task JoinUserAsync(JoinModel model)
        {
            var localHash = _cryptoProvider.GetRandomSaltString();
            var passwordHash = _cryptoProvider.GetPasswordHash(model.PasswordString, localHash);

            User user = new User
            {
                Email = model.Email,
                Phone = model.Phone,
                LocalHash = localHash,
                PasswordHash = passwordHash,
                Role = _db.Roles.First(x => x.Name == "CLIENT")
            };

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
                _db.Users.Add(user);
                _db.Clients.Add(client);

                await _db.SaveChangesAsync();

            }catch(DbUpdateException e)
            {

            }
            
        }
    }
}
