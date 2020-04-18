namespace PlanIt.Web.ViewModels.SubProjects
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class SubProjectProjectViewModel : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DueDate { get; set; }
    }
}
