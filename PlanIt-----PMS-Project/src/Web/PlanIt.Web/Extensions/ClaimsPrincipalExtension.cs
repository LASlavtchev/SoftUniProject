﻿namespace PlanIt.Web.Extensions
{
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsPrincipalExtension
    {
        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var fullName = principal.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullName.Value;
        }

        public static string GetJobTitle(this ClaimsPrincipal principal)
        {
            var occupation = principal.Claims.FirstOrDefault(c => c.Type == "JobTitle");
            return occupation.Value;
        }
    }
}
