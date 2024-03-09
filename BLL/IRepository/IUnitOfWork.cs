
namespace BLL.IRepository
{
    public interface IUnitOfWork<T> where T : class
    {
        IGenericRepository<T> Entity { get; }
        ICampaignRepository _campaignRepository { get; }
        ISMSNumberRepository _smsNumberRepository { get; }

        int Save();
    }
}
