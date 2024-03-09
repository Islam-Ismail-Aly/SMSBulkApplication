using BLL.DTOs.SMSNumber;

namespace BLL.Dtos.Campaign
{
    public class CampaignDetailsDTO
    {

        public string CampaignName { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string ContentMessage { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public IEnumerable<SMSNumberListDTO> PhoneNumbers { get; set; } = new HashSet<SMSNumberListDTO>();
    }
}
