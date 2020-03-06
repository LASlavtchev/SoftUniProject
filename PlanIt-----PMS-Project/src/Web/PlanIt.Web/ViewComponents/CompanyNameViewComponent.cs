namespace PlanIt.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.Company;

    public class CompanyNameViewComponent : ViewComponent
    {
        private readonly UserManager<PlanItUser> userManager;

        public CompanyNameViewComponent(UserManager<PlanItUser> userManager)
        {
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var founder = this.userManager.Users
                .FirstOrDefault(u => u.JobTitle == "founder");

            var viewModel = new CompanyNameViewModel();

            if (founder == null)
            {
                viewModel.Name = "Company Name";
            }
            else
            {
                var founderClaims = this.userManager.GetClaimsAsync(founder).GetAwaiter().GetResult();

                var companyName = founderClaims.FirstOrDefault(c => c.Type == "CompanyName").Value;

                viewModel.Name = companyName;
            }

            return this.View(viewModel);
        }
    }
}
