namespace PlanIt.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Web.Controllers;

    [Authorize(Roles = GlobalConstants.CompanyOwnerRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
