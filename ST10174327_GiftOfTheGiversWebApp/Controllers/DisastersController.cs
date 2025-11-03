using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Models;
using ST10174327_GiftOfTheGiversWebApp.Data;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize] // All actions require login
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Disasters
        public async Task<IActionResult> Index()
        {
            if (_context.Disaster == null)
                return Problem("Entity set 'ApplicationDbContext.Disaster' is null.");

            // Admin sees all, users see only their reports
            var disasters = User.IsInRole("Admin")
                ? await _context.Disaster.ToListAsync()
                : await _context.Disaster.Where(d => d.USERNAME == User.Identity!.Name).ToListAsync();

            return View(disasters);
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(d => d.DISASTER_ID == id);

            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            var currentDate = DateTime.Now.Date;
            var tomorrow = currentDate.AddDays(1);

            var disaster = new Disaster
            {
                STARTDATE = currentDate,
                ENDDATE = tomorrow
            };

            return View(disaster);
        }

        // POST: Disasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DISASTER_ID,STARTDATE,ENDDATE,LOCATION,AID_TYPE,DisasterName,Description")] Disaster disaster)
        {
            if (!ModelState.IsValid)
                return View(disaster);

            // Fixed: Added null check for User.Identity.Name
            disaster.USERNAME = User.Identity?.Name ?? "Unknown";

            if (disaster.STARTDATE < DateTime.Now.Date)
            {
                ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                return View(disaster);
            }

            if (disaster.ENDDATE < disaster.STARTDATE.AddDays(1))
            {
                ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                return View(disaster);
            }

            // Calculate active status
            disaster.IsActive = (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE) ? 1 : 0;

            _context.Add(disaster);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Disaster report submitted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Disasters/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // POST: Disasters/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DISASTER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE,DisasterName,Description,IsActive")] Disaster disaster)
        {
            if (id != disaster.DISASTER_ID)
                return NotFound();

            if (!ModelState.IsValid)
                return View(disaster);

            try
            {
                if (disaster.STARTDATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                    return View(disaster);
                }

                if (disaster.ENDDATE < disaster.STARTDATE.AddDays(1))
                {
                    ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                    return View(disaster);
                }

                // Update active status
                disaster.IsActive = (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE) ? 1 : 0;

                _context.Update(disaster);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Disaster report updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisasterExists(disaster.DISASTER_ID))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Disasters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disaster == null)
                return Problem("Entity set 'ApplicationDbContext.Disaster' is null.");

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            _context.Disaster.Remove(disaster);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Disaster report deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterExists(int id)
        {
            // Fixed: Added null check for _context.Disaster
            return _context.Disaster?.Any(e => e.DISASTER_ID == id) ?? false;
        }
    }
}