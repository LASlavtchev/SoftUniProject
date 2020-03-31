namespace PlanIt.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Web.Infrastructure.Filters;

    [TypeFilter(typeof(InitialRegisterCompanyOwnerAttribute))] // TODO Implement Some Auth filter
#pragma warning disable SA1649 // File name should match first type name
    public class InitialRegisterCompanyOwnerModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly ILogger<RegisterModel> logger;

        public InitialRegisterCompanyOwnerModel(
            UserManager<PlanItUser> userManager,
            ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/Identity/Account/Login");

            // For safety
            if (this.userManager.Users.ToList().Count > 0)
            {
                return this.LocalRedirect(returnUrl);
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
                    JobTitle = "founder",
                    PhoneNumber = this.Input.PhoneNumber,
                    EmailConfirmed = true,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    // Assign to user roles
                    await this.userManager.AddToRoleAsync(user, GlobalConstants.CompanyOwnerRoleName);

                    // Assign to user claims
                    string fullName = $"{user.FirstName} {user.LastName}";
                    await this.userManager.AddClaimAsync(user, new Claim("FullName", fullName));
                    await this.userManager.AddClaimAsync(user, new Claim("JobTitle", user.JobTitle));
                    await this.userManager.AddClaimAsync(user, new Claim("CompanyName", user.CompanyName));

                    this.logger.LogInformation("Founder created a new account with password.");

                    return this.LocalRedirect(returnUrl);
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
            [EmailAddress(ErrorMessage =GlobalConstants.EmailAddressErrorMessage)]
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
            [RegularExpression(GlobalConstants.PhoneNumberRegexPattern, ErrorMessage = GlobalConstants.ErrorMessagePhoneNumber)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
