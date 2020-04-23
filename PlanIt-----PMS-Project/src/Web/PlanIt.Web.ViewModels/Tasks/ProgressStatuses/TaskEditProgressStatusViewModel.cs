namespace PlanIt.Web.ViewModels.Tasks.ProgressStatuses
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskEditProgressStatusViewModel : IMapFrom<ProgressStatus>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
