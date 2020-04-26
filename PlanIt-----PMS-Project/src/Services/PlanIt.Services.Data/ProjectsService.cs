namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectsService : IProjectsService
    {
        private readonly IDeletableEntityRepository<Project> projectsRepository;
        private readonly IProgressStatusesService progressStatusesService;

        public ProjectsService(
            IDeletableEntityRepository<Project> projectsRepository,
            IProgressStatusesService progressStatusesService)
        {
            this.projectsRepository = projectsRepository;
            this.progressStatusesService = progressStatusesService;
        }

        public int AllCount()
        {
            var allCount = this.projectsRepository
                .All()
                .Count();

            return allCount;
        }

        public int AllApprovedCount()
        {
            var allCount = this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved)
                .Count();

            return allCount;
        }

        public int AllCompletedCount()
        {
            var allCount = this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved && p.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                .Count();

            return allCount;
        }

        public int AllCountByManagerId(string userId)
        {
            var count = this.projectsRepository
                .All()
                .Where(p => p.ProjectManagerId == userId)
                .Count();

            return count;
        }

        public int AllCompletedCountByManagerId(string userId)
        {
            var count = this.projectsRepository
                .All()
                .Where(p => p.ProjectManagerId == userId &&
                       p.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                .Count();

            return count;
        }

        public decimal TotalBudgetByManagerId(string userId)
        {
            var budget = this.projectsRepository
                .All()
                .Where(p => p.ProjectManagerId == userId)
                .Select(p => p.Budget)
                .Sum();

            return budget;
        }

        public decimal CompletedTotalBudgetByManagerId(string userId)
        {
            var budget = this.projectsRepository
                .All()
                .Where(p => p.ProjectManagerId == userId &&
                       p.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                .Select(p => p.Budget)
                .Sum();

            return budget;
        }

        public decimal TotalBudgetApproved()
        {
            var budget = this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved)
                .Select(p => p.Budget)
                .Sum();

            return budget;
        }

        public decimal TotalBudgetCompleted()
        {
            var budget = this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved && p.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                .Select(p => p.Budget)
                .Sum();

            return budget;
        }

        public decimal TotalCostsCompleted()
        {
            var budget = this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved && p.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                .Select(p => new
                {
                    AdditionalCosts = p.AdditionalCosts.Select(ac => ac.TotalCost).Sum(),
                    Costs = p.SubProjects.SelectMany(sp => sp.Problems).Select(x => x.HourlyRate * x.TotalHours).Sum(),
                })
                .ToList()
                .Select(a => a.AdditionalCosts + a.Costs)
                .Sum();

            return budget;
        }

        public int AllCountByClientId(int clientId)
        {
            var allCount = this.projectsRepository
                .All()
                .Where(p => p.ClientId == clientId)
                .Count();

            return allCount;
        }

        public int AllApprovedCountByClientId(int clientId)
        {
            var allCount = this.projectsRepository
                .All()
                .Where(p => p.ClientId == clientId && p.IsBudgetApproved)
                .Count();

            return allCount;
        }

        public int AllNotApprovedCountByClientId(int clientId)
        {
            var allCount = this.projectsRepository
                .All()
                .Where(p => p.ClientId == clientId && !p.IsBudgetApproved)
                .Count();

            return allCount;
        }

        public decimal CalculateApprovedProjectsBudgetByClientId(int clientId)
        {
            var sumBudget = this.projectsRepository
                .All()
                .Where(p => p.ClientId == clientId && p.IsBudgetApproved)
                .Select(p => p.Budget)
                .Sum();

            return sumBudget;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var projects = await this.projectsRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return projects;
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
                .Where(p => p.ProjectManagerId == managerId)
                .To<TViewModel>()
                .ToListAsync();

            return managerProjects;
        }

        public IEnumerable<Project> GetAllByManagerId(string managerId)
        {
            var managerProjects = this.projectsRepository
                .All()
                .Where(p => p.ProjectManagerId == managerId)
                .ToList();

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

        public async Task<Project> GetByIdAsync(int projectId)
        {
            var project = await this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<TViewModel> GetDeletedByIdAsync<TViewModel>(int projectId)
        {
            var project = await this.projectsRepository
                .AllWithDeleted()
                .Where(p => p.Id == projectId && p.IsDeleted)
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
                ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusNotAssigned),
            };

            project.Budget = this.SumBudgetBySubProjectsBudget(project);

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> CreateByManagerAsync<TInputModel>(TInputModel inputModel)
        {
            var model = AutoMapperConfig.MapperInstance.Map<Project>(inputModel);

            var project = new Project
            {
                Name = model.Name,
                StartDate = model.StartDate?.ToUniversalTime(),
                DueDate = model.DueDate?.ToUniversalTime(),
                ClientId = model.ClientId,
                ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusNotAssigned),
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> ApproveAsync(int projectId)
        {
            var project = this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .Include(p => p.SubProjects)
                .ThenInclude(sp => sp.Problems)
                .FirstOrDefault();

            project.IsBudgetApproved = true;
            project.StartDate = DateTime.UtcNow;

            if (project.DueDate == null)
            {
                project.DueDate = project.ClientDueDate;
            }
            else if (project.DueDate > project.ClientDueDate)
            {
                project.ClientDueDate = (DateTime)project.DueDate;
            }

            project.ClientBudget = project.Budget;
            project.ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusInProgress);

            foreach (var subProject in project.SubProjects)
            {
                subProject.DueDate = project.DueDate;
                subProject.ProgressStatus = project.ProgressStatus;

                foreach (var problem in subProject.Problems)
                {
                    problem.DueDate = project.DueDate;
                    problem.ProgressStatus = project.ProgressStatus;
                }
            }

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> AssignManagerAsync(int projectId, string projectManagerId)
        {
            var project = this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .FirstOrDefault();

            project.ProjectManagerId = projectManagerId;
            project.ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusAssigned);

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

            project.ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusCanceled);

            this.projectsRepository.Delete(project);
            await this.projectsRepository.SaveChangesAsync();
        }

        public async Task RestoreAsync(int projectId)
        {
            var project = this.projectsRepository
                .AllWithDeleted()
                .Where(p => p.Id == projectId && p.IsDeleted)
                .FirstOrDefault();

            project.ProgressStatus = await this.progressStatusesService.GetByNameAsync(GlobalConstants.ProgressStatusSuspended);

            this.projectsRepository.Undelete(project);
            await this.projectsRepository.SaveChangesAsync();
        }

        public async Task<Project> EditByClientAsync<TInputModel>(TInputModel inputModel)
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

        public async Task<Project> EditByManagerAsync<TInputModel>(TInputModel inputModel)
        {
            var inputProject = AutoMapperConfig.MapperInstance.Map<Project>(inputModel);

            var project = await this.projectsRepository
                .All()
                .Where(i => i.Id == inputProject.Id)
                .FirstOrDefaultAsync();

            if (project.StartDate != inputProject.StartDate?.ToUniversalTime() ||
                project.DueDate != inputProject.DueDate?.ToUniversalTime())
            {
                project.IsBudgetApproved = false;
                project.ProgressStatus = await this.progressStatusesService
                    .GetByNameAsync(GlobalConstants.ProgressStatusSuspended);
            }

            if (inputProject.ProjectManagerId != null)
            {
                project.ProjectManagerId = inputProject.ProjectManagerId;
                project.ProgressStatusId = inputProject.ProgressStatusId;
            }

            project.Name = inputProject.Name;
            project.StartDate = inputProject.StartDate?.ToUniversalTime();
            project.DueDate = inputProject.DueDate?.ToUniversalTime();

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<IEnumerable<TViewModel>> GetAllApprovedAsync<TViewModel>()
        {
            var projects = await this.projectsRepository
                .All()
                .Where(p => p.IsBudgetApproved)
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<TViewModel>> GetAllNotApprovedAsync<TViewModel>()
        {
            var projects = await this.projectsRepository
                .All()
                .Where(p => !p.IsBudgetApproved)
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<TViewModel>> GetAllDeletedAsync<TViewModel>()
        {
            var projects = await this.projectsRepository
                .AllWithDeleted()
                .Where(p => p.IsDeleted && !p.Client.IsDeleted)
                .To<TViewModel>()
                .ToListAsync();

            return projects;
        }

        public async Task<Project> CalculateProjectBudget(int projectId)
        {
            var project = await this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .Include(p => p.SubProjects)
                .FirstOrDefaultAsync();

            project.Budget = this.SumBudgetBySubProjectsBudget(project);

            if (project.IsBudgetApproved)
            {
                project.IsBudgetApproved = false;
                project.ProgressStatus = await this.progressStatusesService
                    .GetByNameAsync(GlobalConstants.ProgressStatusSuspended);
            }

            this.projectsRepository.Update(project);
            await this.projectsRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> ChangeStatusAsync(int projectId, ProgressStatus progressStatus)
        {
            var project = await this.projectsRepository
                .All()
                .Where(p => p.Id == projectId)
                .Include(p => p.SubProjects)
                .Include(sp => sp.ProgressStatus)
                .FirstOrDefaultAsync();

            bool isCompleted = true;

            foreach (var subProject in project.SubProjects)
            {
                if (subProject.ProgressStatus.Name != GlobalConstants.ProgressStatusCompleted)
                {
                    isCompleted = false;
                    break;
                }
            }

            if (isCompleted)
            {
                project.ProgressStatus = progressStatus;

                this.projectsRepository.Update(project);
                await this.projectsRepository.SaveChangesAsync();
            }

            return project;
        }

        private decimal SumBudgetBySubProjectsBudget(Project project)
        {
            var sum = project.SubProjects
                .Where(sp => sp.IsDeleted == false)
                .Select(sp => sp.Budget)
                .Sum();

            return sum;
        }
    }
}
