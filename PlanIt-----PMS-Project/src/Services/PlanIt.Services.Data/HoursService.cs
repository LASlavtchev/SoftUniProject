namespace PlanIt.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;

    public class HoursService : IHoursService
    {
        private readonly IDeletableEntityRepository<Hour> hoursRepository;

        public HoursService(IDeletableEntityRepository<Hour> hoursRepository)
        {
            this.hoursRepository = hoursRepository;
        }

        public async Task<Hour> AddHour(Problem problem, PlanItUser user, decimal hours)
        {
            var hour = new Hour
            {
                Problem = problem,
                User = user,
                WorkedHours = hours,
                Date = DateTime.UtcNow,
            };

            await this.hoursRepository.AddAsync(hour);
            await this.hoursRepository.SaveChangesAsync();

            return hour;
        }
    }
}
