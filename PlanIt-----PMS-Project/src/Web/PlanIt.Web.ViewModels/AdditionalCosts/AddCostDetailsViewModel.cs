namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostDetailsViewModel : IMapFrom<AdditionalCost>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal PricePerQuantity { get; set; }

        public int Quantity { get; set; }

        public decimal TotalCost { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }
    }
}
