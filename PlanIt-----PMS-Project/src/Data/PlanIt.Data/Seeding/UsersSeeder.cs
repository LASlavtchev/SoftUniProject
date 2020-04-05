namespace PlanIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Models;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(PlanItDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<PlanItUser>>();

            if (dbContext.Users.Count() == 1)
            {
                for (int i = 0; i < 15; i++)
                {
                    await SeedUserAsync(dbContext, userManager, i);
                }
            }
        }

        private static async Task SeedUserAsync(PlanItDbContext dbContext, UserManager<PlanItUser> userManager, int i)
        {
            var user = new PlanItUser
            {
                UserName = $"loafer{i}@aaa.bg",
                Email = $"loafer{i}@aaa.bg",
                FirstName = $"Ivan{i}",
                MiddleName = $"Petrov{i}",
                LastName = $"Georgiev{i}",
                CompanyName = $"Project{i}X",
                JobTitle = $"Engineer",
                PhoneNumber = $"+35988797212{i}",
                EmailConfirmed = true,
            };

            var isExist = dbContext.Users.Any(u => u.Email == user.Email);

            if (!isExist)
            {
                var result = await userManager.CreateAsync(user, $"123456-{i}");

                if (result.Succeeded)
                {
                    // Assign to user roles
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

                    // Assign to user claims - needed for the _loginPartial
                    string fullName = $"{user.FirstName} {user.LastName}";
                    await userManager.AddClaimAsync(user, new Claim("FullName", fullName));
                    await userManager.AddClaimAsync(user, new Claim("JobTitle", user.JobTitle));
                    await userManager.AddClaimAsync(user, new Claim("CompanyName", user.CompanyName));
                }
                else
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
