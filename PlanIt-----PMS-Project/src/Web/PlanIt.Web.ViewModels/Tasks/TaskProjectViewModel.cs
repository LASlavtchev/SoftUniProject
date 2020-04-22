namespace PlanIt.Web.ViewModels.Tasks
{
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TaskProjectSubProjectViewModel> SubProjects { get; set; }
    }
}
