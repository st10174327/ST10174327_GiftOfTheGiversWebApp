using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize] // all actions require login
    public class UserDisastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserDisastersController(ApplicationDbContext context) => _context = context;

        // GET: UserDisasters
        public async Task<IActionResult> Index()
        {
            string? username = User.Identity?.Name;

            if (User.IsInRole("Admin"))
                return View(await _context.Disasters.ToListAsync());

            if (string.IsNullOrEmpty(username))
                return Challenge(); // Azure AD login

            var disasters = await _context.Disasters
                .Where(d => d.USERNAME == username)
                .ToListAsync();

            return View(disasters);
        }

        // GET: UserDisasters/Create
        public IActionResult Create() => View(new Disaster
        {
            STARTDATE = DateTime.Now.Date,
            ENDDATE = DateTime.Now.Date.AddDays(1)
        });

        // POST: UserDisasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("STARTDATE,ENDDATE,LOCATION,AID_TYPE,DisasterName,Description")] Disaster disaster)
        {
            string? username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Challenge();

            if (!ModelState.IsValid) return View(disaster);

            if (disaster.STARTDATE < DateTime.Now.Date)
            {
                ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                return View(disaster);
            }

            if (disaster.ENDDATE <= disaster.STARTDATE)
            {
                ModelState.AddModelError("ENDDATE", "End date must be after start date.");
                return View(disaster);
            }

            disaster.USERNAME = username;
            disaster.IsActive = (disaster.STARTDATE <= DateTime.Now && DateTime.Now <= disaster.ENDDATE) ? 1 : 0;

            _context.Disasters.Add(disaster);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Disaster report submitted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: UserDisasters/Edit/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(disaster);
        }

        // POST: UserDisasters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("DISASTER_ID,STARTDATE,ENDDATE,LOCATION,AID_TYPE,DisasterName,Description")] Disaster disaster)
        {
            var existing = await _context.Disasters.FindAsync(id);
            if (existing == null) return NotFound();

            if (!User.IsInRole("Admin") && existing.USERNAME != User.Identity?.Name)
                return Forbid();

            if (ModelState.IsValid)
            {
                existing.STARTDATE = disaster.STARTDATE;
                existing.ENDDATE = disaster.ENDDATE;
                existing.LOCATION = disaster.LOCATION;
                existing.AID_TYPE = disaster.AID_TYPE;
                existing.DisasterName = disaster.DisasterName;
                existing.Description = disaster.Description;
                existing.IsActive = (existing.STARTDATE <= DateTime.Now && DateTime.Now <= existing.ENDDATE) ? 1 : 0;

                _context.Disasters.Update(existing);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Disaster report updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(disaster);
        }

        // GET: UserDisasters/Delete/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(disaster);
        }

        // POST: UserDisasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid();

            _context.Disasters.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
