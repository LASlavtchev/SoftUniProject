namespace PlanIt.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    internal class InvitesSeeder : ISeederInvites
    {
        public async Task SeedAsync(InvitesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Invites.Any())
            {
                return;
            }

            List<Invite> invites = new List<Invite>
            {
                new Invite
                {
                    Email = "loafer101@aaa.bg",
                    Purpose = "Project 1",
                    IsInvited = false,
                    RequestExpiredOn = DateTime.UtcNow,
                },
                new Invite
                {
                    Email = "loafer102@aaa.bg",
                    Purpose = "Project 2",
                    IsInvited = false,
                    RequestExpiredOn = DateTime.UtcNow.AddDays(1),
                },
                new Invite
                {
                    Email = "loafer103@aaa.bg",
                    Purpose = "Project 3",
                    IsInvited = true,
                    ExpiredOn = DateTime.UtcNow.AddDays(2),
                    RequestExpiredOn = DateTime.UtcNow.AddDays(2),
                },
            };

            await dbContext.Invites.AddRangeAsync(invites);
        }
    }
}
