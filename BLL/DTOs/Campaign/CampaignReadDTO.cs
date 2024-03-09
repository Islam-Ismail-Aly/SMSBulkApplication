﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Campaign
{
    public class CampaignReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public int NumberOfSMSNumbers { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
