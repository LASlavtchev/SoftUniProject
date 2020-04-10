namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectsService : IProjectsService
    {
        private readonly IDeletableEntityRepository<Project> projectsRepository;

        public ProjectsService(
            IDeletableEntityRepository<Project> projectsRepository)
        {
            this.projectsRepository = projectsRepository;
        }

        public int AllCount()
        {
            var allCount = this.projectsRepository
                .All()
                .Count();

            return allCount;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByClientIdAsync<TViewModel>(int clientId)
        {
            var clientProjects = await this.projectsRepository
                .All()
                .Where(p => p.ClientId == clientId)
                .To<TViewModel>()
                .ToListAsync();

            return clientProjects;
        }

        public async Task<IEnumerable<TViewModel>> GetAllApprovedByClientIdAsync<TViewModel>(int clientId)
        {
            var projects = await this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved == true && p.ClientId == clientId)
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<TViewModel>> GetAllNotApprovedByClientIdAsync<TViewModel>(int clientId)
        {
            var projects = await this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved == false && p.ClientId == clientId)
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByManagerIdAsync<TViewModel>(string managerId)
        {
            var managerProjects = await this.projectsRepository
                .All()
                .Where(p => p.PlantItUserId == managerId)
                .To<TViewModel>()
                .ToListAsync();

            return managerProjects;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int projectId)
        {
            var project = await this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<Project> CreateByClientAsync<TInputModel>(TInputModel inputModel)
        {
            var model = AutoMapperConfig.MapperInstance.Map<Project>(inputModel);

            var project = new Project
            {
                Name = model.Name,
                ClientBudget = model.ClientBudget,
                ClientDueDate = model.ClientDueDate.ToUniversalTime(),
                ClientId = model.ClientId,
                IsBudgetApproved = false,
            };

            project.Budget = this.SumBudgetBySubProjectsBudget(project);

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> ApproveAsync(int projectId)
        {
            var project = this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .FirstOrDefault();

            project.IsBudgetApproved = true;
            project.StartDate = DateTime.UtcNow;

            if (project.DueDate == null || project.DueDate > project.ClientDueDate)
            {
                project.DueDate = project.ClientDueDate;
            }

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task DeleteAsync(int projectId)
        {
            var project = this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .FirstOrDefault();

            this.projectsRepository.Delete(project);
            await this.projectsRepository.SaveChangesAsync();
        }

        public async Task<Project> EditAsync<TInputModel>(TInputModel inputModel)
        {
            var inputProject = AutoMapperConfig.MapperInstance.Map<Project>(inputModel);

            var project = await this.projectsRepository
                .All()
                .Where(i => i.Id == inputProject.Id)
                .FirstOrDefaultAsync();

            project.Name = inputProject.Name;
            project.ClientDueDate = inputProject.ClientDueDate.ToUniversalTime();
            project.ClientBudget = inputProject.ClientBudget;

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var projects = await this.projectsRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        //public async Task<Project> CreateAsync(string userId)
        //{

        //}

        private decimal SumBudgetBySubProjectsBudget(Project project)
        {
            var sum = project.SubProjects
                .Select(sp => sp.Budget)
                .Sum();

            return sum;
        }
    }
}
