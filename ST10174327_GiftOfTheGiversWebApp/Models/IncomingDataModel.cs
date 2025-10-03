using System.Collections.Generic;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    /// <summary>
    /// Represents a container for incoming data combining disasters, goods donations, and money donations.
    /// Useful for dashboards or data aggregation views.
    /// </summary>
    public class IncomingDataModel
    {
        /// <summary>
        /// List of disasters.
        /// </summary>
        public IEnumerable<Disaster> Disasters { get; set; } = new List<Disaster>();

        /// <summary>
        /// List of goods donations.
        /// </summary>
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; } = new List<GoodsDonation>();

        /// <summary>
        /// List of money donations.
        /// </summary>
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; } = new List<MoneyDonation>();
    }
}
