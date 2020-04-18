using Microsoft.EntityFrameworkCore;
using PlanIt.Data.Common.Repositories;
using PlanIt.Data.Models;
using PlanIt.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanIt.Services.Data
{
    public class SubProjectTypesService : ISubProjectTypesService
    {
        private readonly IDeletableEntityRepository<SubProjectType> subProjectTypesServiceRepository;

        public SubProjectTypesService(IDeletableEntityRepository<SubProjectType> subProjectTypesServiceRepository)
        {
            this.subProjectTypesServiceRepository = subProjectTypesServiceRepository;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var types = await this.subProjectTypesServiceRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return types;
        }
    }
}
