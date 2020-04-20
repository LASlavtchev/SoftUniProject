namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostDeleteViewModel : IMapFrom<AdditionalCost>
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
    }
}
