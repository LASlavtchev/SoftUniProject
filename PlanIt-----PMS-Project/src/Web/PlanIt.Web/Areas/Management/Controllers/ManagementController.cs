namespace PlanIt.Web.Areas.Management.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Web.Controllers;

    [Authorize(Roles = GlobalConstants.ManagementAreaRoleNames)]
    [Area("Management")]
    public class ManagementController : BaseController
    {
    }
}