namespace PlanIt.Web.Controllers
{
    using System;
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
    using PlanIt.Web.ViewModels.SubProjects;

    [Authorize]
    [TypeFilter(typeof(MyProjectsOnlyAttribute))]
    public class AddCostsController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IAdditionalCostsService additionalCostsService;
        private readonly IProjectsService projectsService;

        public AddCostsController(
            UserManager<PlanItUser> userManager,
            IAdditionalCostsService additionalCostsService,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.additionalCostsService = additionalCostsService;
            this.projectsService = projectsService;
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

        public async Task<IActionResult> DetailsByProject(int projectId)
        {
            //var subProject = await this.subProjectsService.GetByIdAsync<SubProjectEditInputModel>(id);

            //if (subProject == null)
            //{
            //    return this.NotFound();
            //}

            //var statuses = await this.progressStatusesService
            //    .GetAllAsync<SubProjectProgressStatusViewModel>();

            //this.ViewData["Statuses"] = statuses;

            return this.View();
        }






        //public async Task<IActionResult> Edit(int id)
        //{
        //    var subProject = await this.subProjectsService.GetByIdAsync<SubProjectEditInputModel>(id);

        //    if (subProject == null)
        //    {
        //        return this.NotFound();
        //    }

        //    var statuses = await this.progressStatusesService
        //        .GetAllAsync<SubProjectProgressStatusViewModel>();

        //    this.ViewData["Statuses"] = statuses;

        //    return this.View(subProject);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, SubProjectEditInputModel inputModel)
        //{
        //    if (id != inputModel.Id)
        //    {
        //        return this.NotFound();
        //    }

        //    var project = await this.projectsService
        //        .GetByIdAsync<SubProjectProjectViewModel>(inputModel.ProjectId);

        //    if (project == null)
        //    {
        //        return this.NotFound();
        //    }

        //    if (project.DueDate != null)
        //    {
        //        if (project.DueDate < inputModel.DueDate?.ToUniversalTime() || project.StartDate > inputModel.DueDate?.ToUniversalTime())
        //        {
        //            this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date or after start date");
        //        }
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        var statuses = await this.progressStatusesService
        //       .GetAllAsync<SubProjectProgressStatusViewModel>();

        //        this.ViewData["Statuses"] = statuses;

        //        return this.View(inputModel);
        //    }

        //    await this.subProjectsService.EditAsync<SubProjectEditInputModel>(inputModel);
        //    await this.projectsService.CalculateProjectBudget(project.Id);

        //    return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        //}



        //[HttpPost]
        //public async Task<IActionResult> Add(SubProjectAddInputModel inputModel)
        //{
        //    var project = await this.projectsService
        //        .GetByIdAsync<SubProjectProjectViewModel>(inputModel.ProjectId);

        //    if (project == null)
        //    {
        //        this.ModelState.AddModelError(string.Empty, "Project does not exist");
        //    }

        //    if (project.DueDate != null)
        //    {
        //        if (project.DueDate < inputModel.DueDate?.ToUniversalTime() || project.StartDate > inputModel.DueDate?.ToUniversalTime())
        //        {
        //            this.ModelState.AddModelError(string.Empty, "SubProject due date have to be before project due date or after start date");
        //        }
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        var subProjectTypes = await this.subProjectTypesService
        //        .GetAllAsync<SubProjectSubProjectTypeViewModel>();

        //        this.ViewData["Types"] = subProjectTypes;

        //        return this.View(inputModel);
        //    }

        //    await this.subProjectsService.CreateAsync<SubProjectAddInputModel>(inputModel);
        //    await this.projectsService.CalculateProjectBudget(project.Id);

        //    return this.RedirectToAction("Details", "Projects", new { area = "Management", id = project.Id });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var subProject = await this.subProjectsService
        //        .GetByIdAsync<SubProjectDeleteViewModel>(id);

        //    if (subProject == null)
        //    {
        //        this.NotFound();
        //    }

        //    await this.subProjectsService.DeleteAsync(id);
        //    await this.projectsService.CalculateProjectBudget(subProject.ProjectId);

        //    return this.RedirectToAction("Details", "Projects", new { area = "Management", id = subProject.ProjectId });
        //}

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
