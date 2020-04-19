namespace PlanIt.Services.Data
{
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class AdditionalCostsService : IAdditionalCostsService
    {
        private readonly IDeletableEntityRepository<AdditionalCost> additionalCostsRepository;

        public AdditionalCostsService(IDeletableEntityRepository<AdditionalCost> additionalCostsRepository)
        {
            this.additionalCostsRepository = additionalCostsRepository;
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
    }
}
