namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using System.Collections.Generic;
    using System.Linq;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostDetailsSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public IEnumerable<AddCostDetailsViewModel> AdditionalCosts { get; set; }

        public decimal AdditionalCostsSum => this.AdditionalCosts.Select(ac => ac.TotalCost).Sum();
    }
}
