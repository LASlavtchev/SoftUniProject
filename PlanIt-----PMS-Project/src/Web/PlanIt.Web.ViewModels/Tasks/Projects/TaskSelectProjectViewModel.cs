namespace PlanIt.Web.ViewModels.Tasks.Projects
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskSelectProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
