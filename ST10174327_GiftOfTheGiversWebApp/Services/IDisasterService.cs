using ST10174327_GiftOfTheGiversWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public interface IDisasterService
    {
        Task<IEnumerable<Disaster>> GetActiveDisastersAsync();
        Task<Disaster> GetDisasterByIdAsync(int id);
        Task<bool> AddDisasterAsync(Disaster disaster);
        Task<bool> UpdateDisasterAsync(Disaster disaster);
        Task<bool> DeleteDisasterAsync(int id);
    }
}
