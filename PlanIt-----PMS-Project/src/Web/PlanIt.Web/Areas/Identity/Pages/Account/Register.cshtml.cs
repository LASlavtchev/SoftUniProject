namespace PlanIt.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Services.Messaging;
    using PlanIt.Web.ViewModels.Invites;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<PlanItUser> signInManager;
        private readonly UserManager<PlanItUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IInvitesService invitesService;

        public RegisterModel(
            UserManager<PlanItUser> userManager,
            SignInManager<PlanItUser> signInManager,
            ILogger<RegisterModel> logger,
            IInvitesService invitesService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.invitesService = invitesService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task<IActionResult> OnGet(int? inviteId, string code)
        {
            if (inviteId == null || code == null)
            {
                return this.NotFound();
            }

            var invite = await this.invitesService.GetByIdAsync<InviteRegisterViewModel>(inviteId);
            if (invite == null)
            {
                return this.NotFound($"Unable to find invite with email '{invite.Email}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            if (!invite.IsInvited || invite.SecurityValue != code)
            {
                return this.NotFound($"Unable to find invite with email '{invite.Email}'.");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/Identity/Account/Login");

            if (this.Input.JobTitle.ToLower() == "founder")
            {
                this.ModelState.AddModelError(string.Empty, "Job Title Founder already exists!!!");
            }

            if (this.ModelState.IsValid)
            {
                var user = new PlanItUser
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    FirstName = this.Input.FirstName,
                    MiddleName = this.Input.MiddleName,
                    LastName = this.Input.LastName,
                    CompanyName = this.Input.Company,
                    JobTitle = this.Input.JobTitle.ToLower(),
                    PhoneNumber = this.Input.PhoneNumber,
                    EmailConfirmed = true,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    // Assign to user roles
                    // await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

                    // Assign to user claims - needed for the _loginPartial
                    string fullName = $"{user.FirstName} {user.LastName}";
                    await this.userManager.AddClaimAsync(user, new Claim("FullName", fullName));
                    await this.userManager.AddClaimAsync(user, new Claim("JobTitle", user.JobTitle));
                    await this.userManager.AddClaimAsync(user, new Claim("CompanyName", user.CompanyName));

                    this.logger.LogInformation("User created a new account with password.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthPassword,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthPassword)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = GlobalConstants.ErrorMessagePasswordConfirmation)]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthUserName,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthUserName)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthUserName,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthUserName)]
            [Display(Name = "Middle Name")]
            public string MiddleName { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthUserName,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthUserName)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthString,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthString)]
            [Display(Name = "Company Name")]
            public string Company { get; set; }

            [Required]
            [StringLength(
                GlobalConstants.MaxLengthString,
                ErrorMessage = GlobalConstants.ErrorMessageStringLength,
                MinimumLength = GlobalConstants.MinLengthString)]
            public string JobTitle { get; set; }

            [Required]
            [RegularExpression(GlobalConstants.PhoneNumberRegexPattern, ErrorMessage = GlobalConstants.ErrorMessagePhoneNumber)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
