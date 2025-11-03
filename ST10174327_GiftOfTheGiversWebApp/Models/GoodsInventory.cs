using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents an inventory record for donated goods.
    /// </summary>
    [Table("GoodsInventory")]
    public class GoodsInventory
    {
        /// <summary>
        /// Primary key: uniquely identifies the inventory record.
        /// </summary>
        [Key]
        [Column("GOODSINVENTORY_ID")]
        public int GOODSINVENTORY_ID { get; set; }

        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required]
        [StringLength(200)]
        [Display(Name = "Item Name")]
        [Column("ITEM_NAME")]
        public string ITEM_NAME { get; set; } = string.Empty;

        /// <summary>
        /// Category of the goods (e.g., Electronics, Clothing).
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Category")]
        [Column("CATEGORY")]
        public string CATEGORY { get; set; } = string.Empty;

        /// <summary>
        /// Description of the item.
        /// </summary>
        [StringLength(500)]
        [Column("DESCRIPTION")]
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// Number of items in this category.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Item count cannot be negative.")]
        [Display(Name = "Quantity")]
        [Column("QUANTITY")]
        public int QUANTITY { get; set; } = 0;

        /// <summary>
        /// Date when the item was added to inventory.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        [Column("DATE_ADDED")]
        public DateTime DATE_ADDED { get; set; } = DateTime.Now;

        /// <summary>
        /// Whether the item is currently available.
        /// </summary>
        [Display(Name = "Is Available")]
        [Column("IS_AVAILABLE")]
        public bool IS_AVAILABLE { get; set; } = true;

        // Navigation properties
        public virtual ICollection<GoodsAllocation>? GoodsAllocations { get; set; }

        public virtual ICollection<GoodsPurchase>? GoodsPurchases { get; set; }

        // Backward compatibility properties
        [NotMapped]
        public int GoodsInventoryId
        {
            get => GOODSINVENTORY_ID;
            set => GOODSINVENTORY_ID = value;
        }

        [NotMapped]
        public string ItemName
        {
            get => ITEM_NAME;
            set => ITEM_NAME = value ?? string.Empty;
        }

        [NotMapped]
        public string? Description
        {
            get => DESCRIPTION;
            set => DESCRIPTION = value;
        }

        [NotMapped]
        public int Quantity
        {
            get => QUANTITY;
            set => QUANTITY = value;
        }

        [NotMapped]
        public DateTime DateAdded
        {
            get => DATE_ADDED;
            set => DATE_ADDED = value;
        }

        [NotMapped]
        public bool IsAvailable
        {
            get => IS_AVAILABLE;
            set => IS_AVAILABLE = value;
        }
    }
}
