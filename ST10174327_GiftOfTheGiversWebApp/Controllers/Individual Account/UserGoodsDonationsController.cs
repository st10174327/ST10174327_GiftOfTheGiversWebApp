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
                return View(await _context.GoodsDonation.ToListAsync());
            }

            // User sees only their own donations
            var userGoodsDonations = await _context.GoodsDonation
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userGoodsDonations);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.GoodsDonation.FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(donation);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View(new GoodsDonation
            {
                DATE = DateTime.Now.Date
            });
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR")] GoodsDonation donation)
        {
            string? currentUsername = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUsername))
                return RedirectToAction("Login", "Account");

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
            var inventoryItem = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == donation.CATEGORY);
            if (inventoryItem != null)
                inventoryItem.ITEM_COUNT += donation.ITEM_COUNT;
            else
                _context.GoodsInventory.Add(new GoodsInventory
                {
                    CATEGORY = donation.CATEGORY,
                    ITEM_COUNT = donation.ITEM_COUNT
                });

            _context.Add(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.GoodsDonation.FindAsync(id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            return View(donation);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("DATE,ITEM_COUNT,CATEGORY,DESCRIPTION,DONOR")] GoodsDonation updatedDonation)
        {
            var existing = await _context.GoodsDonation.FindAsync(id);
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

            // update inventory
            var oldInventory = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == oldCategory);
            if (oldInventory != null) oldInventory.ITEM_COUNT -= oldCount;

            var newInventory = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == existing.CATEGORY);
            if (newInventory != null)
                newInventory.ITEM_COUNT += existing.ITEM_COUNT;
            else
                _context.GoodsInventory.Add(new GoodsInventory
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

            var donation = await _context.GoodsDonation.FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
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
            var donation = await _context.GoodsDonation.FindAsync(id);
            if (donation == null) return NotFound();

            if (!User.IsInRole("Admin") && donation.USERNAME != User.Identity?.Name)
                return Forbid();

            // update inventory
            var inventoryItem = await _context.GoodsInventory.FirstOrDefaultAsync(g => g.CATEGORY == donation.CATEGORY);
            if (inventoryItem != null)
            {
                inventoryItem.ITEM_COUNT -= donation.ITEM_COUNT;
                if (inventoryItem.ITEM_COUNT <= 0)
                    _context.GoodsInventory.Remove(inventoryItem);
            }

            _context.GoodsDonation.Remove(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
