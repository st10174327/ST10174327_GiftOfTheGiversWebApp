using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize] // Require login for all actions
    public class VolunteerManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerManagement
        [Authorize(Roles = "Admin,User")] // Users can see list, Admin can manage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.ToListAsync());
        }

        // GET: VolunteerManagement/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.VolunteerID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: VolunteerManagement/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: VolunteerManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Email,PhoneNumber")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.RegistrationDate = DateTime.Now;
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: VolunteerManagement/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: VolunteerManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerID,Name,Email,PhoneNumber,RegistrationDate")] Volunteer volunteer)
        {
            if (id != volunteer.VolunteerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Volunteers.Any(e => e.VolunteerID == id))
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
            return View(volunteer);
        }

        // GET: VolunteerManagement/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.VolunteerID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: VolunteerManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
