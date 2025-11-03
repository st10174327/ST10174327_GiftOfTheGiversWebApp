using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a disaster event in the system.
    /// </summary>
    public class Disaster
    {
        /// <summary>
        /// Primary key: uniquely identifies a disaster.
        /// </summary>
        [Key]
        [Display(Name = "Disaster ID")]
        public int DISASTER_ID { get; set; }

        /// <summary>
        /// Username of the user who reported or manages this disaster.
        /// Optional.
        /// </summary>
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string? USERNAME { get; set; }

        /// <summary>
        /// The start date of the disaster.
        /// Required and stored as date only.
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Start Date")]
        public DateTime STARTDATE { get; set; }

        /// <summary>
        /// The end date of the disaster.
        /// Required and stored as date only.
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "End Date")]
        public DateTime ENDDATE { get; set; }

        /// <summary>
        /// Location where the disaster occurred.
        /// Required.
        /// </summary>
        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string LOCATION { get; set; } = string.Empty;

        /// <summary>
        /// Type of aid required for the disaster.
        /// Required.
        /// </summary>
        [Required(ErrorMessage = "Aid type is required")]
        [Display(Name = "Aid Type")]
        [StringLength(50, ErrorMessage = "Aid type cannot exceed 50 characters")]
        public string AID_TYPE { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the disaster is currently active.
        /// 1 = Active, 0 = Inactive.
        /// </summary>
        [Display(Name = "Is Active")]
        public int IsActive { get; set; } = 1;

        /// <summary>
        /// Name or description of the disaster.
        /// </summary>
        [Required(ErrorMessage = "Disaster name is required")]
        [Display(Name = "Disaster Name")]
        [StringLength(100, ErrorMessage = "Disaster name cannot exceed 100 characters")]
        public string DisasterName { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the disaster.
        /// </summary>
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property for money allocations related to this disaster.
        /// </summary>
        public virtual ICollection<MoneyAllocation>? MoneyAllocations { get; set; }

        /// <summary>
        /// Navigation property for goods allocations related to this disaster.
        /// </summary>
        public virtual ICollection<GoodsAllocation>? GoodsAllocations { get; set; }

        /// <summary>
        /// Navigation property for money donations related to this disaster.
        /// </summary>
        public virtual ICollection<MoneyDonation>? MoneyDonations { get; set; }

        /// <summary>
        /// Navigation property for goods donations related to this disaster.
        /// </summary>
        public virtual ICollection<GoodsDonation>? GoodsDonations { get; set; }
    }
}