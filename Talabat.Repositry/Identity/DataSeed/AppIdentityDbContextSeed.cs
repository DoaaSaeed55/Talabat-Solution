using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repositry.Identity.DataSeed
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName="AA",
                    Email="Do@gmail.com",
                    UserName="Doaa",
                    PhoneNumber="01135252112"
                };
                await _userManager.CreateAsync(user, password: "P$hfj75%H");
            }
            
        }
    }
}
