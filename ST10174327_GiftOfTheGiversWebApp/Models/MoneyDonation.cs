using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a monetary donation made by a user.
    /// </summary>
    [Table("MoneyDonations")]
    public class MoneyDonation
    {
        /// <summary>
        /// Primary key for the MoneyDonation entity.
        /// </summary>
        [Key]
        [Required]
        [Column("MONEY_DONATION_ID")]
        public int MONEY_DONATION_ID { get; set; }

        /// <summary>
        /// Username of the user making the donation.
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("USERNAME")]
        public string USERNAME { get; set; } = string.Empty;

        /// <summary>
        /// Date when the donation was made.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        [Column("DATE")]
        public DateTime DATE { get; set; } = DateTime.Now;

        /// <summary>
        /// Amount of money donated.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Donation Amount")]
        [Column("AMOUNT", TypeName = "decimal(18,2)")]
        public decimal AMOUNT { get; set; }

        /// <summary>
        /// Name of the donor (optional). Can be "Anonymous".
        /// </summary>
        [Display(Name = "Donor Name")]
        [Column("DONOR")]
        public string? DONOR { get; set; }

        /// <summary>
        /// Payment method used for the donation (e.g., Credit Card, Bank Transfer).
        /// </summary>
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        [Column("PAYMENT_METHOD")]
        public string? PAYMENT_METHOD { get; set; }

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

        // Backward compatibility properties
        [NotMapped]
        public int Id
        {
            get => MONEY_DONATION_ID;
            set => MONEY_DONATION_ID = value;
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
            get => DONOR;
            set => DONOR = value;
        }

        [NotMapped]
        public DateTime DonationDate
        {
            get => DATE;
            set => DATE = value;
        }

        [NotMapped]
        public decimal Amount
        {
            get => AMOUNT;
            set => AMOUNT = value;
        }

        [NotMapped]
        public string? PaymentMethod
        {
            get => PAYMENT_METHOD;
            set => PAYMENT_METHOD = value;
        }
    }
}
