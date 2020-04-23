namespace PlanIt.Web.ViewModels.Management.Projects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectAssignInputModel : IMapFrom<Project>, IMapTo<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProjectManagerId { get; set; }
    }
}
