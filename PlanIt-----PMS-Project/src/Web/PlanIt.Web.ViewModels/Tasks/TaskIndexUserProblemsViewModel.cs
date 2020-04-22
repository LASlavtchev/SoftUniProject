namespace PlanIt.Web.ViewModels.Tasks
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskIndexUserProblemsViewModel : IMapFrom<UserProblem>
    {
        public TaskIndexUserViewModel User { get; set; }
    }
}
