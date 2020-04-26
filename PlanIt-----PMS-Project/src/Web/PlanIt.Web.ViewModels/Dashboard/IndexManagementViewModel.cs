namespace PlanIt.Web.ViewModels.Dashboard
{
    public class IndexManagementViewModel
    {
        public int AllProjectsCount { get; set; }

        public int AllApprovedProjectsCount { get; set; }

        public int AllCompletedProjectsCount { get; set; }

        public int AssignedManagersCount { get; set; }

        public int AssignedUsersCount { get; set; }

        public int FreeUsersCount { get; set; }

        public decimal TotalBudgetApprovedProjects { get; set; }

        public decimal TotalBudgetCompletedProjects { get; set; }

        public decimal TotalCostsCompletedProjects { get; set; }

        public decimal TotalProfit =>
            this.TotalBudgetCompletedProjects - this.TotalCostsCompletedProjects;
    }
}
