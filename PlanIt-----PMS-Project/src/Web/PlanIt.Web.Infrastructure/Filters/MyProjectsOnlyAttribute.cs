namespace PlanIt.Web.Infrastructure.Filters
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;

    public class MyProjectsOnlyAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IProjectsService projectsService;

        public MyProjectsOnlyAttribute(
            UserManager<PlanItUser> userManager,
            IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.projectsService = projectsService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser = context.HttpContext.User;
            var currentUserId = this.userManager.GetUserId(currentUser);

            var projects = this.projectsService.GetAllByManagerId(currentUserId);

            if (currentUser.IsInRole(GlobalConstants.UserRoleName) && projects.Count() == 0)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
