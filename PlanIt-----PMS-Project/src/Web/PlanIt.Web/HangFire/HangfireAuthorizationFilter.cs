namespace PlanIt.Web.HangFire
{
    using Hangfire.Dashboard;
    using PlanIt.Common;

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
                   httpContext.User.IsInRole(GlobalConstants.CompanyOwnerRoleName);
        }
    }
}
