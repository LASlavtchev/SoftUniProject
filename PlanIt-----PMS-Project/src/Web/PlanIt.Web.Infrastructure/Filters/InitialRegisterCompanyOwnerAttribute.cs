namespace PlanIt.Web.Infrastructure.Filters
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PlanIt.Data.Models;

    public class InitialRegisterCompanyOwnerAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserManager<PlanItUser> userManager;

        public InitialRegisterCompanyOwnerAttribute(UserManager<PlanItUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var existingUsers = this.userManager.Users;

            if (existingUsers.Any())
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
