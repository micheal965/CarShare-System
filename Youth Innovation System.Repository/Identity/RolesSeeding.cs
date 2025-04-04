using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Repository.Identity
{
    public static class RolesSeeding
    {
        public async static Task SeedRoles(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] rolesData = { UserRoles.Admin.ToString(), UserRoles.CarOwner.ToString(), UserRoles.Renter.ToString() };
            foreach (var item in rolesData)
            {
                if (!await roleManager.RoleExistsAsync(item))
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }

        }
    }
}
