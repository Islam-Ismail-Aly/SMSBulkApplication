using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Dtos.Subscription
{
    public class SubscriptionAddDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double SentSMSsNum { get; set; }
        public decimal PricingPlanId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string? UserId { get; set; }
        public double NumSMSs { get; set; }
    }
}
