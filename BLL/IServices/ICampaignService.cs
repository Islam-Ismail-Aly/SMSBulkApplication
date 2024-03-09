using BLL.Dtos.Campaign;
using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.DTOs.Subscription;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.IServices
{
    public interface ICampaignService
    {
        IEnumerable<CampaignDTO> GetAllCampaigns();
        public CampaignDTO? GetCampaignById(int id);
        IEnumerable<CampaignReadDTO> GetAllCampaignWithSMSCount();
        IEnumerable<CampaignReadDTO> GetAllCampaignWithSMSCountSubscription(string userId);
        IEnumerable<SMSNumberReadDTO> GetAllCampaignWithSMSNumbers(int id);
        IEnumerable<SMSNumberReadDTO> GetCampaignNumbersSubscription(string userId);
        bool Add(CampaignAddDTO campaignDto, string userId);
        bool Update(CampaignModifyDTO campaignDto);
        bool Delete(int Id);
        public SubscriptionModifyDTO GetTotalParts();

    }
}
