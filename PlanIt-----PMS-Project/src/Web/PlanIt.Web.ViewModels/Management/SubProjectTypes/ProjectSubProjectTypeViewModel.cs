namespace PlanIt.Web.ViewModels.Management.SubProjectsTypes
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectSubProjectTypeViewModel : IMapFrom<SubProjectType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
