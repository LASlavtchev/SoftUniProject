namespace PlanIt.Services.CronJobs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data;

    public class InviteDbCleanupJob : IInviteDbCleanupJob
    {
        private readonly InvitesDbContext context;

        public InviteDbCleanupJob(InvitesDbContext context)
        {
            this.context = context;
        }

        public async Task Work()
        {
            var invites = await this.context
                .Invites
                .Where(i => i.RequestExpiredOn < DateTime.UtcNow || i.ExpiredOn < DateTime.UtcNow)
                .ToListAsync();

            this.context.RemoveRange(invites);
            await this.context.SaveChangesAsync();
        }
    }
}
