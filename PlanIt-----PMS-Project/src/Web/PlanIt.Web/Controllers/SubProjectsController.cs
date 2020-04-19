namespace PlanIt.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.Infrastructure.Filters;
    using PlanIt.Web.ViewModels.SubProjects;

    [Authorize]
    [TypeFilter(typeof(MyProjectsOnlyAttribute))]
    public class SubProjectsController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly ISubProjectsService subProjectsService;
        private readonly ISubProjectTypesService subProjectTypesService;
        private readonly IProjectsService projectsService;
        private readonly IProgressStatusesService progressStatusesService;

        public SubProjectsController(
            UserManager<PlanItUser> userManager,
            ISubProjectsService subProjectsService,
            ISubProjectTypesService subProjectTypesService,
            IProjectsService projectsService,
            IProgressStatusesService progressStatusesService)
        {
            this.userManager = userManager;
            this.subProjectsService = subProjectsService;
            this.subProjectTypesService = subProjectTypesService;
            this.projectsService = projectsService;
            this.progressStatusesService = progressStatusesService;
        }

        public async Task<IActionResult> Create()
        {
            var currentUserId = this.userManager.GetUserId(this.User);

            var projects = new List<SubProjectProjectViewModel>();

            if (this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                projects = (List<SubProjectProjectViewModel>)await this.projectsService
                .GetAllByManagerIdAsync<SubProjectProjectViewModel>(currentUserId);
            }
            else
            {
                projects = (List<SubProjectProjectViewModel>)await this.projectsService
               .GetAllAsync<SubProjectProjectViewModel>();
            }

            var subProjectTypes = await this.subProjectTypesService
                .GetAllAsync<SubProjectSubProjectTypeViewModel>();

            this.ViewData["My Projects"] = projects;
            this.ViewData["Types"] = subProjectTypes;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubProjectCreateInputModel inputModel)
        {
            var project = await this.projectsService
                .GetByIdAsync<SubProjectProjectViewModel>(inputModel.ProjectId);

            if (project == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project does not exist");
            }

            if (project.DueDate != null)
            {
                if (project.DueDate < inputModel.DueDate?.ToUniversalTime() || project.StartDate > inputModel.DueDate?.ToUniversalTime())
                {
                    this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date or after start date");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var currentUserId = this.userManager.GetUserId(this.User);

                var projects = new List<SubProjectProjectViewModel>();

                if (this.User.IsInRole(GlobalConstants.UserRoleName))
                {
                    projects = (List<SubProjectProjectViewModel>)await this.projectsService
                    .GetAllByManagerIdAsync<SubProjectProjectViewModel>(currentUserId);
                }
                else
                {
                    projects = (List<SubProjectProjectViewModel>)await this.projectsService
                   .GetAllAsync<SubProjectProjectViewModel>();
                }

                var subProjectTypes = await this.subProjectTypesService
                .GetAllAsync<SubProjectSubProjectTypeViewModel>();

                this.ViewData["My Projects"] = projects;
                this.ViewData["Types"] = subProjectTypes;

                return this.View(inputModel);
            }

            await this.subProjectsService.CreateAsync<SubProjectCreateInputModel>(inputModel);
            await this.projectsService.CalculateProjectBudget(project.Id);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subProject = await this.subProjectsService.GetByIdAsync<SubProjectEditInputModel>(id);

            if (subProject == null)
            {
                return this.NotFound();
            }

            var statuses = await this.progressStatusesService
                .GetAllAsync<SubProjectProgressStatusViewModel>();

            this.ViewData["Statuses"] = statuses;

            return this.View(subProject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SubProjectEditInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            var project = await this.projectsService
                .GetByIdAsync<SubProjectProjectViewModel>(inputModel.ProjectId);

            if (project == null)
            {
                return this.NotFound();
            }

            if (project.DueDate != null)
            {
                if (project.DueDate < inputModel.DueDate?.ToUniversalTime() || project.StartDate > inputModel.DueDate?.ToUniversalTime())
                {
                    this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date or after start date");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var statuses = await this.progressStatusesService
               .GetAllAsync<SubProjectProgressStatusViewModel>();

                this.ViewData["Statuses"] = statuses;

                return this.View(inputModel);
            }

            await this.subProjectsService.EditAsync<SubProjectEditInputModel>(inputModel);
            await this.projectsService.CalculateProjectBudget(project.Id);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        }

        public async Task<IActionResult> Add(int id)
        {
            var project = await this.projectsService
                .GetByIdAsync<SubProjectProjectViewModel>(id);

            if (project == null)
            {
                return this.NotFound();
            }

            var subProjectTypes = await this.subProjectTypesService
                .GetAllAsync<SubProjectSubProjectTypeViewModel>();

            this.ViewData["Types"] = subProjectTypes;

            var subproject = new SubProjectAddInputModel
            {
                ProjectId = project.Id,
            };

            return this.View(subproject);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubProjectAddInputModel inputModel)
        {
            var project = await this.projectsService
                .GetByIdAsync<SubProjectProjectViewModel>(inputModel.ProjectId);

            if (project == null)
            {
                this.ModelState.AddModelError(string.Empty, "Project does not exist");
            }

            if (project.DueDate != null)
            {
                if (project.DueDate < inputModel.DueDate?.ToUniversalTime() || project.StartDate > inputModel.DueDate?.ToUniversalTime())
                {
                    this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date or after start date");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var subProjectTypes = await this.subProjectTypesService
                .GetAllAsync<SubProjectSubProjectTypeViewModel>();

                this.ViewData["Types"] = subProjectTypes;

                return this.View(inputModel);
            }

            await this.subProjectsService.CreateAsync<SubProjectAddInputModel>(inputModel);
            await this.projectsService.CalculateProjectBudget(project.Id);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var subProject = await this.subProjectsService
                .GetByIdAsync<SubProjectDeleteViewModel>(id);

            if (subProject == null)
            {
                this.NotFound();
            }

            await this.subProjectsService.DeleteAsync(id);
            await this.projectsService.CalculateProjectBudget(subProject.ProjectId);

            return this.RedirectToAction("Details", "Projects", new { area = "Management", id = subProject.ProjectId });
        }
    }
}
