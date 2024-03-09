using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.Campaign
{
    public class CampaignAddDTO
    {
        public string CampaignName { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string ContentMessage { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public List<string>? PhoneNumber { get; set; }
    }
}
