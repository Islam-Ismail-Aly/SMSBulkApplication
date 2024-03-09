using BLL.DTOs.Campaign;
using BLL.DTOs.SMSNumber;
using BLL.IServices;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SMSNumbersAPIv1")]
    public class SMSNumberController : ControllerBase
    {
        private readonly ISMSNumberService _smsNumberService;
        private readonly ApplicationDbContext _context;

        public SMSNumberController(ISMSNumberService smsNumberService, ApplicationDbContext context)
        {
            _smsNumberService = smsNumberService;
            _context = context;
        }

        [HttpGet("GetAllNumbers")]
        public ActionResult GetAllNumbers()
        {
            IEnumerable<SMSNumberReadDTO>? smsNumberDto = _smsNumberService.GetAllNumbers();
            return Ok(smsNumberDto);
        }
        
        [HttpGet("GetAllNumbersById/{Id}")]
        public ActionResult GetAllNumbers(int Id)
        {
            IEnumerable<SMSNumberReadDTO>? smsNumberDto = _smsNumberService.GetAllNumbersById(Id);
            return Ok(smsNumberDto);
        }
    }
}
