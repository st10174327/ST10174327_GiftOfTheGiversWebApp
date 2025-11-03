using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a donation of goods by a user.
    /// </summary>
    [Table("GoodsDonations")]
    public class GoodsDonation
    {
        /// <summary>
        /// Primary key: uniquely identifies the goods donation.
        /// </summary>
        [Key]
        [Column("GOODS_DONATION_ID")]
        public int GOODS_DONATION_ID { get; set; }

        /// <summary>
        /// Username of the person making the donation.
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("USERNAME")]
        public string USERNAME { get; set; } = string.Empty;

        /// <summary>
        /// Date of the donation.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        [Column("DATE")]
        public DateTime DATE { get; set; } = DateTime.Now;

        /// <summary>
        /// Name of the donated item.
        /// </summary>
        [Required]
        [StringLength(200)]
        [Display(Name = "Item Name")]
        [Column("ITEM_NAME")]
        public string ITEM_NAME { get; set; } = string.Empty;

        /// <summary>
        /// Category of the donated goods.
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Category")]
        [Column("CATEGORY")]
        public string? CATEGORY { get; set; }

        /// <summary>
        /// Number of items donated.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [Display(Name = "Quantity")]
        [Column("QUANTITY")]
        public int QUANTITY { get; set; }

        /// <summary>
        /// Description of the donated goods (optional).
        /// </summary>
        [StringLength(500)]
        [Display(Name = "Description")]
        [Column("DESCRIPTION")]
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// Foreign key reference to the associated disaster (optional).
        /// </summary>
        [Display(Name = "Disaster")]
        [Column("DISASTER_ID")]
        public int? DISASTER_ID { get; set; }

        /// <summary>
        /// Navigation property to the associated disaster.
        /// </summary>
        [ForeignKey("DISASTER_ID")]
        public virtual Disaster? Disaster { get; set; }

        // Backward compatibility properties
        [NotMapped]
        public int Id
        {
            get => GOODS_DONATION_ID;
            set => GOODS_DONATION_ID = value;
        }

        [NotMapped]
        public int? DisasterId
        {
            get => DISASTER_ID;
            set => DISASTER_ID = value;
        }

        [NotMapped]
        public string? DonorName
        {
            get => USERNAME;
            set => USERNAME = value ?? string.Empty;
        }

        [NotMapped]
        public string? ItemName
        {
            get => ITEM_NAME;
            set => ITEM_NAME = value ?? string.Empty;
        }

        [NotMapped]
        public int ItemCount
        {
            get => QUANTITY;
            set => QUANTITY = value;
        }

        [NotMapped]
        public DateTime DonationDate
        {
            get => DATE;
            set => DATE = value;
        }

        /// <summary>
        /// Description of the donated goods (optional).
        /// </summary>
        [StringLength(500)]
        [Display(Name = "Description")]
        [Column("DESCRIPTION")]
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// Name of the donor (optional, can be "Anonymous").
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Donor")]
        [Column("DONOR")]
        public string? DONOR { get; set; }

        /// <summary>
        /// Foreign key reference to a disaster (optional).
        /// </summary>
        [Display(Name = "Disaster")]
        [Column("DISASTER_ID")]
        public int? DISASTER_ID { get; set; }

        /// <summary>
        /// Navigation property to the associated disaster.
        /// </summary>
        [ForeignKey("DISASTER_ID")]
        public virtual Disaster? Disaster { get; set; }

        // Navigation property for backward compatibility
        [NotMapped]
        public int? DisasterId
        {
            get => DISASTER_ID;
            set => DISASTER_ID = value;
        }

        // Navigation property for backward compatibility
        [NotMapped]
        public string? DonorName
        {
            get => DONOR;
            set => DONOR = value;
        }

        // Navigation property for backward compatibility
        [NotMapped]
        public string? ItemName
        {
            get => ITEM_NAME;
            set => ITEM_NAME = value ?? string.Empty;
        }

        // Navigation property for backward compatibility
        [NotMapped]
        public int Quantity
        {
            get => ITEM_COUNT;
            set => ITEM_COUNT = value;
        }
    }
}
