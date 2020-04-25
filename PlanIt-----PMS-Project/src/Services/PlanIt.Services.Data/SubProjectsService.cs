namespace PlanIt.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
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

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int subProjectId)
        {
            var subProject = await this.subProjectsRepository
                .All()
                .Where(sp => sp.Id == subProjectId)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return subProject;
        }

        public async Task<SubProject> EditAsync<TInputModel>(TInputModel inputModel)
        {
            var subProjectInputModel = AutoMapperConfig.MapperInstance.Map<SubProject>(inputModel);

            var subProject = await this.subProjectsRepository
                .All()
                .Where(sp => sp.Id == subProjectInputModel.Id)
                .FirstOrDefaultAsync();

            subProject.Budget = subProjectInputModel.Budget;
            subProject.DueDate = subProjectInputModel.DueDate?.ToUniversalTime();
            subProject.ProgressStatusId = subProjectInputModel.ProgressStatusId;

            this.subProjectsRepository.Update(subProject);
            await this.subProjectsRepository.SaveChangesAsync();

            return subProject;
        }

        public async Task DeleteAsync(int subProjectId)
        {
            var subProject = await this.subProjectsRepository
                .All()
                .Where(sp => sp.Id == subProjectId)
                .FirstOrDefaultAsync();

            this.subProjectsRepository.Delete(subProject);
            await this.subProjectsRepository.SaveChangesAsync();
        }

        public async Task<SubProject> ChangeStatusAsync(int subProjectId, ProgressStatus progressStatus)
        {
            var subProject = await this.subProjectsRepository
                .All()
                .Where(sp => sp.Id == subProjectId)
                .Include(sp => sp.Problems)
                .Include(p => p.ProgressStatus)
                .FirstOrDefaultAsync();

            bool isCompleted = true;

            foreach (var problem in subProject.Problems)
            {
                if (problem.ProgressStatus.Name != GlobalConstants.ProgressStatusCompleted)
                {
                    isCompleted = false;
                    break;
                }
            }

            if (isCompleted)
            {
                subProject.ProgressStatus = progressStatus;

                this.subProjectsRepository.Update(subProject);
                await this.subProjectsRepository.SaveChangesAsync();
            }

            return subProject;
        }
    }
}
