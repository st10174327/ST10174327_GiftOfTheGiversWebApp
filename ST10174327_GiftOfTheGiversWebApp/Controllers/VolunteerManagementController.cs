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
            if (User.IsInRole("Admin"))
            {
                // Admin sees all volunteers with task assignments
                var volunteersWithTasks = await _context.Volunteers
                    .Include(v => v.Tasks)
                        .ThenInclude(t => t.VolunteerTask)
                    .ToListAsync();
                return View("AdminIndex", volunteersWithTasks);
            }
            else
            {
                // User sees their own volunteer profile if they have one
                string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
                var userVolunteer = await _context.Volunteers
                    .FirstOrDefaultAsync(v => v.Email == currentUsername);

                if (userVolunteer == null)
                {
                    return RedirectToAction("Register");
                }

                return View("UserIndex", userVolunteer);
            }
        }

        // GET: VolunteerManagement/Register - Allow users to register as volunteers
        [Authorize]
        public IActionResult Register()
        {
            return View();
        }

        // POST: VolunteerManagement/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Register([Bind("Name,Email,PhoneNumber,Address,Availability,EmergencyContact,Skills")] Volunteer volunteer)
        {
            string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");

            if (ModelState.IsValid)
            {
                // Check if user is already registered as volunteer
                var existingVolunteer = await _context.Volunteers
                    .FirstOrDefaultAsync(v => v.Email == currentUsername);

                if (existingVolunteer != null)
                {
                    ModelState.AddModelError("", "You are already registered as a volunteer.");
                    return View(volunteer);
                }

                volunteer.Email = currentUsername ?? throw new InvalidOperationException("Username cannot be null");
                volunteer.RegistrationDate = DateTime.Now;
                volunteer.Status = "Active";

                _context.Add(volunteer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Successfully registered as a volunteer! Welcome to the team.";

                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: VolunteerManagement/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // Get available tasks for assignment
            var availableTasks = await _context.VolunteerTasks
                .Where(t => t.Status == "Open")
                .ToListAsync();

            ViewBag.AvailableTasks = availableTasks;
            return View();
        }

        // POST: VolunteerManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Email,PhoneNumber,Address,Availability,EmergencyContact,Skills")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.RegistrationDate = DateTime.Now;
                volunteer.Status = "Active";
                _context.Add(volunteer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Volunteer added successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: VolunteerManagement/Tasks - Show available tasks for volunteers
        [Authorize]
        public async Task<IActionResult> Tasks()
        {
            var availableTasks = await _context.VolunteerTasks
                .Where(t => t.Status == "Open" || t.Status == "In Progress")
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.Volunteer)
                .ToListAsync();

            return View(availableTasks);
        }

        // GET: VolunteerManagement/ApplyForTask/5
        [Authorize]
        public async Task<IActionResult> ApplyForTask(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.VolunteerTasks.FindAsync(id);
            if (task == null) return NotFound();

            // Check if user is registered as volunteer
            string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.Email == currentUsername);

            if (volunteer == null)
            {
                TempData["ErrorMessage"] = "You need to register as a volunteer first.";
                return RedirectToAction("Register");
            }

            // Check if already assigned to this task
            var existingAssignment = await _context.TaskAssignments
                .FirstOrDefaultAsync(a => a.VolunteerID == volunteer.VolunteerID && a.TaskID == id);

            if (existingAssignment != null)
            {
                TempData["ErrorMessage"] = "You are already assigned to this task.";
                return RedirectToAction("Tasks");
            }

            return View(new TaskAssignment
            {
                VolunteerID = volunteer.VolunteerID,
                TaskID = task.TaskID,
                AssignmentDate = DateTime.Now,
                Status = "Applied"
            });
        }

        // POST: VolunteerManagement/ApplyForTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ApplyForTask([Bind("VolunteerID,TaskID,AssignmentDate,Status")] TaskAssignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Successfully applied for the volunteer task!";
                return RedirectToAction("MyTasks");
            }
            return View(assignment);
        }

        // GET: VolunteerManagement/MyTasks - Show user's assigned tasks
        [Authorize]
        public async Task<IActionResult> MyTasks()
        {
            string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.Email == currentUsername);

            if (volunteer == null)
            {
                return RedirectToAction("Register");
            }

            var myTasks = await _context.TaskAssignments
                .Where(a => a.VolunteerID == volunteer.VolunteerID)
                .Include(a => a.VolunteerTask)
                .ToListAsync();

            return View(myTasks);
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
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerID,Name,Email,PhoneNumber,RegistrationDate,Address,Availability,EmergencyContact,Skills,Status")] Volunteer volunteer)
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

                    TempData["SuccessMessage"] = "Volunteer updated successfully.";
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
