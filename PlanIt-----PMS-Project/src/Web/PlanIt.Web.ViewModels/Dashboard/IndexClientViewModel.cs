namespace PlanIt.Web.ViewModels.Dashboard
{
    public class IndexClientViewModel
    {
        // Projects
        public int ProjectsCount { get; set; }

        public int ProjectsApprovedCount { get; set; }

        public int ProjectsNotApprovedCount { get; set; }

        public decimal TotalBudget { get; set; }
    }
}
