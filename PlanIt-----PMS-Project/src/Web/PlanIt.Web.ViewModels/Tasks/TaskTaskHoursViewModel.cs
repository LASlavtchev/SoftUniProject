namespace PlanIt.Web.ViewModels.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskTaskHoursViewModel : IMapFrom<Problem>
    {
        public int Id { get; set; }

        public string SubProjectProjectName { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public IEnumerable<TaskTaskHoursHourViewModel> Hours { get; set; }

        public IEnumerable<TaskTaskHoursHourViewModel> OrderedHours =>
            this.Hours.OrderBy(h => h.User.FullName);
    }
}
