namespace PlanIt.Web.ViewModels.Tasks
{
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Tasks.Hours;

    public class TaskMyTaskHoursViewModel : IMapFrom<Problem>
    {
        public int Id { get; set; }

        public string SubProjectProjectName { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public IEnumerable<TaskMyTaskHoursHourViewModel> Hours { get; set; }
    }
}
