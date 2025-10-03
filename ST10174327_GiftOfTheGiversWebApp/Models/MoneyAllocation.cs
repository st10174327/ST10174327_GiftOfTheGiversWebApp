using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    public class MoneyAllocation
    {
        [Key]
        public int MoneyAllocationId { get; set; }

        [Required(ErrorMessage = "Disaster is required")]
        [Display(Name = "Disaster")]
        public int DISASTER_ID { get; set; }  // Use uppercase to match Disaster model

        [ForeignKey("DISASTER_ID")]
        public virtual Disaster? Disaster { get; set; }

        [Required(ErrorMessage = "Allocation amount is required")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Allocation amount must be greater than 0")]
        [Display(Name = "Allocation Amount")]
        public decimal AllocationAmount { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Allocation Date")]
        public DateTime AllocationDate { get; set; } = DateTime.Now;

        [Display(Name = "Aid Type")]
        [StringLength(100, ErrorMessage = "Aid type cannot exceed 100 characters")]
        public string? AidType { get; set; }
    }
}