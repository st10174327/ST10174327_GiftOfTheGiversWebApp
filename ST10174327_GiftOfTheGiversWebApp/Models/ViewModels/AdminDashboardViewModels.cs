using System.Collections.Generic;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalVolunteers { get; set; }
        public int ActiveTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<Volunteer> RecentRegistrations { get; set; } = new();
    }

    public class AdminAssignmentVM
    {
        public VolunteerTask? Task { get; set; }
        public List<Volunteer> AvailableVolunteers { get; set; } = new();
    }
}
