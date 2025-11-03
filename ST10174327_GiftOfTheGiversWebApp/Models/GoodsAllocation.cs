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
        /// Foreign key reference to a disaster.
        /// </summary>
        [Required(ErrorMessage = "Disaster is required")]
        [Display(Name = "Disaster")]
        public int DISASTER_ID { get; set; }

        /// <summary>
        /// Navigation property to Disaster.
        /// </summary>
        [ForeignKey("DISASTER_ID")]
        public virtual Disaster? Disaster { get; set; }

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
        /// Foreign key reference to GoodsInventory (optional).
        /// </summary>
        [Display(Name = "Goods Inventory")]
        public int? GOODSINVENTORY_ID { get; set; }

        /// <summary>
        /// Navigation property to GoodsInventory.
        /// </summary>
        [ForeignKey("GOODSINVENTORY_ID")]
        public virtual GoodsInventory? GoodsInventory { get; set; }
    }
}
