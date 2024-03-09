using BLL.Dtos.Subscription;
using BLL.DTOs.Subscription;
using BLL.Entities;

namespace BLL.IServices
{
    public interface ISubscriptionService
    {
        IEnumerable<SubscriptionReadDTO> GetAllSubscription();
        //bool Add(SubscriptionAddDTO subscriptionDto, string userId);
        bool Add(SubscriptionAddDTO subscriptionDto);
        bool Update(string userId, double SentSMSsNum);
        bool UpdateSecondSubscription(int Id, double SentSMSsNum);

        SubscriptionReadDTO? GetNumSMSsByUser(string userId);
    }
}
