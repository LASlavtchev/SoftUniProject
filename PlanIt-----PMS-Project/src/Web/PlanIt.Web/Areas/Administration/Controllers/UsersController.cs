namespace PlanIt.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<PlanItUser> userManager;

        public UsersController(
            UserManager<PlanItUser> userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            var users = this.userManager.Users;

            return this.View(users);
        }
    }
}
