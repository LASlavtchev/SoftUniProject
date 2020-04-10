namespace PlanIt.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Administration.Roles;
    using PlanIt.Web.ViewModels.Administration.Users;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly RoleManager<PlanItRole> roleManager;
        private readonly IUsersService usersService;
        private readonly IClientsServices clientsService;

        public UsersController(
            UserManager<PlanItUser> userManager,
            RoleManager<PlanItRole> roleManager,
            IUsersService usersService,
            IClientsServices clientsService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.usersService = usersService;
            this.clientsService = clientsService;
        }

        public async Task<IActionResult> All()
        {
            var roles = new List<PlanItRole>();
            var companyOwnerRole = await this.roleManager
                .FindByNameAsync(GlobalConstants.CompanyOwnerRoleName);

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName))
            {
                roles = this.roleManager.Roles.ToList();
            }
            else
            {
                roles = this.roleManager
                    .Roles.Where(r => r.Name != GlobalConstants.CompanyOwnerRoleName)
                    .ToList();
            }

            this.ViewData["Roles"] = roles.Select(role => new RoleAddToRoleViewModel
            {
                Name = role.Name,
            });

            var users = await this.usersService.GetAllAsync<UserViewModel>(this.User);
            await this.AddRolesToUsers(users);

            var model = new UsersListViewModel
            {
                Users = users,
            };

            this.ViewData["Users"] = model;

            return this.View();
        }

        public async Task<IActionResult> Deleted()
        {
            var deletedUsers = await this.usersService.GetAllDeletedAsync<UserViewModel>();

            if (deletedUsers.Count() > 0)
            {
                await this.AddRolesToUsers(deletedUsers);
            }

            var model = new UsersListViewModel
            {
                Users = deletedUsers,
            };

            return this.View(model);
        }

        public async Task<IActionResult> AddToRoleAsync()
        {
            var currentUser = await this.userManager
                .GetUserAsync(this.User);

            var roles = new List<PlanItRole>();
            var users = new List<PlanItUser>();
            var companyOwnerRole = await this.roleManager
                .FindByNameAsync(GlobalConstants.CompanyOwnerRoleName);

            if (this.User.IsInRole(GlobalConstants.CompanyOwnerRoleName))
            {
                roles = this.roleManager.Roles.ToList();
                users = this.userManager.Users.Where(u => u.Id != currentUser.Id).ToList();
            }
            else
            {
                roles = this.roleManager
                    .Roles.Where(r => r.Name != GlobalConstants.CompanyOwnerRoleName)
                    .ToList();
                users = this.userManager
                    .Users
                    .Where(u => u.Id != currentUser.Id && !u.Roles.Select(r => r.RoleId).Contains(companyOwnerRole.Id))
                    .ToList();
            }

            this.ViewData["Roles"] = roles.Select(role => new RoleAddToRoleViewModel
            {
                Name = role.Name,
            });

            this.ViewData["Users"] = users.Select(user => new UserAddToRoleViewModel
            {
                Id = user.Id,
                Email = user.Email,
            });

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToRoleAsync(UserAddToRoleInputModel inputModel)
        {
            var isRoleExist = await this.roleManager.RoleExistsAsync(inputModel.Role);

            if (!isRoleExist)
            {
                this.ModelState.AddModelError(string.Empty, "Role does not exist!");
            }

            var existingUser = await this.userManager.FindByIdAsync(inputModel.UserId);

            if (existingUser == null)
            {
                this.ModelState.AddModelError(string.Empty, "User does not exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.All));
            }

            var currentUserRoles = await this.userManager.GetRolesAsync(existingUser);
            var currentUserRole = currentUserRoles.Single();

            if (inputModel.Role == currentUserRole)
            {
                return this.RedirectToAction(nameof(this.All));
            }

            var removeResult = await this.userManager.RemoveFromRoleAsync(existingUser, currentUserRole);

            if (currentUserRole == GlobalConstants.ClientRoleName && removeResult.Succeeded)
            {
                await this.clientsService.DeleteAsync(inputModel.UserId);
            }

            var addResult = await this.userManager.AddToRoleAsync(existingUser, inputModel.Role);

            if (inputModel.Role == GlobalConstants.ClientRoleName && addResult.Succeeded)
            {
                if (!this.clientsService.IsDeletedClientExist(inputModel.UserId))
                {
                    await this.clientsService.CreateAsync(inputModel.UserId);
                }
                else
                {
                    await this.clientsService.RestoreAsync(inputModel.UserId);
                }
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.usersService.GetDeletedByIdAsync<UserViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersService.RestoreAsync(id);

            var userRole = await this.roleManager.FindByIdAsync(user.Roles.Single().RoleId);

            if (userRole.Name == GlobalConstants.ClientRoleName)
            {
                await this.clientsService.RestoreAsync(user.Id);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersService.DeleteAsync(id);

            var userRoles = await this.userManager.GetRolesAsync(user);
            var userRole = userRoles.Single();

            if (userRole == GlobalConstants.ClientRoleName)
            {
                await this.clientsService.DeleteAsync(user.Id);
            }

            return this.RedirectToAction(nameof(this.Deleted));
        }

        private async Task AddRolesToUsers(IEnumerable<UserViewModel> users)
        {
            foreach (var user in users)
            {
                var roleId = user.Roles
                    .Select(role => role.RoleId)
                    .Single();

                user.RoleName = await this.roleManager
                    .Roles
                    .Where(r => r.Id == roleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
