using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents the allocation of money for a disaster or aid event.
    /// </summary>
    public class MoneyAllocation
    {
        /// <summary>
        /// Primary key: uniquely identifies a money allocation record.
        /// </summary>
        [Key]
        public int AllocationId { get; set; }

        /// <summary>
        /// Foreign key reference to a disaster.
        /// </summary>
        [Required(ErrorMessage = "Disaster is required")]
        [Display(Name = "Disaster")]
        public int DisasterId { get; set; }

        /// <summary>
        /// Navigation property to Disaster.
        /// </summary>
        [ForeignKey("DisasterId")]
        public virtual Disaster? Disaster { get; set; }

        /// <summary>
        /// Amount of money allocated.
        /// </summary>
        [Required(ErrorMessage = "Allocation amount is required")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Allocation amount must be greater than 0")]
        [Display(Name = "Allocation Amount")]
        public decimal AllocationAmount { get; set; }

        /// <summary>
        /// Date when the allocation was made.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Allocation Date")]
        public DateTime AllocationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Type of aid for which money is allocated (optional).
        /// </summary>
        [Display(Name = "Aid Type")]
        [StringLength(100, ErrorMessage = "Aid type cannot exceed 100 characters")]
        public string? AidType { get; set; }
    }
}