using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    public class GoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsDonations
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var donations = await _context.GoodsDonation.ToListAsync(); // Removed Include(d => d.InventoryItem)
            return View(donations);
        }

        // GET: GoodsDonations/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.GoodsDonation
                .FirstOrDefaultAsync(d => d.GOODS_DONATION_ID == id); // Removed Include(d => d.InventoryItem)

            if (donation == null)
                return NotFound();

            return View(donation);
        }

        // GET: GoodsDonations/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.GoodsInventory
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();

            return View();
        }

        // POST: GoodsDonations/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ITEM_COUNT, CATEGORY, DESCRIPTION, DONOR, DATE")] GoodsDonation goodsDonation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.GoodsInventory
                    .Select(g => g.CATEGORY)
                    .Distinct()
                    .ToListAsync();
                return View(goodsDonation);
            }

            // Validate date
            if (goodsDonation.DATE < DateTime.Now.Date)
            {
                ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                ViewBag.Categories = await _context.GoodsInventory
                    .Select(g => g.CATEGORY)
                    .Distinct()
                    .ToListAsync();
                return View(goodsDonation);
            }

            // Set DONOR
            goodsDonation.DONOR = goodsDonation.DONOR == "Anonymous" ? "Anonymous" : User.Identity?.Name;

            // Update inventory
            var inventoryItem = await _context.GoodsInventory
                .FirstOrDefaultAsync(g => g.CATEGORY == goodsDonation.CATEGORY);

            if (inventoryItem != null)
            {
                inventoryItem.ITEM_COUNT += goodsDonation.ITEM_COUNT;
            }
            else
            {
                _context.GoodsInventory.Add(new GoodsInventory
                {
                    CATEGORY = goodsDonation.CATEGORY,
                    ITEM_COUNT = goodsDonation.ITEM_COUNT
                });
            }

            // Save donation
            _context.GoodsDonation.Add(goodsDonation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Goods donation successfully recorded!";
            return RedirectToAction(User.IsInRole("Admin") ? "Index" : "Create");
        }

        // GET: GoodsDonations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.GoodsDonation.FindAsync(id);
            if (donation == null)
                return NotFound();

            // Only allow owner or admin to edit
            if (donation.DONOR != User.Identity?.Name && !User.IsInRole("Admin"))
                return Forbid();

            ViewBag.Categories = await _context.GoodsInventory
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();

            return View(donation);
        }

        // POST: GoodsDonations/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GOODS_DONATION_ID, ITEM_COUNT, CATEGORY, DESCRIPTION, DONOR, DATE")] GoodsDonation goodsDonation)
        {
            if (id != goodsDonation.GOODS_DONATION_ID)
                return NotFound();

            // Only allow owner or admin
            if (goodsDonation.DONOR != User.Identity?.Name && !User.IsInRole("Admin"))
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goodsDonation);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Donation updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.GoodsDonation.AnyAsync(e => e.GOODS_DONATION_ID == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(User.IsInRole("Admin") ? "Index" : "Create");
            }

            ViewBag.Categories = await _context.GoodsInventory
                .Select(g => g.CATEGORY)
                .Distinct()
                .ToListAsync();

            return View(goodsDonation);
        }

        // GET: GoodsDonations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.GoodsDonation
                .FirstOrDefaultAsync(d => d.GOODS_DONATION_ID == id);

            if (donation == null)
                return NotFound();

            // Only allow owner or admin
            if (donation.DONOR != User.Identity?.Name && !User.IsInRole("Admin"))
                return Forbid();

            return View(donation);
        }

        // POST: GoodsDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.GoodsDonation.FindAsync(id);
            if (donation == null)
                return NotFound();

            // Only allow owner or admin
            if (donation.DONOR != User.Identity?.Name && !User.IsInRole("Admin"))
                return Forbid();

            _context.GoodsDonation.Remove(donation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Donation deleted successfully!";
            return RedirectToAction(User.IsInRole("Admin") ? "Index" : "Create");
        }
    }
}