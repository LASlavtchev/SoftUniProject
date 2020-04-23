namespace PlanIt.Web.ViewModels.Tasks.Hours
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskMyTaskHoursHourViewModel : IMapFrom<Hour>
    {
        public DateTime Date { get; set; }

        public decimal WorkedHours { get; set; }

        public string UserId { get; set; }
    }
}
