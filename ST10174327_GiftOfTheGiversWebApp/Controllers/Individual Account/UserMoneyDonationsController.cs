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
    public class UserMoneyDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserMoneyDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserMoneyDonations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Login", "Account");

            string currentUsername = User.Identity.Name ?? throw new InvalidOperationException("User not authenticated");
            var userMoneyDonations = await _context.MoneyDonation
                .Where(d => d.USERNAME == currentUsername)
                .ToListAsync();

            return View(userMoneyDonations);
        }

        // GET: Details
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            if (moneyDonation == null) return NotFound();

            return View(moneyDonation);
        }

        // GET: Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            // Get available disasters for allocation
            var availableDisasters = await _context.Disaster
                .Where(d => d.IsActive == 1)
                .ToListAsync();

            ViewBag.AvailableDisasters = availableDisasters;

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("MONEY_DONATION_ID,DATE,AMOUNT,DONOR,DISASTER_ID")] MoneyDonation moneyDonation)
        {
            if (ModelState.IsValid)
            {
                string currentUsername = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
                moneyDonation.USERNAME = currentUsername;

                if (moneyDonation.DATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                    return View(moneyDonation);
                }

                // Handle donor name
                moneyDonation.DONOR = (moneyDonation.DONOR == "Anonymous")
                    ? "Anonymous"
                    : currentUsername;

                // Update central Money table
                var money = _context.Money.FirstOrDefault();
                if (money == null)
                {
                    money = new Money
                    {
                        TotalMoney = moneyDonation.AMOUNT,
                        RemainingMoney = moneyDonation.AMOUNT
                    };
                    _context.Add(money);
                }
                else
                {
                    money.TotalMoney += moneyDonation.AMOUNT;
                    money.RemainingMoney += moneyDonation.AMOUNT;
                    _context.Update(money);
                }

                // Create allocation record if disaster is specified
                if (moneyDonation.DISASTER_ID.HasValue && moneyDonation.DISASTER_ID.Value > 0)
                {
                    var allocation = new MoneyAllocation
                    {
                        DISASTER_ID = moneyDonation.DISASTER_ID.Value,
                        AllocationAmount = moneyDonation.AMOUNT,
                        AllocationDate = moneyDonation.DATE.GetValueOrDefault(),
                        AidType = "Donation Allocation"
                    };
                    _context.MoneyAllocation.Add(allocation);

                    // Update remaining money (only if we have a valid money record)
                    if (money != null)
                    {
                        money.RemainingMoney -= moneyDonation.AMOUNT;
                        _context.Update(money);
                    }
                }

                _context.Add(moneyDonation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Money donation submitted successfully! Thank you for your generous contribution.";

                return RedirectToAction(nameof(Index));
            }
            return View(moneyDonation);
        }

        // GET: Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (moneyDonation == null) return NotFound();

            return View(moneyDonation);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR,DISASTER_ID")] MoneyDonation updatedDonation)
        {
            if (id != updatedDonation.MONEY_DONATION_ID) return NotFound();

            var existingDonation = await _context.MoneyDonation.FindAsync(id);
            if (existingDonation == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    decimal difference = updatedDonation.AMOUNT - existingDonation.AMOUNT;

                    // Update money totals
                    var money = _context.Money.FirstOrDefault();
                    if (money != null)
                    {
                        money.TotalMoney += difference;
                        money.RemainingMoney += difference;
                        _context.Update(money);
                    }

                    // Update donation
                    existingDonation.DATE = updatedDonation.DATE.GetValueOrDefault();
                    existingDonation.AMOUNT = updatedDonation.AMOUNT;
                    existingDonation.DONOR = updatedDonation.DONOR;
                    existingDonation.DISASTER_ID = updatedDonation.DISASTER_ID;
                    _context.Update(existingDonation);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyDonationExists(updatedDonation.MONEY_DONATION_ID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedDonation);
        }

        // GET: Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            if (moneyDonation == null) return NotFound();

            return View(moneyDonation);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (moneyDonation == null) return NotFound();

            // Adjust totals
            var money = _context.Money.FirstOrDefault();
            if (money != null)
            {
                money.TotalMoney -= moneyDonation.AMOUNT;
                money.RemainingMoney -= moneyDonation.AMOUNT;
                _context.Update(money);
            }

            _context.MoneyDonation.Remove(moneyDonation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationExists(int id)
        {
            return _context.MoneyDonation.Any(e => e.MONEY_DONATION_ID == id);
        }
    }
}
