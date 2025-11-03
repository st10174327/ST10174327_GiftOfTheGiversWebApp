using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ST10174327_GiftOfTheGiversWebApp.Models;
using ST10174327_GiftOfTheGiversWebApp.Data;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize]
    public class MoneyAllocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoneyAllocationsController> _logger;

        public MoneyAllocationsController(ApplicationDbContext context, ILogger<MoneyAllocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Helper method to populate ViewBags
        private void PopulateViewBags()
        {
            ViewBag.DisasterTypes = _context.Disasters
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.DISASTER_ID.ToString(),
                    Text = $"{d.DisasterName} - {d.AID_TYPE}"
                })
                .ToList();

            var money = _context.Moneys.FirstOrDefault();
            ViewBag.RemainingMoney = money?.RemainingMoney ?? 0.0m;

            decimal totalAllocated = _context.MoneyAllocations.Sum(m => (decimal?)m.AllocationAmount) ?? 0.0m;
            ViewBag.Total = totalAllocated;
        }

        // List all allocations
        public async Task<IActionResult> Index()
        {
            PopulateViewBags();

            if (_context.MoneyAllocations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoneyAllocation' is null.");
            }

            var allocations = await _context.MoneyAllocations
                .Include(m => m.Disaster)
                .OrderByDescending(m => m.AllocationDate)
                .ToListAsync();

            return View(allocations);
        }

        // Show create form
        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        // Handle POST create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllocationAmount,DISASTER_ID")] MoneyAllocation moneyAllocation)
        {
            var money = _context.Moneys.FirstOrDefault();

            if (money == null)
            {
                ModelState.AddModelError("", "No money available to allocate.");
                PopulateViewBags();
                return View(moneyAllocation);
            }

            if (ModelState.IsValid)
            {
                if (moneyAllocation.AllocationAmount <= 0 || moneyAllocation.AllocationAmount > money.RemainingMoney)
                {
                    ModelState.AddModelError("AllocationAmount", "Invalid allocation amount or insufficient funds.");
                }
                else
                {
                    // Set properties
                    moneyAllocation.AllocationDate = DateTime.UtcNow.Date;

                    var selectedDisaster = await _context.Disasters
                        .FirstOrDefaultAsync(d => d.DISASTER_ID == moneyAllocation.DISASTER_ID);

                    if (selectedDisaster != null)
                    {
                        moneyAllocation.AidType = selectedDisaster.AID_TYPE;

                        // Deduct from remaining money
                        money.RemainingMoney -= moneyAllocation.AllocationAmount;
                        money.LastUpdated = DateTime.UtcNow;
                        _context.Moneys.Update(money);

                        // Save allocation
                        _context.MoneyAllocations.Add(moneyAllocation);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Money allocated successfully!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("DISASTER_ID", "Selected disaster not found.");
                }
            }

            // Log validation errors - REMOVED the ModelState.Keys null check
            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry?.Errors != null)
                {
                    foreach (var error in entry.Errors)
                    {
                        _logger.LogError($"Validation error for {key}: {error.ErrorMessage}");
                    }
                }
            }

            PopulateViewBags();
            return View(moneyAllocation);
        }

        // Details action
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneyAllocation = await _context.MoneyAllocations
                .Include(m => m.Disaster)
                .FirstOrDefaultAsync(m => m.MoneyAllocationId == id);

            if (moneyAllocation == null)
            {
                return NotFound();
            }

            return View(moneyAllocation);
        }
    }
}