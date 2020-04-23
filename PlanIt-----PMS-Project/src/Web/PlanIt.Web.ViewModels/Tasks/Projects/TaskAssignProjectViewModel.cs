namespace PlanIt.Web.ViewModels.Tasks.Projects
{
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Tasks.SubProjects;

    public class TaskAssignProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TaskAssignSubProjectViewModel> SubProjects { get; set; }
    }
}
