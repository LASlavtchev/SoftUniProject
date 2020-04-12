﻿namespace PlanIt.Web.ViewModels.Management.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.SubProjects;
    using PlanIt.Web.ViewModels.Management.Users;

    public class ProjectDetailsViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal Budget { get; set; }

        public bool IsBudgetApproved { get; set; }

        public string ProgressStatusName { get; set; }

        public ProjectManagerViewModel ProjectManager { get; set; }

        public ProjectClientViewModel Client { get; set; }

        public decimal ClientBudget { get; set; }

        public DateTime ClientDueDate { get; set; }

        public IEnumerable<ProjectSubProjectViewModel> SubProjects { get; set; }
    }
}
