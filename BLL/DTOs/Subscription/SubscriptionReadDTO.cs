using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Subscription
{
    public class SubscriptionReadDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double SentSMSsNum { get; set; }
        public decimal PricingPlanId { get; set; }
        public string UserId { get; set; }
        public double NumSMSs { get; set; }
    }
}
