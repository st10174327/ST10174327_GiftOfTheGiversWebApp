using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GoodsPurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsPurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsPurchases
        public async Task<IActionResult> Index()
        {
            var firstMoneyRecord = await _context.Moneys.FirstOrDefaultAsync();
            decimal availableMoney = firstMoneyRecord?.RemainingMoney ?? 0;
            ViewBag.AvailableMoney = availableMoney;

            var goodsPurchases = await _context.GoodsPurchases.ToListAsync();
            return View(goodsPurchases);
        }

        // GET: GoodsPurchases/Create
        public async Task<IActionResult> Create()
        {
            await LoadViewDataAsync();
            return View();
        }

        // POST: GoodsPurchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoodsPurchaseID,GoodsPurchasePrice,ITEM_COUNT,CATEGORY")] GoodsPurchase goodsPurchase)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewDataAsync();
                return View(goodsPurchase);
            }

            var firstMoneyRecord = await _context.Moneys.FirstOrDefaultAsync();
            if (firstMoneyRecord == null)
            {
                ModelState.AddModelError("", "No money record found. Cannot make purchase.");
                await LoadViewDataAsync();
                return View(goodsPurchase);
            }

            decimal totalCost = goodsPurchase.GoodsTotalPrice;

            if (totalCost > firstMoneyRecord.RemainingMoney)
            {
                ModelState.AddModelError("ITEM_COUNT", $"Total cost {totalCost:C} exceeds available money {firstMoneyRecord.RemainingMoney:C}.");
                await LoadViewDataAsync();
                return View(goodsPurchase);
            }

            // Update or create inventory record
            var inventoryItem = await _context.GoodsInventories.FirstOrDefaultAsync(g => g.CATEGORY == goodsPurchase.CATEGORY);
            if (inventoryItem != null)
            {
                inventoryItem.ITEM_COUNT += goodsPurchase.ITEM_COUNT;
                _context.GoodsInventories.Update(inventoryItem);
            }
            else
            {
                _context.GoodsInventories.Add(new GoodsInventory
                {
                    CATEGORY = goodsPurchase.CATEGORY ?? "Unknown",
                    ITEM_COUNT = goodsPurchase.ITEM_COUNT
                });
            }

            // Update money
            firstMoneyRecord.RemainingMoney -= totalCost;
            _context.Moneys.Update(firstMoneyRecord);

            // Add purchase (no need to assign GoodsTotalPrice explicitly)
            _context.GoodsPurchases.Add(goodsPurchase);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Goods purchased successfully!";

            return RedirectToAction(nameof(Index));
        }

        // Utility: Load ViewData for dropdowns and money info
        private async Task LoadViewDataAsync()
        {
            var firstMoneyRecord = await _context.Moneys.FirstOrDefaultAsync();
            ViewBag.AvailableMoney = firstMoneyRecord?.RemainingMoney ?? 0;

            var categories = await _context.GoodsDonations
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories);
        }
    }
}
