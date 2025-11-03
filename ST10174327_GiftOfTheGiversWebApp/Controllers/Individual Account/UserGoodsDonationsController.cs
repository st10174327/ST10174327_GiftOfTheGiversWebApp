using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize] // Require login for everything
    public class UserGoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserGoodsDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserGoodsDonations
        public async Task<IActionResult> Index()
        {
            string? currentUsername = User.Identity?.Name;

            if (User.IsInRole("Admin"))
            {
                // Admin sees all donations
                return View(await _context.GoodsDonations.ToListAsync());
            }

            // User sees only their own donations
            var userGoodsDonations = await _context.GoodsDonations
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userGoodsDonations);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.GoodsDonations.FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(donation);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            // Get available disasters for allocation
            var availableDisasters = await _context.Disasters
                .Where(d => d.IsActive == 1)
                .ToListAsync();

            ViewBag.AvailableDisasters = availableDisasters;

            return View(new GoodsDonation
            {
                DATE = DateTime.Now.Date
            });
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR,DISASTER_ID")] GoodsDonation donation)
        {
            string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
            donation.USERNAME = currentUsername;

            if (!ModelState.IsValid) return View(donation);

            if (donation.DATE < DateTime.Now.Date)
            {
                ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                return View(donation);
            }

            // assign correct username
            donation.USERNAME = currentUsername;

            // if not anonymous, force username as donor
            donation.DONOR = (donation.DONOR == "Anonymous") ? "Anonymous" : currentUsername;

            // update inventory
            var inventoryItem = await _context.GoodsInventories.FirstOrDefaultAsync(g => g.CATEGORY == donation.CATEGORY);
            if (inventoryItem != null)
                inventoryItem.ITEM_COUNT += donation.ITEM_COUNT;
            else
                _context.GoodsInventories.Add(new GoodsInventory
                {
                    CATEGORY = donation.CATEGORY,
                    ITEM_COUNT = donation.ITEM_COUNT
                });

            // Create allocation record if disaster is specified
            if (donation.DISASTER_ID.HasValue && donation.DISASTER_ID.Value > 0)
            {
                var allocation = new GoodsAllocation
                {
                    DISASTER_ID = donation.DISASTER_ID.Value,
                    ITEM_COUNT = donation.ITEM_COUNT,
                    CATEGORY = donation.CATEGORY,
                    AllocationDate = donation.DATE,
                    AidType = "Donation Allocation"
                };
                _context.GoodsAllocations.Add(allocation);
            }

            _context.GoodsDonations.Add(donation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Goods donation submitted successfully! Thank you for your contribution.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.GoodsDonations.FindAsync(id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(donation);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR,DISASTER_ID")] GoodsDonation updatedDonation)
        {
            var existing = await _context.GoodsDonations.FindAsync(id);
            if (existing == null) return NotFound();

            if (!User.IsInRole("Admin") && existing.USERNAME != User.Identity?.Name)
                return Forbid();

            if (!ModelState.IsValid) return View(updatedDonation);

            // track inventory change before overwriting
            int oldCount = existing.ITEM_COUNT;
            string oldCategory = existing.CATEGORY;

            // update fields
            existing.DATE = updatedDonation.DATE;
            existing.ITEM_COUNT = updatedDonation.ITEM_COUNT;
            existing.CATEGORY = updatedDonation.CATEGORY;
            existing.DESCRIPTION = updatedDonation.DESCRIPTION;
            existing.DONOR = (updatedDonation.DONOR == "Anonymous") ? "Anonymous" : existing.USERNAME;
            existing.DISASTER_ID = updatedDonation.DISASTER_ID;

            // update inventory
            var oldInventory = await _context.GoodsInventories.FirstOrDefaultAsync(g => g.CATEGORY == oldCategory);
            if (oldInventory != null) oldInventory.ITEM_COUNT -= oldCount;

            var newInventory = await _context.GoodsInventories.FirstOrDefaultAsync(g => g.CATEGORY == existing.CATEGORY);
            if (newInventory != null)
                newInventory.ITEM_COUNT += existing.ITEM_COUNT;
            else
                _context.GoodsInventories.Add(new GoodsInventory
                {
                    CATEGORY = existing.CATEGORY,
                    ITEM_COUNT = existing.ITEM_COUNT
                });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.GoodsDonations.FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(donation);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.GoodsDonations.FindAsync(id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            // update inventory
            var inventoryItem = await _context.GoodsInventories.FirstOrDefaultAsync(g => g.CATEGORY == donation.CATEGORY);
            if (inventoryItem != null)
            {
                inventoryItem.ITEM_COUNT -= donation.ITEM_COUNT;
                if (inventoryItem.ITEM_COUNT <= 0)
                    _context.GoodsInventories.Remove(inventoryItem);
            }

            _context.GoodsDonations.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsDonationExists(int id) => _context.GoodsDonations.Any(e => e.GOODS_DONATION_ID == id);
    }
}
