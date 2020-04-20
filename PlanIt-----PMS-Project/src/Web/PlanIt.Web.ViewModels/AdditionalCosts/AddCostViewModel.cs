namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostViewModel : IMapFrom<AdditionalCost>
    {
        public int ProjectId { get; set; }
    }
}
