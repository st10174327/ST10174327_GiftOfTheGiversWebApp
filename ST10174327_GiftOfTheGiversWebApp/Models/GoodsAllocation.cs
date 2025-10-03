using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents the allocation of goods for a disaster or aid event.
    /// </summary>
    public class GoodsAllocation
    {
        /// <summary>
        /// Primary key: uniquely identifies a goods allocation record.
        /// </summary>
        [Key]
        public int GoodsAllocationId { get; set; }

        /// <summary>
        /// Number of items allocated.
        /// </summary>
        [Required]
        [Display(Name = "Number of Items")]
        public int ITEM_COUNT { get; set; }

        /// <summary>
        /// Category of goods being allocated (optional).
        /// </summary>
        [Display(Name = "Category")]
        public string? CATEGORY { get; set; }

        /// <summary>
        /// Date of allocation. Defaults to current date.
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Allocation Date")]
        public DateTime? AllocationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Type of aid for which goods are allocated (optional).
        /// </summary>
        public string? AidType { get; set; }

        /// <summary>
        /// Foreign key reference to a disaster (not mapped to DB).
        /// </summary>
        [NotMapped]
        public int DisasterId { get; set; }

        // Optional: Navigation property to Disaster if you want a relationship
        // public virtual Disaster? Disaster { get; set; }
    }
}
