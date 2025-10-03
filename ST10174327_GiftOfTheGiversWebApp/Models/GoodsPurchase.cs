using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a purchase of goods, including item count, price, and category.
    /// </summary>
    public class GoodsPurchase
    {
        /// <summary>
        /// Primary key for the GoodsPurchase record.
        /// </summary>
        [Key]
        public int GoodsPurchaseID { get; set; }

        /// <summary>
        /// Price per item. Must be greater than or equal to 0.
        /// </summary>
        [Required]
        [Display(Name = "Item Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal GoodsPurchasePrice { get; set; }

        /// <summary>
        /// Number of items purchased. Must be non-negative.
        /// </summary>
        [Required]
        [Display(Name = "Number of items")]
        [Range(0, int.MaxValue, ErrorMessage = "Item count cannot be negative.")]
        public int ITEM_COUNT { get; set; }

        /// <summary>
        /// Total price of the goods purchase (calculated as GoodsPurchasePrice * ITEM_COUNT).
        /// </summary>
        [Display(Name = "Total Price")]
        public decimal GoodsTotalPrice
        {
            get => GoodsPurchasePrice * ITEM_COUNT;
        }

        /// <summary>
        /// Optional category of the purchased goods.
        /// </summary>
        [Display(Name = "Category")]
        [StringLength(100)]
        public string? CATEGORY { get; set; }

        /// <summary>
        /// Non-mapped property for linking to GoodsInventory.
        /// </summary>
        [NotMapped]
        public int GOODS_INVENTORY_ID { get; set; }
    }
}
