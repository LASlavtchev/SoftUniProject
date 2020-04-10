using PlanIt.Data.Common.Repositories;
using PlanIt.Data.Models;
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

        public Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            throw new NotImplementedException();
        }
    }
}
