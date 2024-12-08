using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.ServiceInterface
{
    public interface ITokennService
    {

        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager);
    }
}
