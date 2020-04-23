namespace PlanIt.Web.ViewModels.Tasks.Users
{
    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskIndexUserViewModel : IMapFrom<PlanItUser>, IHaveCustomMappings
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PlanItUser, TaskIndexUserViewModel>()
                .ForMember(x => x.FullName, options =>
                {
                    options.MapFrom(u => $"{u.FirstName} {u.LastName}");
                });
        }
    }
}
