using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Models.ServiceModels;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public class AdminServiceProvider : IAdminServiceProvider 
    {
        private const int NTOKENS = 10;

        private readonly ChatContext _db;
        private readonly ChatServiceContext _serviceDb;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IJoinService _joinService;


        public AdminServiceProvider(ChatContext context, ChatServiceContext serviceDb,
            ICryptoProvider cryptoProvider , IJoinService joinService)
        {

            _db = context;
            _serviceDb = serviceDb;
            _cryptoProvider = cryptoProvider;
            _joinService = joinService;

        }


        public Task<bool> ChangeClientAsync(User _client)
        {

            return Task.Run(() => ChangeClient(_client));
        }


        private async Task<bool> ChangeClient(User _client)
        {

            var client = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _client.UserId);

            if (client == null) return false;

            client.Email = _client.Email;
            client.Phone = _client.Phone;

            try
            {
                await ChangeUser(_client);
                await _db.SaveChangesAsync();

                return true;

            }
            catch(DbUpdateException e)
            {
                return false;
            }
        }


        public Task<bool> ChangeEmployeeAsync(Employee employee)
        {
            return Task.Run(() => ChangeEmployee(employee));
        }


        private async Task<bool> ChangeEmployee(Employee _employee)
        {

            var employee = await _db.Employees.SingleOrDefaultAsync(x => x.EmployeeId == _employee.EmployeeId);

            if (employee == null) return false;

            try
            {
                await ChangeUser(_employee);

         
                employee.Age = _employee.Age;


                await _db.SaveChangesAsync();

                return true;

            }
            catch(DbUpdateException e)
            {

                return false;
            }


        }


        private async Task<bool> ChangeUser(User _client)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _client.UserId);

            if (user == null) return false;

            try
            {
                user.Email = _client.Email;
                user.Phone = _client.Phone;


                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                return false;
            }
        }


        private async Task<bool> ChangeUser(Employee _employee)
        {
            var user  = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _employee.UserUserId);

            if (user == null) return false;

            try
            {

              

                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                return false;
            }
        }


        public Task<bool> CreateEmployeeAsync(JoinEmployeeModel model)
        {

            return Task.Run(() => CreateEmployee(model));

        }


        private async Task<bool> CreateEmployee(JoinEmployeeModel model)
        {
            if (await _joinService.canJoin((JoinModel)model) == false)
                return false;

            return await _joinService.JoinEmployee(model);
        }


        public Task<List<Employee>> GetEmployeeListAsync()
        {
            return Task.Run(() => GetEmployeeList());
        }


        private async Task<List<Employee>> GetEmployeeList()
        {

            //Filter unverified employees
            var employees = _db.Employees.Where(x => x.Age != null);

            return await employees.ToListAsync();

        }

        public Task<List<Client>> GetClientListAsync()
        {
            return Task.Run(() => GetClientList());
        }


        private async Task<List<Client>> GetClientList()
        {

            var clients = _db.Clients.Where(x => x.Age != null);

            return await clients.ToListAsync();

        }


        public Task<bool> CreateAdminAsync(JoinAdminModel model)
        {

            return Task.Run(() => CreateAdmin(model));
        }


        private async Task<bool> CreateAdmin(JoinAdminModel model)
        {
            var byteKey = _cryptoProvider.GetTokenBytes(model.AccessToken);

            var token = await _serviceDb.AdminTokens.SingleOrDefaultAsync(x => x.TokenData.SequenceEqual(byteKey));

            if (token == null || token.isUsed == true) return false;

            token.isUsed = true;
            await _serviceDb.SaveChangesAsync();

            return await _joinService.JoinAdmin(model);

        }


        public Task CreateTokensAsync()
        {

            return Task.Run(() => CreateTokens());

        }


        private async Task CreateTokens()
        {
            List<string> sTokens = new List<string>();

            using FileStream fileStream = new FileStream("tokens.txt", FileMode.OpenOrCreate);
            using StreamWriter writer = new StreamWriter(fileStream);

            for (int i = 0; i < NTOKENS; i++)
            {
                sTokens.Add(Extensions.Extensions.GetRandomString(10));

            }


            foreach (var token in sTokens)
            {

                try
                {
                    writer.WriteLine(token);

                    _serviceDb.AdminTokens.Add(new AdminToken
                    {
                        TokenData = _cryptoProvider.GetTokenBytes(token),
                        isUsed = false
                    });

                    await _serviceDb.SaveChangesAsync();
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
