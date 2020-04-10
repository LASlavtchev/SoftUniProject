namespace PlanIt.Web.Areas.Client.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Common;
    using PlanIt.Web.Controllers;

    [Authorize(Roles = GlobalConstants.ClientRoleName)]
    [Area("Client")]
    public class ClientsController : BaseController
    {
    }
}
