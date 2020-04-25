namespace PlanIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AdditionalCostsService : IAdditionalCostsService
    {
        private readonly IDeletableEntityRepository<AdditionalCost> additionalCostsRepository;

        public AdditionalCostsService(IDeletableEntityRepository<AdditionalCost> additionalCostsRepository)
        {
            this.additionalCostsRepository = additionalCostsRepository;
        }

        public decimal SumAdditionalCostsCompletedProjectsByManagerId(string userId)
        {
            var costs = this.additionalCostsRepository
                .All()
                .Where(ac => ac.Project.ProjectManagerId == userId &&
                             ac.Project.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted &&
                             ac.Project.IsDeleted == false)
                .Select(ac => ac.TotalCost)
                .Sum();

            return costs;
        }

        public async Task<AdditionalCost> AddAsync<TInputModel>(TInputModel inputModel)
        {
            var input = AutoMapperConfig.MapperInstance.Map<AdditionalCost>(inputModel);

            var additionalCost = new AdditionalCost
            {
                Name = input.Name,
                Description = input.Description,
                PricePerQuantity = input.PricePerQuantity,
                Quantity = input.Quantity,
                TotalCost = input.PricePerQuantity * input.Quantity,
                ProjectId = input.ProjectId,
                SubProjectId = input.SubProjectId,
            };

            await this.additionalCostsRepository.AddAsync(additionalCost);
            await this.additionalCostsRepository.SaveChangesAsync();

            return additionalCost;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int costId)
        {
            var cost = await this.additionalCostsRepository
                .All()
                .Where(c => c.Id == costId)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return cost;
        }

        public async Task<AdditionalCost> EditAsync<TInputModel>(TInputModel inputModel)
        {
            var input = AutoMapperConfig.MapperInstance.Map<AdditionalCost>(inputModel);

            var additionalCost = await this.additionalCostsRepository
                .All()
                .Where(ac => ac.Id == input.Id)
                .FirstOrDefaultAsync();

            additionalCost.Name = input.Name;
            additionalCost.Description = input.Description;
            additionalCost.PricePerQuantity = input.PricePerQuantity;
            additionalCost.Quantity = input.Quantity;
            additionalCost.TotalCost = input.PricePerQuantity * input.Quantity;

            this.additionalCostsRepository.Update(additionalCost);
            await this.additionalCostsRepository.SaveChangesAsync();

            return additionalCost;
        }

        public async Task DeleteAsync(int costId)
        {
            var cost = await this.additionalCostsRepository
                .All()
                .Where(c => c.Id == costId)
                .FirstOrDefaultAsync();

            this.additionalCostsRepository.HardDelete(cost);
            await this.additionalCostsRepository.SaveChangesAsync();
        }
    }
}
