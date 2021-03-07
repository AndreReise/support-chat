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
    public class AdminServiceProvider : IAdminServiceProvider
    {

        private readonly ChatContext _db;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly IJoinService _joinService;


        public AdminServiceProvider(ChatContext context,
            ICryptoProvider cryptoProvider , IJoinService joinService)
        {

            _db = context;
            _cryptoProvider = cryptoProvider;
            _joinService = joinService;

        }


        public Task<bool> ChangeClientAsync(Client _client)
        {

            return Task.Run(() => ChangeClient(_client));
        }


        private async Task<bool> ChangeClient(Client _client)
        {

            var client = await _db.Clients.SingleOrDefaultAsync(x => x.ClientId == _client.ClientId);

            if (client == null) return false;

            client.Email = _client.Email;
            client.Phone = _client.Phone;
            client.FirstName = _client.FirstName;
            client.LastName = _client.LastName;

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


                employee.FirstName = _employee.FirstName;
                employee.LastName = _employee.LastName;

                employee.Phone = _employee.Phone;
                employee.Email = _employee.Email;

                await _db.SaveChangesAsync();

                return true;

            }
            catch(DbUpdateException e)
            {

                return false;
            }


        }


        private async Task<bool> ChangeUser(Client _client)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _client.ClientId);

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
            var user  = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _employee.EmployeeId);

            if (user == null) return false;

            try
            {

                user.Email = _employee.Email;
                user.Phone = _employee.Phone;

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
            var employees = _db.Employees.Where(x => x.Email != null && x.Phone != null);

            return await employees.ToListAsync();

        }


        public Task<List<Client>> GetClientListAsync()
        {
            return Task.Run(() => GetClientList());
        }


        private async Task<List<Client>> GetClientList()
        {

            var clients = _db.Clients.Where(x => x.Email != null || x.Phone != null);

            return await clients.ToListAsync();

        }
    }
}
