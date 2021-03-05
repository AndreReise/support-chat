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
                employee = new Employee
                {
                    FirstName = _employee.FirstName,
                    LastName = _employee.LastName,

                    Phone = _employee.Phone,
                    Email = _employee.Email
                };

                await _db.SaveChangesAsync();

                //if all is well
                return true;

            }
            catch(DbUpdateException e)
            {

                return false;
            }


        }
        public Task<bool> ChangeUserAsync(User _user)
        {

            return Task.Run(() => ChangeUser(_user));
        }


        private async Task<bool> ChangeUser(User _user)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.UserId == _user.UserId);

            if (user == null) return false;

            try
            {
                user = new User
                {
                    Email = _user.Email,
                    Phone = _user.Phone,
                    Role = _user.Role
                };


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
