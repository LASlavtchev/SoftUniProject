namespace PlanIt.Web.ViewModels.ProgressStatuses
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProgressStatusViewModel : IMapFrom<ProgressStatus>
    {
        public string Name { get; set; }
    }
}
