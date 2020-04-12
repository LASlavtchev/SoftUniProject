namespace PlanIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PlanIt.Common;
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
                new ProgressStatus { Name = GlobalConstants.ProgressStatusNotAssigned },
                new ProgressStatus { Name = GlobalConstants.ProgressStatusInProgress },
                new ProgressStatus { Name = GlobalConstants.ProgressStatusCompleted },
                new ProgressStatus { Name = GlobalConstants.ProgressStatusSuspended },
                new ProgressStatus { Name = GlobalConstants.ProgressStatusApproving },
                new ProgressStatus { Name = GlobalConstants.ProgressStatusCanceled });
        }
    }
}
