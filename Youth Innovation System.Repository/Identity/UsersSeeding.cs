using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Repository.Identity
{
    public static class UsersSeeding
    {
        public static async Task SeedAdmin(this UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                //pictureurl needed
                var user = new ApplicationUser()
                {
                    firstName = "Micheal",
                    lastName = "Ghobrial",
                    Email = "michealghobriall@gmail.com",
                    UserName = "Micheal.Ghobrial",
                    PhoneNumber = "01201605049",
                    status = UserStatus.accepted.ToString()
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
                await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            }
        }
    }
}
