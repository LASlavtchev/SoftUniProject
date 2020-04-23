namespace PlanIt.Web.ViewModels.Management.Clients
{
    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectClientViewModel : IMapFrom<Client>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public string PlantItUserCompanyName { get; set; }

        public string PlantItUserJobTitle { get; set; }

        public string PlantItUserEmail { get; set; }

        public string PlantItUserPhoneNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Client, ProjectClientViewModel>()
                .ForMember(x => x.FullName, options =>
                {
                    options.MapFrom(c => $"{c.PlantItUser.FirstName} {c.PlantItUser.MiddleName} {c.PlantItUser.LastName}");
                });
        }
    }
}
