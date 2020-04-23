namespace PlanIt.Web.ViewModels.Tasks.UserProblems
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Tasks.Users;

    public class TaskIndexUserProblemsViewModel : IMapFrom<UserProblem>
    {
        public TaskIndexUserViewModel User { get; set; }
    }
}
