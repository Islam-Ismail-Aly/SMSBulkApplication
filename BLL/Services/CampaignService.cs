using BLL.Dtos.Campaign;
using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.DTOs.Subscription;
using BLL.Entities;
using BLL.IRepository;
using BLL.IServices;
using DAL.Entities;
using System.Text.Json;

namespace BLL.Services
{
    public class CampaignService : ICampaignService
    {
        private const int MESSAGE_LENGTH = 160;
        private double totalParts = 0;

        private readonly IUnitOfWork<Campaign> _unitOfWork;

        public CampaignService(IUnitOfWork<Campaign> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CampaignDTO> GetAllCampaigns()
        {
            IEnumerable<Campaign> db = _unitOfWork.Entity.GetAll();
            IEnumerable<CampaignDTO> campaignDto = db.Select(c => new CampaignDTO()
            {
                Id = c.Id,
                Name = c.CampaignName,
                StartDate = c.StartDate,
                Status = c.Status,
                UserId = c.UserId,
            });
            return campaignDto;
        }


        public IEnumerable<SMSNumberReadDTO> GetAllCampaignWithSMSNumbers(int Id)
        {
            var db = _unitOfWork._campaignRepository.GetCampaignNumbers(Id);

            if (db is null) { return null; };

            var numbersCampaign = db.Select(p => new SMSNumberReadDTO
            {
                Id = p.Id,
                CampaignName = p.CampaignName,
                PhoneNumber = p.Phones.Count().ToString(),
                UserId = p.UserId,
            });

            return numbersCampaign;
        }

        public IEnumerable<CampaignReadDTO> GetAllCampaignWithSMSCount()
        {
            var db = _unitOfWork._campaignRepository.GetAllCampaignWithSMSCount();
            var CampaignReadDto = db.Select(c => new CampaignReadDTO
            {
                Id = c.Id,
                Name = c.CampaignName,
                NumberOfSMSNumbers = c.Phones.Count(),
                UserId = c.UserId,
            });

            return CampaignReadDto;
        }

        public CampaignDTO? GetCampaignById(int Id)
        {
            var db = _unitOfWork.Entity.GetById(Id);
            if (db is null) { return null; }
            return new CampaignDTO()
            {
                Id = db.Id,
                Name = db.CampaignName,
                StartDate = db.StartDate,
                Status = db.Status,
                UserId = db.UserId,
            };
        }

        #region Old Code of Add Method
        // Need to review 
        //public bool Add(CampaignAddDTO campaignAdd, string userId)
        //{
        //    bool result = false;

        //    Campaign addCampaign = new Campaign
        //    {
        //        CampaignName = campaignAdd.CampaignName,
        //        SenderName = campaignAdd.SenderName,
        //        ContentMessage = campaignAdd.ContentMessage,
        //        StartDate = campaignAdd.StartDate,
        //        Status = campaignAdd.Status,
        //        UserId = userId,
        //    };

        //    _unitOfWork.Entity.Add(addCampaign);
        //    _unitOfWork.Save();

        //    var smsNumbers = campaignAdd.PhoneNumber.Select(number => new SMSNumber
        //    {
        //        CampaignId = addCampaign.Id,
        //        PhoneNumber = number,
        //    }).ToList();

        //    _unitOfWork.Entity.AddRange(smsNumbers);
        //    result = _unitOfWork.Save() > 0;

        //    if (result == true)
        //        return true;
        //    else
        //        return result;
        //}
        #endregion

        public bool Add(CampaignAddDTO campaignDto, string userId)
        {
            bool result = false;
            string /*part, */ partFocus = string.Empty;

            List<string> combinedMessages = new List<string>();

            string combinedMessage = $"{campaignDto.ContentMessage}";

            if (combinedMessage.Length <= MESSAGE_LENGTH)
            {
                Campaign addCampaign = new Campaign
                {
                    CampaignName = campaignDto.CampaignName,
                    SenderName = campaignDto.SenderName,
                    ContentMessage = combinedMessage,
                    StartDate = campaignDto.StartDate,
                    Status = campaignDto.Status,
                    UserId = userId,
                };

                _unitOfWork.Entity.Add(addCampaign);
                _unitOfWork.Save();

                var smsNumbers = campaignDto.PhoneNumber.Select(number => new SMSNumber
                {
                    CampaignId = addCampaign.Id,
                    PhoneNumber = number,
                }).ToList();

                _unitOfWork.Entity.AddRange(smsNumbers);
                result = _unitOfWork.Save() > 0;
            }
            else
            {
               totalParts = (int)Math.Ceiling((double)combinedMessage.Length / MESSAGE_LENGTH);

                for (int i = 0; i < totalParts; i++)
                {
                    int startIndex = i * MESSAGE_LENGTH;
                    int length = Math.Min(MESSAGE_LENGTH, combinedMessage.Length - startIndex);
                    //part = combinedMessage.Substring(startIndex, length);
                    partFocus = combinedMessage;
                }

                Campaign addCampaign = new Campaign
                {
                    CampaignName = campaignDto.CampaignName,
                    SenderName = campaignDto.SenderName,
                    ContentMessage = partFocus,
                    StartDate = campaignDto.StartDate,
                    Status = campaignDto.Status,
                    UserId = userId,
                };

                _unitOfWork.Entity.Add(addCampaign);
                _unitOfWork.Save();

                var smsNumbers = campaignDto.PhoneNumber.Select(number => new SMSNumber
                {
                    CampaignId = addCampaign.Id,
                    PhoneNumber = number,
                }).ToList();

                _unitOfWork.Entity.AddRange(smsNumbers);
                result = _unitOfWork.Save() > 0;
            }
            return result;
        }

        public bool Update(CampaignModifyDTO campaignDto)
        {
            var campaignModify = _unitOfWork.Entity.GetById(campaignDto.Id);

            if (campaignModify is null)
            {
                return false;
            }

            campaignModify.CampaignName = campaignDto.CampaignName;
            campaignModify.StartDate = campaignDto.StartDate;
            campaignModify.Status = campaignDto.Status;

            return _unitOfWork.Save() > 0;
        }

        public bool Delete(int Id)
        {
            _unitOfWork.Entity.Delete(Id);
            return _unitOfWork.Save() > 0;
        }

        public IEnumerable<CampaignReadDTO> GetAllCampaignWithSMSCountSubscription(string userId)
        {
            var db = _unitOfWork._campaignRepository.GetAllCampaignWithSMSCountSubscription(userId);
            var CampaignReadDto = db.Select(c => new CampaignReadDTO
            {
                Id = c.Id,
                Name = c.CampaignName,
                NumberOfSMSNumbers = c.Phones.Count(),
                UserId = c.UserId,
            });

            return CampaignReadDto;
        }

        public IEnumerable<SMSNumberReadDTO> GetCampaignNumbersSubscription(string userId)
        {
            var db = _unitOfWork._campaignRepository.GetCampaignNumbersSubscription(userId);

            if (db is null) { return null; };

            var numbersCampaign = db.Select(p => new SMSNumberReadDTO
            {
                Id = p.Id,
                CampaignName = p.CampaignName,
                PhoneNumber = p.Phones.Count().ToString(),
                UserId = p.UserId,
            });

            return numbersCampaign;
        }

        public SubscriptionModifyDTO GetTotalParts()
        {
            var totalPartDto = new SubscriptionModifyDTO { SentSMSsNum = totalParts};
            return totalPartDto;
        }
    }
}
