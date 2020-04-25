namespace PlanIt.Web.ViewModels.Dashboard
{
    public class IndexUserViewModel
    {
        public int MyProjectsCount { get; set; }

        public int MyCompletedProjectsCount { get; set; }

        public int MyTasksCount { get; set; }

        public int MyCompletedTasksCount { get; set; }

        public decimal TotalBudgetMyProjects { get; set; }

        public decimal TotalBudgetMyCompletedProjects { get; set; }

        public decimal SumAdditionalCostsMyCompletedProjects { get; set; }

        public decimal SumCostsMyCompletedProjects { get; set; }

        public decimal TotalCostsMyCompletedProjects =>
            this.SumAdditionalCostsMyCompletedProjects + this.SumCostsMyCompletedProjects;

        public decimal TotalProfit =>
            this.TotalBudgetMyCompletedProjects - this.TotalCostsMyCompletedProjects;
    }
}
