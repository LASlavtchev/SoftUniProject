namespace PlanIt.Web.ViewModels.Management.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectAdditionalCostViewModel : IMapFrom<AdditionalCost>
    {
        public decimal TotalCost { get; set; }
    }
}
