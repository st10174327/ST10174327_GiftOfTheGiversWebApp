using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a volunteer in the Gift of the Givers system.
    /// </summary>
    public class Volunteer
    {
        /// <summary>
        /// Primary key for the Volunteer entity.
        /// </summary>
        [Key]
        [Display(Name = "Volunteer ID")]
        public int VolunteerID { get; set; }

        /// <summary>
        /// Full name of the volunteer.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public required string Name { get; set; }

        /// <summary>
        /// Email address of the volunteer.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [Display(Name = "Email Address")]
        public required string Email { get; set; }

        /// <summary>
        /// Phone number of the volunteer.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Residential address of the volunteer (optional).
        /// </summary>
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        /// <summary>
        /// Emergency contact information for the volunteer (optional).
        /// </summary>
        [StringLength(500, ErrorMessage = "Emergency contact cannot exceed 500 characters")]
        [Display(Name = "Emergency Contact")]
        public string? EmergencyContact { get; set; }

        /// <summary>
        /// Availability of the volunteer (optional, e.g., "Mon-Fri, 9am-5pm").
        /// </summary>
        [StringLength(200, ErrorMessage = "Availability cannot exceed 200 characters")]
        [Display(Name = "Availability")]
        public string? Availability { get; set; }

        /// <summary>
        /// Date when the volunteer registered.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Current status of the volunteer (default: "Active").
        /// </summary>
        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Skills possessed by the volunteer (optional).
        /// </summary>
        [StringLength(500, ErrorMessage = "Skills cannot exceed 500 characters")]
        [Display(Name = "Skills & Qualifications")]
        public string? Skills { get; set; }

        /// <summary>
        /// Indicates whether the volunteer is currently active.
        /// </summary>
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Additional notes about the volunteer (optional).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        [Display(Name = "Additional Notes")]
        public string? Notes { get; set; }

        /// <summary>
        /// Preferred areas of work for the volunteer (optional).
        /// </summary>
        [StringLength(200, ErrorMessage = "Preferred areas cannot exceed 200 characters")]
        [Display(Name = "Preferred Areas")]
        public string? PreferredAreas { get; set; }

        /// <summary>
        /// ID number or passport number of the volunteer (optional).
        /// </summary>
        [StringLength(50, ErrorMessage = "ID number cannot exceed 50 characters")]
        [Display(Name = "ID/Passport Number")]
        public string? IdNumber { get; set; }

        /// <summary>
        /// Date of birth of the volunteer (optional).
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Navigation property: A volunteer can have multiple task assignments.
        /// </summary>
        public ICollection<TaskAssignment> Tasks { get; set; } = new List<TaskAssignment>();
    }
} 