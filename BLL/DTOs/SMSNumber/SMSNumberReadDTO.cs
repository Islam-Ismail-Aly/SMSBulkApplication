using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.SMSNumber
{
    public class SMSNumberReadDTO
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
