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
        private readonly ISettingsService settingsService;
        private readonly IInvitesService invitesService;

        public DashboardController(
            UserManager<PlanItUser> usermanager,
            ISettingsService settingsService,
            IInvitesService invitesService)
        {
            this.usermanager = usermanager;
            this.settingsService = settingsService;
            this.invitesService = invitesService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                Invites = await this.invitesService.GetAllAsync<InviteViewModel>(),
                SettingsCount = this.settingsService.GetCount(), 
            };

            return this.View(viewModel);
        }
    }
}
