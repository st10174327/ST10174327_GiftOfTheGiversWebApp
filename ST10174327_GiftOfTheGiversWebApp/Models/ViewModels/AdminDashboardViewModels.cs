using System.Collections.Generic;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalVolunteers { get; set; }
        public int ActiveTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalDisasters { get; set; }
        public int ActiveDisasters { get; set; }
        public decimal TotalMoneyDonations { get; set; }
        public int TotalGoodsDonations { get; set; }
        public decimal TotalDonationsValue { get; set; }
        public List<Volunteer> RecentRegistrations { get; set; } = new();
        public List<Disaster> RecentDisasters { get; set; } = new();
    }

    public class AdminAssignmentVM
    {
        public VolunteerTask? Task { get; set; }
        public List<Volunteer> AvailableVolunteers { get; set; } = new();
    }

    public class DonationsViewModel
    {
        public List<MoneyDonation> MoneyDonations { get; set; } = new();
        public List<GoodsDonation> GoodsDonations { get; set; } = new();
    }

    public class AllocationsViewModel
    {
        public List<MoneyAllocation> MoneyAllocations { get; set; } = new();
        public List<GoodsAllocation> GoodsAllocations { get; set; } = new();
    }
}
