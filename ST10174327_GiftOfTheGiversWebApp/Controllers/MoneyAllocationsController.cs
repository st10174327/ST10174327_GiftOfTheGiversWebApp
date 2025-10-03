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
            ViewBag.DisasterTypes = _context.Disaster
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.DISASTER_ID.ToString(),
                    Text = $"{d.DisasterName} - {d.AID_TYPE}"  // FIXED: Changed AidType to AID_TYPE
                })
                .ToList();

            var money = _context.Money.FirstOrDefault();
            ViewBag.RemainingMoney = money?.RemainingMoney ?? 0.0m;

            decimal totalAllocated = _context.MoneyAllocation.Sum(m => (decimal?)m.AllocationAmount) ?? 0.0m;
            ViewBag.Total = totalAllocated;
        }

        // List all allocations
        public async Task<IActionResult> Index()
        {
            PopulateViewBags();

            if (_context.MoneyAllocation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MoneyAllocation' is null.");
            }

            var allocations = await _context.MoneyAllocation
                .Include(m => m.Disaster)  // Eager load the Disaster data
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
        public async Task<IActionResult> Create([Bind("AllocationAmount,DISASTER_ID")] MoneyAllocation moneyAllocation)  // FIXED: Changed DisasterId to DISASTER_ID
        {
            var money = _context.Money.FirstOrDefault();

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

                    var selectedDisaster = await _context.Disaster
                        .FirstOrDefaultAsync(d => d.DISASTER_ID == moneyAllocation.DISASTER_ID);  // FIXED: Changed DisasterId to DISASTER_ID

                    if (selectedDisaster != null)
                    {
                        moneyAllocation.AidType = selectedDisaster.AID_TYPE;  // FIXED: Changed AidType to AID_TYPE

                        // Deduct from remaining money
                        money.RemainingMoney -= moneyAllocation.AllocationAmount;
                        money.LastUpdated = DateTime.UtcNow;
                        _context.Update(money);

                        // Save allocation
                        _context.MoneyAllocation.Add(moneyAllocation);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Money allocated successfully!";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("DISASTER_ID", "Selected disaster not found.");  // FIXED: Changed DisasterId to DISASTER_ID
                }
            }

            // Log validation errors
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    _logger.LogError($"Validation error for {key}: {error.ErrorMessage}");
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

            var moneyAllocation = await _context.MoneyAllocation
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