namespace PlanIt.Web.ViewModels.Client.SubProjects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public string SubProjectTypeName { get; set; }

        public decimal Budget { get; set; }

        public string ProgressStatusName { get; set; }
    }
}
