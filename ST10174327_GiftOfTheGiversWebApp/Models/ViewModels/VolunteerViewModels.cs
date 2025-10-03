using ST10174327_GiftOfTheGiversWebApp.Models;
using System;
using System.Collections.Generic;

namespace ST10174327_GiftOfTheGiversWebApp.Models.ViewModels
{
    /// <summary>
    /// ViewModel for displaying key statistics and recent registrations on the volunteer dashboard.
    /// </summary>
    public class VolunteerDashboardViewModel
    {
        /// <summary>
        /// Total number of registered volunteers.
        /// </summary>
        public int TotalVolunteers { get; set; }

        /// <summary>
        /// Total number of tasks currently active.
        /// </summary>
        public int ActiveTasks { get; set; }

        /// <summary>
        /// Total number of tasks completed.
        /// </summary>
        public int CompletedTasks { get; set; }

        /// <summary>
        /// Recent volunteer registrations for display purposes.
        /// Initialized to avoid null reference exceptions.
        /// </summary>
        public List<Volunteer> RecentRegistrations { get; set; } = new List<Volunteer>();
    }

    /// <summary>
    /// ViewModel for assigning volunteers to a specific task.
    /// </summary>
    public class AssignVolunteerViewModel
    {
        /// <summary>
        /// The task to which volunteers are being assigned.
        /// </summary>
        public VolunteerTask? Task { get; set; }

        /// <summary>
        /// List of available volunteers that can be assigned to the task.
        /// Initialized to avoid null reference exceptions.
        /// </summary>
        public List<Volunteer> AvailableVolunteers { get; set; } = new List<Volunteer>();

        /// <summary>
        /// IDs of volunteers selected for assignment (useful for form posting).
        /// </summary>
        public List<int> SelectedVolunteerIDs { get; set; } = new List<int>();

        /// <summary>
        /// Remaining volunteer spots for this task.
        /// Computed property to simplify UI display.
        /// </summary>
        public int RemainingSpots => Task != null
                                     ? Math.Max(Task.RequiredVolunteers - Task.CurrentVolunteers, 0)
                                     : 0;
    }
}
