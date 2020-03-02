namespace PlanIt.Data
{
    using Microsoft.AspNetCore.Identity;
    using PlanIt.Common;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = GlobalConstants.MinLengthPassword;
            options.Password.RequiredUniqueChars = 1;

            options.User.RequireUniqueEmail = true;

            // options.SignIn.RequireConfirmedEmail = true;
        }
    }
}
