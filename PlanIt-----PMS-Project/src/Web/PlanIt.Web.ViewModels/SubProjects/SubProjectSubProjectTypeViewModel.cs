namespace PlanIt.Web.ViewModels.SubProjects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class SubProjectSubProjectTypeViewModel : IMapFrom<SubProjectType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
