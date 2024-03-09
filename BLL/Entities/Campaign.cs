namespace DAL.Entities
{
    public class Campaign
    {
        public Campaign()
        {
            Phones = new HashSet<SMSNumber>();
        }

        public int Id { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string ContentMessage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public virtual IEnumerable<SMSNumber> Phones { get; set; } 
    }
}
