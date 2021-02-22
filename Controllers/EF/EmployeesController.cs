using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport.Controllers.EF
{
    public class EmployeesController : Controller
    {
        private readonly SupportContext _context;

        public EmployeesController(SupportContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var gL_SupportContext = _context.Employees.Include(e => e.SexNavigation).Include(e => e.WorkTimeNavigation);
            return View(await gL_SupportContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.SexNavigation)
                .Include(e => e.WorkTimeNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["Sex"] = new SelectList(_context.Sexes, "SexId", "Sex1");
            ViewData["WorkTime"] = new SelectList(_context.WorkTimes, "WorkTimeId", "From");
            ViewData["WorkTimeTo"] = new SelectList(_context.WorkTimes, "WorkTimeId", "To");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,SecondName,Age,Phone,Email,Sex,WorkTime,PasswordHash,EmployeeGuid")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Sex"] = new SelectList(_context.Sexes, "SexId", "Sex1", employee.Sex);
            ViewData["WorkTime"] = new SelectList(_context.WorkTimes, "WorkTimeId", "WorkTimeId", employee.WorkTime);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["Sex"] = new SelectList(_context.Sexes, "SexId", "Sex1", employee.Sex);
            ViewData["WorkTime"] = new SelectList(_context.WorkTimes, "WorkTimeId", "WorkTimeId", employee.WorkTime);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,SecondName,Age,Phone,Email,Sex,WorkTime,PasswordHash,EmployeeGuid")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Sex"] = new SelectList(_context.Sexes, "SexId", "Sex1", employee.Sex);
            ViewData["WorkTime"] = new SelectList(_context.WorkTimes, "WorkTimeId", "WorkTimeId", employee.WorkTime);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.SexNavigation)
                .Include(e => e.WorkTimeNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
