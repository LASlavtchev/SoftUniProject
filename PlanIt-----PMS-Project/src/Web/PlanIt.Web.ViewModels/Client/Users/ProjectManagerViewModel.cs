namespace PlanIt.Web.ViewModels.Client.Users
{
    using System.ComponentModel;

    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectManagerViewModel : IMapFrom<PlanItUser>, IHaveCustomMappings
    {
        [DisplayName("Full name")]
        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PlanItUser, ProjectManagerViewModel>()
                .ForMember(x => x.FullName, options =>
                {
                    options.MapFrom(u => $"{u.FirstName} {u.MiddleName} {u.LastName}");
                });
        }
    }
}
