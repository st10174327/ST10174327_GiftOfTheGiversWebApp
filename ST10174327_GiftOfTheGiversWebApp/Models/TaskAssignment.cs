using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents an assignment of a task to a volunteer.
    /// </summary>
    public class TaskAssignment
    {
        /// <summary>
        /// Primary key: uniquely identifies the task assignment.
        /// </summary>
        [Key]
        public int AssignmentId { get; set; }

        /// <summary>
        /// Foreign key reference to the volunteer.
        /// </summary>
        [Required]
        [Display(Name = "Volunteer")]
        public int VolunteerId { get; set; }

        /// <summary>
        /// Navigation property to the volunteer.
        /// </summary>
        [ForeignKey("VolunteerId")]
        public virtual Volunteer? Volunteer { get; set; }

        /// <summary>
        /// Foreign key reference to the task.
        /// </summary>
        [Required]
        [Display(Name = "Task")]
        public int TaskId { get; set; }

        /// <summary>
        /// Navigation property to the task.
        /// </summary>
        [ForeignKey("TaskId")]
        public virtual VolunteerTask? Task { get; set; }

        /// <summary>
        /// Date when the task was assigned.
        /// </summary>
        [Required]
        [Display(Name = "Assigned Date")]
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Status of the task assignment (e.g., Assigned, In Progress, Completed).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Assigned";

        /// <summary>
        /// Any additional notes about the task assignment.
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
