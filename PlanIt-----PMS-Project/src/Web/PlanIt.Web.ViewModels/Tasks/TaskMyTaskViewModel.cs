namespace PlanIt.Web.ViewModels.Tasks
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskMyTaskViewModel : IMapFrom<Problem>
    {
        public int Id { get; set; }

        public string SubProjectProjectName { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public string ProgressStatusName { get; set; }

        public decimal TotalHours { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
