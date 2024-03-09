using BLL.Authentication;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double SentSMSsNum { get; set; }
        public decimal PricingPlanId { get; set; }
        public string? UserId { get; set; }
        public double NumSMSs { get; set; }
    }
}
