namespace PlanIt.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using PlanIt.Common;
    using PlanIt.Data.Models;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly SignInManager<PlanItUser> signInManager;

        public IndexModel(
            UserManager<PlanItUser> userManager,
            SignInManager<PlanItUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.User.Claims.Select(c => c.Value).Contains("founder"))
            {
                if (this.Input.JobTitle.ToLower() == "founder")
                {
                    this.ModelState.AddModelError(string.Empty, "Job Title Founder already exists!!!");
                }
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            user.FirstName = this.Input.FirstName;
            user.MiddleName = this.Input.MiddleName;
            user.LastName = this.Input.LastName;

            if (this.Input.Company != "founder")
            {
                user.CompanyName = this.Input.Company;
            }

            user.JobTitle = this.Input.JobTitle.ToLower();

            var claims = await this.userManager.GetClaimsAsync(user);
            await this.userManager.RemoveClaimsAsync(user, (IEnumerable<Claim>)claims);

            string fullName = $"{user.FirstName} {user.LastName}";
            await this.userManager.AddClaimAsync(user, new Claim("FullName", fullName));
            await this.userManager.AddClaimAsync(user, new Claim("JobTitle", user.JobTitle));
            await this.userManager.AddClaimAsync(user, new Claim("CompanyName", user.CompanyName));

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(PlanItUser user)
        {
            var email = await this.userManager.GetEmailAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var jobTitle = user.CompanyName;

            this.Email = email;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                JobTitle = user.JobTitle.ToLower(),
            };

            if (jobTitle != "founder")
            {
                this.Input.Company = jobTitle;
            }
        }

        public class InputModel
        {
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
            [Display(Name = "Job Title")]
            public string JobTitle { get; set; }

            [Phone]
            [Required]
            [RegularExpression(GlobalConstants.PhoneNumberRegexPattern, ErrorMessage = GlobalConstants.ErrorMessagePhoneNumber)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
