namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class SubProjectsService : ISubProjectsService
    {
        private readonly IDeletableEntityRepository<SubProject> subProjectsRepository;
        private readonly IProjectsService projectsService;

        public SubProjectsService(
            IDeletableEntityRepository<SubProject> subProjectsRepository,
            IProjectsService projectsService)
        {
            this.subProjectsRepository = subProjectsRepository;
            this.projectsService = projectsService;
        }

        public async Task<SubProject> CreateAsync<TInputModel>(TInputModel inputModel)
        {
            var inputSubProject = AutoMapperConfig.MapperInstance.Map<SubProject>(inputModel);
            var project = await this.projectsService.GetByIdAsync(inputSubProject.ProjectId);

            var subProject = new SubProject
            {
                SubProjectTypeId = inputSubProject.SubProjectTypeId,
                ProjectId = inputSubProject.ProjectId,
                Budget = inputSubProject.Budget,
                ProgressStatusId = project.ProgressStatusId,
            };

            if (inputSubProject.DueDate != null)
            {
                subProject.DueDate = inputSubProject.DueDate?.ToUniversalTime();
            }
            else
            {
                subProject.DueDate = project.DueDate;
            }

            await this.subProjectsRepository.AddAsync(subProject);
            await this.subProjectsRepository.SaveChangesAsync();

            return subProject;
        }
    }
}
