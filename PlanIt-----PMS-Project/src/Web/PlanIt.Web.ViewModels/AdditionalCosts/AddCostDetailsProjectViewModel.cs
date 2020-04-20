namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using System.Collections.Generic;
    using System.Linq;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostDetailsProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddCostDetailsViewModel> AdditionalCosts { get; set; }

        public IEnumerable<AddCostDetailsSubProjectViewModel> SubProjects { get; set; }

        public List<AddCostDetailsViewModel> AdditionalCostsToProjectOnly =>
            this.AdditionalCosts.Where(ac => ac.SubProjectSubProjectTypeName == null)
            .ToList();

        public decimal AdditionalCostsToProjectOnlyTotalCost =>
            this.AdditionalCostsToProjectOnly.Select(c => c.TotalCost).Sum();

        public decimal TotalCost => this.AdditionalCosts.Select(c => c.TotalCost).Sum();
    }
}
