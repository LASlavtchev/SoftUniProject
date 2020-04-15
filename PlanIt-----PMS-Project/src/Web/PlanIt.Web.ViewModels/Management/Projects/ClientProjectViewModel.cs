namespace PlanIt.Web.ViewModels.Management.Projects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ClientProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProgressStatusName { get; set; }

        public decimal Budget { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsBudgetApproved { get; set; }
    }
}
