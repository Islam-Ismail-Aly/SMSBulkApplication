using BLL.Authentication;
using BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SMSAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTService _jwt;

        public AuthenService(UserManager<ApplicationUser> userManager, 
                            IOptions<JWTService> jwt,
                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthenDto> LoginAsync(TokenRequestDto tokenModel)
        {
            var authenModel = new AuthenDto();

            var user = await _userManager.FindByEmailAsync(tokenModel.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, tokenModel.Password))
            {
                authenModel.Message = "Email or Password not valid!";
                return authenModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            authenModel.IsAuthenticated = true;
            authenModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenModel.Email = user.Email;
            authenModel.UserName = user.UserName;
            authenModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authenModel.Roles = roles.ToList();

            return authenModel;
        }

        public async Task<string> AddRoleAsync(RoleDto roleModel)
        {
            var userId = await _userManager.FindByIdAsync(roleModel.UserId);

            if (userId == null || !await _roleManager.RoleExistsAsync(roleModel.Role))
                return "Invalid UserId or Role!";

            var roles = await _userManager.GetRolesAsync(userId);

            if (roles.Contains(roleModel.Role))
                return "User take to this Role!";

            var result = await _userManager.AddToRoleAsync(userId, roleModel.Role);

            return result.Succeeded ? string.Empty : "Error";
        }

        public async Task<AuthenDto> RegisterAsync(RegisterDto registerModel)
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email) != null)
            {
                return new AuthenDto { Message = "Email is already registered" };
            }

            if (await _userManager.FindByEmailAsync(registerModel.UserName) != null)
            {
                return new AuthenDto { Message = "UserName is already registered" };
            }

            var user = new ApplicationUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                return new AuthenDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthenDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var securityCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: securityCredentials);

            return jwtSecurityToken;
        }


    }
}
