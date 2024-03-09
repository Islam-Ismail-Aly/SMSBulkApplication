using BLL.Dtos.Subscription;
using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.DTOs.Subscription;
using BLL.Entities;
using BLL.IRepository;
using BLL.IServices;

namespace BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork<Subscription> _unitOfWork;

        public SubscriptionService(IUnitOfWork<Subscription> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Add(SubscriptionAddDTO subscriptionDto)
        {
            Subscription subscription = new Subscription()
            {
                StartDate = subscriptionDto.StartDate,
                EndDate = subscriptionDto.EndDate,
                NumSMSs = subscriptionDto.NumSMSs,
                PricingPlanId = subscriptionDto.PricingPlanId,
                SentSMSsNum = subscriptionDto.SentSMSsNum,
                UserId = subscriptionDto.UserId,
            };

            _unitOfWork.Entity.Add(subscription);
            _unitOfWork.Save();
            return _unitOfWork.Save() > 0;
        }

        public IEnumerable<SubscriptionReadDTO> GetAllSubscription()
        {
            IEnumerable<Subscription> db = _unitOfWork.Entity.GetAll();
            IEnumerable<SubscriptionReadDTO> subscriptionDto = db.Select(c => new SubscriptionReadDTO()
            {
                Id = c.Id,
                EndDate = c.EndDate,
                StartDate = c.StartDate,
                NumSMSs = c.NumSMSs,
                PricingPlanId = c.PricingPlanId,
                SentSMSsNum = c.SentSMSsNum,
                UserId = c.UserId,
            });
            return subscriptionDto;
        }

        public IEnumerable<SMSNumberReadDTO> GetAllCampaignWithSMSNumbers(int userId)
        {
            var db = _unitOfWork._campaignRepository.GetCampaignNumbers(userId);

            if (db is null) { return null; };

            var numbersCampaign = db.Select(p => new SMSNumberReadDTO
            {
                Id = p.Id,
                CampaignName = p.CampaignName,
                PhoneNumber = p.Phones.Count().ToString(),
            });

            return numbersCampaign;
        }

        public bool Update(string userId, double SentSMSsNum)
        {
            var subscription = _unitOfWork.Entity.GetByUserId(userId);
            if (subscription is null)
            {
                return false;
            }

            subscription.SentSMSsNum = SentSMSsNum;

            return _unitOfWork.Save() > 0;
        }

        public SubscriptionReadDTO? GetNumSMSsByUser(string userId)
        {
            var numSMS = _unitOfWork.Entity.GetByUserId(userId);
            if (numSMS is null)
            {
                return null;
            }
            return new SubscriptionReadDTO()
            {
               UserId = userId,
               NumSMSs = numSMS.NumSMSs,
            };
        }

        public bool UpdateSecondSubscription(int Id, double SentSMSsNum)
        {
            var subscription = _unitOfWork.Entity.GetById(Id);
            if (subscription is null)
            {
                return false;
            }

            subscription.SentSMSsNum = SentSMSsNum;

            return _unitOfWork.Save() > 0;
        }
    }
}
