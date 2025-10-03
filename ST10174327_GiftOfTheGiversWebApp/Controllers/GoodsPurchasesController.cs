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
            var firstMoneyRecord = await _context.Money.FirstOrDefaultAsync();
            decimal availableMoney = firstMoneyRecord?.RemainingMoney ?? 0;
            ViewBag.AvailableMoney = availableMoney;

            var purchases = await _context.GoodsPurchase.ToListAsync();
            return View(purchases);
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

            var firstMoneyRecord = await _context.Money.FirstOrDefaultAsync();
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
            var inventoryItem = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == goodsPurchase.CATEGORY);
            if (inventoryItem != null)
            {
                inventoryItem.ITEM_COUNT += goodsPurchase.ITEM_COUNT;
                _context.GoodsInventory.Update(inventoryItem);
            }
            else
            {
                _context.GoodsInventory.Add(new GoodsInventory
                {
                    CATEGORY = goodsPurchase.CATEGORY,
                    ITEM_COUNT = goodsPurchase.ITEM_COUNT
                });
            }

            // Update money
            firstMoneyRecord.RemainingMoney -= totalCost;
            _context.Money.Update(firstMoneyRecord);

            // Add purchase (no need to assign GoodsTotalPrice explicitly)
            _context.GoodsPurchase.Add(goodsPurchase);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Goods purchased successfully!";

            return RedirectToAction(nameof(Index));
        }

        // Utility: Load ViewData for dropdowns and money info
        private async Task LoadViewDataAsync()
        {
            var firstMoneyRecord = await _context.Money.FirstOrDefaultAsync();
            ViewBag.AvailableMoney = firstMoneyRecord?.RemainingMoney ?? 0;

            var categories = await _context.GoodsDonation
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories);
        }
    }
}
