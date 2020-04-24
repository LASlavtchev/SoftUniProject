namespace PlanIt.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Dashboard;

    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IInvitesService invitesService;
        private readonly IUsersService usersService;
        private readonly IClientsServices clientsServices;
        private readonly IProjectsService projectsService;

        public DashboardController(
            UserManager<PlanItUser> userManager,
            IInvitesService invitesService,
            IUsersService usersService,
            IClientsServices clientsServices,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.invitesService = invitesService;
            this.usersService = usersService;
            this.clientsServices = clientsServices;
            this.projectsService = projectsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();

            var currentUserId = this.userManager.GetUserId(this.User);

            if (this.User.IsInRole(GlobalConstants.ClientRoleName))
            {
                var currentClientId = await this.clientsServices.GetClientIdByUserIdAsync(currentUserId);

                var clientViewModel = new IndexClientViewModel
                {
                    ProjectsCount = this.projectsService.AllCountByClientId(currentClientId),
                    ProjectsApprovedCount = this.projectsService.AllApprovedCountByClientId(currentClientId),
                    ProjectsNotApprovedCount = this.projectsService.AllNotApprovedCountByClientId(currentClientId),
                    TotalBudget = this.projectsService.CalculateApprovedProjectsBudgetByClientId(currentClientId),
                };

                viewModel.ClientDashboard = clientViewModel;

                return this.View(viewModel);
            }

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName) || this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName))
            {
                var administrationViewModel = new IndexAdministrationViewModel
                {
                    InvitesCount = this.invitesService.GetAllCount(),
                    InvitesApprovedCount = this.invitesService.GetAllApprovedCount(),
                    InvitesInvitedExpiredOnCount = this.invitesService.GetAllInvitedExpiredOnCount(),
                    InvitesRequestExpiredOnCount = this.invitesService.GetAllRequestExpiredOnCount(),
                    UsersWithDeletedCount = this.usersService.GetAllWithDeletedCount(),
                    UsersCount = this.usersService.GetAllCount(),
                    UsersDeletedCount = this.usersService.GetAllDeletedCount(),
                    ClientsCount = this.clientsServices.GetAllCount(),
                };

                viewModel.AdministrationDashboard = administrationViewModel;

                return this.View(viewModel);
            }

            return this.View(viewModel);

        }
    }
}
