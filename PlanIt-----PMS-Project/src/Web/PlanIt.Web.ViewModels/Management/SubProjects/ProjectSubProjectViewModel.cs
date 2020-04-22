namespace PlanIt.Web.ViewModels.Management.SubProjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Management.AdditionalCosts;
    using PlanIt.Web.ViewModels.Management.Tasks;

    public class ProjectSubProjectViewModel : IMapFrom<SubProject>
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public decimal Budget { get; set; }

        public string ProgressStatusName { get; set; }

        public DateTime DueDate { get; set; }

        public IEnumerable<ProjectSubProjectProblemViewModel> Problems { get; set; }

        public decimal TotalSubProjectCosts => this.Problems.Select(p => p.Cost).Sum();

        public IEnumerable<ProjectSubProjectAdditionalCostViewModel> AdditionalCosts { get; set; }

        public decimal TotalAdditionalCosts => this.AdditionalCosts.Select(ac => ac.TotalCost).Sum();
    }
}
