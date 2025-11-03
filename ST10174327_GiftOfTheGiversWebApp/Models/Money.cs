using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents the total and remaining monetary funds available for donations or allocations.
    /// </summary>
    [Table("Money")]
    public class Money
    {
        [Key]
        [Column("MONEY_ID")]
        public int MONEY_ID { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        [Column("TOTAL_AMOUNT", TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal TOTAL_AMOUNT { get; set; }

        [Required]
        [Display(Name = "Remaining Amount")]
        [Column("REMAINING_AMOUNT", TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal REMAINING_AMOUNT { get; set; }

        [Display(Name = "Last Updated")]
        [Column("LAST_UPDATED")]
        public DateTime LAST_UPDATED { get; set; } = DateTime.UtcNow;

        // Backward compatibility properties
        [NotMapped]
        public int MoneyId
        {
            get => MONEY_ID;
            set => MONEY_ID = value;
        }

        [NotMapped]
        public decimal TotalMoney
        {
            get => TOTAL_AMOUNT;
            set => TOTAL_AMOUNT = value;
        }

        [NotMapped]
        public decimal RemainingMoney
        {
            get => REMAINING_AMOUNT;
            set => REMAINING_AMOUNT = value;
        }

        [NotMapped]
        public DateTime LastUpdated
        {
            get => LAST_UPDATED;
            set => LAST_UPDATED = value;
        }

        [NotMapped]
        public decimal TotalAmount
        {
            get => TOTAL_AMOUNT;
            set => TOTAL_AMOUNT = value;
        }
    }
}