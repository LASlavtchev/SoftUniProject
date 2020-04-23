namespace PlanIt.Web.ViewModels.Tasks.SubProjects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskEditSubProjectViewModel : IMapFrom<SubProject>
    {
        public DateTime? DueDate { get; set; }
    }
}
