namespace PlanIt.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Administration.Roles;

    public class UserViewModel : IMapFrom<PlanItUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.MiddleName} {this.LastName}";

        public bool IsDeleted { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public IEnumerable<IdentityUserRole<string>> Roles { get; set; }

        public string RoleName { get; set; }
    }
}
