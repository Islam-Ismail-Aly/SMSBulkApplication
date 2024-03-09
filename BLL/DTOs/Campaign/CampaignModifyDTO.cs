using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Campaign
{
    public class CampaignModifyDTO
    {
        public int Id { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
