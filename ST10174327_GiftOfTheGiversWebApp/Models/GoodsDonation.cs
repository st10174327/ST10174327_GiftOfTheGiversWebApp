using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a donation of goods by a user.
    /// </summary>
    public class GoodsDonation
    {
        /// <summary>
        /// Primary key: uniquely identifies the goods donation.
        /// </summary>
        [Key]
        public int GOODS_DONATION_ID { get; set; }

        /// <summary>
        /// Username of the person making the donation.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string USERNAME { get; set; } = string.Empty;

        /// <summary>
        /// Date of the donation.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        public DateTime DATE { get; set; }

        /// <summary>
        /// Number of items donated.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Item count must be at least 1.")]
        [Display(Name = "Number of Items")]
        public int ITEM_COUNT { get; set; }

        /// <summary>
        /// Category of the donated goods.
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Category")]
        public string CATEGORY { get; set; } = string.Empty;

        /// <summary>
        /// Description of the donated goods.
        /// </summary>
        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; } = string.Empty;

        /// <summary>
        /// Name of the donor (optional, can be "Anonymous").
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Donor")]
        public string? DONOR { get; set; }

        /// <summary>
        /// Foreign key reference to a disaster (optional).
        /// </summary>
        [Display(Name = "Disaster")]
        public int? DISASTER_ID { get; set; }

        /// <summary>
        /// Navigation property to Disaster.
        /// </summary>
        [ForeignKey("DISASTER_ID")]
        public virtual Disaster? Disaster { get; set; }
    }
}
