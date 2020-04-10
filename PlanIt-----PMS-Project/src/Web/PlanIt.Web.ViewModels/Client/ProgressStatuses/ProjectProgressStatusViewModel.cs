namespace PlanIt.Web.ViewModels.Client.ProgressStatuses
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectProgressStatusViewModel : IMapFrom<ProgressStatus>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
