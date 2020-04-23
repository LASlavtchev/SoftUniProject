namespace PlanIt.Web.ViewModels.AdditionalCosts.Projects
{
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.AdditionalCosts.SubProjects;

    public class AddCostAddToSubProjectProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddCostAddToSubProjectSubProjectViewModel> SubProjects { get; set; }
    }
}
