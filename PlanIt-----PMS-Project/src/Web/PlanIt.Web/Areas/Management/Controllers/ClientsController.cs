namespace PlanIt.Web.Areas.Management.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ClientsController : ManagementController
    {
        private readonly IClientsServices clientsService;
        private readonly IProjectsService projectsService;

        public ClientsController(
            IClientsServices clientsService,
            IProjectsService projectsService)
        {
            this.clientsService = clientsService;
            this.projectsService = projectsService;
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        public async Task<IActionResult> Index()
        {
            var clients = await this.clientsService.GetAllAsync<ClientClientViewModel>();

            return this.View(clients);
        }

        [Authorize(Roles = GlobalConstants.ManagementRoleNames)]
        public async Task<IActionResult> Projects(int id)
        {
            var projects = await this.projectsService.GetAllByClientIdAsync<ClientProjectViewModel>(id);
            return this.View(projects);
        }
    }
}
