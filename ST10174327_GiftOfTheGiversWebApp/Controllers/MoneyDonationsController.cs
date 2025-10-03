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
    public class MoneyDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoneyDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoneyDonations
        public async Task<IActionResult> Index()
        {
            return _context.MoneyDonation != null
                ? View(await _context.MoneyDonation.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.MoneyDonation' is null.");
        }

        // GET: MoneyDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MoneyDonation == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation
                .FirstOrDefaultAsync(m => m.MONEY_DONATION_ID == id);

            return moneyDonation == null ? NotFound() : View(moneyDonation);
        }

        // GET: MoneyDonations/Create
        public IActionResult Create() => View();

        // POST: MoneyDonations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
        {
            if (!ModelState.IsValid) return View(moneyDonation);

            string? currentUsername = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUsername))
            {
                return RedirectToAction("Login", "Account");
            }

            moneyDonation.USERNAME = currentUsername;

            // ✅ Fix: only disallow future dates
            if (!moneyDonation.DATE.HasValue)
            {
                ModelState.AddModelError("DATE", "Date is required.");
                return View(moneyDonation);
            }
            if (moneyDonation.DATE.Value.Date > DateTime.Now.Date)
            {
                ModelState.AddModelError("DATE", "Date cannot be in the future.");
                return View(moneyDonation);
            }

            // ✅ Fix: respect user-entered donor name, default to username
            if (string.IsNullOrEmpty(moneyDonation.DONOR))
            {
                moneyDonation.DONOR = currentUsername;
            }

            // Update Money totals
            var money = await _context.Money.FirstOrDefaultAsync();
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

            _context.Add(moneyDonation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: MoneyDonations/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MoneyDonation == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            return moneyDonation == null ? NotFound() : View(moneyDonation);
        }

        // POST: MoneyDonations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MONEY_DONATION_ID,USERNAME,DATE,AMOUNT,DONOR")] MoneyDonation moneyDonation)
        {
            if (id != moneyDonation.MONEY_DONATION_ID) return NotFound();

            if (!ModelState.IsValid) return View(moneyDonation);

            var existingMoneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (existingMoneyDonation == null) return NotFound();

            // Adjust Money totals
            decimal donationDifference = moneyDonation.AMOUNT - existingMoneyDonation.AMOUNT;
            var money = await _context.Money.FirstOrDefaultAsync();
            if (money != null)
            {
                money.TotalMoney += donationDifference;
                money.RemainingMoney += donationDifference;
                _context.Update(money);
            }

            // Update editable fields
            existingMoneyDonation.DATE = moneyDonation.DATE;
            existingMoneyDonation.AMOUNT = moneyDonation.AMOUNT;
            existingMoneyDonation.DONOR = string.IsNullOrEmpty(moneyDonation.DONOR)
                ? existingMoneyDonation.USERNAME
                : moneyDonation.DONOR;

            _context.Update(existingMoneyDonation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: MoneyDonations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MoneyDonation == null) return NotFound();

            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            return moneyDonation == null ? NotFound() : View(moneyDonation);
        }

        // POST: MoneyDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moneyDonation = await _context.MoneyDonation.FindAsync(id);
            if (moneyDonation != null)
            {
                var money = await _context.Money.FirstOrDefaultAsync();
                if (money != null)
                {
                    money.TotalMoney -= moneyDonation.AMOUNT;
                    money.RemainingMoney -= moneyDonation.AMOUNT;
                    _context.Update(money);
                }

                _context.Remove(moneyDonation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MoneyDonationExists(int id)
        {
            return _context.MoneyDonation.Any(e => e.MONEY_DONATION_ID == id);
        }
    }
}
