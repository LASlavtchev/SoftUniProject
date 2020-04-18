namespace PlanIt.Web.ViewModels.Management.SubProjects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public decimal Budget { get; set; }

        public string ProgressStatusName { get; set; }
    }
}
