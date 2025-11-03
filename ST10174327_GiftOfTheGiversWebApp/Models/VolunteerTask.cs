using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a task that can be assigned to volunteers.
    /// </summary>
    public class VolunteerTask
    {
        /// <summary>
        /// Primary key: uniquely identifies the task.
        /// </summary>
        [Key]
        public int TaskId { get; set; }

        /// <summary>
        /// Title of the task.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the task.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Date when the task was created.
        /// </summary>
        [Required]
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Due date for the task (optional).
        /// </summary>
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Status of the task (e.g., New, In Progress, Completed).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "New";

        /// <summary>
        /// Priority of the task (e.g., Low, Medium, High).
        /// </summary>
        [StringLength(20)]
        public string? Priority { get; set; }

        /// <summary>
        /// Location where the task needs to be performed.
        /// </summary>
        [StringLength(200)]
        public string? Location { get; set; }

        /// <summary>
        /// Collection of task assignments.
        /// </summary>
        public virtual ICollection<TaskAssignment>? Assignments { get; set; }
    }
}
