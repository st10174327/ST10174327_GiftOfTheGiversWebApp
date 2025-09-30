using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Models
{
    public class IncomingDataModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<GoodsDonation> GoodsDonations { get; set; }
        public IEnumerable<MoneyDonation> MoneyDonations { get; set; }
    }
}
