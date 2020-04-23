namespace PlanIt.Web.ViewModels.AdditionalCosts.SubProjects
{
    using System.Collections.Generic;
    using System.Linq;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostCostsBySubProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public IEnumerable<AddCostCostsBySubProjectViewModel> AdditionalCosts { get; set; }

        public decimal AdditionalCostsSum => this.AdditionalCosts.Select(ac => ac.TotalCost).Sum();
    }
}
