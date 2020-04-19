namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using System.Collections.Generic;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddCostsSubProjectViewModel> SubProjects { get; set; }
    }
}
