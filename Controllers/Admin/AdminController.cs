using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Clients()
        {
            ViewBag.Clients = await _adminService.GetClientListAsync();

            return View("Views/Admin/Clients/Clients.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> ChangeClient(Guid id)
        {
            ViewBag.User = await _db.Users.SingleOrDefaultAsync(x => x.UserId == id);

            return View("Views/Admin/Clients/ChangeClient.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeClient(User client)
        {
            if (ModelState.IsValid)
            {
                await _adminService.ChangeClientAsync(client);
            }

            return RedirectToAction(nameof(Clients));
            
        }


        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            ViewBag.Employees = await _adminService.GetEmployeeListAsync();

            return View("Views/Admin/Employees/Employees.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> ChangeEmployee(int id)
        {
            ViewBag.Employee = await _db.Employees.SingleOrDefaultAsync(x => x.EmployeeId == id);

            return View("Views/Admin/Employees/ChangeEmployee.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _adminService.ChangeEmployeeAsync(employee);

            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpGet]
        public IActionResult CreateEmployee()
        {

            return View("Views/Admin/Employees/CreateEmployee.cshtml");

        }



        [HttpPost]
        public async Task<IActionResult> CreateEmployee(JoinEmployeeModel model)
        {

            if (ModelState.IsValid)
            {

                await _adminService.CreateEmployeeAsync(model);

            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAdmin(JoinAdminModel model)
        {
            if (ModelState.IsValid)
            {
                await _adminService.CreateAdminAsync(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> CreateTokens()
        {
            await _adminService.CreateTokensAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
