namespace PlanIt.Web.ViewModels.AdditionalCosts.Projects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostAddToProjectProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }
    }
}
