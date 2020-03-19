namespace PlanIt.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Data.Repositories;
    using PlanIt.Services.Data;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Invites;

#pragma warning disable SA1649 // File name should match first type name
    public class InviteModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly ILogger<InviteModel> logger;
        private readonly IInvitesService invitesServices;

        public InviteModel(
            UserManager<PlanItUser> userManager,
            ILogger<InviteModel> logger,
            IInvitesService invitesServices)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.invitesServices = invitesServices;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/Identity/Account/Login");

            var existingUser = await this.userManager
                .Users
                .SingleOrDefaultAsync(u => u.Email == this.Input.Email);

            var existingInvite = await this.invitesServices
                .GetByEmailAsync<InviteUserCreateViewModel>(this.Input.Email);

            if (existingUser != null)
            {
                this.ModelState.AddModelError(string.Empty, $"User with email {existingUser.Email} already exists!!!");
            }

            if (existingInvite != null)
            {
                this.ModelState.AddModelError(string.Empty, $"Invite with email {existingInvite.Email} already exists!!!");
            }

            if (this.ModelState.IsValid)
            {
                var invite = this.invitesServices.CreateInviteByUserAsync(this.Input.Email, this.Input.Purpose);

                return this.LocalRedirect(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel : IMapTo<Invite>
        {
            [Required]
            [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthString,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthString)]
            public string Purpose { get; set; }
        }
    }
}
