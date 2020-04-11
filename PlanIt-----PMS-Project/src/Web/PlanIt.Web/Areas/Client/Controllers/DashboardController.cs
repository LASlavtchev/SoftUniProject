namespace PlanIt.Web.Areas.Clients.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.Areas.Client.Controllers;
    using PlanIt.Web.ViewModels.Client.Dashboard;

    public class DashboardController : ClientsController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IClientsServices clientsServices;
        private readonly IProjectsService projectsService;

        public DashboardController(
            UserManager<PlanItUser> userManager,
            IClientsServices clientsServices,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.clientsServices = clientsServices;
            this.projectsService = projectsService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            var currentClientId = await this.clientsServices.GetClientIdByUserIdAsync(currentUserId);

            var viewModel = new IndexViewModel
            {
                ProjectsCount = this.projectsService.AllCountByClientId(currentClientId),
                ProjectsApprovedCount = this.projectsService.AllApprovedCountByClientId(currentClientId),
                ProjectsNotApprovedCount = this.projectsService.AllNotApprovedCountByClientId(currentClientId),
                TotalBudget = this.projectsService.CalculateApprovedProjectsBudgetByClientId(currentClientId),
            };

            return this.View(viewModel);
        }
    }
}
