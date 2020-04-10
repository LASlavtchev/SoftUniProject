using PlanIt.Data.Common.Repositories;
using PlanIt.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanIt.Services.Data
{
    public class SubProjectsService : ISubProjectsService
    {
        private readonly IDeletableEntityRepository<SubProject> subProjectsRepository;

        public SubProjectsService(IDeletableEntityRepository<SubProject> subProjectsRepository)
        {
            this.subProjectsRepository = subProjectsRepository;
        }

        
    }
}
