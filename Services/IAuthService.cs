using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======
using TechnicalSupport.Utils;
>>>>>>> 278f6d21b3af26581a8e15dbbb5b837aaffb30ea
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
<<<<<<< HEAD
       Task AuthenticateUserAsync(AuthModel model);
=======
       Task<AuthStatusResult> AuthenticateUserAsync(AuthModel model);
>>>>>>> 278f6d21b3af26581a8e15dbbb5b837aaffb30ea

       Task SignOutAsync();

    }
}
