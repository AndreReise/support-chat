using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    interface IAdminServiceProvider
    {
        public List<User> GetUserListAsync();

        public Task<bool> ChangeUserAsync(User _user);

        public List<Employee> GetEmployeeListAsync();

        public Task<bool> CreateEmployeeAsync(JoinEmployeeModel model);
        public Task<bool> ChangeEmployeeAsync(Employee _employee);

    }
}
