namespace PlanIt.Services.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProblemsService : IProblemsService
    {
        private readonly IDeletableEntityRepository<Problem> problemsRepository;
        private readonly IProgressStatusesService progressStatusesService;

        public ProblemsService(
            IDeletableEntityRepository<Problem> problemsRepository,
            IProgressStatusesService progressStatusesService)
        {
            this.problemsRepository = problemsRepository;
            this.progressStatusesService = progressStatusesService;
        }

        public async Task<Problem> AssignAsync<TInputModel>(PlanItUser user, TInputModel model)
        {
            var input = AutoMapperConfig.MapperInstance.Map<Problem>(model);

            var problem = new Problem
            {
                Name = input.Name,
                Instructions = input.Instructions,
                SubProjectId = input.SubProjectId,
                DueDate = input.DueDate?.ToUniversalTime(),
                HourlyRate = input.HourlyRate,
            };

            problem.ProgressStatus = await this.progressStatusesService
                .GetByNameAsync(GlobalConstants.ProgressStatusAssigned);

            var userProblem = new UserProblem
            {
                User = user,
                Problem = problem,
            };

            problem.UserProblems.Add(userProblem);

            await this.problemsRepository.AddAsync(problem);
            await this.problemsRepository.SaveChangesAsync();

            return problem;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var problems = await this.problemsRepository
                .All()
                .Where(t => t.SubProject.Project.IsDeleted == false)
                .OrderBy(t => t.SubProject.Project.Name)
                .ThenBy(t => t.SubProject.SubProjectType.Name)
                .To<TViewModel>()
                .ToListAsync();

            return problems;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByUserIdAsync<TViewModel>(string userId)
        {
            var problems = await this.problemsRepository
                .All()
                .Where(t => t.SubProject.Project.IsDeleted == false)
                .Where(t => t.UserProblems.Select(up => up.PlanItUserId).Contains(userId))
                .OrderBy(t => t.SubProject.Project.Name)
                .ThenBy(t => t.SubProject.SubProjectType.Name)
                .To<TViewModel>()
                .ToListAsync();

            return problems;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByManagerIdAsync<TViewModel>(string userId)
        {
            var problems = await this.problemsRepository
                .All()
                .Where(t => t.SubProject.Project.IsDeleted == false)
                .Where(t => t.SubProject.Project.ProjectManagerId == userId)
                .OrderBy(t => t.SubProject.Project.Name)
                .ThenBy(t => t.SubProject.SubProjectType.Name)
                .To<TViewModel>()
                .ToListAsync();

            return problems;
        }

        public async Task<Problem> GetByIdAsync(int taskId)
        {
            var problem = await this.problemsRepository
                .All()
                .Where(t => t.Id == taskId)
                .FirstOrDefaultAsync();

            return problem;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int taskId)
        {
            var problem = await this.problemsRepository
                .All()
                .Where(t => t.Id == taskId)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return problem;
        }

        public async Task<Problem> AddHoursAsync(int taskId, decimal hours)
        {
            var problem = await this.problemsRepository
               .All()
               .Where(t => t.Id == taskId)
               .FirstOrDefaultAsync();

            problem.TotalHours += hours;

            this.problemsRepository.Update(problem);
            await this.problemsRepository.SaveChangesAsync();

            return problem;
        }

        public async Task<Problem> AddUserAsync(int taskId, PlanItUser user)
        {
            var problem = await this.problemsRepository
               .All()
               .Where(t => t.Id == taskId)
               .FirstOrDefaultAsync();

            var userProblem = new UserProblem
            {
                User = user,
                Problem = problem,
            };

            problem.UserProblems.Add(userProblem);

            this.problemsRepository.Update(problem);
            await this.problemsRepository.SaveChangesAsync();

            return problem;
        }

        public async Task<Problem> ChangeStatusAsync(int taskId, ProgressStatus progressStatus)
        {
            var problem = await this.problemsRepository
               .All()
               .Where(t => t.Id == taskId)
               .FirstOrDefaultAsync();

            problem.ProgressStatus = progressStatus;

            this.problemsRepository.Update(problem);
            await this.problemsRepository.SaveChangesAsync();

            return problem;
        }
    }
}
