namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProgressStatusesService : IProgressStatusesService
    {
        private readonly IDeletableEntityRepository<ProgressStatus> progressStatusesRepository;

        public ProgressStatusesService(IDeletableEntityRepository<ProgressStatus> progressStatusesRepository)
        {
            this.progressStatusesRepository = progressStatusesRepository;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var all = await this.progressStatusesRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return all;
        }

        public async Task<ProgressStatus> GetByNameAsync(string progressStatusName)
        {
            var status = await this.progressStatusesRepository
                .All()
                .Where(s => s.Name == progressStatusName)
                .FirstOrDefaultAsync();

            return status;
        }
    }
}
