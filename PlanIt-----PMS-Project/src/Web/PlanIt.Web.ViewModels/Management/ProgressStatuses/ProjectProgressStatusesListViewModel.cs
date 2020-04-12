namespace PlanIt.Web.ViewModels.Management.ProgressStatuses
{
    using System.Collections.Generic;

    public class ProjectProgressStatusesListViewModel
    {
        public IEnumerable<ProjectProgressStatusViewModel> Statuses { get; set; }
    }
}
