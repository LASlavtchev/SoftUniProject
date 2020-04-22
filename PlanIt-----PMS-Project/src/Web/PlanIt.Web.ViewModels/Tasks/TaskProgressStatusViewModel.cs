namespace PlanIt.Web.ViewModels.Tasks
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskProgressStatusViewModel : IMapFrom<ProgressStatus>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
