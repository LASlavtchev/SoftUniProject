namespace PlanIt.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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

        public SubProjectsController(
            UserManager<PlanItUser> userManager,
            ISubProjectsService subProjectsService,
            ISubProjectTypesService subProjectTypesService,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.subProjectsService = subProjectsService;
            this.subProjectTypesService = subProjectTypesService;
            this.projectsService = projectsService;
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

            if (project.DueDate != null && project.DueDate < inputModel.DueDate?.ToUniversalTime())
            {
                this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date");
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



    }
}
