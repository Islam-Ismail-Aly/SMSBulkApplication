using BLL.Authentication;
using BLL.Dtos.Campaign;
using BLL.Dtos.Subscription;
using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.DTOs.Subscription;
using BLL.IServices;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SubscriptionAPIv1")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ICampaignService _campaignService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? _userId = string.Empty;
        public SubscriptionController(IHttpContextAccessor httpContextAccessor, ISubscriptionService subscriptionService, ICampaignService campaignService, UserManager<ApplicationUser> userManager)
        {
            _subscriptionService = subscriptionService;
            _campaignService = campaignService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("AddSubscription")]
        public ActionResult AddCampaign([FromForm] SubscriptionAddDTO subscriptionAddDTO)
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = _userManager.GetUserId(User);
            subscriptionAddDTO.UserId = _userId;
            bool isAdded = _subscriptionService.Add(subscriptionAddDTO);
            //bool isAdded = _subscriptionService.Add(subscriptionAddDTO);

            return isAdded ? NoContent() : BadRequest();
        }

        [HttpGet("GetAllSubscriptions")]
        public ActionResult<IEnumerable<SubscriptionReadDTO>> GetAllSubscriptions()
        {
            IEnumerable<SubscriptionReadDTO> SubscDto = _subscriptionService.GetAllSubscription();
            return Ok(SubscDto);
        }

        [HttpPut("UpdateSubscription")]
        public ActionResult UpdateSubscription()
        {
            //var userId = _userManager.GetUserId(User);

            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            SubscriptionModifyDTO modifyDTO = new SubscriptionModifyDTO();

            IEnumerable<CampaignReadDTO>? NumOfPhones = _campaignService.GetAllCampaignWithSMSCountSubscription(_userId);

            IEnumerable<SMSNumberReadDTO>? NumOfSMS = _campaignService.GetCampaignNumbersSubscription(_userId);

            var numPhones = NumOfPhones.Select(x => x.NumberOfSMSNumbers).Count();

            var numSMS = NumOfSMS.Select(x => x.PhoneNumber).Count();

            var SentSMSsNum = numPhones * numSMS;

            modifyDTO.SentSMSsNum = SentSMSsNum;

            //SubscriptionReadDTO? SubscDto = _subscriptionService.GetNumSMSsByUser(userId);

            IEnumerable<SubscriptionReadDTO> allSubscription = _subscriptionService.GetAllSubscription();

            foreach (var item in allSubscription)
            {
                var sentSMSdb = item.SentSMSsNum + SentSMSsNum;
                if (sentSMSdb <= item.NumSMSs)
                {
                    var Id = item.Id;
                    bool isEdited = _subscriptionService.UpdateSecondSubscription(Id, SentSMSsNum);
                    return isEdited ? NoContent() : BadRequest();
                }
                else
                {
                    foreach (var itemSecond in allSubscription)
                    {
                        if (sentSMSdb > itemSecond.NumSMSs && itemSecond.SentSMSsNum < item.SentSMSsNum)
                        {
                            var Id = itemSecond.Id;
                            bool isEdited = _subscriptionService.UpdateSecondSubscription(Id, SentSMSsNum);
                            return isEdited ? NoContent() : BadRequest();
                        }
                    }
                }
            }

            return NoContent();
        }

        [HttpGet("GetNumSMSsByUser")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<SubscriptionReadDTO> GetNumSMSsByUser()
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = _userManager.GetUserId(User);
            SubscriptionReadDTO? SubscDto = _subscriptionService.GetNumSMSsByUser(_userId);
            return Ok(SubscDto);
        }
    }
}
