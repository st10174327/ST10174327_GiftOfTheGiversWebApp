using ST10174327_GiftOfTheGiversWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public interface IVolunteerService
    {
        Task<bool> RegisterVolunteerAsync(Volunteer volunteer);
        Task<bool> AssignTaskAsync(TaskAssignment task);
        Task<IEnumerable<Volunteer>> GetAllVolunteersAsync();
        Task<IEnumerable<TaskAssignment>> GetVolunteerTasksAsync(string volunteerId);
        Task<bool> UpdateVolunteerStatusAsync(string volunteerId, bool isActive);
    }
}
