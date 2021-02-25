using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Services;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ChatContext _db;
        private readonly IAuthService _authService;

        public AccountController(ChatContext db , IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            var result = await _authService.AuthenticateUserAsync(model);
            if (result.isSuccessful == false) return Ok("ERROR!!!!!");
            return Ok();
        }
    }
}
