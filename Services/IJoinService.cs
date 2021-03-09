using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IJoinService
    {
        public Task<bool> canJoin(JoinModel model);

        public Task JoinClient(JoinModel model);

        public Task<bool> JoinEmployee(JoinEmployeeModel model);

        public Task<bool> JoinAdmin(JoinModel model);
    }
}
