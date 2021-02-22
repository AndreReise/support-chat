using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IAuthService
    {
       Task AuthenticateUserAsync(AuthModel model);

        Task SignOutAsync();

    }
}
