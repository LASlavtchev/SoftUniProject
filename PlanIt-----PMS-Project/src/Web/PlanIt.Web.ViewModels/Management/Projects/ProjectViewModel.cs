namespace PlanIt.Web.ViewModels.Management.Projects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.Users;

    public class ProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public ProjectClientViewModel Client { get; set; }

        public decimal ClientBudget { get; set; }

        public DateTime ClientDueDate { get; set; }
    }
}
