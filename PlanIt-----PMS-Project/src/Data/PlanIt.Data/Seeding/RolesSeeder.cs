namespace PlanIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(PlanItDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<PlanItRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.CompanyOwnerRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.ProjectManagerRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.ClientRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.UserRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<PlanItRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new PlanItRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
