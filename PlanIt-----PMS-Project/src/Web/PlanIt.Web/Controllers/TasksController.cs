namespace PlanIt.Web.Controllers
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
            var tasks = Enumerable.Empty<TaskIndexViewModel>();

            if (this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                var currentUserId = this.userManager.GetUserId(this.User);

                tasks = await this.problemsService.GetAllByManagerIdAsync<TaskIndexViewModel>(currentUserId);
            }
            else
            {
                tasks = await this.problemsService.GetAllAsync<TaskIndexViewModel>();
            }

            return this.View(tasks);
        }

        public async Task<IActionResult> MyTasks()
        {
            var currentUserId = this.userManager.GetUserId(this.User);

            var tasks = await this.problemsService
                .GetAllByUserIdAsync<TaskMyTaskViewModel>(currentUserId);

            return this.View(tasks);
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
                .GetByIdAsync<TaskProjectViewModel>(projectId);
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
                .GetByIdAsync<TaskProjectSubProjectViewModel>(inputModel.SubProjectId);

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
                    .GetByIdAsync<TaskProjectViewModel>(inputModel.ProjectId);

            if (selectedProject == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project doesn`t exist!");
            }

            if (inputModel.DueDate != null)
            {
                if (inputModel.DueDate > subProject.DueDate)
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
        public async Task<IActionResult> ChangeStatus(int taskId, string status)
        {
            var task = await this.problemsService.GetByIdAsync(taskId);

            if (task == null)
            {
                return this.NotFound();
            }

            var progressStatus = await this.progressStatusesService.GetByNameAsync(status);

            if (progressStatus == null)
            {
                return this.NotFound();
            }

            await this.problemsService.ChangeStatusAsync(taskId, progressStatus);

            return this.RedirectToAction(nameof(this.MyTasks));
        }

        [TypeFilter(typeof(MyProjectsOnlyAttribute))]
        public async Task<IActionResult> AddUser(int id)
        {
            var task = await this.problemsService.GetByIdAsync<TaskAddUserInputModel>(id);

            if (task == null)
            {
                return this.NotFound();
            }

            var selectUsers = await this.SelectUsersAsync(this.User);

            this.ViewData["Users"] = selectUsers;

            return this.View(task);
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
