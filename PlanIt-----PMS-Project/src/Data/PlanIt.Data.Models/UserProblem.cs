namespace PlanIt.Data.Models
{
    using PlanIt.Data.Common.Models;

    public class UserProblem
    {
        public string PlanItUserId { get; set; }

        public PlanItUser User { get; set; }

        public int ProblemId { get; set; }

        public Problem Problem { get; set; }
    }
}
