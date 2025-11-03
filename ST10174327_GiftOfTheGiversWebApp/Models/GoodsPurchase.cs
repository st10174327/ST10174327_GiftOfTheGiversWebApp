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
        [Display(Name = "Goods Purchase ID")]
        public int GoodsPurchaseID { get; set; }

        /// <summary>
        /// Price per item. Must be greater than or equal to 0.
        /// </summary>
        [Required(ErrorMessage = "Item price is required")]
        [Display(Name = "Item Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal GoodsPurchasePrice { get; set; }

        /// <summary>
        /// Number of items purchased. Must be non-negative.
        /// </summary>
        [Required(ErrorMessage = "Item count is required")]
        [Display(Name = "Number of Items")]
        [Range(0, int.MaxValue, ErrorMessage = "Item count cannot be negative.")]
        public int ITEM_COUNT { get; set; }

        /// <summary>
        /// Total price of the goods purchase (calculated as GoodsPurchasePrice * ITEM_COUNT).
        /// </summary>
        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal GoodsTotalPrice { get; set; }

        /// <summary>
        /// Optional category of the purchased goods.
        /// </summary>
        [Display(Name = "Category")]
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? CATEGORY { get; set; }

        /// <summary>
        /// Name or description of the purchased goods.
        /// </summary>
        [Display(Name = "Item Name")]
        [StringLength(200, ErrorMessage = "Item name cannot exceed 200 characters")]
        public string? ItemName { get; set; }

        /// <summary>
        /// Date when the purchase was made.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Additional notes about the purchase.
        /// </summary>
        [Display(Name = "Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        /// <summary>
        /// Foreign key reference to GoodsInventory.
        /// </summary>
        [Display(Name = "Goods Inventory")]
        public int GOODSINVENTORY_ID { get; set; }

        /// <summary>
        /// Navigation property to GoodsInventory.
        /// </summary>
        [ForeignKey("GOODSINVENTORY_ID")]
        public virtual GoodsInventory? GoodsInventory { get; set; }

        /// <summary>
        /// Method to calculate and set the total price.
        /// Call this method before saving to ensure total price is calculated.
        /// </summary>
        public void CalculateTotalPrice()
        {
            GoodsTotalPrice = GoodsPurchasePrice * ITEM_COUNT;
        }

        /// <summary>
        /// Method to automatically calculate total price when properties change.
        /// </summary>
        public void UpdateTotalPrice()
        {
            CalculateTotalPrice();
        }
    }
}  