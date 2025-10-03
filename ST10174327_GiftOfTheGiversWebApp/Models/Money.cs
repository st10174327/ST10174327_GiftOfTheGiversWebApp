using System.ComponentModel.DataAnnotations;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents the total and remaining monetary funds available for donations or allocations.
    /// </summary>
    public class Money
    {
        [Key]
        public int MoneyId { get; set; }

        [Required]
        [Display(Name = "Total Money")]
        [DataType(DataType.Currency)]
        public decimal TotalMoney { get; set; }

        [Required]
        [Display(Name = "Remaining Money")]
        [DataType(DataType.Currency)]
        public decimal RemainingMoney { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
} 