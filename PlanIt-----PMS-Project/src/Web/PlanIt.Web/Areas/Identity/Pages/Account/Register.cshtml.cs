﻿namespace PlanIt.Web.Areas.Identity.Pages.Account
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
    using PlanIt.Services.Messaging;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<PlanItUser> signInManager;
        private readonly UserManager<PlanItUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<PlanItUser> userManager,
            SignInManager<PlanItUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
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
            returnUrl ??= this.Url.Content("~/");

            if (this.Input.Occupation.ToLower() == "founder")
            {
                this.ModelState.AddModelError(string.Empty, "Occupation Founder already exists!!!");
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
                    CompanyName = this.Input.Company.ToLower(),
                    Occupation = this.Input.Occupation,
                    PhoneNumber = this.Input.PhoneNumber,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    // Assign to user roles
                    // await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

                    // Assign to user claims - needed for the _loginPartial
                    string fullName = $"{user.FirstName} {user.LastName}";
                    await this.userManager.AddClaimAsync(user, new Claim("FullName", fullName));
                    await this.userManager.AddClaimAsync(user, new Claim("Occupation", user.Occupation));
                    await this.userManager.AddClaimAsync(user, new Claim("CompanyName", user.CompanyName));

                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code },
                        protocol: this.Request.Scheme);

                    var messageToSend = $"Please confirm your account by " +
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                    //await this.emailSender.SendEmailAsync(
                    //    "admin@admin.bg",
                    //    "admin",
                    //    this.Input.Email,
                    //    "Confirm your email",
                    //    $"{messageToSend}");

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
            public string Occupation { get; set; }

            [Required]
            [RegularExpression(GlobalConstants.PhoneNumberRegexPattern, ErrorMessage = GlobalConstants.ErrorMessagePhoneNumber)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
