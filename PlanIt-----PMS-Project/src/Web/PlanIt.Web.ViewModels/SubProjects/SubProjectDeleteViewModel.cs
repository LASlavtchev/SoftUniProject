namespace PlanIt.Web.ViewModels.SubProjects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class SubProjectDeleteViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
    }
}
