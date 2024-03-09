using Azure.Core;
using BLL.Dtos.Campaign;
using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.DTOs.Subscription;
using BLL.IRepository;
using BLL.IServices;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace SMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "CampaignAPIv1")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? _userId = string.Empty;

        public CampaignController(IHttpContextAccessor httpContextAccessor, ICampaignService campaignService, ISubscriptionService subscriptionService, ApplicationDbContext context)
        {
            _campaignService = campaignService;
            _subscriptionService = subscriptionService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet("GetAllCampaigns")]
        public ActionResult<IEnumerable<CampaignDTO>> GetAllCampaigns()
        {
            IEnumerable<CampaignDTO> campaignDto = _campaignService.GetAllCampaigns();
            return Ok(campaignDto);
        }

        [HttpGet("GetCampaign/{Id}")]
        public ActionResult<CampaignDTO> GetCampaign(int Id)
        {
            CampaignDTO? campaign = _campaignService.GetCampaignById(Id);
            if (campaign is null) { return NotFound(); }

            return Ok(campaign);
        }

        [HttpGet("GetCampaignSMSCount/{Id}")]
        public async Task<ActionResult<CampaignReadDTO>> GetCampaignSMSCount(int Id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Phones)
                .Select(c => new CampaignReadDTO
                {
                    Id = c.Id,
                    Name = c.CampaignName,
                    NumberOfSMSNumbers = c.Phones.Select(s => s.PhoneNumber).Count(),
                })
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (campaign == null)
            {
                return NotFound();
            }

            return campaign;
        }

        [HttpGet("GetCampaignNumbers/{Id}")]
        public ActionResult GetCampaignNumbers(int Id)
        {
            IEnumerable<SMSNumberReadDTO>? numberReads = _campaignService.GetAllCampaignWithSMSNumbers(Id);
            if (numberReads == null) { return NotFound(); }
            return Ok(numberReads);
        }

        [HttpPost("AddCampaign")]
        public ActionResult AddCampaign([FromForm] CampaignAddDTO campaignAddDto)
        {
            bool isAdded = _campaignService.Add(campaignAddDto, _userId);

            SubscriptionModifyDTO modifyDTO = new SubscriptionModifyDTO();

            IEnumerable<CampaignReadDTO>? NumOfPhones = _campaignService.GetAllCampaignWithSMSCountSubscription(_userId);

            IEnumerable<SMSNumberReadDTO>? NumOfSMS = _campaignService.GetCampaignNumbersSubscription(_userId);

            var numPhones = NumOfPhones.Select(x => x.NumberOfSMSNumbers).Count();

            var numSMS = NumOfSMS.Select(x => x.PhoneNumber).Count();

            var SentSMSsNum = numPhones * numSMS;

            var totalParts = _campaignService.GetTotalParts();

            modifyDTO.SentSMSsNum = SentSMSsNum + (totalParts.SentSMSsNum - 1);

            IEnumerable<SubscriptionReadDTO> allSubscription = _subscriptionService.GetAllSubscription();

            foreach (var item in allSubscription)
            {
                var sentSMSdb = item.SentSMSsNum + SentSMSsNum;
                if (sentSMSdb <= item.NumSMSs)
                {
                    var Id = item.Id;
                    bool isEdited = _subscriptionService.UpdateSecondSubscription(Id, modifyDTO.SentSMSsNum);
                    //return isEdited ? NoContent() : BadRequest();
                }
                else
                {
                    foreach (var itemSecond in allSubscription)
                    {
                        if (sentSMSdb > itemSecond.NumSMSs && itemSecond.SentSMSsNum < item.SentSMSsNum)
                        {
                            var Id = itemSecond.Id;
                            bool isEdited = _subscriptionService.UpdateSecondSubscription(Id, modifyDTO.SentSMSsNum);
                            //return isEdited ? NoContent() : BadRequest();
                        }
                    }
                }
            }
            return isAdded ? NoContent() : BadRequest();
        }
        

        [HttpPut("EditCampaign")]
        public ActionResult Edit(CampaignModifyDTO modifyDTO)
        {
            bool isEdited = _campaignService.Update(modifyDTO);

            return isEdited ? NoContent() : BadRequest();
        }

        [HttpDelete("DeleteCampaign/{Id}")]
        public ActionResult Delete(int Id)
        {
            bool isDeleted = _campaignService.Delete(Id);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
