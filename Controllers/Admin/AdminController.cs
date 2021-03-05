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
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly ChatContext _db;
        private readonly IJoinService _joinService;
        private readonly IAdminServiceProvider _adminService;

        public AdminController(
            ChatContext context, IJoinService joinService,
            IAdminServiceProvider adminService)
        {

            _db = context;
            _joinService = joinService;
            _adminService = adminService;


        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetCurrentChatSessions()
        {
            return Ok();
        }

        //
        [HttpGet]
        public async Task<IActionResult> ClientList()
        {
            ViewBag.Clients = await _adminService.GetClientListAsync();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangeClient(User n_user)
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
