using Microsoft.AspNetCore.Mvc;
using TechnicalSupport.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IAuthService
    {
       Task<AuthStatusResult> AuthenticateUserAsync(AuthModel model);

       Task SignOutAsync();

    }
}
