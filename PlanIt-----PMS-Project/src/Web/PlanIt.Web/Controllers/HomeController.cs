namespace PlanIt.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly UserManager<PlanItUser> userManager;

        public HomeController(UserManager<PlanItUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = this.userManager.Users;

            if (!users.Any())
            {
                return this.Redirect("/Identity/Account/InitialRegisterCompanyOwner");
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Identity/Account/Login");
            }

            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
