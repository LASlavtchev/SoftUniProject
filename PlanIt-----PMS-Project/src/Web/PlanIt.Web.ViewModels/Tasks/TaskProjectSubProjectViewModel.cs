namespace PlanIt.Web.ViewModels.Tasks
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
