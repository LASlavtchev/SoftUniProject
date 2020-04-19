namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostsSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }
    }
}
