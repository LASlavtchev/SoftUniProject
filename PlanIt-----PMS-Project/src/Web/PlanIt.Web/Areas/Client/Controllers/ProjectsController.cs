﻿namespace PlanIt.Web.Areas.Client.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Client.Projects;

    public class ProjectsController : ClientsController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IProjectsService projectService;
        private readonly IProgressStatusesService progressStatusesService;
        private readonly IClientsServices clientsServices;

        public ProjectsController(
            UserManager<PlanItUser> userManager,
            IProjectsService projectService,
            IProgressStatusesService progressStatusesService,
            IClientsServices clientsServices)
        {
            this.userManager = userManager;
            this.projectService = projectService;
            this.progressStatusesService = progressStatusesService;
            this.clientsServices = clientsServices;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var clientId = await this.clientsServices.GetClientIdByUserIdAsync(currentUser.Id);

            var allProjects = new ProjectsListViewModel
            {
                ApprovedProjects = await this.projectService.GetAllApprovedByClientIdAsync<ProjectViewModel>(clientId),
                NotApprovedProjects = await this.projectService.GetAllNotApprovedByClientIdAsync<ProjectViewModel>(clientId),
            };

            return this.View(allProjects);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var clientId = await this.clientsServices.GetClientIdByUserIdAsync(currentUser.Id);

            inputModel.ClientId = clientId;

            await this.projectService.CreateByClientAsync<ProjectCreateInputModel>(inputModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await this.projectService.GetByIdAsync<ProjectDetailsViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            return this.View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var project = await this.projectService.GetByIdAsync<ProjectViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            await this.projectService.ApproveAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await this.projectService.GetByIdAsync<ProjectViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            await this.projectService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await this.projectService.GetByIdAsync<ProjectEditInputModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

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
                return this.View(inputModel);
            }

            var project = await this.projectService.EditAsync<ProjectEditInputModel>(inputModel);

            return this.RedirectToAction(nameof(this.Details), new { project.Id });
        }
    }
}
