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
    [Authorize] // All actions require login
    public class UserDisastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDisastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserDisasters
        public async Task<IActionResult> Index()
        {
            string? currentUsername = User.Identity?.Name;

            if (User.IsInRole("Admin"))
            {
                // Admin sees all disasters
                return View(await _context.Disaster.ToListAsync());
            }

            if (string.IsNullOrEmpty(currentUsername))
                return RedirectToAction("Login", "Account");

            // Normal user sees only their disasters
            var userDisasters = await _context.Disaster
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userDisasters);
        }

        // GET: UserDisasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disaster.FirstOrDefaultAsync(m => m.DISASTER_ID == id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid(); // prevent other users from snooping

            return View(disaster);
        }

        // GET: UserDisasters/Create
        public IActionResult Create()
        {
            return View(new Disaster
            {
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(1)
            });
        }

        // POST: UserDisasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            string? currentUsername = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUsername))
                return RedirectToAction("Login", "Account");

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

            // assign username & active state
            disaster.USERNAME = currentUsername;
            disaster.IsActive = (disaster.STARTDATE <= DateTime.Now && DateTime.Now <= disaster.ENDDATE) ? 1 : 0;

            _context.Add(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: UserDisasters/Edit/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(disaster);
        }

        // POST: UserDisasters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("DISASTER_ID,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            var existingDisaster = await _context.Disaster.FindAsync(id);
            if (existingDisaster == null) return NotFound();

            if (!User.IsInRole("Admin") && existingDisaster.USERNAME != User.Identity?.Name)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    // update only allowed fields
                    existingDisaster.STARTDATE = disaster.STARTDATE;
                    existingDisaster.ENDDATE = disaster.ENDDATE;
                    existingDisaster.LOCATION = disaster.LOCATION;
                    existingDisaster.AID_TYPE = disaster.AID_TYPE;

                    // recalc active flag
                    existingDisaster.IsActive = (existingDisaster.STARTDATE <= DateTime.Now && DateTime.Now <= existingDisaster.ENDDATE) ? 1 : 0;

                    _context.Update(existingDisaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Disaster.Any(e => e.DISASTER_ID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disaster);
        }

        // GET: UserDisasters/Delete/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var disaster = await _context.Disaster.FirstOrDefaultAsync(m => m.DISASTER_ID == id);
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
            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null) return NotFound();

            if (!User.IsInRole("Admin") && disaster.USERNAME != User.Identity?.Name)
                return Forbid();

            _context.Disaster.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
