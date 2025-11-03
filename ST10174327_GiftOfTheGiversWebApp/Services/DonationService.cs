using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext _context;

        public DonationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMoneyDonationAsync(MoneyDonation donation)
        {
            try
            {
                donation.DATE = DateTime.UtcNow;
                _context.MoneyDonation.Add(donation);
                
                // Update total money
                var money = await _context.Money.FirstOrDefaultAsync();
                if (money == null)
                {
                    money = new Money { TotalMoney = 0, RemainingMoney = 0 };
                    _context.Money.Add(money);
                }
                money.TotalMoney += donation.AMOUNT;
                money.RemainingMoney += donation.AMOUNT;
                money.LastUpdated = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding money donation: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddGoodsDonationAsync(GoodsDonation donation)
        {
            try
            {
                donation.DATE = DateTime.UtcNow;
                _context.GoodsDonations.Add(donation);
                
                // Update inventory
                var inventory = await _context.GoodsInventories
                    .FirstOrDefaultAsync(i => i.ITEM_NAME == donation.ITEM_NAME);
                
                if (inventory == null)
                {
                    inventory = new GoodsInventory
                    {
                        ITEM_NAME = donation.ITEM_NAME,
                        CATEGORY = donation.CATEGORY,
                        QUANTITY = 0,
                        DATE_ADDED = DateTime.UtcNow,
                        IS_AVAILABLE = true
                    };
                    _context.GoodsInventories.Add(inventory);
                }
                
                inventory.QUANTITY += donation.QUANTITY;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<MoneyDonation>> GetMoneyDonationsAsync()
        {
            return await _context.MoneyDonation
                .Include(d => d.Disaster)
                .OrderByDescending(d => d.DATE)
                .ToListAsync();
        }

        public async Task<IEnumerable<GoodsDonation>> GetGoodsDonationsAsync()
        {
            return await _context.GoodsDonation
                .Include(d => d.Disaster)
                .OrderByDescending(d => d.DATE)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalMoneyDonationsAsync()
        {
            var money = await _context.Money.FirstOrDefaultAsync();
            return money?.TotalMoney ?? 0;
        }

        public async Task<int> GetTotalGoodsDonationsAsync()
        {
            return await _context.GoodsDonation.SumAsync(d => d.QUANTITY);
        }
    }
}
