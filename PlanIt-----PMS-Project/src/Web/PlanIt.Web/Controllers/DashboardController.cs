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
        private readonly IAdditionalCostsService additionalCostsService;
        private readonly IProblemsService problemsService;

        public DashboardController(
            UserManager<PlanItUser> userManager,
            IInvitesService invitesService,
            IUsersService usersService,
            IClientsServices clientsServices,
            IProjectsService projectsService,
            IAdditionalCostsService additionalCostsService,
            IProblemsService problemsService)
        {
            this.userManager = userManager;
            this.invitesService = invitesService;
            this.usersService = usersService;
            this.clientsServices = clientsServices;
            this.projectsService = projectsService;
            this.additionalCostsService = additionalCostsService;
            this.problemsService = problemsService;
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
            }

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName) ||
                this.User.IsInRole(GlobalConstants.AdministratorRoleName))
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
            }

            //if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName) ||
            //    this.User.IsInRole(GlobalConstants.ProjectManagerRoleName))
            //{
            //    var managementViewModel = new IndexManagementViewModel
            //    {
            //        InvitesCount = this.invitesService.GetAllCount(),
            //        InvitesApprovedCount = this.invitesService.GetAllApprovedCount(),
            //        InvitesInvitedExpiredOnCount = this.invitesService.GetAllInvitedExpiredOnCount(),
            //        InvitesRequestExpiredOnCount = this.invitesService.GetAllRequestExpiredOnCount(),
            //        UsersWithDeletedCount = this.usersService.GetAllWithDeletedCount(),
            //        UsersCount = this.usersService.GetAllCount(),
            //        UsersDeletedCount = this.usersService.GetAllDeletedCount(),
            //        ClientsCount = this.clientsServices.GetAllCount(),
            //    };

            //    viewModel.ManagementDashboard = managementViewModel;
            //}

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName) ||
                this.User.IsInRole(GlobalConstants.ProjectManagerRoleName) ||
                this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                var userViewModel = new IndexUserViewModel
                {
                    MyProjectsCount = this.projectsService.AllCountByManagerId(currentUserId),
                    MyCompletedProjectsCount = this.projectsService.AllCompletedCountByManagerId(currentUserId),
                    MyTasksCount = this.problemsService.AllTasksCountByUserId(currentUserId),
                    MyCompletedTasksCount = this.problemsService.AllCompletedTasksCountByUserId(currentUserId),
                    TotalBudgetMyProjects = this.projectsService.TotalBudgetByManagerId(currentUserId),
                    TotalBudgetMyCompletedProjects = this.projectsService
                    .CompletedTotalBudgetByManagerId(currentUserId),
                    SumAdditionalCostsMyCompletedProjects = this.additionalCostsService
                    .SumAdditionalCostsCompletedProjectsByManagerId(currentUserId),
                    SumCostsMyCompletedProjects = this.problemsService
                    .SumCostsCompleteProjectsByManagerId(currentUserId),
                };

                viewModel.UserDashboard = userViewModel;
            }

            return this.View(viewModel);
        }
    }
}
