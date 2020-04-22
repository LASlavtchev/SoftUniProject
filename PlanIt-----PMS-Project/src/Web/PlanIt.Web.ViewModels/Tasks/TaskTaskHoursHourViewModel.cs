namespace PlanIt.Web.ViewModels.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskTaskHoursHourViewModel : IMapFrom<Hour>
    {
        public DateTime Date { get; set; }

        public decimal WorkedHours { get; set; }

        public TaskTaskHoursHourUserViewModel User { get; set; }
    }
}
