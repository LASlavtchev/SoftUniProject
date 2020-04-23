namespace PlanIt.Web.ViewModels.AdditionalCosts.SubProjects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostAddToSubProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }
    }
}
