namespace PlanIt.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UserAdminViewModel : IMapFrom<PlanItUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public bool IsDeleted { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public string UserRoleRoleName { get; set; }
    }
}
