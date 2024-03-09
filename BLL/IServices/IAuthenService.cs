using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Authentication;

namespace BLL.Interfaces
{
    public interface IAuthenService
    {
        Task<AuthenDto> RegisterAsync(RegisterDto registerModel);
        Task<AuthenDto> LoginAsync(TokenRequestDto tokenModel);
        Task<string> AddRoleAsync(RoleDto roleModel);
    }
}
