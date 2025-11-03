using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a monetary donation made by a user.
    /// </summary>
    public class MoneyDonation
    {
        /// <summary>
        /// Primary key for the MoneyDonation entity.
        /// </summary>
        [Key]
        [Required]
        public int MONEY_DONATION_ID { get; set; }

        /// <summary>
        /// Username of the user making the donation.
        /// </summary>
        [Required]
        public string USERNAME { get; set; }

        /// <summary>
        /// Date when the donation was made.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Donation Date")]
        public DateTime? DATE { get; set; }

        /// <summary>
        /// Amount of money donated.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Donation Amount")]
        public decimal AMOUNT { get; set; }

        /// <summary>
        /// Name of the donor (optional). Can be "Anonymous".
        /// </summary>
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
