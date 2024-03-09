using BLL.IRepository;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
    {
        private readonly ApplicationDbContext _context;
        public CampaignRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Campaign> GetAllCampaignWithSMSCount()
        {
            return _context.Campaigns
            .Include(c => c.Phones);
        }
        
        public IEnumerable<Campaign> GetAllCampaignWithSMSCountSubscription(string userId)
        {
            return _context.Campaigns
            .Include(c => c.Phones)
            .Where(c => c.UserId == userId);
        }

        public IEnumerable<Campaign> GetCampaignNumbers(int Id)
        {
            return _context.Campaigns
            .Include(c => c.Phones)
            .Where(c => c.Id == Id);
        }

        public IEnumerable<Campaign>? GetCampaignNumbersSubscription(string userId)
        {
            return _context.Campaigns
            .Include(c => c.Phones)
            .Where(c => c.UserId == userId);
        }
    }
}
