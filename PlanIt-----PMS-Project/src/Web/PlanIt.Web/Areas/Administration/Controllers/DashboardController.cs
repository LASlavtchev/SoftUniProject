namespace PlanIt.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly UserManager<PlanItUser> usermanager;
        private readonly IInvitesService invitesService;
        private readonly IUsersService usersService;
        private readonly IClientsServices clientsServices;

        public DashboardController(
            UserManager<PlanItUser> usermanager,
            IInvitesService invitesService,
            IUsersService usersService,
            IClientsServices clientsServices)
        {
            this.usermanager = usermanager;
            this.invitesService = invitesService;
            this.usersService = usersService;
            this.clientsServices = clientsServices;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
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

            return this.View(viewModel);
        }
    }
}
