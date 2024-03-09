using BLL.Authentication;
using BLL.Interfaces;
using Final.Project.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ApiExplorerSettings(GroupName = "AuthenticationAPIv1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenService;
        private readonly UserManager<ApplicationUser> _Usermanager;

        public AuthenticationController(IAuthenService authenService, UserManager<ApplicationUser> usermanager)
        {
            _authenService = authenService;
            _Usermanager = usermanager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            var result = await _authenService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(ModelState);

            //return Ok(result);
            return Ok(new {token = result.Token, ExpiresOn = result.ExpiresOn});
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestDto model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var result = await _authenService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(new { result.Message, result.IsAuthenticated });

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCustomRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var result = await _authenService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangepassDto passwordDto)
        {
            //get the user
            ApplicationUser? currentUser = await _Usermanager.GetUserAsync(User);
            if (currentUser is null)
            {
                return NotFound();
            }

            //confirm old password
            var isValiduser = await _Usermanager.CheckPasswordAsync(currentUser!, passwordDto.OldPassword);
            if (!isValiduser)
            {
                return NotFound();
            }

            if (passwordDto.NewPassword != passwordDto.ConfirmNewPassword)
            {
                return NoContent();
            }

            //change password
            await _Usermanager.ChangePasswordAsync(currentUser!, passwordDto.OldPassword, passwordDto.NewPassword);

            return Ok();
        }

    }
}
