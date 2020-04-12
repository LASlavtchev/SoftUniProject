namespace PlanIt.Web.Areas.Management.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.Projects;
    using PlanIt.Web.ViewModels.ProgressStatuses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProjectsController : ManagementController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly RoleManager<PlanItRole> roleManager;
        private readonly IClientsServices clientsServices;
        private readonly IProjectsService projectsService;

        public ProjectsController(
            UserManager<PlanItUser> userManager,
            RoleManager<PlanItRole> roleManager,
            IClientsServices clientsServices,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.clientsServices = clientsServices;
            this.projectsService = projectsService;
        }

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

        public async Task<IActionResult> Create()
        {
            var clients = await this.clientsServices.GetAllAsync<ProjectCreateClientViewModel>();
            this.ViewData["Clients"] = clients;

            return this.View();
        }

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

        public async Task<IActionResult> Assign(int id)
        {
            var project = await this.projectsService.GetByIdAsync<ProjectAssignInputModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

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
            else if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                users = users.Where(u => !u.Roles.Select(r => r.RoleId).Contains(adminRole.Id) &&
                                         !u.Roles.Select(r => r.RoleId).Contains(companyOwnerRole.Id));
            }
            else if (this.User.IsInRole(GlobalConstants.ProjectManagerRoleName))
            {
                users = users.Where(u => u.Roles.Select(r => r.RoleId).Contains(userRole.Id));
            }

            var selectedUsers = users.ToList();
            selectedUsers.Add(currentUser);

            this.ViewData["Users"] = selectedUsers;

            return this.View();
        }

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
    }
}
