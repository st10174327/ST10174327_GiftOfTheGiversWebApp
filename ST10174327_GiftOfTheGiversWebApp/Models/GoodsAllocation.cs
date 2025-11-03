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
        public int AllocationId { get; set; }

        /// <summary>
        /// Foreign key reference to a disaster.
        /// </summary>
        [Required(ErrorMessage = "Disaster is required")]
        [Display(Name = "Disaster")]
        public int DisasterId { get; set; }

        /// <summary>
        /// Foreign key reference to goods inventory.
        /// </summary>
        [Required(ErrorMessage = "Goods inventory is required")]
        [Display(Name = "Goods Inventory")]
        [ForeignKey("GoodsInventory")]
        public int GoodsInventoryId { get; set; }

        /// <summary>
        /// Navigation property to Disaster.
        /// </summary>
        [ForeignKey("DisasterId")]
        public virtual Disaster? Disaster { get; set; }

        /// <summary>
        /// Navigation property to GoodsInventory.
        /// </summary>
        public virtual GoodsInventory? GoodsInventory { get; set; }

        /// <summary>
        /// Number of items allocated.
        /// </summary>
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Date of allocation. Defaults to current date.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Allocation Date")]
        public DateTime AllocationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Type of aid for which goods are allocated (optional).
        /// </summary>
        [StringLength(100)]
        public string? AidType { get; set; }
    }
}
