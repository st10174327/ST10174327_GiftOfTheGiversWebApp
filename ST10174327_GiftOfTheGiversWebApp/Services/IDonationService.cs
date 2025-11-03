using ST10174327_GiftOfTheGiversWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public interface IDonationService
    {
        Task<bool> AddMoneyDonationAsync(MoneyDonation donation);
        Task<bool> AddGoodsDonationAsync(GoodsDonation donation);
        Task<IEnumerable<MoneyDonation>> GetMoneyDonationsAsync();
        Task<IEnumerable<GoodsDonation>> GetGoodsDonationsAsync();
        Task<decimal> GetTotalMoneyDonationsAsync();
        Task<int> GetTotalGoodsDonationsAsync();
    }
}
