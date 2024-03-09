using BLL.Dtos.Campaign;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface ICampaignRepository : IGenericRepository<Campaign>
    {
        IEnumerable<Campaign>? GetCampaignNumbers(int Id);
        IEnumerable<Campaign>? GetCampaignNumbersSubscription(string userId);
        IEnumerable<Campaign>? GetAllCampaignWithSMSCount();
        IEnumerable<Campaign>? GetAllCampaignWithSMSCountSubscription(string userId);
    }
}
