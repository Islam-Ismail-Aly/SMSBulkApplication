using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.IRepository;
using BLL.IServices;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class SMSNumberService : ISMSNumberService
    {
        private readonly IUnitOfWork<SMSNumber> _unitOfWork;

        public SMSNumberService(IUnitOfWork<SMSNumber> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SMSNumberReadDTO>? GetAllNumbers()
        {
            IEnumerable<SMSNumber>? smsNumberDb = _unitOfWork._smsNumberRepository.GetAllNumbers();

            if (smsNumberDb is null) { return null; };

            var campaignDto = smsNumberDb.Select(c => new SMSNumberReadDTO
            {
                Id = c.Id,
                CampaignName = c.Campaign.CampaignName,
                PhoneNumber = c.PhoneNumber,
            });
            return campaignDto;
        }
        
        public IEnumerable<SMSNumberReadDTO>? GetAllNumbersById(int id)
        {
            IEnumerable<SMSNumber>? smsNumberDb = _unitOfWork._smsNumberRepository.GetAllNumbersById(id);

            if (smsNumberDb is null) { return null; };

            var campaignDto = smsNumberDb.Select(c => new SMSNumberReadDTO
            {
                Id = c.Id,
                CampaignName = c.Campaign.CampaignName,
                PhoneNumber = c.PhoneNumber,
            });
            return campaignDto;
        }
    }
}
