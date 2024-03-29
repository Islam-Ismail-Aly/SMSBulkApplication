﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class SMSNumber
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int CampaignId { get; set; }

        [ForeignKey(nameof(CampaignId))]
        public virtual Campaign Campaign { get; set; } = null;
    }
}
