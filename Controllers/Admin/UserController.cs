using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Services;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Controllers.Admin
{
    [Authorize(Roles  = "ADMIN")]
    public class UserController : Controller
    {
        private readonly ChatContext _db;
        private readonly IJoinService _joinService;

        public UserController(ChatContext context , IJoinService joinService)
        {

            _db = context;
            _joinService = joinService;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ChangeUser(User n_user)
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> EmployeeList()
        {
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ChangeEmployee(Employee employee)
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateEmployee(JoinModel model)
        {
            return RedirectToAction(nameof(EmployeeList));
        }

    }
}
