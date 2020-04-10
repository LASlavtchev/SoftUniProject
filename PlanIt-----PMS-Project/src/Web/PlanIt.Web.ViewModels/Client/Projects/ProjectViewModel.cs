namespace PlanIt.Web.ViewModels.Client.Projects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsBudgetApproved { get; set; }

        public decimal ClientBudget { get; set; }

        public DateTime ClientDueDate { get; set; }
    }
}
