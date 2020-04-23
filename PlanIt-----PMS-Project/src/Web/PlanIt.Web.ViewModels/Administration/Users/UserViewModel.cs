namespace PlanIt.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UserViewModel : IMapFrom<PlanItUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public IEnumerable<IdentityUserRole<string>> Roles { get; set; }

        public string RoleName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PlanItUser, UserViewModel>()
                .ForMember(x => x.FullName, options =>
                {
                    options.MapFrom(u => $"{u.FirstName} {u.MiddleName} {u.LastName}");
                });
        }
    }
}
