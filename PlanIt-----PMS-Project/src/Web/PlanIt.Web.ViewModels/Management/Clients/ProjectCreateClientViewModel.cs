namespace PlanIt.Web.ViewModels.Management.Clients
{
    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectCreateClientViewModel : IMapFrom<Client>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Client, ProjectCreateClientViewModel>()
                .ForMember(x => x.FullName, options =>
                {
                    options.MapFrom(c => $"{c.PlantItUser.FirstName} {c.PlantItUser.MiddleName} {c.PlantItUser.LastName}");
                });
        }
    }
}
