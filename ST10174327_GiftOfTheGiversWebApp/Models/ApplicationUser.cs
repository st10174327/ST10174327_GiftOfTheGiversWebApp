using Microsoft.AspNetCore.Identity;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber2 { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
    }
}
