namespace PlanIt.Web.Areas.Management.Controllers
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
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.ProgressStatuses;
    using PlanIt.Web.ViewModels.Management.Projects;

    public class ProjectsController : ManagementController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly RoleManager<PlanItRole> roleManager;
        private readonly IClientsServices clientsServices;
        private readonly IProjectsService projectsService;
        private readonly IProgressStatusesService progressStatusesService;

        public ProjectsController(
            UserManager<PlanItUser> userManager,
            RoleManager<PlanItRole> roleManager,
            IClientsServices clientsServices,
            IProjectsService projectsService,
            IProgressStatusesService progressStatusesService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.clientsServices = clientsServices;
            this.projectsService = projectsService;
            this.progressStatusesService = progressStatusesService;
        }

        public async Task<IActionResult> MyProjects()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var projects = await this.projectsService
                .GetAllByManagerIdAsync<ProjectMyProjectViewModel>(currentUser.Id);

            return this.View(projects);
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        public async Task<IActionResult> Index()
        {
            var projects = new ProjectsListViewModel
            {
                ApprovedProjects = await this.projectsService.GetAllApprovedAsync<ProjectViewModel>(),
                NotApprovedProjects = await this.projectsService.GetAllNotApprovedAsync<ProjectViewModel>(),
                DeletedProjects = await this.projectsService.GetAllDeletedAsync<ProjectViewModel>(),
            };

            return this.View(projects);
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        public async Task<IActionResult> Create()
        {
            var clients = await this.clientsServices.GetAllAsync<ProjectCreateClientViewModel>();
            this.ViewData["Clients"] = clients;

            return this.View();
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        [HttpPost]
        public async Task<ActionResult> Create(ProjectCreateInputModel inputModel)
        {
            var client = await this.clientsServices
                .GetClientByIdAsync<ProjectCreateClientViewModel>(inputModel.ClientId);

            if (client == null)
            {
                this.ModelState.AddModelError(string.Empty, "Client does not exist");
            }

            if (!this.ModelState.IsValid)
            {
                var clients = await this.clientsServices.GetAllAsync<ProjectCreateClientViewModel>();
                this.ViewData["Clients"] = clients;

                return this.View(inputModel);
            }

            await this.projectsService.CreateByManagerAsync<ProjectCreateInputModel>(inputModel);
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await this.projectsService.GetByIdAsync<ProjectDetailsViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            return this.View(project);
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        public async Task<IActionResult> Assign(int id)
        {
            var project = await this.projectsService.GetByIdAsync<ProjectAssignInputModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            var selectedUsers = await this.SelectUsersAsync(this.User);
            this.ViewData["Users"] = selectedUsers;

            return this.View();
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        [HttpPost]
        public async Task<IActionResult> Assign(int id, ProjectAssignInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            var project = await this.projectsService.AssignManagerAsync(id, inputModel.ProjectManagerId);

            return this.RedirectToAction(nameof(this.Details), new { project.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await this.projectsService.GetByIdAsync<ProjectEditInputModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            var selectedUsers = await this.SelectUsersAsync(this.User);
            var statuses = await this.progressStatusesService.GetAllAsync<ProjectProgressStatusViewModel>();

            this.ViewData["Users"] = selectedUsers;
            this.ViewData["Statuses"] = statuses;

            return this.View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProjectEditInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                var selectedUsers = await this.SelectUsersAsync(this.User);
                var statuses = await this.progressStatusesService.GetAllAsync<ProjectProgressStatusViewModel>();

                this.ViewData["Users"] = selectedUsers;
                this.ViewData["Statuses"] = statuses;

                return this.View(inputModel);
            }

            var project = await this.projectsService.EditByManagerAsync<ProjectEditInputModel>(inputModel);

            return this.RedirectToAction(nameof(this.Details), new { project.Id });
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await this.projectsService.GetByIdAsync<ProjectViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            await this.projectsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var project = await this.projectsService.GetDeletedByIdAsync<ProjectViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            await this.projectsService.RestoreAsync(id);

            return this.RedirectToAction(nameof(this.Index));
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
                .Where(u => !u.Roles.Select(r => r.RoleId).Contains(clientRole.Id));

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
