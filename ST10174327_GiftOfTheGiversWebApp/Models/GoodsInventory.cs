using System.ComponentModel.DataAnnotations;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents an inventory record for donated goods.
    /// </summary>
    public class GoodsInventory
    {
        /// <summary>
        /// Primary key: uniquely identifies the inventory record.
        /// </summary>
        [Key]
        public int GOODS_INVENTORY_ID { get; set; }

        /// <summary>
        /// Category of the goods (e.g., Electronics, Clothing).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CATEGORY { get; set; } = string.Empty;

        /// <summary>
        /// Number of items in this category.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Item count cannot be negative.")]
        public int ITEM_COUNT { get; set; } = 0;
    }
}
