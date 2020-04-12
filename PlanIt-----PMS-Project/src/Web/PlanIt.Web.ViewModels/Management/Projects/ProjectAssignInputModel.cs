namespace PlanIt.Web.ViewModels.Management.Projects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Management.Clients;
    using PlanIt.Web.ViewModels.Management.Users;

    public class ProjectAssignInputModel : IMapFrom<Project>, IMapTo<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProjectManagerId { get; set; }
    }
}
