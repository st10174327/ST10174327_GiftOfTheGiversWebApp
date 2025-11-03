using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admins can manage allocations
    public class GoodsAllocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsAllocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsAllocations
        public async Task<IActionResult> Index()
        {
            if (_context.GoodsAllocation == null)
                return Problem("Entity set 'ApplicationDbContext.GoodsAllocation' is null.");

            var allocations = await _context.GoodsAllocation
                .Include(g => g.Disaster)
                .Include(g => g.GoodsInventory)
                .ToListAsync();

            return View(allocations);
        }

        // GET: GoodsAllocations/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: GoodsAllocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ITEM_COUNT, AllocationDate")] GoodsAllocation goodsAllocation, string category, string aidType)
        {
            // Validate form
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(goodsAllocation);
            }

            // Validate disaster
            var disaster = await _context.Disaster.FirstOrDefaultAsync(d => d.AID_TYPE == aidType && d.IsActive == 1);
            if (disaster == null)
            {
                ModelState.AddModelError("AidType", "Selected disaster is not active or does not exist.");
                await PopulateDropdowns();
                return View(goodsAllocation);
            }

            // Validate inventory
            var selectedGood = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == category);
            if (selectedGood == null)
            {
                ModelState.AddModelError("CATEGORY", "Selected category not found in inventory.");
                await PopulateDropdowns();
                return View(goodsAllocation);
            }

            if (goodsAllocation.ITEM_COUNT > selectedGood.ITEM_COUNT)
            {
                ModelState.AddModelError("ITEM_COUNT", "Cannot allocate more items than available in inventory.");
                await PopulateDropdowns();
                return View(goodsAllocation);
            }

            // Update inventory
            selectedGood.ITEM_COUNT -= goodsAllocation.ITEM_COUNT;

            // Set allocation details
            goodsAllocation.CATEGORY = category;
            goodsAllocation.AidType = aidType;
            goodsAllocation.AllocationDate = DateTime.Now.Date;
            goodsAllocation.DISASTER_ID = disaster.DISASTER_ID;
            goodsAllocation.GOODSINVENTORY_ID = selectedGood.GOODS_INVENTORY_ID;

            _context.GoodsAllocation.Add(goodsAllocation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Goods allocated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: GoodsAllocations/GetSumOfItemCount (AJAX)
        [HttpPost]
        public async Task<IActionResult> GetSumOfItemCount(string category)
        {
            if (string.IsNullOrEmpty(category))
                return Json(0);

            var sumOfItemCount = await _context.GoodsInventory
                .Where(g => g.CATEGORY == category)
                .SumAsync(g => g.ITEM_COUNT);

            return Json(sumOfItemCount);
        }

        // Helper: Populate dropdown lists for Create view
        private async Task PopulateDropdowns()
        {
            ViewBag.Categories = await _context.GoodsInventory
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();

            ViewBag.DisasterTypes = await _context.Disaster
                .Where(d => d.IsActive == 1)
                .Select(d => new SelectListItem
                {
                    Value = d.AID_TYPE,
                    Text = d.AID_TYPE
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
