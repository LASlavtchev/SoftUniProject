namespace PlanIt.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IUsersService usersService;

        public UsersController(
            UserManager<PlanItUser> userManager,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public async Task<ActionResult> All()
        {
            var users = await this.usersService.GetAllAsync<UserAdminViewModel>(this.User);
            var deletedUsers = await this.usersService.GetAllDeletedAsync<UserAdminViewModel>();

            var model = new UsersListAdminViewModel
            {
                Users = users,
            };

            return this.View(model);
        }

        public async Task<ActionResult> Deleted()
        {
            var deletedUsers = await this.usersService.GetAllDeletedAsync<UserAdminViewModel>();

            var model = new UsersListAdminViewModel
            {
                Users = deletedUsers,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Restore(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.usersService.GetDeletedByIdAsync<UserAdminDeleteViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersService.RestoreAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<ActionResult> SoftDelete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.usersService.GetByIdAsync<UserAdminDeleteViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersService.SoftDeleteAsync(id);

            return this.RedirectToAction(nameof(this.Deleted));
        }

        [HttpPost]
        public async Task<ActionResult> HardDelete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.usersService.GetByIdAsync<UserAdminDeleteViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersService.SoftDeleteAsync(id);

            return this.RedirectToAction(nameof(this.Deleted));
        }
    }
}
