using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IAdminServiceProvider
    {
        public Task<List<Client>> GetClientListAsync();

        public Task<bool> ChangeUserAsync(User _user);

        public Task<List<Employee>> GetEmployeeListAsync();

        public Task<bool> CreateEmployeeAsync(JoinEmployeeModel model);

        public Task<bool> ChangeEmployeeAsync(Employee _employee);

    }
}
