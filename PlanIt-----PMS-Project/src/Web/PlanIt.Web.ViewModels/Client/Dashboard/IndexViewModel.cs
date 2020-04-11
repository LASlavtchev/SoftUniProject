namespace PlanIt.Web.ViewModels.Client.Dashboard
{
    public class IndexViewModel
    {
        // Projects
        public int ProjectsCount { get; set; }

        public int ProjectsApprovedCount { get; set; }

        public int ProjectsNotApprovedCount { get; set; }

        public decimal TotalBudget { get; set; }
    }
}
