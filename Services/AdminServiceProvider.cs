using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private const int TRACE_COUNT = 1000;
        private const int ERROR_COUNT = 100;

        //cache dictionary
        private const string CLIENT_LIST = "CACHE_CLIENT_LIST";
        private const string EMPLOYEE_LIST = "EMPLOYEE_CLIENT_LIST";
        private const string ERROR_LOG = "ERROR_LOGS";


        private readonly ChatContext _db;
        private readonly ChatServiceContext _serviceDb;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IJoinService _joinService;
        private readonly IMemoryCache _cache;


        public AdminServiceProvider(ChatContext context, ChatServiceContext serviceDb,
            ICryptoProvider cryptoProvider , IJoinService joinService , IMemoryCache cache)
        {

            _db = context;
            _serviceDb = serviceDb;
            _cryptoProvider = cryptoProvider;
            _joinService = joinService;
            _cache = cache;

        }


        public Task<bool> ChangeClientAsync(Client _client)
        {

            return Task.Run(() => ChangeClient(_client));
        }


        private async Task<bool> ChangeClient(Client _client)
        {

            var client = await _db.Clients.Include( x => x.User).SingleOrDefaultAsync(x => x.ClientId == _client.ClientId);

            if (client == null) return false;

            try
            {
                await ChangeUser(_client.User);
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
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserGuid == _client.UserGuid);

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
            var user  = await _db.Users.SingleOrDefaultAsync(x => x.UserGuid == _employee.UserGuid);

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
            List<Employee> employeeList;

            if(!_cache.TryGetValue(EMPLOYEE_LIST , out employeeList))
            {
                //Filter unverified employees
                var employees = _db.Employees.Include(x => x.User).Where(x => x.Age != null);

                if (employees != null)
                {
                    employeeList = await employees.ToListAsync();

                    _cache.Set(
                        EMPLOYEE_LIST, employeeList,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            
            return employeeList;

        }

        public Task<List<Client>> GetClientListAsync()
        {
            return Task.Run(() => GetClientList());
        }


        private async Task<List<Client>> GetClientList()
        {
            List<Client> clientList;

            if(!_cache.TryGetValue(CLIENT_LIST , out clientList))
            {
                var clients = _db.Clients.Include(x => x.User).Where(x => x.Age != null);

                if (clients != null)
                {
                    clientList = await clients.ToListAsync();

                    _cache.Set(
                        CLIENT_LIST, clientList,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
            }
            


            return clientList;
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

        public List<ErrorLog> GetErrorLogs()
        {
            List<ErrorLog> errorLogList;
            if(!_cache.TryGetValue(ERROR_LOG , out errorLogList))
            {
                errorLogList = _serviceDb.TechnicalLogs
                .OrderBy(x => x.Time)
                .AsEnumerable()
                .TakeLast(ERROR_COUNT).ToList();

                _cache.Set(ERROR_LOG, errorLogList,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }

            return errorLogList;
        }



        public List<TraceLog> GetTraceLogs()
        {
            return _serviceDb.TraceLogs
                .OrderBy(x => x.Id)
                .AsEnumerable()
                .TakeLast(TRACE_COUNT).ToList();
        }

        public Task<ErrorLog> GetErrorLogAsync(int id)
        {
            return Task.Run(() => GetErrorLog(id));
        }

        private async Task<ErrorLog> GetErrorLog(int id)
        {
            return await _serviceDb.TechnicalLogs.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<int> GetActiveOperatorsAsync()
        {
            return Task.Run(() => GetActiveOperators());
        }

        private async Task<int> GetActiveOperators()
        {
            return await _db.Employees.Where(x => x.StatusOnline == true).CountAsync();
        }
    }
}
