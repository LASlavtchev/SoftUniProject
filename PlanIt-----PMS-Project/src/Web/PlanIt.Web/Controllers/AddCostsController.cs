namespace PlanIt.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.Infrastructure.Filters;
    using PlanIt.Web.ViewModels.AdditionalCosts;

    [Authorize]
    [TypeFilter(typeof(MyProjectsOnlyAttribute))]
    public class AddCostsController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IAdditionalCostsService additionalCostsService;
        private readonly IProjectsService projectsService;
        private readonly ISubProjectsService subProjectsService;

        public AddCostsController(
            UserManager<PlanItUser> userManager,
            IAdditionalCostsService additionalCostsService,
            IProjectsService projectsService,
            ISubProjectsService subProjectsService)
        {
            this.userManager = userManager;
            this.additionalCostsService = additionalCostsService;
            this.projectsService = projectsService;
            this.subProjectsService = subProjectsService;
        }

        public async Task<IActionResult> AddToSubProject(int? id)
        {
            var projects = await this.GetProjects(this.User);
            this.ViewData["Projects"] = projects;

            if (id == null)
            {
                return this.View();
            }

            var projectId = (int)id;
            var selectedProject = await this.projectsService
                .GetByIdAsync<AddCostProjectViewModel>(projectId);

            this.ViewData["Project"] = selectedProject;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToSubProject(AddCostAddToSubProjectInputModel inputModel)
        {
            if (inputModel.SubProjectId == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project part doesn`t exist!");
            }

            var selectedProject = await this.projectsService
                    .GetByIdAsync<AddCostProjectViewModel>(inputModel.ProjectId);

            if (selectedProject == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project doesn`t exist!");
            }

            if (!this.ModelState.IsValid)
            {
                var projects = await this.GetProjects(this.User);

                this.ViewData["Projects"] = projects;
                this.ViewData["Project"] = selectedProject;

                return this.View(inputModel);
            }

            await this.additionalCostsService.AddAsync<AddCostAddToSubProjectInputModel>(inputModel);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = selectedProject.Id });
        }

        public async Task<IActionResult> AddToProject()
        {
            var projects = await this.GetProjects(this.User);
            this.ViewData["Projects"] = projects;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToProject(AddCostAddToProjectInputModel inputModel)
        {
            var project = await this.projectsService
                    .GetByIdAsync<AddCostProjectViewModel>(inputModel.ProjectId);

            if (project == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project doesn`t exist!");
            }

            if (!this.ModelState.IsValid)
            {
                var projects = await this.GetProjects(this.User);

                this.ViewData["Projects"] = projects;

                return this.View(inputModel);
            }

            await this.additionalCostsService.AddAsync<AddCostAddToProjectInputModel>(inputModel);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        }

        public async Task<IActionResult> CostsByProject(int projectId)
        {
            var projectCosts = await this.projectsService
                .GetByIdAsync<AddCostDetailsProjectViewModel>(projectId);

            if (projectCosts == null)
            {
                return this.NotFound();
            }

            return this.View(projectCosts);
        }

        public async Task<IActionResult> CostsBySubProject(int subProjectId)
        {
            var subProject = await this.subProjectsService
                .GetByIdAsync<AddCostDetailsSubProjectViewModel>(subProjectId);

            if (subProject == null)
            {
                return this.NotFound();
            }

            return this.View(subProject);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cost = await this.additionalCostsService.GetByIdAsync<AddCostEditInputModel>(id);

            if (cost == null)
            {
                return this.NotFound();
            }

            return this.View(cost);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddCostEditInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            var cost = await this.additionalCostsService
                .GetByIdAsync<AddCostViewModel>(inputModel.Id);

            if (cost == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.additionalCostsService.EditAsync<AddCostEditInputModel>(inputModel);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = cost.ProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cost = await this.additionalCostsService
                .GetByIdAsync<AddCostDeleteViewModel>(id);

            if (cost == null)
            {
                this.NotFound();
            }

            await this.additionalCostsService.DeleteAsync(id);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = cost.ProjectId });
        }

        private async Task<IEnumerable<AddCostSelectProjectViewModel>> GetProjects(ClaimsPrincipal principal)
        {
            var currentUserId = this.userManager.GetUserId(principal);

            var projects = new List<AddCostSelectProjectViewModel>();

            if (this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                projects = (List<AddCostSelectProjectViewModel>)await this.projectsService
                .GetAllByManagerIdAsync<AddCostSelectProjectViewModel>(currentUserId);
            }
            else
            {
                projects = (List<AddCostSelectProjectViewModel>)await this.projectsService
               .GetAllAsync<AddCostSelectProjectViewModel>();
            }

            return projects;
        }
    }
}
