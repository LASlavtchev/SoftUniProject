namespace PlanIt.Web.ViewModels.Management.SubProjectTypes
{
    using System.Collections.Generic;

    using PlanIt.Web.ViewModels.Management.SubProjectsTypes;

    public class ProjectSubProjectTypesListViewModel
    {
        public IEnumerable<ProjectSubProjectTypeViewModel> SubProjectTypes { get; set; }
    }
}
