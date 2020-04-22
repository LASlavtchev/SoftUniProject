namespace PlanIt.Web.ViewModels.Tasks
{
    using System;
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskIndexViewModel : IMapFrom<Problem>
    {
        public int Id { get; set; }

        public string SubProjectProjectName { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public IEnumerable<TaskIndexUserProblemsViewModel> UserProblems { get; set; }

        public string ProgressStatusName { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal TotalHours { get; set; }

        public decimal TotalCost => this.TotalHours * this.HourlyRate;

        public DateTime? DueDate { get; set; }
    }
}
