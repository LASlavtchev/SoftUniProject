namespace PlanIt.Web.ViewModels.Client.Projects
{
    using System.Collections.Generic;

    public class ProjectsListViewModel
    {
        public IEnumerable<ProjectViewModel> ApprovedProjects { get; set; }

        public IEnumerable<ProjectViewModel> NotApprovedProjects { get; set; }
    }
}
