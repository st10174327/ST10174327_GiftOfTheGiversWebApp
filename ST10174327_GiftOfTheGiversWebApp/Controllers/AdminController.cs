using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using ST10174327_GiftOfTheGiversWebApp.Models.ViewModels;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var dashboard = new AdminDashboardVM
            {
                TotalVolunteers = await _context.Volunteers.CountAsync(),
                ActiveTasks = await _context.VolunteerTasks.CountAsync(t => t.Status == "Open" || t.Status == "In Progress"),
                CompletedTasks = await _context.VolunteerTasks.CountAsync(t => t.Status == "Completed"),
                TotalDisasters = await _context.Disasters.CountAsync(),
                ActiveDisasters = await _context.Disasters.CountAsync(d => d.IsActive == 1),
                TotalMoneyDonations = await _context.MoneyDonations.SumAsync(m => m.AMOUNT ?? 0),
                TotalGoodsDonations = await _context.GoodsDonations.SumAsync(g => g.ITEM_COUNT ?? 0),
                TotalDonationsValue = (await _context.MoneyDonations.SumAsync(m => m.AMOUNT) ?? 0) + (await _context.GoodsDonations.SumAsync(g => g.ITEM_COUNT) ?? 0),
                RecentDisasters = await _context.Disasters
                    .OrderByDescending(d => d.STARTDATE)
                    .Take(5)
                    .ToListAsync(),
                RecentRegistrations = await _context.Volunteers
                    .OrderByDescending(v => v.RegistrationDate)
                    .Take(5)
                    .ToListAsync()
            };
            return View(dashboard);
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Disasters()
        {
            var disasters = await _context.Disasters
                .Include(d => d.MoneyAllocations)
                .Include(d => d.GoodsAllocations)
                .ToListAsync();
            return View(disasters);
        }

        [HttpGet]
        public async Task<IActionResult> Donations()
        {
            var viewModel = new DonationsViewModel
            {
                MoneyDonations = await _context.MoneyDonations
                    .Include(m => m.Disaster)
                    .ToListAsync(),
                GoodsDonations = await _context.GoodsDonations
                    .Include(g => g.Disaster)
                    .ToListAsync()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Allocations()
        {
            var viewModel = new AllocationsViewModel
            {
                MoneyAllocations = await _context.MoneyAllocations
                    .Include(m => m.Disaster)
                    .ToListAsync(),
                GoodsAllocations = await _context.GoodsAllocations
                    .Include(g => g.Disaster)
                    .Include(g => g.GoodsInventory)
                    .ToListAsync()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Inventory()
        {
            var inventory = await _context.GoodsInventories.ToListAsync();
            return View(inventory);
        }
    }
}
