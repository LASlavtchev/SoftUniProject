namespace PlanIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    internal class ProgressStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(PlanItDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProgressStatuses.Any())
            {
                return;
            }

            await dbContext.ProgressStatuses.AddRangeAsync(
                new ProgressStatus { Name = "Not Assigned" },
                new ProgressStatus { Name = "In Progress" },
                new ProgressStatus { Name = "Completed" },
                new ProgressStatus { Name = "Suspended" },
                new ProgressStatus { Name = "Approving" },
                new ProgressStatus { Name = "Canceled" });
        }
    }
}
