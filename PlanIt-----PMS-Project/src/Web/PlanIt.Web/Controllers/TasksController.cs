﻿namespace PlanIt.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.Infrastructure.Filters;
    using PlanIt.Web.ViewModels.Tasks;
    using PlanIt.Web.ViewModels.Tasks.ProgressStatuses;
    using PlanIt.Web.ViewModels.Tasks.Projects;
    using PlanIt.Web.ViewModels.Tasks.SubProjects;

    [Authorize]
    public class TasksController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly RoleManager<PlanItRole> roleManager;
        private readonly IProjectsService projectsService;
        private readonly ISubProjectsService subProjectsService;
        private readonly IProblemsService problemsService;
        private readonly IHoursService hoursService;
        private readonly IProgressStatusesService progressStatusesService;

        public TasksController(
            UserManager<PlanItUser> userManager,
            RoleManager<PlanItRole> roleManager,
            IProjectsService projectsService,
            ISubProjectsService subProjectsService,
            IProblemsService problemsService,
            IHoursService hoursService,
            IProgressStatusesService progressStatusesService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.projectsService = projectsService;
            this.subProjectsService = subProjectsService;
            this.problemsService = problemsService;
            this.hoursService = hoursService;
            this.progressStatusesService = progressStatusesService;
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> Index()
        {
            var problems = Enumerable.Empty<TaskIndexViewModel>();

            if (this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                var currentUserId = this.userManager.GetUserId(this.User);

                problems = await this.problemsService.GetAllByManagerIdAsync<TaskIndexViewModel>(currentUserId);
            }
            else
            {
                problems = await this.problemsService.GetAllAsync<TaskIndexViewModel>();
            }

            return this.View(problems);
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> TaskHours(int id)
        {
            var problem = await this.problemsService.GetByIdAsync<TaskTaskHoursViewModel>(id);

            if (problem == null)
            {
                return this.NotFound();
            }

            return this.View(problem);
        }

        public async Task<IActionResult> MyTasks()
        {
            var currentUserId = this.userManager.GetUserId(this.User);

            var problems = await this.problemsService
                .GetAllByUserIdAsync<TaskMyTaskViewModel>(currentUserId);

            return this.View(problems);
        }

        public async Task<IActionResult> MyTaskHours(int id)
        {
            var problem = await this.problemsService
                .GetByIdAsync<TaskMyTaskHoursViewModel>(id);

            if (problem == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.userManager.GetUserId(this.User);

            problem.Hours = problem.Hours.Where(h => h.UserId == currentUserId);

            return this.View(problem);
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> Assign(int? id)
        {
            var projects = await this.GetProjects(this.User);
            this.ViewData["Projects"] = projects;

            if (id == null)
            {
                return this.View();
            }

            var projectId = (int)id;
            var selectedProject = await this.projectsService
                .GetByIdAsync<TaskAssignProjectViewModel>(projectId);
            var selectUsers = await this.SelectUsersAsync(this.User);

            this.ViewData["Project"] = selectedProject;
            this.ViewData["Users"] = selectUsers;

            return this.View();
        }

        [HttpPost]
        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> Assign(TaskAssignInputModel inputModel)
        {
            var subProject = await this.subProjectsService
                .GetByIdAsync<TaskAssignSubProjectViewModel>(inputModel.SubProjectId);

            if (subProject == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project part doesn`t exist!");
            }

            var user = await this.userManager.FindByIdAsync(inputModel.UserId);

            if (user == null)
            {
                this.ModelState.AddModelError(string.Empty, "User is not assigned!");
            }

            var selectedProject = await this.projectsService
                    .GetByIdAsync<TaskAssignProjectViewModel>(inputModel.ProjectId);

            if (selectedProject == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project doesn`t exist!");
            }

            if (inputModel.DueDate != null)
            {
                if (inputModel.DueDate?.ToUniversalTime() > subProject.DueDate)
                {
                    this.ModelState.AddModelError(string.Empty, $"Due date has to be before project part Due date");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var projects = await this.GetProjects(this.User);
                var selectUsers = await this.SelectUsersAsync(this.User);

                this.ViewData["Projects"] = projects;
                this.ViewData["Project"] = selectedProject;
                this.ViewData["Users"] = selectUsers;

                return this.View(inputModel);
            }

            await this.problemsService.AssignAsync<TaskAssignInputModel>(user, inputModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddHours(int id, decimal hours)
        {
            var problem = await this.problemsService.GetByIdAsync(id);

            if (problem == null)
            {
                return this.NotFound();
            }

            if (hours <= 0)
            {
                return this.RedirectToAction(nameof(this.MyTasks));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.hoursService.AddHourAsync(problem, user, hours);
            await this.problemsService.AddHoursAsync(id, hours);

            return this.RedirectToAction(nameof(this.MyTasks));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int problemId, string status)
        {
            var problem = await this.problemsService.GetByIdAsync(problemId);

            if (problem == null)
            {
                return this.NotFound();
            }

            var progressStatus = await this.progressStatusesService.GetByNameAsync(status);

            if (progressStatus == null)
            {
                return this.NotFound();
            }

            await this.problemsService.ChangeStatusAsync(problemId, progressStatus);

            if (status == GlobalConstants.ProgressStatusCompleted)
            {
                var subProject = await this.subProjectsService.ChangeStatusAsync(problem.SubProjectId, progressStatus);

                if (subProject.ProgressStatus.Name == GlobalConstants.ProgressStatusCompleted)
                {
                    await this.projectsService.ChangeStatusAsync(subProject.ProjectId, progressStatus);
                }
            }

            return this.RedirectToAction(nameof(this.MyTasks));
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> AddUser(int id)
        {
            var problem = await this.problemsService.GetByIdAsync<TaskAddUserInputModel>(id);

            if (problem == null)
            {
                return this.NotFound();
            }

            var selectUsers = await this.SelectUsersAsync(this.User);

            this.ViewData["Users"] = selectUsers;

            return this.View(problem);
        }

        [HttpPost]
        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> AddUser(int id, TaskAddUserInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                this.ModelState.AddModelError(string.Empty, "Task doesn`t exist!!!");
            }

            var selectedUser = await this.userManager.FindByIdAsync(inputModel.UserId);

            if (selectedUser == null)
            {
                this.ModelState.AddModelError(string.Empty, "User doesn`t exist!!!");
            }

            if (!this.ModelState.IsValid)
            {
                var selectUsers = await this.SelectUsersAsync(this.User);

                this.ViewData["Users"] = selectUsers;

                return this.View(inputModel);
            }

            await this.problemsService.AddUserAsync(id, selectedUser);

            return this.RedirectToAction(nameof(this.Index));
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> Edit(int id)
        {
            var problem = await this.problemsService.GetByIdAsync<TaskEditInputModel>(id);

            if (problem == null)
            {
                return this.NotFound();
            }

            var statuses = await this.progressStatusesService.GetAllAsync<TaskEditProgressStatusViewModel>();
            this.ViewData["Statuses"] = statuses;

            return this.View(problem);
        }

        [HttpPost]
        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> Edit(int id, TaskEditInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            var subProject = await this.subProjectsService
                .GetByIdAsync<TaskEditSubProjectViewModel>(inputModel.SubProjectId);

            if (subProject == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project part doesn`t exist!");
            }

            if (inputModel.DueDate != null)
            {
                if (inputModel.DueDate?.ToUniversalTime() > subProject.DueDate)
                {
                    this.ModelState.AddModelError(string.Empty, $"Due date has to be before project part Due date");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var statuses = await this.progressStatusesService.GetAllAsync<TaskEditProgressStatusViewModel>();
                this.ViewData["Statuses"] = statuses;

                return this.View(inputModel);
            }

            await this.problemsService.EditAsync<TaskEditInputModel>(inputModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        private async Task<IEnumerable<TaskSelectProjectViewModel>> GetProjects(ClaimsPrincipal principal)
        {
            var currentUserId = this.userManager.GetUserId(principal);

            var projects = new List<TaskSelectProjectViewModel>();

            if (principal.IsInRole(GlobalConstants.UserRoleName))
            {
                projects = (List<TaskSelectProjectViewModel>)await this.projectsService
                .GetAllByManagerIdAsync<TaskSelectProjectViewModel>(currentUserId);
            }
            else
            {
                projects = (List<TaskSelectProjectViewModel>)await this.projectsService
               .GetAllAsync<TaskSelectProjectViewModel>();
            }

            return projects;
        }

        private async Task<List<PlanItUser>> SelectUsersAsync(ClaimsPrincipal user)
        {
            var currentUser = await this.userManager.GetUserAsync(user);

            var userRole = await this.roleManager.FindByNameAsync(GlobalConstants.UserRoleName);
            var adminRole = await this.roleManager.FindByNameAsync(GlobalConstants.AdministratorRoleName);
            var companyOwnerRole = await this.roleManager.FindByNameAsync(GlobalConstants.CompanyOwnerRoleName);
            var clientRole = await this.roleManager.FindByNameAsync(GlobalConstants.ClientRoleName);

            var users = this.userManager
                .Users
                .Where(u => !u.Roles.Select(r => r.RoleId).Contains(clientRole.Id) &&
                            !u.Roles.Select(r => r.RoleId).Contains(adminRole.Id));

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName))
            {
                users = users.Where(u => !u.Roles.Select(r => r.RoleId).Contains(companyOwnerRole.Id));
            }
            else if (this.User.IsInRole(GlobalConstants.ProjectManagerRoleName))
            {
                users = users.Where(u => u.Roles.Select(r => r.RoleId).Contains(userRole.Id));
            }

            var selectedUsers = users.ToList();
            selectedUsers.Add(currentUser);

            return selectedUsers;
        }
    }
}
