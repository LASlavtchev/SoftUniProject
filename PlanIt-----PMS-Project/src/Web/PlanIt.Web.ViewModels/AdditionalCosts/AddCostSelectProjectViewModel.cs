namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostSelectProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
